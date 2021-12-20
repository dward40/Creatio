 define("ContactCommunicationDetail", ["ContactCommunicationDetailResources", "terrasoft", "ServiceHelper", "ProcessModuleUtilities"],
    function(resources, Terrasoft, ServiceHelper, ProcessModuleUtilities) {
        return {
            entitySchemaName: "ContactCommunication",
            methods: {
				
					
	init: function () {
        this.callParent(arguments);
		console.log(this);
		console.log("sa")
        this.checkAdminRole();
      },	
	  
	  		checkAdminRole: function () {
		var currentUser = Terrasoft.SysValue.CURRENT_USER.value;
				var serviceData = {
					currentUser: currentUser
				};
        var config = {
          serviceName: "DsnCheckRole",
          methodName: "CheckAdmRoleUser",
          timeout: 100000,
          callback: function (response) {response.CheckRoleUserResult},
          data: serviceData,
          scope: this,
        };
        ServiceHelper.callService(config);
      },
				
			
                /**
                * Возвращает запреты на использование.
                * @protected
                * @return {Object} Объект, который содержит свойства запретов на использование.
                */
                getRestrictions: function() {
                    return {                        
                        "DoNotUseCall": { /*Не использовать телефон*/
                            "RestrictCaption": this.get("Resources.Strings.DoNotUseCall"),
                            "Caption": this.get("Resources.Strings.DoNotUseCallCaption")
                        },
                        "DoNotUseSms": { /*Не использовать SMS*/
                            "RestrictCaption": this.get("Resources.Strings.DoNotUseSms"),
                            "Caption": this.get("Resources.Strings.DoNotUseSmsCaption")
                        },
                        "DoNotUseFax": { /*Не использовать факс*/
                            "RestrictCaption": this.get("Resources.Strings.DoNotUseFax"),
                            "Caption": this.get("Resources.Strings.DoNotUseFaxCaption")
                        },
                        "DoNotUseMail": { /*Не использовать почту*/
                            "RestrictCaption": this.get("Resources.Strings.DoNotUseMail"),
                            "Caption": this.get("Resources.Strings.DoNotUseMailCaption")
                        }
                    };
                }
            },
            diff: /**SCHEMA_DIFF*/[

			]/**SCHEMA_DIFF*/
        };
    });