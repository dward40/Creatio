using System;
using System.Web;
using Terrasoft.Configuration.DsnPokemonIntegrationService;
using Terrasoft.Core;
using System.Net.Http;
using System.Net;
using Terrasoft.Configuration.DsnYandexCovideService;
using Newtonsoft.Json;

namespace Terrasoft.Configuration.DsnYandexGeocoderHelper
{
    public class GeocoderHelper
    {
        //Переменные
        private readonly UserConnection userConnection;
        private readonly DsnYandexCovidApiClient _apiClient;
        public readonly DsnDataBaseClient _dbClient;


        public GeocoderHelper(UserConnection userConnection){
            this.userConnection = userConnection;
            _dbClient = new DsnDataBaseClient(userConnection);
            _apiClient = new DsnYandexCovidApiClient();
        }

        public string GetGeocoderInfoJson(string lat, string lon)
        {
            
            string apiUrlGeoCoder = _dbClient.GetGeoCoderApiUrl();
            if (apiUrlGeoCoder.Substring(apiUrlGeoCoder.Length - 1) != "=")
            {
                apiUrlGeoCoder = apiUrlGeoCoder + "=";
            }
            var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);
            httpValueCollection[""] = lon + "," +lat;
            var apiUrl = apiUrlGeoCoder + httpValueCollection.ToString().Remove(0,1);

            var result = _apiClient.GetResponseApi(apiUrl);
                       
            return result;
        }
    }
}