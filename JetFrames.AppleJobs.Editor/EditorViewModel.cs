using BrightSharp.Mvvm;
using System.Collections.Generic;
using System.Linq;
using AppleJobs.Data;
using AppleJobs.Data.Models.ModelsJobs;
using System.Windows.Input;
using BrightSharp.Commands;
using System.Data.Entity;
using System.Windows;
using BrightSharp.Extensions;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using AppleJobs.Data.Models;
using JetFrames.AppleJobs.Editor.Views;
using JetFrames.AppleJobs.Editor.ViewModels;
using System.Collections;

namespace JetFrames.AppleJobs.Editor
{
    public class EditorViewModel : ObservableObject
    {
        private AppleJobsContext context;

        public EditorViewModel(AppleJobsContext context)
        {
            this.context = context;
        }

        public string Name { get; set; }
        public string Email { get; set; }

        List<Model> _models;
        public IEnumerable<Model> Models
        {
            get
            {
                return _models ?? (_models = context.Models.ToList());
            }
        }
        List<ModelCategory> _categories;
        public IEnumerable<ModelCategory> Categories
        {
            get
            {
                return _categories ?? (_categories = context.ModelCategories.ToList());
            }
        }

        public void OnLoaded()
        {

        }

        public bool CanAdd(object item)
        {
            if (item is ModelJobPriceTemplate)
            {
                var mjpt = (ModelJobPriceTemplate)item;
                return mjpt.Customers_Id != default(int) &&
                       mjpt.ModelJobs_Id != default(int);
            }
            else if (item is ModelJob)
            {
                var mj = (ModelJob)item;
                return !string.IsNullOrWhiteSpace(mj.Name) &&
                       mj.Models_Id != default(int);
            }
            else if (item is Model)
            {
                var m = (Model)item;
                return !string.IsNullOrWhiteSpace(m.Name) &&
                       m.ModelCategories_Id != default(int);
            }
            else if (item is ModelCategory)
            {
                var mc = (ModelCategory)item;
                return !string.IsNullOrWhiteSpace(mc.Name);
            }
            return true;
        }

        List<ModelJobPriceTemplate> _modelJobPriceTemplates;
        public IEnumerable<ModelJobPriceTemplate> ModelJobPriceTemplates
        {
            get
            {
                if (ModelJobs == null) throw new InvalidOperationException();
                return _modelJobPriceTemplates ?? (_modelJobPriceTemplates = context.ModelJobPriceTemplates.ToList());
            }
        }

        List<ModelJob> _modelJobs;
        public IEnumerable<ModelJob> ModelJobs
        {
            get
            {
                return _modelJobs ?? (_modelJobs = context.ModelJobs.ToList());
            }
        }

        List<Order> _orders;
        public IEnumerable<Order> Orders
        {
            get
            {
                return _orders ?? (_orders = context.Orders.Take(1000).ToList());
            }
        }
        List<OrderStatus> _orderStatuses;
        public IEnumerable<OrderStatus> OrderStatuses
        {
            get
            {
                return _orderStatuses ?? (_orderStatuses = context.OrderStatuses.ToList());
            }
        }

        List<Accessories> _accessories;
        public IEnumerable<Accessories> Accessories
        {
            get
            {
                return _accessories ?? (_accessories = context.Accessories.ToList());
            }
        }

        List<Employee> _employees;
        public IEnumerable<Employee> Employees
        {
            get
            {
                return _employees ?? (_employees = context.Employees.ToList());
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new AsyncCommand(p =>
                {
                    var dg = (Keyboard.FocusedElement as UIElement).FindAncestor<DataGrid>();
                    if (dg != null)
                    {
                        dg.CommitEdit(DataGridEditingUnit.Row, true);
                    }

                    return context.SaveChangesAsync();
                })
                        .OnComplete(() => ShowMessage("Сохранено"))
                        .OnFail(ex => ShowMessage("Ошибка при сохранении.", "Red"));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(p =>
                {
                    _categories = null;
                    _modelJobPriceTemplates = null;
                    _models = null;
                    _modelJobs = null;
                    _accessories = null;
                    _employees = null;
                    _orders = null;
                    _orderStatuses = null;

                    foreach (var entity in context.ChangeTracker.Entries())
                    {
                        entity.State = EntityState.Detached;
                    }
                    RaisePropertyChanged(nameof(Categories));
                    RaisePropertyChanged(nameof(ModelJobPriceTemplates));
                    RaisePropertyChanged(nameof(Models));
                    RaisePropertyChanged(nameof(ModelJobs));
                    RaisePropertyChanged(nameof(Accessories));
                    RaisePropertyChanged(nameof(Employees));
                    RaisePropertyChanged(nameof(Orders));
                    RaisePropertyChanged(nameof(OrderStatuses));
                    ShowMessage("Обновлено из базы данных");
                    OnLoaded();
                });
            }
        }

