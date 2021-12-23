namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
    using System;
    using Terrasoft.Core.ImageAPI;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.ServiceModel.Activation;
    using Terrasoft.Core;
    using Terrasoft.Web.Common;
    using Terrasoft.Core.Entities;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Terrasoft.Core.Factories;
    using Terrasoft.Core.DB;
    using System.Data;
    using global::Common.Logging;
    using System.IO;



    public class DsnPokemonIntegrationHelper

    {
       //Блок переменных//

       private readonly DsnDataBaseClient _dbClient;
       private readonly DsnPokemonApiClient _apiClient;
       public ILog log;


        //Конструктор
        public DsnPokemonIntegrationHelper (UserConnection userConnection)
        {

            _dbClient = new DsnDataBaseClient(userConnection);
            _apiClient = new DsnPokemonApiClient();

        }

        //Метод нужно переименовать в соответствии с правилами, которые я тебе кидал.
        
        /// <summary>
        /// Вспомогательная фукция, вызывает остальные методы для записи покемона
        /// </summary>
        /// <param name="name">Имя покемона</param>

        /// <returns>Возвращает результат выполнения о создании покемона</returns>
        public string HelperFunc(string name)
        {

            HttpResponseMessage result;
            DsnPokemonDTO ability;
            MemoryStream imgStream;
            log = LogManager.GetLogger("API Pokemon.co");

            

            if (String.IsNullOrEmpty(name) || (name.Contains("/") == true) || (name.Contains("\\") == true))
            {
                return _dbClient.GetLocallizableString("DsnPokemonsSection", "DsnEnterCorrectName");
            };

            //Для лучшей читаемости лучше вынести метод GetPokemonId в отдельную строку и проверять в условии результат его выполнения
            /*
                Guid pokemonId = _dbClient.GetPokemonId(name);
                if(!pokemonId.IsEmpty())
                {
                
                }
            */
            if (_dbClient.GetPokemonId(name) != Guid.Empty)
            {
                return _dbClient.GetLocallizableString("DsnPokemonsSection", "DsnPokemonAlreadyHas");
            };

            var uri = _dbClient.getUriApi();

            try
            {
                result = _apiClient.GetResponse(uri, name);
                log.Info(uri + name);
            }
            //Я бы написал блок catch так:
            /*
                catch(Exception ex)
                {
                    var result = $"Ошибка получения покемона. {ex}";
                    log.Error(result);
                    return result;
                }
            
            */
            catch (Exception)
            {
                // Код после return не сработает
                return "Ошибка соединения с API";
                log.Error(result.StatusCode);
                throw;
            }

            if (result.IsSuccessStatusCode)
            {
                var body = result.Content;
                       string responseString = body.ReadAsStringAsync().Result;
                       ability = JsonConvert.DeserializeObject<DsnPokemonDTO>(responseString);
                       
                try
                {
                    imgStream = _dbClient.GetProfilePhotoIdByUrl(ability.sprites.front_default);
                }
                catch (Exception)
                {
                    log.Error("Не удалось скачать, изображение недоступно");
                    throw;
                }
                // Логичнее сохранять картинку после сохранения покемона, потому что если в процессе сохранения покемона произойдет ошибка
                // То мы получим лишнюю картинку в SysImage 
                var imgID = _dbClient.SaveImage(imgStream, name);
                _dbClient.CreatePokemon(name, ability.height, ability.weight, imgID);
                return ("Покемон создан " + name);
            }

            return _dbClient.GetLocallizableString("DsnPokemonsSection", "DsnPokemonDoenstExist");
        }
        


    }


}
