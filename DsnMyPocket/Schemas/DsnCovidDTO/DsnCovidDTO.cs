// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

namespace Terrasoft.Configuration.DsnYandexCovideService
{ 
public class PolicyAction
{
    public string policy_type_code { get; set; }
    public string policy_type_display { get; set; }
    public string policyvalue { get; set; }
    public int policyvalue_actual { get; set; }
    public bool? flagged { get; set; }
    public bool? is_general { get; set; }
    public object notes { get; set; }
    public string flag_value_display_field { get; set; }
    public string policy_value_display_field { get; set; }
}

public class StringencyData
{
    public string date_value { get; set; }
    public string country_code { get; set; }
    public int confirmed { get; set; }
    public int deaths { get; set; }
    public double stringency_actual { get; set; }
    public double stringency { get; set; }
}

public class DsnCovidDTO
{
    public List<PolicyAction> policyActions { get; set; }
    public StringencyData stringencyData { get; set; }
}

}