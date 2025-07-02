using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WaterCarrier.Domain.Enums;
using WaterCarrier.Domain.Models;

namespace WaterCarrier.ViewModels
{
    public partial class EmployeeEditorViewModel : ObservableObject
    {
        private Guid _employeeId;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _patronymic = string.Empty;

        [ObservableProperty]
        private DateTime _birthDate = DateTime.Now;

        [ObservableProperty]
        private Position _position;

        [ObservableProperty]
        private bool? _dialogResult;

        public Employee? Employee { get; private set; }

        public ObservableCollection<Position> Positions { get; }

        public EmployeeEditorViewModel()
        {
            Positions = new ObservableCollection<Position>(Enum.GetValues<Position>());
        }

        public void SetEmployee(Employee employee)
        {
            _employeeId = employee.Id;
            LastName = employee.LastName;
            FirstName = employee.FirstName;
            Patronymic = employee.Patronymic;
            BirthDate = employee.BirthDate;
            Position = employee.Position;
        }

        [RelayCommand]
        private void Save()
        {
            var (employee, error) = Domain.Models.Employee.Create(
                _employeeId,
                LastName,
                FirstName,
                Patronymic,
                BirthDate,
                Position
            );

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Employee = employee;
            DialogResult = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            DialogResult = false;
        }
    }
} 