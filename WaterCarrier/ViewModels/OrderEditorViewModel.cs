using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WaterCarrier.Domain.Models;
using WaterCarrier.Application.Interfaces;

namespace WaterCarrier.ViewModels
{
    public partial class OrderEditorViewModel : ObservableObject
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICounterpartyRepository _counterpartyRepository;
        private Guid _orderId;

        [ObservableProperty]
        private DateTime _date = DateTime.Now;

        [ObservableProperty]
        private decimal _amount;

        [ObservableProperty]
        private Employee? _selectedEmployee;

        [ObservableProperty]
        private Counterparty? _selectedCounterparty;
        
        public ObservableCollection<Employee> Employees { get; } = new();
        public ObservableCollection<Counterparty> Counterparties { get; } = new();

        [ObservableProperty]
        private bool? _dialogResult;

        public Order? Order { get; private set; }

        public OrderEditorViewModel(IEmployeeRepository employeeRepository, ICounterpartyRepository counterpartyRepository)
        {
            _employeeRepository = employeeRepository;
            _counterpartyRepository = counterpartyRepository;
            LoadDependencies();
        }

        private async void LoadDependencies()
        {
            var employees = await _employeeRepository.GetAllAsync();
            foreach(var emp in employees) Employees.Add(emp);

            var counterparties = await _counterpartyRepository.GetAllAsync();
            foreach(var c in counterparties) Counterparties.Add(c);
        }

        public void SetOrder(Order order)
        {
            _orderId = order.Id;
            Date = order.Date;
            Amount = order.Amount;
            SelectedEmployee = Employees.FirstOrDefault(e => e.Id == order.Employee.Id);
            SelectedCounterparty = Counterparties.FirstOrDefault(c => c.Id == order.Counterparty.Id);
        }

        [RelayCommand]
        private void Save()
        {
            var (order, error) = Domain.Models.Order.Create(_orderId, Date, Amount, SelectedEmployee!, SelectedCounterparty!);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Order = order;
            DialogResult = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            DialogResult = false;
        }
    }
} 