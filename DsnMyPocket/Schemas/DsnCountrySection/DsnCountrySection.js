define("DsnCountrySection", ["ModalBox"], function (ModalBox) {
  return {
    entitySchemaName: "Country",
	attributes: {
	},
	messages: {
		"DataFromModal": {
					mode: Terrasoft.MessageMode.PTP,
					direction: Terrasoft.MessageDirectionType.SUBSCRIBE
				},
		"ButtonClose": {
					mode: Terrasoft.MessageMode.PTP,
					direction: Terrasoft.MessageDirectionType.SUBSCRIBE
				},
		"GetModuleInfo": {
					mode: Terrasoft.MessageMode.PTP,
					direction: Terrasoft.MessageDirectionType.SUBSCRIBE
				},
	},
    details: /**SCHEMA_DETAILS*/ {} /**SCHEMA_DETAILS*/,
    diff: /**SCHEMA_DIFF*/ [
      {
        operation: "insert",
        parentName: "ActionButtonsContainer",
        propertyName: "items",
        name: "MainContactSectionButton",
        values: {
          itemType: Terrasoft.ViewItemType.BUTTON,
          caption: { bindTo: "Resources.Strings.DsnGetCountryInfo" },
          click: { bindTo: "getInfoCountry" },
          layout: {
            column: 1,
            row: 6,
            colSpan: 1,
          },
          style: "red",
        },
      },
    ] /**SCHEMA_DIFF*/,
    methods: {
			subscribeSandboxEvents: function() {
				this.callParent(arguments);
				this.sandbox.subscribe("DataFromModal", function(arg) {
				}, this, [this.sandbox.id + "_" + "DsnCovidModule"]);
			},
			loadCovidModalBox: function() {
				var sandbox = this.sandbox;
				var config = {
					heightPixels: 420,
					widthPixels: 750
				};
				var moduleName = "DsnCovidModule";
				var moduleId = sandbox.id + "_" + moduleName;
				var renderTo = ModalBox.show(config, function() {
				});
				sandbox.loadModule(moduleName, {
					id: moduleId,
					renderTo: renderTo
				});
				document.getElementById(renderTo.id).id = "CovidModalBoxContainer";
			},
			getInfoCountry: function() {
				this.loadCovidModalBox();
			},
			onEntityInitialized: function() {
				this.callParent(arguments);
			}
    },
  };
});
