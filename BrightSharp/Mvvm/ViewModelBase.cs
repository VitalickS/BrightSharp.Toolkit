using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BrightSharp.Mvvm
{
    public class ObservableObject : INotifyPropertyChanged
    {
        static readonly DependencyObject designTimeObject = new DependencyObject();
        public static bool IsInDesignTime
        {
            get
            {
                try
                {
                    return DesignerProperties.GetIsInDesignMode(designTimeObject);
                }
                catch(Exception)
                {
                    return true;
                }

            }
        }

        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get
            {
                return PropertyChanged;
            }
        }

        public ObservableObject()
        {
        }

        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if(propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }
            MemberExpression body = propertyExpression.Body as MemberExpression;
            if(body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }
            PropertyInfo property = body.Member as PropertyInfo;
            if(property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }
            return property.Name;
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(GetPropertyName(propertyExpression)));
        }

        protected Boolean Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            if(EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            field = newValue;
            this.RaisePropertyChanged(propertyExpression);
            return true;
        }

        protected bool Set<T>(string propertyName, ref T field, T newValue)
        {
            if(EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaisePropertyChangedAll()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}