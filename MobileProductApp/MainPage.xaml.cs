using System.Net.Http.Json;
using MobileProductApp.Models;

namespace MobileProductApp;

public partial class MainPage : ContentPage
{
    private readonly HttpClient _httpClient;

    public MainPage()
    {
        InitializeComponent();

        _httpClient = new HttpClient();
    }

    private async void LoadProducts_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            string apiUrl =
                "http://192.168.1.35:5000/api/Product";

            List<Product>? products =
                await _httpClient.GetFromJsonAsync<List<Product>>(apiUrl);

            if (products != null)
            {
                ProductCollectionView.ItemsSource = products;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }
}