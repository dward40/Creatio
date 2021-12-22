define("DsnPokemons1Page", [
  "ConfigurationConstants",
  "css!DsnCSSButton",
], function (ConfigurationConstants) {
  return {
    entitySchemaName: "DsnPokemons",
    attributes: {
      Hw: {
        dataValueType: Terrasoft.DataValueType.FLOAT,
        dependencies: [
          {
            columns: ["DsnWeight", "DsnHeight"],
            methodName: "calculateHW",
          },
        ],
      },
      DsnLookupType: {
        dataValueType: Terrasoft.DataValueType.LOOKUP,
        lookupListConfig: {
          filters: [
            function () {
              var filterGroup = Ext.create("Terrasoft.FilterGroup");
              filterGroup.add(
                "FirstChar",
                Terrasoft.createColumnFilterWithParameter(
                  Terrasoft.ComparisonType.NOT_START_WITH,
                  "Name",
                  "f"
                )
              );
              return filterGroup;
            },
          ],
        },
      },
      DsnContact: {
        dependencies: [
          {
            columns: ["DsnContact"],
            methodName: "findAgeContact",
          },
        ],
      },
    },
    modules: /**SCHEMA_MODULES*/ {} /**SCHEMA_MODULES*/,
    messages: {
      CountDetail: {
        mode: Terrasoft.MessageMode.PTP,
        direction: Terrasoft.MessageDirectionType.PUBLISH,
      },
    },
    details: /**SCHEMA_DETAILS*/ {
      Files: {
        schemaName: "FileDetailV2",
        entitySchemaName: "DsnPokemonsFile",
        filter: {
          masterColumn: "Id",
          detailColumn: "DsnPokemons",
        },
      },
      DsnContactInPokemon: {
        schemaName: "DsnContactDetail",
        entitySchemaName: "DsnFavoritePokemonDatail",
        filter: {
          detailColumn: "DsnLookupPokemon",
          masterColumn: "Id",
        },
      },
      DsnSchemabba61df3Detail69bf033c: {
        schemaName: "DsnSchemabba61df3Detail",
        entitySchemaName: "DsnMovmType",
        filter: {
          detailColumn: "DsnDsnPokemons",
          masterColumn: "Id",
        },
      },
    } /**SCHEMA_DETAILS*/,
    methods: {
      findAgeContact: function () {
        var contact = this.get("DsnContact").value;
        var age = 0;
        console.log(age);
        var select = Ext.create("Terrasoft.EntitySchemaQuery", {
          rootSchemaName: "Contact",
        });

        select.addColumn("Age");
        select.filters.add(
          "Id",
          Terrasoft.createColumnFilterWithParameter(
            Terrasoft.ComparisonType.EQUAL,
            "Id",
            contact
          )
        );
        select.getEntityCollection(function (result) {
          if (result.success) {
            this.age = result.collection.collection.items[0].values.Age;
          }
        }, this);
      },
      asyncValidate: function (callback, scope) {
        this.callParent([
          function (response) {
            if (!this.validateResponse(response)) {
              return;
            }
            Terrasoft.chain(
              function (next) {
                this.validateContactAge(function (response) {
                  if (this.validateResponse(response)) {
                    next();
                  }
                }, this);
              },
              function (next) {
                callback.call(scope, response);
                next();
              },
              this
            );
          },
          this,
        ]);
      },
      validateContactAge: function (callback, scope) {
        //debugger;
        var result = {
          success: true,
        };
        if (this.age < 18) {
          result.message = "Возраст контакта меньше 18 лет";
          result.success = false;
        }
        callback.call(scope || this, result);
      },
      setValidationConfig: function () {
        this.callParent(arguments);
        this.addColumnValidator("DsnWeight", this.WeightValidator);
      },
      WeightValidator: function () {
        var invalidMessage = "";
        if (this.get("DsnWeight") < 1) {
          invalidMessage = " меньше 0, заполните вес.";
        }
        return {
          // Сообщение об ошибке валидации.
          invalidMessage: invalidMessage,
        };
      },
      onEntityInitialized: function () {
        this.callParent(arguments);
        this.calculateHW();
      },
      CountDetail: function () {
        var id = this.get("Id");
        console.log(id);
        var count = this.sandbox.publish("CountDetail", {}, ["CountDetail"]);
        //debugger;
        Terrasoft.showInformation("В любимчиках у " + count + " людей");
      },
      calculateHW: function () {
        var height = this.get("DsnHeight");
        if (!height) {
          height = 0;
        }
        var weight = this.get("DsnWeight");
        if (!weight) {
          weight = 0;
        }

        var sum = height + weight;
        this.set("DsnHw", sum);
      },
      // Вызывается перед открытием диалогового окна выбора изображения.
      beforePhotoFileSelected: function () {
        return true;
      },
      // Получает изображение по ссылке.
      getPhotoSrcMethod: function () {
        // Получение ссылки на изображение из колонки объекта.
        var imageColumnValue = this.get("DsnPokemonPhoto");
        // Если ссылка установлена, то метод возвращает url файла с изображением.
        if (imageColumnValue) {
          return this.getSchemaImageUrl(imageColumnValue);
        }
        // Если ссылка не установлена, то возвращает изображение по умолчанию.
        return this.Terrasoft.ImageUrlBuilder.getUrl(
          this.get("Resources.Images.DefaultImage")
        );
      },
      // Обрабатывает изменение изображения.
      // photo — файл с изображением.
      onPhotoChange: function (photo) {
        if (!photo) {
          this.set("DsnPokemonPhoto", null);
          return;
        }
        // Выполняется загрузка файла в базу данных. По окончании загрузки вызывается onPhotoUploaded.
        this.Terrasoft.ImageApi.upload({
          file: photo,
          onComplete: this.onPhotoUploaded,
          onError: this.Terrasoft.emptyFn,
          scope: this,
        });
      },
      // Сохраняет ссылку на измененное изображение.
      // imageId — Id сохраненного файла из базы данных.
      onPhotoUploaded: function (imageId) {
        var imageData = {
          value: imageId,
          displayValue: "Image",
        };
        // Колонке изображения присваивается ссылка на изображение.
        this.set("DsnPokemonPhoto", imageData);
      },
    },
    dataModels: /**SCHEMA_DATA_MODELS*/ {} /**SCHEMA_DATA_MODELS*/,
    businessRules: /**SCHEMA_BUSINESS_RULES*/ {
      DsnWeight: {
        "0557c7ec-7d44-480e-b84f-a4b857846a57": {
          uId: "0557c7ec-7d44-480e-b84f-a4b857846a57",
          enabled: true,
          removed: false,
          ruleType: 0,
          property: 0,
          logical: 0,
          conditions: [
            {
              comparisonType: 2,
              leftExpression: {
                type: 1,
                attribute: "DsnLookupType",
              },
            },
          ],
        },
      },
      DsnHeight: {
        "1272fd3d-94de-41d2-9837-fe3272ff07c7": {
          uId: "1272fd3d-94de-41d2-9837-fe3272ff07c7",
          enabled: true,
          removed: false,
          ruleType: 0,
          property: 0,
          logical: 0,
          conditions: [
            {
              comparisonType: 2,
              leftExpression: {
                type: 1,
                attribute: "DsnLookupType",
              },
            },
          ],
        },
      },
    } /**SCHEMA_BUSINESS_RULES*/,
    diff: /**SCHEMA_DIFF*/ [
      {
        operation: "insert",
        name: "PokemonCountMsg",
        values: {
          itemType: Terrasoft.ViewItemType.BUTTON,
          caption: "Посчитать кол-во записей в детали (сообщения)",
          click: {
            bindTo: "CountDetail",
          },
          enabled: true,
          classes: {
            textClass: ["test2"],
            wrapperClass: ["test2"],
          },
          style: "green",
        },
        parentName: "ActionButtonsContainer",
        propertyName: "items",
        index: 3,
      },
      // Контейнер-обертка, в который будет размещен компонент.
      {
        // Операция добавления.
        operation: "insert",
        // Мета-имя родительского контейнера, в который добавляется компонент.
        parentName: "Header",
        // Изображение добавляется в коллекцию компонентов
        // родительского контейнера.
        propertyName: "items",
        // Мета-имя компонента схемы, над которым производится действие.
        name: "PhotoContainer",
        // Свойства, передаваемые в конструктор компонента.
        values: {
          // Тип элемента — контейнер.
          itemType: Terrasoft.ViewItemType.CONTAINER,
          // Имя CSS класса.
          wrapClass: ["image-edit-container"],
          // Размещение в родительском контейнере.
          layout: { column: 0, row: 0, rowSpan: 3, colSpan: 3 },
          // Массив дочерних элементов.
          items: [],
        },
      },
      // Поле [UsrLogo] — поле с логотипом контрагента.
      {
        operation: "insert",
        parentName: "PhotoContainer",
        propertyName: "items",
        name: "DsnPokemonPhoto",
        values: {
          // Метод, который получает изображение по ссылке.
          getSrcMethod: "getPhotoSrcMethod",
          // Метод, который вызывается при изменении изображения.
          onPhotoChange: "onPhotoChange",
          // Метод, который вызывается перед вызовом диалогового окна выбора изображения.
          beforeFileSelected: "beforePhotoFileSelected",
          // Свойство, которое определяет возможность редактирования (изменения, удаления) изображения.
          readonly: true,
          // View-генератор элемента управления.
          generator: "ImageCustomGeneratorV2.generateCustomImageControl",
        },
      },
      // Изменение расположения поля [Name].
      {
        // Операция слияния.
        operation: "merge",
        name: "Name",
        parentName: "Header",
        propertyName: "items",
        values: {
          bindTo: "Name",
          layout: {
            column: 3,
            row: 0,
            colSpan: 20,
          },
        },
      },
      // Изменение расположения поля [ModifiedBy].
      {
        operation: "merge",
        name: "ModifiedBy",
        parentName: "Header",
        propertyName: "items",
        values: {
          bindTo: "ModifiedBy",
          layout: {
            column: 3,
            row: 2,
            colSpan: 20,
          },
        },
      },
      // Изменение расположения поля [Type].
      {
        operation: "merge",
        name: "Type",
        parentName: "Header",
        propertyName: "items",
        values: {
          bindTo: "Type",
          layout: {
            column: 3,
            row: 1,
            colSpan: 20,
          },
          contentType: Terrasoft.ContentType.ENUM,
        },
      },

      {
        operation: "insert",
        name: "DsnName0c0524be-7490-4b38-9872-3a9f69ffc1f5",
        values: {
          layout: {
            colSpan: 24,
            rowSpan: 1,
            column: 0,
            row: 0,
            layoutName: "ProfileContainer",
          },
          bindTo: "DsnName",
        },
        parentName: "ProfileContainer",
        propertyName: "items",
        index: 0,
      },
      {
        operation: "insert",
        name: "LOOKUP2af0a5b2-815d-41e2-b1b1-dd872e3bc89f",
        values: {
          layout: {
            colSpan: 9,
            rowSpan: 1,
            column: 2,
            row: 0,
            layoutName: "Header",
          },
          bindTo: "DsnLookupType",
          enabled: true,
          contentType: 5,
        },
        parentName: "Header",
        propertyName: "items",
        index: 1,
      },
      {
        operation: "insert",
        name: "FLOAT30041ac1-ced1-456f-bdde-e852b881cc56",
        values: {
          layout: {
            colSpan: 9,
            rowSpan: 1,
            column: 2,
            row: 1,
            layoutName: "Header",
          },
          bindTo: "DsnWeight",
          enabled: true,
        },
        parentName: "Header",
        propertyName: "items",
        index: 2,
      },
      {
        operation: "insert",
        name: "FLOATf8241463-5819-4a77-991a-95175cad6c67",
        values: {
          layout: {
            colSpan: 9,
            rowSpan: 1,
            column: 2,
            row: 2,
            layoutName: "Header",
          },
          bindTo: "DsnHeight",
          enabled: true,
        },
        parentName: "Header",
        propertyName: "items",
        index: 3,
      },
      {
        operation: "insert",
        name: "DsnHW",
        values: {
          bindTo: "DsnHw",
          layout: {
            colSpan: 7,
            rowSpan: 1,
            column: 2,
            row: 3,
          },
        },
        parentName: "Header",
        propertyName: "items",
        index: 4,
      },
      {
        operation: "insert",
        name: "DsnContact1d872cdb-e35a-4325-a2ac-fd818d9a7007",
        values: {
          layout: {
            colSpan: 12,
            rowSpan: 1,
            column: 2,
            row: 4,
            layoutName: "Header",
          },
          bindTo: "DsnContact",
        },
        parentName: "Header",
        propertyName: "items",
        index: 5,
      },
      {
        operation: "insert",
        name: "NotesAndFilesTab",
        values: {
          caption: {
            bindTo: "Resources.Strings.NotesAndFilesTabCaption",
          },
          items: [],
          order: 0,
        },
        parentName: "Tabs",
        propertyName: "tabs",
        index: 0,
      },
      {
        operation: "insert",
        name: "Files",
        values: {
          itemType: 2,
        },
        parentName: "NotesAndFilesTab",
        propertyName: "items",
        index: 0,
      },
      {
        operation: "insert",
        name: "NotesControlGroup",
        values: {
          itemType: 15,
          caption: {
            bindTo: "Resources.Strings.NotesGroupCaption",
          },
          items: [],
        },
        parentName: "NotesAndFilesTab",
        propertyName: "items",
        index: 1,
      },
      {
        operation: "insert",
        name: "Notes",
        values: {
          bindTo: "DsnNotes",
          dataValueType: 1,
          contentType: 4,
          layout: {
            column: 0,
            row: 0,
            colSpan: 24,
          },
          labelConfig: {
            visible: false,
          },
          controlConfig: {
            imageLoaded: {
              bindTo: "insertImagesToNotes",
            },
            images: {
              bindTo: "NotesImagesCollection",
            },
          },
        },
        parentName: "NotesControlGroup",
        propertyName: "items",
        index: 0,
      },
      {
        operation: "insert",
        name: "DsnContactInPokemon",
        values: {
          itemType: 2,
          markerValue: "added-detail",
        },
        parentName: "NotesAndFilesTab",
        propertyName: "items",
        index: 2,
      },
      {
        operation: "insert",
        name: "DsnSchemabba61df3Detail69bf033c",
        values: {
          itemType: 2,
          markerValue: "added-detail",
        },
        parentName: "NotesAndFilesTab",
        propertyName: "items",
        index: 3,
      },
      {
        operation: "merge",
        name: "ESNTab",
        values: {
          order: 1,
        },
      },
    ] /**SCHEMA_DIFF*/,
  };
});
