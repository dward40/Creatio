﻿using System;
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

namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
   

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class DsnPokemonIntegrationService : BaseService
    {
        ILog log = LogManager.GetLogger("API Pokemon.co");

        /// <summary>
        /// Начальная точка входа в процесс. Вызывается из секции DsnPokemonsSection.
        /// Кнопка "Добавить покемона по API"
        /// </summary>
        /// <param name="name">имя покемона</param>
        /// <returns>Возвращает результат выполнения операции.</returns>
        /// 
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
                ResponseFormat = WebMessageFormat.Json)]

        public string AddPokemon(string name)
        {
            DsnPokemonIntegrationHelper helper = new DsnPokemonIntegrationHelper(UserConnection);
            try
            {
                var result = helper.GetPokemon(name);

                return result;

            }
            catch (Exception ex)
            {
                log.Error("Ошибка при создании покемона Error: " +ex);
                return "Ошибка при создании покемона " + ex.Message;
                throw;
            }
           
        }


    }
}