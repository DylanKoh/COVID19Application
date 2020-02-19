using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GloblaLibraryCOVID19;

namespace COVID19Application
{
    public class API
    {
        string loginConnection = "http://10.0.2.2:53104/FTEs/Login";
        string connection = "http://10.0.2.2:53104";
        WebClient webClient = new WebClient();
        public API()
        {
            webClient.Headers.Add("Content-Type", "application/json");
        }
        public async Task<FTEs> Login(string username, string password)
        {
            var _url = Path.Combine(loginConnection, $"?userID={username}&password={password}");
            var response = await webClient.UploadDataTaskAsync(_url, "POST", Encoding.UTF8.GetBytes(""));
            var returnResponseText = Encoding.Default.GetString(response);
            if (returnResponseText != "User cannot be found! Please check your username or password and try again!")
            {
                var returnClass = JsonConvert.DeserializeObject<FTEs>(returnResponseText);
                return returnClass;
            }
            return null;

        }
        public async Task<string> PostAsync(ContactTracing data, string url)
        {
            var _url = Path.Combine(connection, url);
            var json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var response = await webClient.UploadDataTaskAsync(_url, "POST", json);
            var returnText = Encoding.Default.GetString(response);
            return returnText;
        }
        public async Task<List<Locations>> GetLocationsAsync(string url)
        {
            var _url = Path.Combine(connection, url);
            var response = await webClient.UploadDataTaskAsync(_url, "POST", Encoding.UTF8.GetBytes(""));
            var returnList = JsonConvert.DeserializeObject<List<Locations>>(Encoding.Default.GetString(response));
            return returnList;
        }
    }
}
