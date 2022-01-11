using System.Net.Http;

namespace Terrasoft.Configuration
{
    public class DsnYandexCovidApiClient
    {
        /// <summary>
        /// Получает данные о местоположении по координатам
        /// </summary>
        /// <param name="uri">Ссылка API</param>
        /// <param name="lat">Широта</param>
        /// <param name="lon">Долгота</param>
        /// <returns>Возвращает контент ответа</returns>
        public HttpResponseMessage GetCountryInfo(string uri, string lat, string lon)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(uri + lon + "," + lat).Result;
            return result;

        }

        /// <summary>
        /// Получаем данные о Covid-19 на нужную дату и по нужной стране
        /// </summary>
        /// <param name="covidApiUri">СсылкаAPI</param>
        /// <param name="countryCode">Код страны в формате Alpha3</param>
        /// <param name="date">Дата</param>
        /// <returns>Возвращает контент ответа</returns>
        public HttpResponseMessage GetCovidData(string covidApiUri, string countryCode, string date)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(covidApiUri+ countryCode + "/"+ date).Result;
            return result;
        }

        /// <summary>
        /// Получаем данные о погоде 
        /// </summary>
        /// <param name="WeatherApiUri">Ссылка API </param>
        /// <param name="lat">Широта</param>
        /// <param name="lon">Долгота</param>
        /// <returns>Возвращает контент ответа</returns>
        public HttpResponseMessage GetWeatherData(string WeatherApiUri, string lat, string lon)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Yandex-API-Key", "e2875631-62c0-4149-91f9-836ac693b63f");
            var result = httpClient.GetAsync(WeatherApiUri + "lat=" + lat + "&" + "lon=" +lon).Result;
            return result;
        }

    }

} 