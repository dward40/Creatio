// Исправить отступы
define("DsnPokemonsSection", ["RightUtilities", "ServiceHelper", "ProcessModuleUtilities", "css!DsnCSSButton"],function(RightUtilities, ServiceHelper, ProcessModuleUtilities) {
	return {
		entitySchemaName: "DsnPokemons",
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,		
		messages: {
			"CountDetail": {
				mode: Terrasoft.MessageMode.PTP,
				direction: Terrasoft.MessageDirectionType.PUBLISH
			},
		},
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "PokemonCountMsg",
				"values": {
					"itemType": Terrasoft.ViewItemType.BUTTON,
					"caption": "Посчитать кол-во записей в детали (сообщения)",
					"click": {
						"bindTo": "CountDetail"
					},
					"enabled": true,
					"classes": {
						"textClass": [
							"test2"
						],
						"wrapperClass": [
							"test2"
						]
					},
					"style": "green"
				},
				"parentName": "CombinedModeActionButtonsCardLeftContainer",
				"propertyName": "items",
				"index": 3
			},
			            {
                /* Выполняется операция добавления элемента на страницу. */
                "operation": "insert",
                /* Мета-имя родительского контейнера, в который добавляется кнопка. */
                "parentName": "SeparateModeActionButtonsContainer",
                /* Кнопка добавляется в коллекцию элементов родительского элемента. */
                "propertyName": "items",
                /* Мета-имя добавляемой кнопки. */
                "name": "PokemonCountESQ",
                /* Свойства, передаваемые в конструктор элемента. */
                "values": {
                    /* Тип добавляемого элемента — кнопка. */
                    "itemType": Terrasoft.ViewItemType.BUTTON,
                    /* Привязка заголовка кнопки к локализуемой строке схемы. */
                    "caption": {bindTo: "Resources.Strings.DsnResultValue"},
                    /* Привязка метода-обработчика нажатия кнопки. */
                    "click": {bindTo: "CountPokemonSubscribe"},
                    /* Привязка свойства доступности кнопки. */
                    "enabled": true,
                    /* Стиль отображения кнопки. */
					"classes": {
						"textClass": ["test2"],
						"wrapperClass": ["test2"]
					},
                    "style": Terrasoft.controls.ButtonEnums.style.GREEN
					
                }			
            }
		]/**SCHEMA_DIFF*/,
		methods: {
		CountDetail: function(){
			var id = this.get("Id");
			console.log(id);
			var count = this.sandbox.publish("CountDetail", {}, ["CountDetail"]);
			// В коммите не должно быть кусков кода, оставшихся с дебаггинга
			// В качестве исключения могут быть какие-нибудь куски кода, которые в данный момент не должны попасть на прод, но в следующем релизе будут нужны 
			//debugger;
			Terrasoft.showInformation("В любимчиках у " +count + " людей"); 
		},
			
		init: function() {
            this.callParent(arguments);
            const operationNames = ["DsnAddPokemonApi"];
            RightUtilities.checkCanExecuteOperations(operationNames, function(result) {
            this.set("AddButtonPokemonApi", result.DsnAddPokemonApi);
            }, this);
              
        },		
		openApiWindow: function(){
			
		var controlConfig = {
			PokemonName: {
			customConfig: {
				className: "Terrasoft.TextEdit",
			},
			dataValueType: Terrasoft.DataValueType.TEXT,
			// Использовать локализуемую строку вместо русского текста 
			caption: "Введите имя:",
			value: "",
		},
			
	};
		Terrasoft.utils.inputBox(
	// Использовать локализуемую строку вместо русского текста 		
    	"Имя покемона",
			
    function(returnCode, controlData) {
            if (returnCode === "ok") {
				
				var name = controlData.PokemonName.value;
				var serviceData = {
					name: name
				};
				var config = {
						serviceName: "DsnPokemonIntegrationService",
						methodName: "GetPokemonInfo",
						timeout: 100000,
						callback: function(response){this.showInformationDialog(response.GetPokemonInfoResult)},
						data: serviceData,
						scope: this
				};
				ServiceHelper.callService(config);
            }
    },
			
    ["ok", "cancel"],
			
    this,
 	controlConfig,
    {defaultButton: 0,});	
				
		},
			
		getSectionActions: function() {
		var actionMenuItems = this.callParent(arguments);
	
	actionMenuItems.addItem(this.getButtonMenuItem({
		"Click":   {"bindTo": "openApiWindow"},
		"Caption": {"bindTo": "Resources.Strings.DsnAddPokemonApi"},
		"Enabled": {"bindTo": "AddButtonPokemonApi"},
		"Visible": true,
	}));
	return actionMenuItems;
},		
		CountPokemonSubscribe: function(){
					var activeRow = this.get("ActiveRow");
					var pokemon = this.get("GridData").get(activeRow).get("DsnName");
					var select = Ext.create("Terrasoft.EntitySchemaQuery",
				{
					rootSchemaName: "DsnFavoritePokemonDatail"
				});
				
				select.addColumn("DsnLookupPokemon");						 
				select.filters.add("PokemonName", Terrasoft.createColumnFilterWithParameter(
				Terrasoft.ComparisonType.EQUAL, "DsnLookupPokemon.DsnName", pokemon));
   				select.getEntityCollection(function (result) {
          			if (!result.success) {
					// Использовать локализуемую строку вместо русского текста 
             		this.showInformationDialog("Ошибка запроса данных");
             		return;
          }

           var count = 0;
           result.collection.each(function (item) {
             count++;
           });
             this.showInformationDialog(count);
   }, this);

		}
	}
	};
});
