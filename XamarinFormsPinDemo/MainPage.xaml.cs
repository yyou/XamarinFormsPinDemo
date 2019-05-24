using System;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace XamarinFormsPinDemo {
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            BindingContext = new MainPageViewModel();

            Appearing += (object sender, EventArgs e) => {
                HiddenEntry.Focus();
            };

            this.HiddenEntry.Unfocused += (object sender, FocusEventArgs e) => {
                HiddenEntry.Focus();
                base.OnAppearing();
            };
        }
	}

    public class MainPageViewModel: BindableObject {
        public MainPageViewModel() {
            Pin = String.Empty;
            ShowsError = false;
        }

        private bool _showsError;
        public Boolean ShowsError {
            get => _showsError;
            set => SetProperty(ref _showsError, value);
        }

        private string _pin = "";
        public string Pin {
            get => _pin;
            set {                
                SetProperty(ref _pin, value);
                if (IsValidPin(value)) {
                    //PIN is correct. do whatever you want
                    Application.Current.MainPage.Navigation.PushModalAsync(new CorrectPinPage());
                } else {
                    ShowsError = (value.Length == 4);
                }
            }
        }

        private Boolean IsValidPin(string pin) {
            if (String.IsNullOrEmpty(pin) || pin.Length != 4) {
                return false;
            }

            for (var i = 0; i < 4; ++i) {
                if (!Char.IsNumber(pin[i])) {
                    return false;
                }
            }
            return true;
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
