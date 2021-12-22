define("ContactPageV2", ["ServiceHelper", "ProcessModuleUtilities"], function (ServiceHelper, ProcessModuleUtilities) {
  return {
    entitySchemaName: "Contact",
    attributes: {
		"isAdmin":
		{
			dataValueType: Terrasoft.DataValueType.Bool,
			value: false,
		}
	},
    modules: /**SCHEMA_MODULES*/ {} /**SCHEMA_MODULES*/,
    details: /**SCHEMA_DETAILS*/ {
      DsnPokemonDetail: {
        schemaName: "DsnContactDetail",
        entitySchemaName: "DsnFavoritePokemonDatail",
        filter: {
          detailColumn: "DsnLookupContact",
          masterColumn: "Id",
        },
      },
    } /**SCHEMA_DETAILS*/,
    businessRules: /**SCHEMA_BUSINESS_RULES*/ {} /**SCHEMA_BUSINESS_RULES*/,
    messages: {
      GetContactSex: {
        mode: Terrasoft.MessageMode.PTP,
        direction: Terrasoft.MessageDirectionType.SUBSCRIBE,
      },
    },
    methods: {
      	
	  init: function () {
        this.callParent(arguments);
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
          callback: function (response) {
			this.set("isAdmin",response.CheckAdmRoleUserResult );
			  
		  },
          data: serviceData,
          scope: this,
        };
        ServiceHelper.callService(config);
      },
      subscribeSandboxEvents: function () {
        this.callParent(arguments);

        this.sandbox.subscribe(
          "GetContactSex",
          function (args) {
            args.sex = this.$Gender;
            return args;
          },
          this,
          ["GetContactSex"]
        );
      },
    },
    dataModels: /**SCHEMA_DATA_MODELS*/ {} /**SCHEMA_DATA_MODELS*/,
    diff: /**SCHEMA_DIFF*/ [
	  { 
		operation: "merge",
        name: "Email",
        values: {
          visible: {"bindTo": "isAdmin"},
        },
      },		   
      {
        operation: "insert",
        name: "DsnPokemonDetail",
        values: {
          itemType: 2,
          markerValue: "added-detail",
        },
        parentName: "GeneralInfoTab",
        propertyName: "items",
        index: 5,
      },
      {
        operation: "merge",
        name: "JobTabContainer",
        values: {
          order: 2,
        },
      },
      {
        operation: "merge",
        name: "HistoryTab",
        values: {
          order: 5,
        },
      },
      {
        operation: "merge",
        name: "NotesAndFilesTab",
        values: {
          order: 6,
        },
      },
      {
        operation: "merge",
        name: "ESNTab",
        values: {
          order: 7,
        },
      },
  
	  
    ] /**SCHEMA_DIFF*/,
  };
});
