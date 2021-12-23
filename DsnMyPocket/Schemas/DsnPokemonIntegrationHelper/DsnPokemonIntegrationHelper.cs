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
namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
    



    public class DsnPokemonIntegrationHelper

    {
       //Блок переменных//
       private readonly UserConnection userConnectionHelper;
       private readonly DsnDataBaseClient _dbClient;
       private readonly DsnPokemonApiClient _apiClient;
       public ILog log;
       private readonly Guid type = DsnPokemonType.Bug;


        //Конструктор
        public DsnPokemonIntegrationHelper (UserConnection userConnection)
        {

            _dbClient = new DsnDataBaseClient(userConnection);
            _apiClient = new DsnPokemonApiClient();
            userConnectionHelper = userConnection;
        }


        /// <summary>
        /// Вспомогательная фукция, вызывает остальные методы для записи покемона
        /// </summary>
        /// <param name="name">Имя покемона</param>
        /// <returns>Возвращает результат выполнения о создании покемона</returns>
        public string HelperFunc(string name)
        {

            HttpResponseMessage result;
            DsnPokemonDTO ability;
            Guid imgGuid;
            log = LogManager.GetLogger("API Pokemon.co");


            if (String.IsNullOrEmpty(name) || (name.Contains("/") == true) || (name.Contains("\\") == true))
            {
                return userConnectionHelper.GetLocalizableString("DsnPokemonsSection", "DsnEnterCorrectName");
            };

            if (_dbClient.GetPokemonId(name) != Guid.Empty)
            {
                return userConnectionHelper.GetLocalizableString("DsnPokemonsSection", "DsnPokemonAlreadyHas");
            };

            var uri = _dbClient.getUriApi();

            try
            {
                result = _apiClient.GetResponse(uri, name);
                log.Info(uri + name);
            }
            catch (Exception)
            {
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
                    var imgUrl = ability.sprites.front_default;
                    imgGuid = _dbClient.GetProfilePhotoIdByUrl(imgUrl, name);
                }
                catch (Exception)
                {
                    log.Error("Не удалось скачать, изображение недоступно");
                    throw;
                }

                _dbClient.CreatePokemon(name, ability.height, ability.weight, type, imgGuid);
                return ("Покемон создан " + name);
            }

            return userConnectionHelper.GetLocalizableString("DsnPokemonsSection", "DsnPokemonDoenstExist");
        }
        


    }


}