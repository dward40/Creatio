define("DsnCovidModulePage", ["ModalBox"], function(ModalBox) {
	return {
		attributes: {
			"TestText": {
				dataValueType: Terrasoft.DataValueType.TEXT,
				type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN
			}
		},
		messages: {
			"DataFromModal": {
				mode: Terrasoft.MessageMode.PTP,
				direction: Terrasoft.MessageDirectionType.PUBLISH
			}
		},
		methods: {
			init: function(callback, scope) {
				this.callParent(arguments);
			},
			onRender: function() {
			},
			onCloseButtonClick: function() {
				this.sandbox.publish("DataFromModal", { test: this.get("TestText") }, [this.sandbox.id]);
				ModalBox.close();
			}
		},
		diff: [
			{
				"operation": "insert",
				"name": "MyContainer",
				"propertyName": "items",
				"values": {
					"itemType": Terrasoft.ViewItemType.CONTAINER,
					"items": []
				}
			},
			{
				"operation": "insert",
				"parentName": "MyContainer",
				"propertyName": "items",
				"name": "MyGridContainer",
				"values": {
					"itemType": Terrasoft.ViewItemType.GRID_LAYOUT,
					"items": []
				}
			},
			{
				"operation": "insert",
				"parentName": "MyGridContainer",
				"propertyName": "items",
				"name": "TestText",
				"values": {
					"bindTo": "TestText",
					"caption": "Test text",
					"layout": {"column": 0, "row": 0, "colSpan": 10}
				}
			},
			{
				"operation": "insert",
				"parentName": "MyGridContainer",
				"name": "CloseButton",
				"propertyName": "items",
				"values": {
					"itemType": Terrasoft.ViewItemType.BUTTON,
					"style": Terrasoft.controls.ButtonEnums.style.BLUE,
					"click": {bindTo: "onCloseButtonClick"},
					"markerValue": "CloseButton",
					"caption": "OK",
					"layout": { "column": 0, "row": 1, "colSpan": 3 }
				}
			}
		]
	};
});