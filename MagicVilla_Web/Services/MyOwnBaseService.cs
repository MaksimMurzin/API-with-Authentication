using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Services
{
    /// steps to reproduce a service,  <summary>
    /// 1) Make a new Http Client to send a request
    /// 
    /// 2) Make a http message, add accept header as "json
    /// 3) 
    /// </summary>
    public class MyOwnBaseService : IBaseService
    {
        //public APIResponse responseModel { get; set; }
        //private readonly IHttpClientFactory _httpClient;

        //public MyOwnBaseService(IHttpClientFactory httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        //public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        //{
        //    var client = _httpClient.CreateClient();
        //    var message = new HttpRequestMessage();
        //    message.Headers.Add("Accept", "application/json");
        //    message.RequestUri = new Uri(apiRequest.Url);

        //    if(apiRequest.Data != null)
        //    {
        //        message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data));
        //    }

        //    await client.SendAsync();
        //}
        
        public APIResponse responseModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            throw new NotImplementedException();
        }
    }
}
