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
    public partial class CounterpartyViewModel : ObservableObject
    {
        private readonly ICounterpartyRepository _counterpartyRepository;
        private readonly IDialogService _dialogService;

        private Counterparty? _selectedCounterparty;
        public Counterparty? SelectedCounterparty
        {
            get => _selectedCounterparty;
            set
            {
                if (SetProperty(ref _selectedCounterparty, value))
                {
                    ((RelayCommand)EditCounterpartyCommand).NotifyCanExecuteChanged();
                    ((RelayCommand)DeleteCounterpartyCommand).NotifyCanExecuteChanged();
                }
            }
        }
        
        public ObservableCollection<Counterparty> Counterparties { get; }

        public ICommand LoadCounterpartiesCommand { get; }
        public ICommand AddCounterpartyCommand { get; }
        public ICommand EditCounterpartyCommand { get; }
        public ICommand DeleteCounterpartyCommand { get; }

        public CounterpartyViewModel(ICounterpartyRepository counterpartyRepository, IDialogService dialogService)
        {
            _counterpartyRepository = counterpartyRepository;
            _dialogService = dialogService;

            Counterparties = new ObservableCollection<Counterparty>();

            LoadCounterpartiesCommand = new RelayCommand(async () => await LoadCounterpartiesAsync());
            AddCounterpartyCommand = new RelayCommand(async () => await AddCounterpartyAsync());
            EditCounterpartyCommand = new RelayCommand(async () => await EditCounterpartyAsync(), CanDeleteOrEdit);
            DeleteCounterpartyCommand = new RelayCommand(async () => await DeleteCounterpartyAsync(), CanDeleteOrEdit);
        }

        private async Task LoadCounterpartiesAsync()
        {
            Counterparties.Clear();
            var counterparties = await _counterpartyRepository.GetAllAsync();
            foreach (var counterparty in counterparties)
            {
                Counterparties.Add(counterparty);
            }
        }

        private async Task DeleteCounterpartyAsync()
        {
            if (SelectedCounterparty != null)
            {
                var result = await _counterpartyRepository.DeleteAsync(SelectedCounterparty.Id);
                if (result.success)
                {
                    Counterparties.Remove(SelectedCounterparty);
                }
                else
                {
                    MessageBox.Show(result.errorMessage, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task EditCounterpartyAsync()
        {
            if (SelectedCounterparty == null) return;
            
            var editorViewModel = App.Current.Services.GetRequiredService<CounterpartyEditorViewModel>();
            editorViewModel.SetCounterparty(SelectedCounterparty);

            var result = _dialogService.ShowDialog(editorViewModel);

            if (result == true && editorViewModel.Counterparty != null)
            {
                await _counterpartyRepository.UpdateAsync(editorViewModel.Counterparty);
                await LoadCounterpartiesAsync();
            }
        }

        private async Task AddCounterpartyAsync()
        {
            var editorViewModel = App.Current.Services.GetRequiredService<CounterpartyEditorViewModel>();
            
            var result = _dialogService.ShowDialog(editorViewModel);

            if (result == true && editorViewModel.Counterparty != null)
            {
                await _counterpartyRepository.AddAsync(editorViewModel.Counterparty);
                await LoadCounterpartiesAsync();
            }
        }

        private bool CanDeleteOrEdit()
        {
            return SelectedCounterparty != null;
        }
    }
} 