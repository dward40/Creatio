using System.ServiceModel;
using Terrasoft.Web.Common;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System;
using Terrasoft.Configuration.DsnYandexGeocoderHelper;
using Common.Logging;

namespace Terrasoft.Configuration.DsnYandexGeocoderService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class YandexGeocoderService : BaseService
    {
        ILog log = LogManager.GetLogger("YandexGeocoder");

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
                ResponseFormat = WebMessageFormat.Json)]

        //Начальная точка входа в процедуру.
        public string GetGeocoderInfo(string lat, string lon)
        {

            GeocoderHelper helper = new GeocoderHelper(UserConnection);

            try
            {
                var result = helper.GetGeocoderInfoJson(lat, lon);
                log.Info($"Успешный запрос по координатам lat={lat}, lon={lon}");
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