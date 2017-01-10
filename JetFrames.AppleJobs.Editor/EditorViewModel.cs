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
using JetFrames.AppleJobs.Editor.ViewModels;
using System.Collections;
using AppleJobs.Data.Models.Articles;
using AppleJobs.Data.Models.Orders;
using AppleJobs.Data.Models.Inventory;
using System.ComponentModel;

namespace JetFrames.AppleJobs.Editor
{
    public class EditorViewModel : ObservableObject
    {
        private AppleJobsContext context;

        public EditorViewModel(AppleJobsContext context)
        {
            this.context = context;
        }

        #region Data Properties

        List<Model> _models;
        public IEnumerable<Model> Models { get { return GetSingleton(ref _models, context.Models); } }
        List<ModelCategory> _categories;
        public IEnumerable<ModelCategory> Categories { get { return GetSingleton(ref _categories, context.ModelCategories); } }
        private List<News> _news;
        public IEnumerable<News> News { get { return GetSingleton(ref _news, context.News); } }
        private List<NewsCategory> _newsCategories;
        public IEnumerable<NewsCategory> NewsCategories { get { return GetSingleton(ref _newsCategories, context.NewsCategories); } }
        List<ModelJobPriceTemplate> _modelJobPriceTemplates;
        public IEnumerable<ModelJobPriceTemplate> ModelJobPriceTemplates
        {
            get
            {
                context.ModelJobs.Count();
                return GetSingleton(ref _modelJobPriceTemplates, context.ModelJobPriceTemplates);
            }
        }
        List<ModelJob> _modelJobs;
        public IEnumerable<ModelJob> ModelJobs { get { return GetSingleton(ref _modelJobs, context.ModelJobs); } }
        List<Order> _orders;
        public IEnumerable<Order> Orders { get { return GetSingleton(ref _orders, context.Orders.OrderByDescending(o => o.DateCreated).Take(2000)); } }
        List<OrderStatus> _orderStatuses;
        public IEnumerable<OrderStatus> OrderStatuses { get { return GetSingleton(ref _orderStatuses, context.OrderStatuses); } }
        List<Accessories> _accessories;
        public IEnumerable<Accessories> Accessories { get { return GetSingleton(ref _accessories, context.Accessories); } }
        List<Employee> _employees;
        public IEnumerable<Employee> Employees { get { return GetSingleton(ref _employees, context.Employees); } }

        #endregion

        #region Initialize Grid

        public static void InitGrid(DataGrid dg)
        {
            CommandManager.AddPreviewCanExecuteHandler(dg, DataGridPreviewCanExecute);
            dg.InitializingNewItem += DataGridInitializingNewItem;
        }
        private static void DataGridPreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == DataGrid.DeleteCommand)
                e.Handled = App.Locator.Editor.DeleteEntity(((DataGrid)sender).SelectedItems, true);
        }

        private static void DataGridInitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            App.Locator.Editor.AddEntity(e.NewItem);
        }

        #endregion

        #region Commands
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
                });
            }
        }

        public ICommand AddNewPriceTemplateCommand
        {
            get
            {
                return new RelayCommand(p =>
                {
                    var vm = new NewPriceTemplateVm(Models, ModelJobs, ModelJobPriceTemplates, viewModel =>
                    {
                        var item = new ModelJobPriceTemplate
                        {
                            Price = viewModel.NewPrice,
                            Customers_Id = 1,
                            ModelJobs_Id = viewModel.SelectedModelJob.Id,
                            IsPriceFrom = true
                        };
                        _modelJobPriceTemplates.Add(item);
                        context.Entry(item).State = EntityState.Added;
                        CollectionViewSource.GetDefaultView(ModelJobPriceTemplates).Refresh();
                    });
                    var dialog = new Views.NewPriceTemplateDialog
                    {
                        DataContext = vm,
                        Owner = Application.Current.MainWindow
                    };
                });
            }
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

        #endregion

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

        public void AddEntity(object newItem)
        {
            if (newItem != null)
            {
                if (newItem is Order)
                {
                    var mjpt = (Order)newItem;
                    mjpt.Customers_Id = 1;
                }
                if (newItem is ModelJobPriceTemplate)
                {
                    var mjpt = (ModelJobPriceTemplate)newItem;
                    mjpt.Customers_Id = 1;
                }
                context.Entry(newItem).State = EntityState.Added;
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

        private static IEnumerable<T> GetSingleton<T>(ref List<T> entities, IQueryable<T> query) where T : class
        {
            if (entities != null) return entities;
            entities = query.ToList();

            var view = CollectionViewSource.GetDefaultView(entities) as IEditableCollectionView;
            if (view != null) view.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtBeginning;

            return entities;
        }
    }
}
