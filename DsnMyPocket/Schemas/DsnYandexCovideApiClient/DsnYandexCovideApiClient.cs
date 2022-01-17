using Common.Logging;
using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Terrasoft.Configuration
{
    public class DsnYandexCovidApiClient
    {
        private HttpResponseMessage result;
        ILog log = LogManager.GetLogger("Internal testing");
        public string GetResponseApi(string apiUrl, Dictionary<string, string> headers = null) {

            HttpClient httpClient = new HttpClient();
            if (!headers.IsNullOrEmpty())
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            result = httpClient.GetAsync(apiUrl).Result;
            
            if (result.StatusCode != HttpStatusCode.OK)
                {

                log.Error("Error: " + $"{result.StatusCode} {apiUrl}");
                throw new WebException(result.StatusCode.ToString() +" "+ apiUrl);
                
            }
            log.Info("Successful " + $"{result.StatusCode} {apiUrl}");
            var body = result.Content;
            string responseString = body.ReadAsStringAsync().Result;

            return responseString;
        }
    }

} 