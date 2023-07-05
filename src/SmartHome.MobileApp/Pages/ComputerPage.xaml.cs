using SmartHome.MobileApp.ViewModels;

namespace SmartHome.MobileApp.Pages;

public partial class ComputerPage : ContentPage
{
    private readonly ComputerViewModel _viewModel;

    public ComputerPage(ComputerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _viewModel.Subscribe();
        await _viewModel.SetupAsync();
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _viewModel.Unsubscribe();
        base.OnNavigatedFrom(args);
    }


}