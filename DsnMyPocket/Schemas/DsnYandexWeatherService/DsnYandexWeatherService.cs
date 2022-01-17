using System.ServiceModel;
using Terrasoft.Web.Common;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System;
using Common.Logging;

namespace Terrasoft.Configuration.YandexWeatherService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class DsnYandexWeatherService : BaseService
    {
        ILog log = LogManager.GetLogger("YandexWeatherApi");

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
                ResponseFormat = WebMessageFormat.Json)]

        //Начальная точка входа в процедуру.
        public string GetWeatherdInfoJson(string lat, string lon)
        {

            DsnYandexWeatherServiceHelper helper = new DsnYandexWeatherServiceHelper(UserConnection);
            try
            {
                var result = helper.GetWeatherdInfoJson(lat, lon);
                log.Info($"Успешный запрос погоды по координатам lat={lat}, lon={lon}");
                return result;
            }
            catch (Exception error)
            {
                log.Error($"Error:  {error.Message}");
                throw;
            }



        }


    }
}