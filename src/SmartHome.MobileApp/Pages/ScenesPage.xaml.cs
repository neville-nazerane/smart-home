using SmartHome.MobileApp.ViewModels;

namespace SmartHome.MobileApp.Pages;

public partial class ScenesPage : ContentPage
{
    private readonly ScenesViewModel _viewModel;

    public ScenesPage(ScenesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
		await _viewModel.InitAsync();
        base.OnNavigatedTo(args);
    }

}