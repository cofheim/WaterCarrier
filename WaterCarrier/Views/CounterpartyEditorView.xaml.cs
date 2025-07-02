using System.ComponentModel;
using System.Windows;
using WaterCarrier.ViewModels;

namespace WaterCarrier.Views
{
    public partial class CounterpartyEditorView : Window
    {
        public CounterpartyEditorView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is CounterpartyEditorViewModel oldVm)
            {
                oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            }
            if (e.NewValue is CounterpartyEditorViewModel newVm)
            {
                newVm.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CounterpartyEditorViewModel.DialogResult))
            {
                DialogResult = (sender as CounterpartyEditorViewModel)?.DialogResult;
            }
        }
    }
} 