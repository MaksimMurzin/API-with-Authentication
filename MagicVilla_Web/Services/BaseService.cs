using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient{ get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
                responseModel = new();
                _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if(apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                    break;

                };

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(apiContent);

                return response;
            }
            catch(Exception ex) 
            {
                var dto = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject(res);
                return (T)APIResponse;
            }
        }
    }
}
