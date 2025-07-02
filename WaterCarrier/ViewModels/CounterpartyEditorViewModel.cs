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
    public partial class CounterpartyEditorViewModel : ObservableObject
    {
        private readonly IEmployeeRepository _employeeRepository;
        private Guid _counterpartyId;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _inn = string.Empty;

        [ObservableProperty]
        private Employee? _curator;
        
        public ObservableCollection<Employee> Employees { get; } = new();

        [ObservableProperty]
        private bool? _dialogResult;

        public Counterparty? Counterparty { get; private set; }

        public CounterpartyEditorViewModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            LoadEmployees();
        }

        private async void LoadEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            foreach(var emp in employees)
            {
                Employees.Add(emp);
            }
        }

        public void SetCounterparty(Counterparty counterparty)
        {
            _counterpartyId = counterparty.Id;
            Name = counterparty.Name;
            Inn = counterparty.Inn;
            Curator = Employees.FirstOrDefault(e => e.Id == counterparty.Curator.Id);
        }

        [RelayCommand]
        private void Save()
        {
            var (counterparty, error) = Domain.Models.Counterparty.Create(_counterpartyId, Name, Inn, Curator!);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Counterparty = counterparty;
            DialogResult = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            DialogResult = false;
        }
    }
} 