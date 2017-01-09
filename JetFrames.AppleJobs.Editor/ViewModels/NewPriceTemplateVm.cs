using AppleJobs.Data.Models.ModelsJobs;
using BrightSharp.Commands;
using BrightSharp.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace JetFrames.AppleJobs.Editor.ViewModels
{
    public class NewPriceTemplateVm : ObservableObject
    {
        private readonly IEnumerable<ModelJobPriceTemplate> modelJobPriceTemplates;
        private readonly IEnumerable<ModelJob> modelJobs;

        public NewPriceTemplateVm(IEnumerable<Model> models, IEnumerable<ModelJob> modelJobs, IEnumerable<ModelJobPriceTemplate> modelJobPriceTemplates,
            Action<NewPriceTemplateVm> createFunc)
        {
            this.modelJobs = modelJobs;
            this.modelJobPriceTemplates = modelJobPriceTemplates;
            Models = models;
            CreateCommand = new RelayCommand(p => { createFunc?.Invoke(this); RaisePropertyChanged(nameof(FreeModelJobs)); }, p => SelectedModelJob != null);
        }

        private Model _selectedModel;
        public Model SelectedModel
        {
            get { return _selectedModel; }
            set { _selectedModel = value; RaisePropertyChanged(nameof(SelectedModel)); RaisePropertyChanged(nameof(FreeModelJobs)); }
        }

        public IEnumerable<Model> Models { get; private set; }

        public IEnumerable<ModelJob> FreeModelJobs
        {
            get
            {
                if (SelectedModel == null) return null;
                var res = modelJobs.Where(mj => mj.Model.Id == SelectedModel.Id && modelJobPriceTemplates.All(p => p.ModelJobs_Id != mj.Id));
                return res;
            }
        }

        private ModelJob _selectedModelJob;
        public ModelJob SelectedModelJob
        {
            get { return _selectedModelJob; }
            set { _selectedModelJob = value; RaisePropertyChanged(nameof(SelectedModelJob)); }
        }


        private int? _newPrice;
        public int? NewPrice
        {
            get { return _newPrice; }
            set { _newPrice = value; RaisePropertyChanged(nameof(NewPrice)); }
        }

        public ICommand CreateCommand
        {
            get; set;
        }
    }
}
