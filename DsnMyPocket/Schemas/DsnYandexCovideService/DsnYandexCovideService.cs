using System.ServiceModel;
using Terrasoft.Web.Common;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System;
using Common.Logging;

namespace Terrasoft.Configuration.DsnYandexCovideService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class DsnYandexCovideService: BaseService
    {
        ILog log = LogManager.GetLogger("Internal testing");

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
                ResponseFormat = WebMessageFormat.Json)]

        //Начальная точка входа в процедуру.
        public string GetData(string lat, string lon, string date)
        {   

            DsnYandexCovideServiceHelper helper = new DsnYandexCovideServiceHelper(UserConnection);
            try
            {
                var result = helper.GetData(lat, lon, date);
                log.Info($"Успешный запрос данных {lat}, lon={lon}, date={date}");
                return result;
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}");
                return "Ошибка при выполнении запроса " + error.Message;
            }
            
            

        }


        }
}