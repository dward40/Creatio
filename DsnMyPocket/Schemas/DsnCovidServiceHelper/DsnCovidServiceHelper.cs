using System;
using System.Web;
using Terrasoft.Configuration.DsnPokemonIntegrationService;
using Terrasoft.Core;
using System.Net.Http;
using System.Net;
using Terrasoft.Configuration.DsnYandexCovideService;
using Newtonsoft.Json;

namespace Terrasoft.Configuration.DsnCovidServiceHelper
{
    public class CovidHelper
    {
        //Переменные
        private readonly UserConnection userConnection;
        private readonly DsnYandexCovidApiClient _apiClient;
        public readonly DsnDataBaseClient _dbClient;



        public CovidHelper(UserConnection userConnection)
        {
            this.userConnection = userConnection;
            _dbClient = new DsnDataBaseClient(userConnection);
            _apiClient = new DsnYandexCovidApiClient();
        }

        public string GetCovidInfoJson(string countryCode, DateTime date)
        {
            
            var dateformat = date.ToString("yyyy-MM-dd");
            string apiUrlCovid = _dbClient.GetOxCovidApiUrl();
            if (apiUrlCovid.Substring(apiUrlCovid.Length - 1) != "/")
            {
                apiUrlCovid = apiUrlCovid + "/";
            }
            var builder = new UriBuilder(apiUrlCovid + countryCode + "/" + dateformat);
            string ApiUrl = builder.ToString();
            var result = _apiClient.GetResponseApi(ApiUrl);

            return result;
        }
    }
}