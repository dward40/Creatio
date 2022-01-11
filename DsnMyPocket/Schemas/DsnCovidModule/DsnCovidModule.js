define("DsnCovidModule", ["ModalBox", "BaseSchemaModuleV2"],
		function(ModalBox) {
	Ext.define("Terrasoft.configuration.DsnCovidModule", {
		extend: "Terrasoft.BaseSchemaModule",
		alternateClassName: "Terrasoft.DsnCovidModule",
		/**
		 * @inheritDoc Terrasoft.BaseSchemaModule#generateViewContainerId
		 * @overridden
		 */
		generateViewContainerId: false,
		/**
		 * @inheritDoc Terrasoft.BaseSchemaModule#initSchemaName
		 * @overridden
		 */
		initSchemaName: function() {
			this.schemaName = "DsnCovidPage2";
		},
		
		getViewModel: function(){
			var viewModel = this.Ext.create("Terrasoft.baseViewModel", {
				
			});
			return viewModel;
		},
		/**
		 * @inheritDoc Terrasoft.BaseSchemaModule#initHistoryState
		 * @overridden
		 */
		initHistoryState: Terrasoft.emptyFn,
	});
	return Terrasoft.DsnCovidModule;
});