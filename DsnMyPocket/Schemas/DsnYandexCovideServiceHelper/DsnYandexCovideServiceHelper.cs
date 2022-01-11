using Common.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Terrasoft.Configuration.DsnPokemonIntegrationService;
using Terrasoft.Core;
using System.Globalization;
using System.Collections.Generic;
using Terrasoft.Configuration.DsnYaWeatherDTO;

namespace Terrasoft.Configuration.DsnYandexCovideService
{
    public class DsnYandexCovideServiceHelper
    {

        //���� ����������//
        private readonly UserConnection userConnectionHelper;
        private readonly DsnDataBaseClient _dbClient;
        private readonly DsnYandexCovidApiClient _apiClient;
        public ILog log;
        private HttpResponseMessage responseGeoCoder;
        private HttpResponseMessage responseCovidInfo;
        private HttpResponseMessage responseWeahter;

        public DsnYandexCovideServiceHelper(UserConnection userConnection)
        {
            _dbClient = new DsnDataBaseClient(userConnection);
            _apiClient = new DsnYandexCovidApiClient();
            userConnectionHelper = userConnection;


        }


        public string GetDataHelper(string lat, string lon, string date)
        {
            //����������
            string apiGeoCoder = _dbClient.GetGeoCoderApi();
            string ISOAlpha3Code = "";
            //����������� � ������� ���� ����(dddd-mm-dd)
            var resultDate = date.Substring(0, 10);


            //�������� ����������� ������ �� �����������

            responseGeoCoder = _apiClient.GetCountryInfo(apiGeoCoder, lat, lon);

            if (responseGeoCoder.StatusCode.ToString() != "OK")
            {
                return userConnectionHelper.GetLocalizableString("DsnCovidPage2", "DsnGeoCoderError");
            }

            var body = responseGeoCoder.Content;
            string responseString = body.ReadAsStringAsync().Result;
            var geoData = JsonConvert.DeserializeObject<DsnGeoCoderDTO>(responseString);
            var countryCode = geoData.response.GeoObjectCollection.featureMember[0].GeoObject.metaDataProperty.GeocoderMetaData.Address.country_code;

            if (countryCode == null)
            {
                return userConnectionHelper.GetLocalizableString("DsnCovidPage2", "DsnCountryCodeNull");
            }

            //����������� alpha 2 -> alpha 3
            RegionInfo info = new RegionInfo(countryCode);
            ISOAlpha3Code = info.ThreeLetterISORegionName;

            //�������� ������ � Covid-19 �� ������ � ����

            var covidApiUri = _dbClient.GetOxCovidApi();
            responseCovidInfo = _apiClient.GetCovidData(covidApiUri, ISOAlpha3Code, resultDate);

            if (responseCovidInfo.StatusCode.ToString() != "OK")
            {
                return userConnectionHelper.GetLocalizableString("DsnCovidPage2", "DsnCovidError");
            }

            var bodyCovid = responseCovidInfo.Content;
            string responseStringCovid = bodyCovid.ReadAsStringAsync().Result;
            var covidData = JsonConvert.DeserializeObject<DsnCovidDTO>(responseStringCovid);

            var confirmed = covidData.stringencyData.confirmed;
            var deahts = covidData.stringencyData.deaths;
            var stringency = covidData.stringencyData.stringency;

            //�� ����������� �������� ������
            var WeatherApiUri = _dbClient.GetOYaWeatherApi();
            responseWeahter = _apiClient.GetWeatherData(WeatherApiUri, lat, lon);

            if (responseWeahter.StatusCode.ToString() != "OK")
            {
                return userConnectionHelper.GetLocalizableString("DsnCovidPage2", "DsnYaWeatherError");
            }

            var bodyWeahter = responseWeahter.Content;
            string responseStringWeather = bodyWeahter.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject<DsnYWeatherDTO>(responseStringWeather);

            var temperature = weatherData.fact.temp;
            var moisture = weatherData.fact.humidity;
            var windSpeed = weatherData.fact.wind_speed;
            var weatherIconName = weatherData.fact.icon;


            var paramToJson = new Dictionary<string, string>()
                {
                    { "confirmed", confirmed.ToString()},
                    { "deahts", deahts.ToString()},
                    { "stringency", stringency.ToString()},
                    { "icon", weatherIconName},
                    { "temperature",temperature.ToString()},
                    { "windSpeed",windSpeed.ToString()},
                    { "moisture",moisture.ToString()},



                };
            var resultJson = JsonConvert.SerializeObject(paramToJson);
            return resultJson;




        }


    }
}