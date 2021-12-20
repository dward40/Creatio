 define("ContactCommunicationDetail", ["ContactCommunicationDetailResources", "terrasoft", "ServiceHelper", "ProcessModuleUtilities"],
    function(resources, Terrasoft, ServiceHelper, ProcessModuleUtilities) {
        return {
            entitySchemaName: "ContactCommunication",
            methods: {

			
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
				{
					"operation": "merge",
					"name": "CommunicationsContainer",
					"values": {
						"generateId": false,
						

					}
				},

			]/**SCHEMA_DIFF*/
        };
    });