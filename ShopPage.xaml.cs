using ModureVladLab7.Models;
using Plugin.LocalNotification;
namespace ModureVladLab7;

public partial class ShopPage : ContentPage
{
	public ShopPage()
	{
		InitializeComponent();
	}
    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Address;

        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };

        var shopLocation = locations?.FirstOrDefault();

        var myLocation = await Geolocation.GetLocationAsync();

        /*
        var myLocation = new Location(46.7731796289, 23.6213886738);
        // pentru Windows Machine
        */

        if (shopLocation != null && myLocation != null)
        {
            var distance = myLocation.CalculateDistance(
                shopLocation,
                DistanceUnits.Kilometers);

            if (distance < 5)
            {
                var request = new NotificationRequest
                {
                    Title = "Ai de facut cumparaturi in apropiere!",
                    Description = address,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(1)
                    }
                };

                LocalNotificationCenter.Current.Show(request);
            }

            await Map.OpenAsync(shopLocation, options);
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var shop = (Shop)BindingContext;

            if (shop == null || shop.ID == 0)
            {
                await DisplayAlert(
                    "Error",
                    "Shop not saved yet.",
                    "OK");
                return;
            }

            bool confirm = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this shop?",
                "Yes",
                "No");

            if (!confirm)
            {
                return;
            }

            await App.Database.DeleteShopAsync(shop);
            await Navigation.PopAsync();
        }

    }
}