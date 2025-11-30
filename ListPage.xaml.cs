using ModureVladLab7.Models;

namespace ModureVladLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}


    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        var shopl = (ShopList)BindingContext;
        var selectedProduct = listView.SelectedItem as Product;
        if (selectedProduct == null)
        {
            return;
        }
         
        var listProducts = await App.Database.GetListProductsRawAsync(shopl.ID);
        var lp = listProducts.FirstOrDefault(x => x.ProductID == selectedProduct.ID);

        if (lp != null)
        {
            await App.Database.DeleteListProductAsync(lp);
        }

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }




    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });

    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
}