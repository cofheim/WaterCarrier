using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace WaterCarrier.Services
{
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel)
        {
            // Находим View по имени ViewModel
            // Например, EmployeeEditorViewModel -> EmployeeEditorView
            var viewModelType = typeof(TViewModel);
            var viewTypeName = viewModelType.FullName.Replace("ViewModel", "View");
            var viewType = viewModelType.Assembly.GetType(viewTypeName);

            if (viewType == null)
            {
                throw new InvalidOperationException($"Не удалось найти View для ViewModel '{viewModelType.Name}'");
            }
            
            var view = (Window)Activator.CreateInstance(viewType)!;
            view.DataContext = viewModel;
            view.Owner = System.Windows.Application.Current.MainWindow;

            return view.ShowDialog();
        }
    }
} 