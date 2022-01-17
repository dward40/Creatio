using System.ServiceModel;
using Terrasoft.Web.Common;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System;
using Terrasoft.Configuration.DsnYandexGeocoderHelper;
using Terrasoft.Configuration.DsnCovidServiceHelper;
using Common.Logging;

namespace Terrasoft.Configuration.DsnCovidService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class CovidService : BaseService
    {
        ILog log = LogManager.GetLogger("Internal testing");

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
                ResponseFormat = WebMessageFormat.Json)]

        //Начальная точка входа в процедуру.
        public string GetCovidInfo(string countryCode, DateTime date)
        {

            CovidHelper helper = new CovidHelper(UserConnection);

            try
            {  
                var result = helper.GetCovidInfoJson(countryCode, date);
                log.Info("Успешный запрос: " + result);
                return result;
            }
            catch (Exception error)
            {
                log.Error("errorr: " + error.Message);
                throw;
            }



        }


    }
}