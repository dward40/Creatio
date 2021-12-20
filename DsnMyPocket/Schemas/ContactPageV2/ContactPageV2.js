define("ContactPageV2", ["ServiceHelper", "ProcessModuleUtilities"], function (ServiceHelper, ProcessModuleUtilities) {
  return {
    entitySchemaName: "Contact",
    attributes: {},
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
