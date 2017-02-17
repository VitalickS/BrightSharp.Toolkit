using System.IO;
using Microsoft.Win32;

namespace BrightSharp.Toolkit.Extra
{
    public static class DesignTimeConfig
    {
        public static void SetNugetPackagesDirectory(string nugetDir)
        {
            if (Directory.Exists(nugetDir))
            {
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Brightsharp").SetValue("NUGET_PACKAGES", nugetDir);
            }
        }
        public static void DesignTimeUnRegisterNugetPackages()
        {
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Brightsharp").SetValue("NUGET_PACKAGES", null);
        }

        internal static string GetNugetPackagesDirectory()
        {
            var regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Brightsharp");
            if (regKey != null)
            {
                var regValue = regKey.GetValue("NUGET_PACKAGES");
                return (regValue ?? string.Empty).ToString();
            }
            return null;
        }
    }
}
