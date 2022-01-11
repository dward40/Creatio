define("DsnTypeMovmentDetail", ["ConfigurationGrid", "ConfigurationGridGenerator", "ConfigurationGridUtilities"], function() {
	return {
		entitySchemaName: "DsnMovementType",
		attributes: {
			"IsEditable": {
				dataValueType: Terrasoft.DataValueType.BOOLEAN,
				type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
				value: true
			}
		},
		details: {},
		diff: [
				{
					"operation": "merge",
					"name": "DataGrid",
					"values": {
						"className": "Terrasoft.ConfigurationGrid",
						"generator": "ConfigurationGridGenerator.generatePartial",
						"generateControlsConfig": {"bindTo": "generatActiveRowControlsConfig"},
						"changeRow": {"bindTo": "changeRow"},
						"unSelectRow": {"bindTo": "unSelectRow"},
						"onGridClick": {"bindTo": "onGridClick"},
						"activeRowActions": [
							{
								"className": "Terrasoft.Button",
								"style": this.Terrasoft.controls.ButtonEnums.style.TRANSPARENT,
								"tag": "save",
								"markerValue": "save",
								"imageConfig": {"bindTo": "Resources.Images.SaveIcon"}
							},
							{
								"className": "Terrasoft.Button",
								"style": this.Terrasoft.controls.ButtonEnums.style.TRANSPARENT,
								"tag": "cancel",
								"markerValue": "cancel",
								"imageConfig": {"bindTo": "Resources.Images.CancelIcon"}
							},
							{
								"className": "Terrasoft.Button",
								"style": this.Terrasoft.controls.ButtonEnums.style.TRANSPARENT,
								"tag": "remove",
								"markerValue": "remove",
								"imageConfig": {"bindTo": "Resources.Images.RemoveIcon"}
							}
						],
						"initActiveRowKeyMap": {"bindTo": "initActiveRowKeyMap"},
						"activeRowAction": {"bindTo": "onActiveRowAction"},
						"multiSelect": false
					}
				}
			],
		mixins: {
			ConfigurationGridUtilites: "Terrasoft.ConfigurationGridUtilities",
			OrderUtilities: "Terrasoft.OrderUtilities"
		},
		methods: {}
	};
});