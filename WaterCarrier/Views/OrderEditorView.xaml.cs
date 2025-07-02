using System.ComponentModel;
using System.Windows;
using WaterCarrier.ViewModels;

namespace WaterCarrier.Views
{
    public partial class OrderEditorView : Window
    {
        public OrderEditorView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is OrderEditorViewModel oldVm)
            {
                oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            }
            if (e.NewValue is OrderEditorViewModel newVm)
            {
                newVm.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderEditorViewModel.DialogResult))
            {
                DialogResult = (sender as OrderEditorViewModel)?.DialogResult;
            }
        }
    }
} 