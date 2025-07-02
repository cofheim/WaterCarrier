namespace WaterCarrier.Services
{
    public interface IDialogService
    {
        bool? ShowDialog<TViewModel>(TViewModel viewModel);
    }
} 