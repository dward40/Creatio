define("DsnPokemons1Page", [], function() {
	return {
		entitySchemaName: "DsnPokemons",
		attributes: {},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{
			"Files": {
				"schemaName": "FileDetailV2",
				"entitySchemaName": "DsnPokemonsFile",
				"filter": {
					"masterColumn": "Id",
					"detailColumn": "DsnPokemons"
				}
			}
		}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
			
			 getPhotoSrcMethod: function() {
                /* Получает ссылку на изображение из колонки объекта. */
                var imageColumnValue = this.get("PokemonLogo");
                /* Если ссылка установлена, то метод возвращает url файла с изображением. */
                if (imageColumnValue) {
                    return this.getSchemaImageUrl(imageColumnValue);
                }
                /* Если ссылка не установлена, то возвращает изображение по умолчанию. */
                return this.Terrasoft.ImageUrlBuilder.getUrl(this.get("Resources.Images.DefaultImage"));
            },
			
			  onPhotoChange: function(photo) {
                if (!photo) {
                    this.set("UsrLogo", null);
                    return;
                }
                /* Выполняет загрузку файла в базу данных. По окончании загрузки вызывается onPhotoUploaded. */
                this.Terrasoft.ImageApi.upload({
                    file: photo,
                    onComplete: this.onPhotoUploaded,
                    onError: this.Terrasoft.emptyFn,
                    scope: this
                });
            },
			
			
		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[

            /* Метаданные для добавления на страницу записи пользовательского поля с изображением. */
            {
                /* Выполняется операция добавления элемента на страницу. */
                "operation": "insert",
                /* Мета-имя родительского контейнера, в который добавляется поле. */
                "parentName": "Header",
                /* Изображение добавляется в коллекцию элементов родительского элемента. */
                "propertyName": "items",
                /* Мета-имя добавляемого изображения. */
                "name": "PhotoContainer",
                /* Свойства, передаваемые в конструктор элемента. */
                "values": {
                    /* Тип добавляемого элемента — контейнер. */
                    "itemType": Terrasoft.ViewItemType.CONTAINER,
                    /* Имя CSS класса. */
                    "wrapClass": ["image-edit-container"],
                    /* Настройка расположения изображения. */
                    "layout": { 
                        /* Номер столбца. */
                        "column": 0, 
                        /* Номер строки. */
                        "row": 0, 
                        /* Диапазон занимаемых строк. */
                        "rowSpan": 3, 
                        /* Диапазон занимаемых столбцов. */
                        "colSpan": 3 
                    },
                    /* Массив дочерних элементов. */
                    "items": []
                }
            },
            /* Поле [UsrLogo] — поле с логотипом контрагента. */
            {
                "operation": "insert",
                "parentName": "PhotoContainer",
                "propertyName": "items",
                "name": "PokemonLogo",
                "values": {
                    /* Метод, который получает изображение по ссылке. */
                    "getSrcMethod": "getPhotoSrcMethod",
                    /* Свойство, которое определяет возможность редактирования изображения. */
                    "readonly": true,
                    /* View-генератор элемента управления. */
                    "generator": "ImageCustomGeneratorV2.generateCustomImageControl"
                }
            },
			
			{
				"operation": "insert",
				"name": "DsnName0c0524be-7490-4b38-9872-3a9f69ffc1f5",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "DsnName"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FLOAT30041ac1-ced1-456f-bdde-e852b881cc56",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "Header"
					},
					"bindTo": "DsnWeight",
					"enabled": true
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FLOATf8241463-5819-4a77-991a-95175cad6c67",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "Header"
					},
					"bindTo": "DsnHeight",
					"enabled": true
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "LOOKUP2af0a5b2-815d-41e2-b1b1-dd872e3bc89f",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 2,
						"layoutName": "Header"
					},
					"bindTo": "DsnLookupType",
					"enabled": true,
					"contentType": 5
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "NotesAndFilesTab",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.NotesAndFilesTabCaption"
					},
					"items": [],
					"order": 0
				},
				"parentName": "Tabs",
				"propertyName": "tabs",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Files",
				"values": {
					"itemType": 2
				},
				"parentName": "NotesAndFilesTab",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "NotesControlGroup",
				"values": {
					"itemType": 15,
					"caption": {
						"bindTo": "Resources.Strings.NotesGroupCaption"
					},
					"items": []
				},
				"parentName": "NotesAndFilesTab",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Notes",
				"values": {
					"bindTo": "DsnNotes",
					"dataValueType": 1,
					"contentType": 4,
					"layout": {
						"column": 0,
						"row": 0,
						"colSpan": 24
					},
					"labelConfig": {
						"visible": false
					},
					"controlConfig": {
						"imageLoaded": {
							"bindTo": "insertImagesToNotes"
						},
						"images": {
							"bindTo": "NotesImagesCollection"
						}
					}
				},
				"parentName": "NotesControlGroup",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "merge",
				"name": "ESNTab",
				"values": {
					"order": 1
				}
			}
		]/**SCHEMA_DIFF*/
	};
});
