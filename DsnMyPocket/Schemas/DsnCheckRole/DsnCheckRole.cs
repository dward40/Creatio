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

    public class DsnCheckRole: BaseService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
ResponseFormat = WebMessageFormat.Json)]

        public bool CheckAdmRoleUser(string currentUser)
        {

            Select select = (Select)new Select(UserConnection)
                .Column("Id")
                .From("SysUserInRole")
                .Where("SysUserId").IsEqual(Column.Parameter(currentUser))
                .And("SysRoleId").IsEqual(Column.Parameter(DsnUserRoles.admin)) as Select;


            Guid result = select.ExecuteScalar<Guid>();
            if (result == Guid.Empty)
            {
                return false;
            }
            return true;
        }
    }

}
