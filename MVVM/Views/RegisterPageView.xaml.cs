using SepidMelk.MVVM.ViewModel;

namespace SepidMelk.MVVM.Views;

public partial class RegisterPageView : ContentPage
{
	public RegisterPageView()
	{
		InitializeComponent();
		BindingContext = new RegisterPageViewModel();
	}

  

  
}