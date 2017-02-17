using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using CefSharp;
using CefSharp.WinForms;

namespace BrightSharp.Toolkit.Extra
{
    public partial class CKEditor
    {
        internal ChromiumWebBrowser Browser;
        private CKEditorObjectForScripting _objectForScripting;

        public static void InitializeCef(bool useCache = true, bool reinit = false)
        {
            Dispatcher.CurrentDispatcher.ShutdownStarted += (s, e) => Cef.Shutdown();
            if (!reinit && Cef.IsInitialized) return;
            CefSettings settings;

            if (useCache)
            {
                var cacheDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "cefCache");
                Directory.CreateDirectory(cacheDir);
                settings = new CefSettings
                {
                    CachePath = cacheDir
                };
            }
            else
            {
                settings = new CefSettings();
            }

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "local",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(Environment.CurrentDirectory)
            });

            Cef.Initialize(settings);
        }

        private static void InitializeDesignTimeDirectory()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var nugetPackages = DesignTimeConfig.GetNugetPackagesDirectory();
                if (!Directory.Exists(nugetPackages)) return;
                var prefix = IntPtr.Size == 4 ? "x86" : "x64";
                var allDirectoriesToCopy = Directory.GetDirectories(nugetPackages, "cef.redist.*")
                    .Union(Directory.GetDirectories(nugetPackages, "CefSharp.*"))
                    .SelectMany(d => Directory.GetDirectories(d, prefix, SearchOption.AllDirectories));
                foreach (var dirForCopy in allDirectoriesToCopy)
                {
                    foreach (var file in Directory.GetFiles(dirForCopy))
                    {
                        try { File.Copy(file, Path.Combine(Environment.CurrentDirectory, Path.GetFileName(file)), false); }
                        catch (Exception) { }
                    }
                    foreach (var file in Directory.GetFiles(Directory.GetParent(dirForCopy).FullName))
                    {
                        try { File.Copy(file, Path.Combine(Environment.CurrentDirectory, Path.GetFileName(file)), false); }
                        catch (Exception) { }
                    }
                }
            }

        }

        static CKEditor()
        {
            InitializeDesignTimeDirectory();
            if (!File.Exists("CefSharp.Core.dll"))
                return;
            InitializeCef();
        }

        public CKEditor()
        {
            InitializeComponent();
        }


        public override void OnApplyTemplate()
        {
            if (!File.Exists("CefSharp.dll"))
                return;

            var host = (WindowsFormsHost)Template.FindName("PART_Host", this);


            host.Child = Browser = new ChromiumWebBrowser("about:blank");

            _objectForScripting = new CKEditorObjectForScripting(this);
            Browser.KeyboardHandler = new FixKeyboardHandler();

            Browser.IsBrowserInitializedChanged += (s, e) =>
            {
                if (e.IsBrowserInitialized)
                    RefreshState();
            };
            base.OnApplyTemplate();
        }

        public void RefreshState()
        {
            if (Browser != null && Browser.IsBrowserInitialized)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    var docContent = BuildDocContent();
                    Browser.LoadString(docContent, "about:blank");
                }));
                UpdateDevToolsState();
            }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (Browser != null && _objectForScripting != null && (string)newContent != _objectForScripting.NewData)
            {
                //This is not from js
                Browser.EvaluateScriptAsync($"setData(`{newContent}`)").Wait(TimeSpan.FromSeconds(2));
            }

            base.OnContentChanged(oldContent, newContent);
        }

        public string PackageType
        {
            get { return (string)GetValue(PackageTypeProperty); }
            set { SetValue(PackageTypeProperty, value); }
        }

        public static readonly DependencyProperty PackageTypeProperty =
            DependencyProperty.Register("PackageType", typeof(string), typeof(CKEditor), new PropertyMetadata("standard", UpdatePackageType));

        private static void UpdatePackageType(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CKEditor)d;
            editor.RefreshState();
        }


        public bool ShowDevTools
        {
            get { return (bool)GetValue(ShowDevToolsProperty); }
            set { SetValue(ShowDevToolsProperty, value); }
        }

        public static readonly DependencyProperty ShowDevToolsProperty =
            DependencyProperty.Register("ShowDevTools", typeof(bool), typeof(CKEditor), new PropertyMetadata(false, ShowDevToolsChanged));

        private static void ShowDevToolsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (CKEditor)d;
            editor.UpdateDevToolsState();
        }

        private void UpdateDevToolsState()
        {
            if (Browser != null && Browser.IsBrowserInitialized)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    if (ShowDevTools)
                        Browser.ShowDevTools();
                    else
                        Browser.CloseDevTools();
                }));
            }
        }

        private string BuildDocContent()
        {
            var docContent = Properties.Resources.HtmlEditor;
            if (EditorConfiguration != null)
            {
                EditorConfiguration.Optimize(PackageType);
                docContent = docContent.Replace("$$CONFIG",
                    new JavaScriptSerializer().Serialize(EditorConfiguration));
                docContent = docContent.Replace("$$PACKAGE", PackageType);
            }

            docContent = docContent.Replace("$$CONTENT", (Content ?? "").ToString());

            return docContent;
        }


        public CKEditorConfig EditorConfiguration
        {
            get { return (CKEditorConfig)GetValue(EditorConfigurationProperty); }
            set { SetValue(EditorConfigurationProperty, value); }
        }

        public static readonly DependencyProperty EditorConfigurationProperty =
            DependencyProperty.Register("EditorConfiguration", typeof(CKEditorConfig), typeof(CKEditor), new PropertyMetadata(new CKEditorConfig()));


    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CKEditorConfig
    {
        public string[] contentsCss { get; set; }
        public string language { get; set; } = Thread.CurrentThread.CurrentUICulture.Name;
        public string removeButtons { get; set; } =
            "Save,Print,Preview,Maximize,ShowBlocks,Paste,PasteText,PasteFromWord";
        public string extraAllowedContent { get; set; } = "span;ul;li;table;td;style;*[id];*(*);*{*}";
        public string removePlugins { get; set; } = "resize";

        public void Optimize(string packageType)
        {
            if (string.Equals("basic", packageType, StringComparison.InvariantCultureIgnoreCase))
            {
                //TODO Basic configuration has many problems.
                //TODO Need think do we need it, or can we use Standard package as Basic to emulate Basic (from CKE) ?
            }
        }
    }

    internal class CKEditorObjectForScripting
    {
        private CKEditor _editor;

        internal string NewData;

        public CKEditorObjectForScripting(CKEditor editor)
        {
            _editor = editor;
            _editor.Browser.RegisterJsObject("$$OBJ", this);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void SetupContent(string newData)
        {
            NewData = newData;
            _editor.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => _editor.Content = NewData));
        }
    }

    public class CKEditorDesignTimeSettings
    {
        public static string DesignTimeCefBinariesDirectory { get; set; }
    }
    class FixKeyboardHandler : IKeyboardHandler
    {
        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode,
            CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            //F10, Alt
            if (windowsKeyCode == 0x79 || windowsKeyCode == 0x12)
            {
                return true;
            }
            return false;
        }

        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode,
            CefEventFlags modifiers, bool isSystemKey)
        {
            if (windowsKeyCode == 0x79 || windowsKeyCode == 0x12)
            {
                return true;
            }
            return false;
        }
    }


}
