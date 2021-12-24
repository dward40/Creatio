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
using System.Text.RegularExpressions;

namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
    



    public class DsnPokemonIntegrationHelper

    {
       //Блок переменных//
       private readonly UserConnection userConnectionHelper;
       private readonly DsnDataBaseClient _dbClient;
       private readonly DsnPokemonApiClient _apiClient;
       public ILog log;
       


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
        public string GetPokemon(string name)
        {

            HttpResponseMessage result;
            DsnPokemonDTO ability;
            Guid imgGuid;
            log = LogManager.GetLogger("API Pokemon.co");


            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                //DsnEnterCorrectName -  Введите корректное имя покемона
                return userConnectionHelper.GetLocalizableString("DsnPokemonsSection", "DsnEnterCorrectName");
            };

            if (_dbClient.GetPokemonId(name) != Guid.Empty)
            {
                //DsnPokemonAlreadyHas -  такой покемон у нас уже есть
                return userConnectionHelper.GetLocalizableString("DsnPokemonsSection", "DsnPokemonAlreadyHas");
            };

              var uri = _dbClient.getUriApi();
              result = _apiClient.GetResponse(uri, name);

            if (result.StatusCode.ToString() == "OK")
            {
                var body = result.Content;
                string responseString = body.ReadAsStringAsync().Result;
                ability = JsonConvert.DeserializeObject<DsnPokemonDTO>(responseString);
                var imgUrl = ability.sprites.front_default;
                _dbClient.CreatePokemon(name, ability.height, ability.weight, DsnPokemonType.Bug, imgUrl);

                return ("Покемон создан " + name);
            }
            else
            {
                log.Error("Ошибка обращения к URL " + new Exception());
                return result.StatusCode.ToString();
            }

        }


    }


}