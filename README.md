# XamarinFormsPinDemo

This is a demo program for PIN entry in Xamarin Forms platform. Instead of creating a separate control for PIN display and entry, this solution uses the default keyboard for entering PIN. This program is inspired by Alex Dunn's implementation at https://alexdunn.org/2017/03/30/xamarin-controls-xamarin-forms-pinview/ .

In Alex's solution, the PIN is shown in 4 different small Entries. The first Entry box gets focus initially when the page is loaded. With help of Entry control's TextChanged event and a custom behavior class, when user enters a digit, control focus can move to next Entry. One of the issues in this implementation is that when user is entering PIN in the half way (i.e. entered 2 digits) and change mind to delete the previously-entered digits, the user can't delete the previous digit using backspace key and has to manually move focus to the previous Entry box.

In the solution of this repository, the PIN are actually entered into a hidden Entry and the 4 visible Entries are just for displaying the digits. Therefore, the visible Entries can be replaced with any other controls. This also solve the backspace issue mentioned above.

Firstly, create the hidden Entry and 4 visible Entry box and setup data-binding.

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             ...>   
........
    <ScrollView>
        <StackLayout Padding="30, 90, 30, 0">
            <Entry x:Name="HiddenEntry" IsVisible="False" Text="{Binding Pin}" Keyboard="Numeric" 
                HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand">
                <Entry.Behaviors>
                    <behaviors:EntryLengthValidatorBehavior MaxLength="4" />
                </Entry.Behaviors>
            </Entry>
            
            <Grid HorizontalOptions="Center" VerticalOptions="Center">
                <Entry x:Name="Entry1" WidthRequest="50" HeightRequest="50" IsPassword="True" 
                    Grid.Row="0" Grid.Column="0" HorizontalTextAlignment="Center" IsEnabled="False"
                    BindingContext="{x:Reference Name=HiddenEntry}" Margin="0"
                    Text="{Binding Path=Text, Mode=OneWay, Converter={StaticResource PinEntry1Converter}}" />

                <Entry x:Name="Entry2" WidthRequest="50" HeightRequest="50" IsPassword="True"
                    Grid.Row="0" Grid.Column="1" HorizontalTextAlignment="Center"  IsEnabled="False"
                    BindingContext="{x:Reference Name=HiddenEntry}"
                    Text="{Binding Path=Text, Mode=OneWay, Converter={StaticResource PinEntry2Converter}}" />

                <Entry x:Name="Entry3" WidthRequest="50" HeightRequest="50" IsPassword="True" 
                    Grid.Row="0" Grid.Column="2" HorizontalTextAlignment="Center" IsEnabled="False"
                    BindingContext="{x:Reference Name=HiddenEntry}"
                    Text="{Binding Path=Text, Mode=OneWay, Converter={StaticResource PinEntry3Converter}}" />

                <Entry x:Name="Entry4" WidthRequest="50" HeightRequest="50" IsPassword="True" 
                    Grid.Row="0" Grid.Column="3" HorizontalTextAlignment="Center"  IsEnabled="False"
                    BindingContext="{x:Reference Name=HiddenEntry}"
                    Text="{Binding Path=Text, Mode=OneWay, Converter={StaticResource PinEntry4Converter}}" />
            </Grid>
.......
        </StackLayout>
    </ScrollView>

</ContentPage>
```

Then create the viewmodel class for data-binding.
```C#
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
                ShowsError = false;
                return false;
            }

            for (var i = 0; i < 4; ++i) {
                if (!Char.IsNumber(pin[i])) {
                    ShowsError = true;
                    return false;
                }
            }
            if (pin != "1234") {
                ShowsError = true;
                return false;
            }
            ShowsError = false;
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
```
    
And then attach the viewmodel class to the related page class.
```C#
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
```
