using System.ComponentModel;
using System.Windows;
using WaterCarrier.ViewModels;

namespace WaterCarrier.Views
{
    /// <summary>
    /// Interaction logic for EmployeeEditorView.xaml
    /// </summary>
    public partial class EmployeeEditorView : Window
    {
        public EmployeeEditorView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is EmployeeEditorViewModel oldVm)
            {
                oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            }
            if (e.NewValue is EmployeeEditorViewModel newVm)
            {
                newVm.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmployeeEditorViewModel.DialogResult))
            {
                DialogResult = (sender as EmployeeEditorViewModel)?.DialogResult;
            }
        }
    }
} 