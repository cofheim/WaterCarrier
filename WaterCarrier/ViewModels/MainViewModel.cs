using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace WaterCarrier.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject? _currentViewModel;

        private readonly IServiceScopeFactory _scopeFactory;
        private IServiceScope? _currentScope;

        public MainViewModel(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            ShowEmployeeView(); 
        }

        [RelayCommand]
        private void ShowEmployeeView()
        {
            _currentScope?.Dispose();
            _currentScope = _scopeFactory.CreateScope();
            CurrentViewModel = _currentScope.ServiceProvider.GetRequiredService<EmployeeViewModel>();
        }

        [RelayCommand]
        private void ShowCounterpartyView()
        {
            _currentScope?.Dispose();
            _currentScope = _scopeFactory.CreateScope();
            CurrentViewModel = _currentScope.ServiceProvider.GetRequiredService<CounterpartyViewModel>();
        }

        [RelayCommand]
        private void ShowOrderView()
        {
            _currentScope?.Dispose();
            _currentScope = _scopeFactory.CreateScope();
            CurrentViewModel = _currentScope.ServiceProvider.GetRequiredService<OrderViewModel>();
        }
    }
} 