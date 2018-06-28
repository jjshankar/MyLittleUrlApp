using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyLittleUrlApp.Models;

namespace MyLittleUrlApp.ApiHelpers
{
    public class MyLittleUrlWebAPIHelper
    {
        private HttpClient _httpClient;

        public MyLittleUrlWebAPIHelper()
        {
            _httpClient = new HttpClient();

            // Read from config file
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            string serviceAddressUri = configBuilder.Build().GetValue<string>("ServiceAddressUri");

            _httpClient.BaseAddress = new Uri(serviceAddressUri);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAllUrls()
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/littleurl");
                httpResponse.EnsureSuccessStatusCode();
                string sRet = await httpResponse.Content.ReadAsStringAsync();
                // Content.ReadAsAsync<LittleUrl>();
                return sRet; // SerializeAsJson(newItem);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetUrlByKey(string key)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/littleurl/" + key);
                httpResponse.EnsureSuccessStatusCode();

                // LittleUrl newItem = null;
                // newItem = await httpResponse.Content.ReadAsAsync<LittleUrl>();
                // return SerializeAsJson(newItem);

                string sRet = await httpResponse.Content.ReadAsStringAsync();
                return sRet;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateNewUrl(string longUrl)
        {
            try
            {
                LittleUrl newItem = new LittleUrl { LongUrl = longUrl };
                HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("api/littleurl", newItem);
                httpResponse.EnsureSuccessStatusCode();

                //newItem = await httpResponse.Content.ReadAsAsync<LittleUrl>();
                //return newItem.ShortUrl;

                string sRet = await httpResponse.Content.ReadAsStringAsync();
                return sRet;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string SerializeAsJson(LittleUrl item)
        {
            return "{ " + String.Format("\"id\": {0}, \"longUrl\": \"{1}\", \"shortUrl\": \"{2}\"",
                    item.Id, item.LongUrl, item.ShortUrl) + " }";
        }
    }
}
