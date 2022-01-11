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
    
    // Тут, в принципе, так же как и в DsnYandexCovideService
    // структурная ошибка. Необходимо разделить логику взаимодействия
    // с covid api, геокодером и яндекс.погодой. 
    // Это 3 разные интеграции и они должны иметь 
    // собственные интеграционные сервисы и утилитные классы.
    // Возможно, стоит сделать какой-нибудь управляющий класс, который будет запускать
    // эти 3 сервиса поочередно и возвращать 1 объект с нужными данными в ответе.
    public class DsnYandexCovideServiceHelper
    {

        //Áëîê ïåðåìåííûõ//
        //Слово Helper в userConnection-е очень глаз режет, но не критично
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

        //Зачем в названии метода слово Helper?
        //Метод должен принимать дату не в строковом формате, а в DateTime
        //При передаче даты с клиента на сервер могут возникнуть сложности и ее действительно нужно передавать 
        //строкой, насколько я помню, но при получении ее нужно кастовать в DateTime и работать именно с DateTime-ом
        public string GetDataHelper(string lat, string lon, string date)
        {
            //Ïåðåìåííûå
            //Непонятно, что делает метод GetGeoCoderApi. Лучше переименовать в GetGeoCoderApiUrl
            string apiGeoCoder = _dbClient.GetGeoCoderApi();
            string ISOAlpha3Code = "";
            //Ïðåîáðàçóåì ê íóæíîìó òèïó äàòó(dddd-mm-dd)
            //Тут нужен нормальный обработчик даты, который будет переводить ее
            //в нужный формат.
            var resultDate = date.Substring(0, 10);


            //Ïîëó÷àåì íåîáõîäèìûå äàííûå ïî êîîðäèíàòàì

            responseGeoCoder = _apiClient.GetCountryInfo(apiGeoCoder, lat, lon);
            
            //Не нужен ToString, юзай HttpStatusCode.Ok из System.Net
            if (responseGeoCoder.StatusCode.ToString() != "OK")
            {
                return userConnectionHelper.GetLocalizableString("DsnCovidPage2", "DsnGeoCoderError");
            }

            var body = responseGeoCoder.Content;
            string responseString = body.ReadAsStringAsync().Result;
            var geoData = JsonConvert.DeserializeObject<DsnGeoCoderDTO>(responseString);
            //Что, если в geatureMember 0 элементов? или какой-нибудь элемент после featureMember - null?
            var countryCode = geoData.response.GeoObjectCollection.featureMember[0].GeoObject.metaDataProperty.GeocoderMetaData.Address.country_code;

            if (countryCode == null)
            {
                return userConnectionHelper.GetLocalizableString("DsnCovidPage2", "DsnCountryCodeNull");
            }

            //Ïðåîáðàçóåì alpha 2 -> alpha 3
            RegionInfo info = new RegionInfo(countryCode);
            ISOAlpha3Code = info.ThreeLetterISORegionName;

            //Ïîëó÷àåì äàííûå î Covid-19 ïî ñòðàíå è äàòå

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

            //Ïî êîîðäèíàòàì ïîëó÷àåì ïîãîäó
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
