using Common.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Terrasoft.Configuration.DsnPokemonIntegrationService;
using Terrasoft.Core;
using System.Globalization;
using System.Collections.Generic;
using Terrasoft.Configuration.DsnYaWeatherDTO;
using System.Net;
using System;
using Terrasoft.Configuration.DsnYandexGeocoderService;
using Terrasoft.Configuration.DsnCovidService;
using System.Web;
using System.Collections.Specialized;

namespace Terrasoft.Configuration.YandexWeatherService
{
    public class DsnYandexWeatherServiceHelper
    {

        //Переменные
        private readonly UserConnection userConnection;
        private readonly DsnYandexCovidApiClient _apiClient;
        public readonly DsnDataBaseClient _dbClient;

        public DsnYandexWeatherServiceHelper(UserConnection userConnection)
        {
            this.userConnection = userConnection;
            _dbClient = new DsnDataBaseClient(userConnection);
            _apiClient = new DsnYandexCovidApiClient();

        }

        public string GetWeatherdInfoJson(string lat, string lon)
        {
            string apiUrlYandexWeather = _dbClient.GetOYaWeatherApiUrl();
            if (apiUrlYandexWeather.Substring(apiUrlYandexWeather.Length - 1) != "?")
            {
                apiUrlYandexWeather = apiUrlYandexWeather + "?";
            }
            string ApiKey = _dbClient.GetOYandexWeatherApiKey();

            var headers = new Dictionary<string, string>();
            headers.Add("X-Yandex-API-Key", ApiKey);

            var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);
            httpValueCollection["lat"] = lat;
            httpValueCollection["lon"] = lon;
            var apiUrl = apiUrlYandexWeather + httpValueCollection.ToString();
            var result = _apiClient.GetResponseApi(apiUrl, headers);

            return result;
        }

    }
}