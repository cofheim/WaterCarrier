using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
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
    public partial class EmployeeViewModel : ObservableObject
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDialogService _dialogService;

        private Employee? _selectedEmployee;
        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (SetProperty(ref _selectedEmployee, value))
                {
                    ((RelayCommand)EditEmployeeCommand).NotifyCanExecuteChanged();
                    ((RelayCommand)DeleteEmployeeCommand).NotifyCanExecuteChanged();
                }
            }
        }
        
        public ObservableCollection<Employee> Employees { get; }

        public ICommand LoadEmployeesCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }

        public EmployeeViewModel(IEmployeeRepository employeeRepository, IDialogService dialogService)
        {
            _employeeRepository = employeeRepository;
            _dialogService = dialogService;

            Employees = new ObservableCollection<Employee>();

            LoadEmployeesCommand = new RelayCommand(async () => await LoadEmployeesAsync());
            AddEmployeeCommand = new RelayCommand(async () => await AddEmployeeAsync());
            EditEmployeeCommand = new RelayCommand(async () => await EditEmployeeAsync(), CanDeleteOrEdit);
            DeleteEmployeeCommand = new RelayCommand(async () => await DeleteEmployeeAsync(), CanDeleteOrEdit);
        }

        private async Task LoadEmployeesAsync()
        {
            Employees.Clear();
            var employees = await _employeeRepository.GetAllAsync();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private async Task DeleteEmployeeAsync()
        {
            if (SelectedEmployee != null)
            {
                var result = await _employeeRepository.DeleteAsync(SelectedEmployee.Id);
                if (result.success)
                {
                    Employees.Remove(SelectedEmployee);
                }
                else
                {
                    MessageBox.Show(result.errorMessage, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task EditEmployeeAsync()
        {
            if (SelectedEmployee == null) return;
            
            var editorViewModel = App.Current.Services.GetRequiredService<EmployeeEditorViewModel>();
            editorViewModel.SetEmployee(SelectedEmployee);

            var result = _dialogService.ShowDialog(editorViewModel);

            if (result == true && editorViewModel.Employee != null)
            {
                await _employeeRepository.UpdateAsync(editorViewModel.Employee);
                await LoadEmployeesAsync();
            }
        }

        private async Task AddEmployeeAsync()
        {
            var editorViewModel = App.Current.Services.GetRequiredService<EmployeeEditorViewModel>();
            
            var result = _dialogService.ShowDialog(editorViewModel);

            if (result == true && editorViewModel.Employee != null)
            {
                await _employeeRepository.AddAsync(editorViewModel.Employee);
                await LoadEmployeesAsync();
            }
        }

        private bool CanDeleteOrEdit()
        {
            return SelectedEmployee != null;
        }
    }
} 