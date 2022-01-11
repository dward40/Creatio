using System.Net.Http;

namespace Terrasoft.Configuration
{
    // В ApiClient-е должен быть 1 метод Get, поскольку все 3 запроса твоих запроса - это GET запросы.
    // Параметры должны подставляться в ссылку в helper-е 
    // и ApiClient должен получать уже готовую ссылку и делать запрос.
    // Так же, если нужно, apiClient должен подставить в ссылку токен авторизации или логин/пароль
    // В зависимости от типа авториацации в api.
    // Поищи builder-ы для подставления параметров в ссылки. Сам я такими не пользовался, но уверен, что такие есть (можешь написать свой)
    // Например, если у тебя есть базовая ссылка и тебе нужно подставить в нее ширину, долгототу, то ты создаешь
    // Экземпляр класса builder-а и передаешь туда словарь с названием параметра и значением. А он возвращает готовую ссылку
    public class DsnYandexCovidApiClient
    {
        /// <summary>
        /// Ïîëó÷àåò äàííûå î ìåñòîïîëîæåíèè ïî êîîðäèíàòàì
        /// </summary>
        /// <param name="uri">Ññûëêà API</param>
        /// <param name="lat">Øèðîòà</param>
        /// <param name="lon">Äîëãîòà</param>
        /// <returns>Âîçâðàùàåò êîíòåíò îòâåòà</returns>
        public HttpResponseMessage GetCountryInfo(string uri, string lat, string lon)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(uri + lon + "," + lat).Result;
            return result;

        }

        /// <summary>
        /// Ïîëó÷àåì äàííûå î Covid-19 íà íóæíóþ äàòó è ïî íóæíîé ñòðàíå
        /// </summary>
        /// <param name="covidApiUri">ÑñûëêàAPI</param>
        /// <param name="countryCode">Êîä ñòðàíû â ôîðìàòå Alpha3</param>
        /// <param name="date">Äàòà</param>
        /// <returns>Âîçâðàùàåò êîíòåíò îòâåòà</returns>
        public HttpResponseMessage GetCovidData(string covidApiUri, string countryCode, string date)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(covidApiUri+ countryCode + "/"+ date).Result;
            return result;
        }

        /// <summary>
        /// Ïîëó÷àåì äàííûå î ïîãîäå 
        /// </summary>
        /// <param name="WeatherApiUri">Ññûëêà API </param>
        /// <param name="lat">Øèðîòà</param>
        /// <param name="lon">Äîëãîòà</param>
        /// <returns>Âîçâðàùàåò êîíòåíò îòâåòà</returns>
        public HttpResponseMessage GetWeatherData(string WeatherApiUri, string lat, string lon)
        {
            HttpClient httpClient = new HttpClient();
            //Ключ вынести в системную настройку
            httpClient.DefaultRequestHeaders.Add("X-Yandex-API-Key", "e2875631-62c0-4149-91f9-836ac693b63f");
            var result = httpClient.GetAsync(WeatherApiUri + "lat=" + lat + "&" + "lon=" +lon).Result;
            return result;
        }

    }

} 
