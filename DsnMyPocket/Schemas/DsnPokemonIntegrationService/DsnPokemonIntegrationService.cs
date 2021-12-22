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

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class DsnPokemonIntegrationService : BaseService
    {
       

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
                ResponseFormat = WebMessageFormat.Json)]

        /// <summary>
        /// ��������� ����� ����� � �������. ���������� �� ������ DsnPokemonsSection.
        /// ������ "�������� �������� �� API"
        /// </summary>
        /// <param name="name">��� ��������</param>
        /// <returns>���������� ��������� ���������� ��������.</returns>
        public string AddPokemonApi(string name)
        {
            DsnPokemonIntegrationHelper helper = new DsnPokemonIntegrationHelper(UserConnection);

           var result = helper.HelperFunc(name);

            return result;

        }


    }
}