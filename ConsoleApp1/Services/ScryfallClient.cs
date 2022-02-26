using RestSharp;
using TheFiremind.Models;
using TheFiremind.Options;

namespace TheFiremind.Services;

internal class ScryfallClient
{
    readonly RestClient restClient;
    public ScryfallClient(RestClient restClient, SettingsOptions settings)
    {
        //var 
        this.restClient = restClient;
        //this.restClient.
    }

    //internal Card GetCard(string name)
    //{

    //}
}
