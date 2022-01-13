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
using Terrasoft.Configuration.YandexWeatherService;

namespace Terrasoft.Configuration.DsnYandexCovideService
{
    public class DsnYandexCovideServiceHelper
    {

        //Блок переменных//
        private readonly UserConnection userConnection;
        public ILog log;

        public DsnYandexCovideServiceHelper(UserConnection userConnection)
        {
            this.userConnection = userConnection;
        }




        public string GetData(string lat, string lon, string date)
        {
            //Преобразуем дату в DateTime
            var dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            //Получаем код страны по API ЯндексаГеокодера
            YandexGeocoderService _yandexGeocoderService = new YandexGeocoderService();
            var json = _yandexGeocoderService.GetGeocoderInfo(lat, lon);
            var geoData = JsonConvert.DeserializeObject<DsnGeoCoderDTO>(json);
            var countryCode = geoData?.response.GeoObjectCollection?.featureMember[0].GeoObject.metaDataProperty.GeocoderMetaData.Address.country_code ?? null;

            if (countryCode == null)
            {
                return this.userConnection.GetLocalizableString("DsnCovidPage2", "DsnCountryCodeNull");
            }

            //Преобразуем alpha 2 -> alpha 3
            RegionInfo info = new RegionInfo(countryCode);
            var ISOAlpha3Code = info.ThreeLetterISORegionName;

            //Получаем данные по Covid-19
            CovidService _covidService = new CovidService();
            json = _covidService.GetCovidInfo(ISOAlpha3Code, dateTime);
            var covidData = JsonConvert.DeserializeObject<DsnCovidDTO>(json);

            var confirmed = covidData.stringencyData.confirmed;
            var deahts = covidData.stringencyData.deaths;
            var stringency = covidData.stringencyData.stringency;

            //Получаем данные Яндекс.Погода
            DsnYandexWeatherService _yandexWeatherService = new DsnYandexWeatherService();
            json = _yandexWeatherService.GetWeatherdInfoJson(lat, lon);
            var weatherData = JsonConvert.DeserializeObject<DsnYWeatherDTO>(json);

            var temperature = weatherData.fact.temp;
            var humidity = weatherData.fact.humidity;
            var windSpeed = weatherData.fact.wind_speed;
            var weatherIconName = weatherData.fact.icon;


            //Формирует JSON для отправки на клиент
            var paramToJson = new Dictionary<string, string>()
                {
                    { "confirmed", confirmed.ToString()},
                    { "deahts", deahts.ToString()},
                    { "stringency", stringency.ToString()},
                    { "icon", weatherIconName},
                    { "temperature",temperature.ToString()},
                    { "windSpeed",windSpeed.ToString()},
                    { "moisture",humidity.ToString()},



                };
            var resultJson = JsonConvert.SerializeObject(paramToJson);
            return resultJson;

        }




    }
}