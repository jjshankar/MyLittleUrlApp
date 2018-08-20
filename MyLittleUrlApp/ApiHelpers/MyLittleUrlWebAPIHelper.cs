using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyLittleUrlApp.Models;

namespace MyLittleUrlApp.ApiHelpers
{
    public class MyLittleUrlWebAPIHelper
    {
        private static HttpClient _httpClient;
        private static string _serviceAddressUri;

        public static HttpClient GetApiClient()
        {
            HttpClient httpClient;

            // Read from config file
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _serviceAddressUri = configBuilder.Build().GetValue<string>("ServiceAddressUri");

            // SSL
            if (_isSSL && RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                httpClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                });
            }
            else
            {
                httpClient = new HttpClient();
            }

            // Set SecurityProtocols
            if(_isSSL)
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            httpClient.BaseAddress = new Uri(_serviceAddressUri);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        public MyLittleUrlWebAPIHelper()
        {
            if (_httpClient == null)
                _httpClient = GetApiClient();
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
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<string> GetUrlByKey(string key)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/littleurl/" + key);
                //httpResponse.EnsureSuccessStatusCode();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {

                    // LittleUrl newItem = null;
                    // newItem = await httpResponse.Content.ReadAsAsync<LittleUrl>();
                    // return SerializeAsJson(newItem);

                    string sRet = await httpResponse.Content.ReadAsStringAsync();
                    return sRet;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> CreateNewUrl(string longUrl)
        {
            try
            {
                LittleUrl newItem = new LittleUrl { LongUrl = longUrl };
                HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("api/littleurl", newItem);

                if (httpResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    Exception noServiceEx = new Exception(httpResponse.ReasonPhrase);
                    throw noServiceEx;
                }

                httpResponse.EnsureSuccessStatusCode();
                //newItem = await httpResponse.Content.ReadAsAsync<LittleUrl>();
                //return newItem.ShortUrl;

                string sRet = await httpResponse.Content.ReadAsStringAsync();
                return sRet;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string _SerializeAsJson(LittleUrl item)
        {
            return "{ " + String.Format("\"id\": {0}, \"longUrl\": \"{1}\", \"shortUrl\": \"{2}\"",
                    item.UrlId, item.LongUrl, item.ShortUrl) + " }";
        }

        private static bool _isSSL
        {
            get => _serviceAddressUri.StartsWith("https", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
