namespace Mod08.View;
using Mod08.ViewModel;


public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
		BindingContext = new UserViewModel();
	}
}