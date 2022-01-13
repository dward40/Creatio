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
                //return result.StatusCode.ToString() + apiUrl;
                 
                throw new HttpResponseException(result.StatusCode.ToString() +" "+ apiUrl);

            }

            var body = result.Content;
            string responseString = body.ReadAsStringAsync().Result;

            return responseString;
        }
    }

} 