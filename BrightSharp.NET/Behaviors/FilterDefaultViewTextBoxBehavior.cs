using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace BrightSharp.Behaviors
{
    public class FilterDefaultViewTextBoxBehavior : Behavior<TextBox>
    {
        readonly DispatcherTimer _timer = new DispatcherTimer();
        
        public FilterDefaultViewTextBoxBehavior()
        {
            _timer.Tick += Timer_Tick;
            FilterDelay = TimeSpan.FromSeconds(1);
            IgnoreCase = null; //Case sensitive if any char upper case
            FilterStringPropertyName = "FilterString";

        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            if (ItemsSource != null)
            {
                var view = CollectionViewSource.GetDefaultView(ItemsSource);
                if (view is ListCollectionView)
                {
                    var listCollectionView = (ListCollectionView)view;
                    if (listCollectionView.IsAddingNew || listCollectionView.IsEditingItem) return;
                }
                view.Filter = CustomFilter ?? GetDefaultFilter(ItemsSource.GetType().GetGenericArguments()[0]);
            }
        }

        public string FilterStringPropertyName { get; set; }

        public bool HasFilterText
        {
            get { return (bool)GetValue(HasFilterTextProperty); }
            set { SetValue(HasFilterTextProperty, value); }
        }

        public static readonly DependencyProperty HasFilterTextProperty =
            DependencyProperty.Register("HasFilterText", typeof(bool), typeof(FilterDefaultViewTextBoxBehavior), new PropertyMetadata(false));



        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(FilterDefaultViewTextBoxBehavior), new PropertyMetadata(null));


        public bool? IgnoreCase { get; set; }
        private Predicate<object> GetDefaultFilter(Type filterItemType)
        {
            Func<object, string> dataFunc;
            var pInfo = filterItemType.GetProperty(FilterStringPropertyName);
            if (pInfo == null)
            {
                dataFunc = (x) => string.Join(",", filterItemType.GetProperties().Where(p => !p.Name.EndsWith("Id")).Select(p => (p.GetValue(x, null) ?? string.Empty).ToString()));
            }
            else
            {
                dataFunc = (x) => (pInfo.GetValue(x, null) ?? string.Empty).ToString();
            }
            var filterText = AssociatedObject.Text;
            var ic = IgnoreCase ?? !filterText.Any(char.IsUpper);
            return x =>
            {
                if (x == null || string.IsNullOrEmpty(filterText)) return true;
                var propValStr = dataFunc(x);
                foreach (var item in filterText.Split(';'))
                {
                    if (propValStr.IndexOf(item, ic ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == -1)
                        return false;
                }
                return true;
            };
        }

        public Predicate<object> CustomFilter { get; set; }

        public TimeSpan FilterDelay { get { return _timer.Interval; } set { _timer.Interval = value; } }

        protected override void OnAttached()
        {
            base.OnAttached();
            HasFilterText = !string.IsNullOrEmpty(AssociatedObject.Text);
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            _timer.Stop(); _timer.Start();
            HasFilterText = !string.IsNullOrEmpty(AssociatedObject.Text);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
            _timer.Tick -= Timer_Tick;
            base.OnDetaching();
        }
    }
}
