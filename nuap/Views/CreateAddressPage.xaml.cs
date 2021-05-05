namespace nuap.Views
{
    using Xamarin.Forms.Maps;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAddressPage : ContentPage
    {
        public CreateAddressPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            maps.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(4.638233, -74.1412343),
                    Distance.FromKilometers(30)
                )
            );
        }
    }
}