define("DsnCovidPage2", [
  "ModalBox",
  "ServiceHelper",
  "MaskHelper",
  "css!DsnButtonColorCss",
], function (ModalBox, ServiceHelper,MaskHelper) {
  return {
    attributes: {
	  $maskIdModalBox: {},
      Confirmed: {
        dataValueType: Terrasoft.DataValueType.INTEGER,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      Deaths: {
        dataValueType: Terrasoft.DataValueType.INTEGER,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      Strigency: {
        dataValueType: Terrasoft.DataValueType.INTEGER,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      Lat: {
        dataValueType: Terrasoft.DataValueType.TEXT,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      Lon: {
        dataValueType: Terrasoft.DataValueType.TEXT,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      Tepmperatura: {
        dataValueType: Terrasoft.DataValueType.INTEGER,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      Moisture: {
        dataValueType: Terrasoft.DataValueType.INTEGER,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      WindSpeed: {
        dataValueType: Terrasoft.DataValueType.TEXT,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
        value: "",
      },
      Date: {
        dataValueType: Terrasoft.DataValueType.DATE,
        type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
      },
    },
    messages: {},
    methods: {
      onRender: function () {
        this.callParent(arguments);
		this.$Date = this.GetCurrentDate();
        this.createWeatherImg();
        this.loadAPIYmaps(this.createmap);
		
      },
	   
	   //Текущая дата
	   GetCurrentDate: function(){
			var today = new Date();
			var dd = String(today.getDate()).padStart(2, '0');
			var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
			var yyyy = today.getFullYear();
			today = dd + '.' + mm + '.' + yyyy;
			return today;
		},
		
      //Получаем необходимые данные для заполнения модального окна
      getDataServerDsnCovidYandex: function (lat, lon, date) {
        if (date == null) {
          return Terrasoft.showMessage("Необходимо заполнить дату");
        }
        var serviceData = {
          lat: lat,
          lon: lon,
          date: date,
        };
        var config = {
          serviceName: "DsnYandexCovideService",
          methodName: "GetData",
          timeout: 100000,
          callback: function (response) {
            console.log(response.GetDataResult);
            if (response.GetDataResult.includes("Ошибка")) {
              Terrasoft.showInformation(response.GetDataResult);
              return;
            }
            var objResult = JSON.parse(response.GetDataResult);

            this.$Confirmed = objResult.confirmed;
            this.$Deaths = objResult.deahts;
            this.$Strigency = objResult.confirmed;
            this.$Tepmperatura = objResult.temperature;
            this.$WindSpeed = objResult.windSpeed;
            this.$Moisture = objResult.moisture;
            this.updateWeatherImg(objResult.icon);
			Terrasoft.Mask.hide(this.$maskIdModalBox);

          },
          data: serviceData,
          scope: this,
        };
        ServiceHelper.callService(config);
      },

      //Создаем контейнер для изображения погоды
      createWeatherImg: function (icon) {
        var TempContainer = Ext.get("DsnCovidPage2RightBlockContainer");

        Ext.create("Ext.Img", {
          id: "TempImg",
          src: this.get("Resources.Strings.DsnWeatherDefault"),
          renderTo: TempContainer,
        });
      },

      //Обновляем контейнер для изображения погоды
      updateWeatherImg: function (icon) {
        document.getElementById("TempImg").src =
          "https://yastatic.net/weather/i/icons/funky/dark/" + icon + ".svg";
      },

      //Добавляем скрипт для работы Яндекс карт
      addScriptYamaps: function () {
        var sript = document.createElement("script");
        sript.src =
          "https://api-maps.yandex.ru/2.1/?apikey=ваш API-ключ&lang=ru_RU";
        document.head.appendChild(sript);
        return sript;
      },

      //Создаем карту с параметрами
      createmap: function (parentContent) {
        ymaps.ready(function () {
          var center = [61.909977497765205, 100.24431793368167];
          var map = new ymaps.Map("DsnCovidPage2MapContainerContainer", {
            center: center,
            zoom: 2,
            controls: [],
          });

          var myPlacemark = new ymaps.Placemark(
            center,
            {},
            {
              draggable: true, // метку можно перемещать
              preset: "twirl#whiteStretchyIcon",
            }
          );
          myPlacemark.events.add("dragend", function (e) {
            // Получение ссылки на объект, который был передвинут.
            var thisPlacemark = e.get("target");
            // Определение координат метки
            var coords = thisPlacemark.geometry.getCoordinates();
            parentContent.$Lat = coords[0].toString();
            parentContent.$Lon = coords[1].toString();
			parentContent.$maskIdModalBox = Terrasoft.Mask.show({selector: "#t-comp223"});
            parentContent.getDataServerDsnCovidYandex(
              parentContent.$Lat,
              parentContent.$Lon,
              parentContent.$Date
            );
          });
          map.geoObjects.add(myPlacemark); // Размещение геообъекта на карте.
        });

        ///
      },

      //Загружаем карту
      loadAPIYmaps: function (callback) {
        var parentContent = this;
        sript = this.addScriptYamaps();
        if (sript.readyState === "loaded") {
          callback(parentContent);
        } else {
          sript.onload = function () {
            callback(parentContent);
          };
        }
      },

      onCloseButtonClick: function () {
        ModalBox.close();
      },
    },
    diff: [
      {
        operation: "merge",
        name: "CloseButton",
        values: {
          click: { bindTo: "onCloseButtonClick" },
        },
      },
      //Главный контейнер
      {
        operation: "insert",
        propertyName: "items",
        name: "MainContainer",
        values: {
          wrapClass: ["main-container"],
          itemType: Terrasoft.ViewItemType.CONTAINER,
          items: [],
        },
      },
      //Левый контейнер
      {
        operation: "insert",
        propertyName: "items",
        name: "LeftBlock",
        parentName: "MainContainer",
        values: {
          wrapClass: ["left-container"],
          itemType: Terrasoft.ViewItemType.CONTAINER,
          items: [],
        },
      },
      //Центральный контейнер
      {
        operation: "insert",
        propertyName: "items",
        name: "CentrBlock",
        parentName: "MainContainer",
        values: {
          wrapClass: ["centr-container"],
          itemType: Terrasoft.ViewItemType.CONTAINER,
          items: [],
        },
      },
      //Правый контейнер
      {
        operation: "insert",
        propertyName: "items",
        name: "RightBlock",
        parentName: "MainContainer",
        values: {
          wrapClass: ["right-container"],
          itemType: Terrasoft.ViewItemType.CONTAINER,
          items: [],
        },
      },
      //Контейнер карты
      {
        operation: "insert",
        propertyName: "items",
        name: "MapContainer",
        //parentName: "MainContainer",
        values: {
          //wrapClass: ["left-container"],
          itemType: Terrasoft.ViewItemType.CONTAINER,
          items: [],
        },
      },
      // Элементы левого контейнера
      {
        operation: "insert",
        parentName: "LeftBlock",
        propertyName: "items",
        name: "Date",
        values: {
          bindTo: "Date",
        },
      },
      {
        operation: "insert",
        parentName: "LeftBlock",
        propertyName: "items",
        name: "confirmed",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnConfirmed" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "Confirmed",
        },
      },
      {
        operation: "insert",
        parentName: "LeftBlock",
        propertyName: "items",
        name: "deaths",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnDeaths" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "Deaths",
        },
      },
      {
        operation: "insert",
        parentName: "LeftBlock",
        propertyName: "items",
        name: "strigency",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnStrigency" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "Strigency",
        },
      },
      //Элементы Центрального контейнера
      {
        operation: "insert",
        parentName: "CentrBlock",
        propertyName: "items",
        name: "lat",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnLat" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "Lat",
        },
      },
      {
        operation: "insert",
        parentName: "CentrBlock",
        propertyName: "items",
        name: "lon",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnLon" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "Lon",
        },
      },
      {
        operation: "insert",
        parentName: "CentrBlock",
        propertyName: "items",
        name: "Temperature",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnTemp" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "Tepmperatura",
        },
      },
      {
        operation: "insert",
        parentName: "CentrBlock",
        propertyName: "items",
        name: "Moisture",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnMoisture" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "Moisture",
        },
      },
      {
        operation: "insert",
        parentName: "CentrBlock",
        propertyName: "items",
        name: "WindSpeed",
        values: {
          classes: {
            labelClass: ["card-content-container3"],
          },
          labelConfig: {
            caption: { bindTo: "Resources.Strings.DsnWindSpeed" },
          },
          controlConfig: {
            readonly: true,
          },
          bindTo: "WindSpeed",
        },
      },
    ],
  };
});
