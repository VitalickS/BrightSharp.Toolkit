﻿using System.Windows.Controls;
using System.Windows.Input;

namespace JetFrames.AppleJobs.Editor.Views
{
    /// <summary>
    /// Interaction logic for ModelCategories.xaml
    /// </summary>
    public partial class ModelCategories : UserControl
    {
        public ModelCategories()
        {
            InitializeComponent();
            EditorViewModel.InitGrid(dg);
        }
    }
}