        public ICommand AddNewPriceTemplateCommand
        {
            get
            {
                return new RelayCommand(p =>
                {
                    var vm = new NewPriceTemplateVm(Models, ModelJobs, ModelJobPriceTemplates, CreateNewModelJobPriceTemplate);
                    var dialog = new NewPriceTemplateDialog
                    {
                        DataContext = vm,
                        Owner = Application.Current.MainWindow
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        CreateNewModelJobPriceTemplate(vm);
                    }
                });
            }
        }

        private void CreateNewModelJobPriceTemplate(NewPriceTemplateVm vm)
        {
            var item = new ModelJobPriceTemplate
            {
                Price = vm.NewPrice,
                Customers_Id = 1,
                ModelJobs_Id = vm.SelectedModelJob.Id,
                IsPriceFrom = true
            };
            _modelJobPriceTemplates.Add(item);
            context.Entry(item).State = EntityState.Added;
            CollectionViewSource.GetDefaultView(ModelJobPriceTemplates).Refresh();
        }

        private object _currentRow;

        public object CurrentRow
        {
            get { return _currentRow; }
            set { _currentRow = value; RaisePropertyChanged(nameof(CurrentRow)); }
        }


        public ICommand ModelJobDropDownLoadedCommand
        {
            get
            {
                return new RelayCommand<int>(mId =>
                {
                    var view = (CollectionViewSource)Application.Current.MainWindow.FindResource("modelJobsViewSource");
                    view.View.Filter = o =>
                    {
                        var mj = (ModelJob)o;

                        return mj.Models_Id == mId;
                    };
                });
            }
        }

        public ICommand ModelJobDropDownUnLoadedCommand
        {
            get
            {
                return new RelayCommand(p =>
                {
                    var cb = (ComboBox)p;
                    cb.ItemsSource = null;
                    var view = (CollectionViewSource)Application.Current.MainWindow.FindResource("modelJobsViewSource");
                    view.View.Filter = null;
                });
            }
        }

        public bool DeleteEntity(object entity, bool promt)
        {
            if (entity != null && (entity.GetType().IsPublic || entity is IEnumerable))
            {
                if (promt)
                    if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                        return true;

                if (entity is IEnumerable)
                    foreach (var item in (IEnumerable)entity)
                        DeleteEntityInt(item);
                else
                    DeleteEntityInt(entity);
            }

            return false;
        }

        private void DeleteEntityInt(object entity)
        {
            var currentState = context.Entry(entity).State;
            if (currentState == EntityState.Added)
            {
                context.Entry(entity).State = EntityState.Detached;
            }
            else if (currentState != EntityState.Detached)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public void AddEntity(object entity)
        {
            if (entity != null)
            {
                context.Entry(entity).State = EntityState.Added;
            }
        }

        #region Message

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(nameof(Message)); }
        }

        private string _messageForeground;

        public string MessageForeground
        {
            get { return _messageForeground; }
            set { _messageForeground = value; RaisePropertyChanged(nameof(MessageForeground)); }
        }

        private string _messageAnimationState;
        public string MessageAnimationState
        {
            get { return _messageAnimationState; }
            set { _messageAnimationState = value; RaisePropertyChanged(nameof(MessageAnimationState)); }
        }
        private void ShowMessage(string message, string foreground = "DarkGreen")
        {
            Message = message;
            MessageForeground = foreground;
            MessageAnimationState = ""; MessageAnimationState = "New";
        }

        #endregion
    }
}
