namespace Terrasoft.Configuration.DsnCheckRole
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

            var sysAdmin = "83A43EBC-F36B-1410-298D-001E8C82BCAD";
            var result = "";

            Select select = (Select)new Select(UserConnection)
                .Column("Id")
                .From("SysUserInRole")
                .Where("SysUserId").IsEqual(Column.Parameter(currentUser))
                .And("SysRoleId").IsEqual(Column.Parameter(sysAdmin)) as Select;


            result = select.ExecuteScalar<Guid>().ToString();
            if (result == "00000000-0000-0000-0000-000000000000")
            {
                return false;
            }
            return true;
        }
    }

}
