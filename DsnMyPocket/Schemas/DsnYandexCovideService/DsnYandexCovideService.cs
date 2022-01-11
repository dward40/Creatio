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

        //Íà÷àëüíàÿ òî÷êà âõîäà â ïðîöåäóðó.
        public string GetData(string lat, string lon, string date)
        {   
            
            DsnYandexCovideServiceHelper helper = new DsnYandexCovideServiceHelper(UserConnection);
            try
            {
                var result = helper.GetDataHelper(lat, lon, date);
                return result;
            }
            // exs - не критично, но глаз режет
            catch (Exception exs)
            {
                //Русский язык поехал
                return "Îøèáêà ïðè âûïîëíåíèè çàïðîñà " + exs.Message;
                //Код после return не отработает
                throw;
            }
            
            

        }


        }
}
