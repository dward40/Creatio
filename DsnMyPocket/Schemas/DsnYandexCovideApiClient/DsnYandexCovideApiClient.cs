using System.Net.Http;

namespace Terrasoft.Configuration
{
    public class DsnYandexCovidApiClient
    {
        /// <summary>
        /// �������� ������ � �������������� �� �����������
        /// </summary>
        /// <param name="uri">������ API</param>
        /// <param name="lat">������</param>
        /// <param name="lon">�������</param>
        /// <returns>���������� ������� ������</returns>
        public HttpResponseMessage GetCountryInfo(string uri, string lat, string lon)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(uri + lon + "," + lat).Result;
            return result;

        }

        /// <summary>
        /// �������� ������ � Covid-19 �� ������ ���� � �� ������ ������
        /// </summary>
        /// <param name="covidApiUri">������API</param>
        /// <param name="countryCode">��� ������ � ������� Alpha3</param>
        /// <param name="date">����</param>
        /// <returns>���������� ������� ������</returns>
        public HttpResponseMessage GetCovidData(string covidApiUri, string countryCode, string date)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(covidApiUri+ countryCode + "/"+ date).Result;
            return result;
        }

        /// <summary>
        /// �������� ������ � ������ 
        /// </summary>
        /// <param name="WeatherApiUri">������ API </param>
        /// <param name="lat">������</param>
        /// <param name="lon">�������</param>
        /// <returns>���������� ������� ������</returns>
        public HttpResponseMessage GetWeatherData(string WeatherApiUri, string lat, string lon)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Yandex-API-Key", "e2875631-62c0-4149-91f9-836ac693b63f");
            var result = httpClient.GetAsync(WeatherApiUri + "lat=" + lat + "&" + "lon=" +lon).Result;
            return result;
        }

    }

} 