using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsPinDemo {
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CorrectPinPage : ContentPage
	{
		public CorrectPinPage ()
		{
			InitializeComponent ();
            BindingContext = new CorrectPinViewModel();
		}
	}

    public class CorrectPinViewModel {
        public CorrectPinViewModel() {
            NavigateBackCommand = new Command(NavigateBack);
        }

        public ICommand NavigateBackCommand { get; }

        private void NavigateBack() {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}