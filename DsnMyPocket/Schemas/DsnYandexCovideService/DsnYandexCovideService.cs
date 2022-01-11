using System.ServiceModel;
using Terrasoft.Web.Common;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System;

namespace Terrasoft.Configuration.DsnYandexCovideService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class DsnYandexCovideService: BaseService
    {


        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
                ResponseFormat = WebMessageFormat.Json)]

        //Начальная точка входа в процедуру.
        public string GetData(string lat, string lon, string date)
        {   

            DsnYandexCovideServiceHelper helper = new DsnYandexCovideServiceHelper(UserConnection);
            try
            {
                var result = helper.GetDataHelper(lat, lon, date);
                return result;
            }
            catch (Exception exs)
            {
                return "Ошибка при выполнении запроса " + exs.Message;
                throw;
            }
            
            

        }


        }
}