using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WaterCarrier.Application.Interfaces;
using WaterCarrier.Domain.Models;
using WaterCarrier.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace WaterCarrier.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDialogService _dialogService;

        private Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (SetProperty(ref _selectedOrder, value))
                {
                    ((RelayCommand)EditOrderCommand).NotifyCanExecuteChanged();
                    ((RelayCommand)DeleteOrderCommand).NotifyCanExecuteChanged();
                }
            }
        }
        
        public ObservableCollection<Order> Orders { get; }

        public ICommand LoadOrdersCommand { get; }
        public ICommand AddOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }

        public OrderViewModel(IOrderRepository orderRepository, IDialogService dialogService)
        {
            _orderRepository = orderRepository;
            _dialogService = dialogService;

            Orders = new ObservableCollection<Order>();

            LoadOrdersCommand = new RelayCommand(async () => await LoadOrdersAsync());
            AddOrderCommand = new RelayCommand(async () => await AddOrderAsync());
            EditOrderCommand = new RelayCommand(async () => await EditOrderAsync(), CanDeleteOrEdit);
            DeleteOrderCommand = new RelayCommand(async () => await DeleteOrderAsync(), CanDeleteOrEdit);
        }

        private async Task LoadOrdersAsync()
        {
            Orders.Clear();
            var orders = await _orderRepository.GetAllAsync();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
        }

        private async Task DeleteOrderAsync()
        {
            if (SelectedOrder != null)
            {
                var result = await _orderRepository.DeleteAsync(SelectedOrder.Id);
                if (result.success)
                {
                    Orders.Remove(SelectedOrder);
                }
                else
                {
                    MessageBox.Show(result.errorMessage, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task EditOrderAsync()
        {
            if (SelectedOrder == null) return;
            
            var editorViewModel = App.Current.Services.GetRequiredService<OrderEditorViewModel>();
            editorViewModel.SetOrder(SelectedOrder);

            var result = _dialogService.ShowDialog(editorViewModel);

            if (result == true && editorViewModel.Order != null)
            {
                await _orderRepository.UpdateAsync(editorViewModel.Order);
                await LoadOrdersAsync();
            }
        }

        private async Task AddOrderAsync()
        {
            var editorViewModel = App.Current.Services.GetRequiredService<OrderEditorViewModel>();
            
            var result = _dialogService.ShowDialog(editorViewModel);

            if (result == true && editorViewModel.Order != null)
            {
                await _orderRepository.AddAsync(editorViewModel.Order);
                await LoadOrdersAsync();
            }
        }

        private bool CanDeleteOrEdit()
        {
            return SelectedOrder != null;
        }
    }
} 