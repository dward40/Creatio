using System;
using Terrasoft.Core;
using System.Net;
using Terrasoft.Core.DB;
using System.IO;
using Terrasoft.Core.ImageAPI;

namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
    
    public class DsnDataBaseClient
    {
        UserConnection UserConnection;
        private ImageAPI _imageApi;
        string Type = "d7e5ab09-b2ed-45f5-8966-b69fad0c0b87";

        //Конструктор
        public DsnDataBaseClient(UserConnection userConnection)
        {
            UserConnection = userConnection;
            _imageApi = new ImageAPI(userConnection);
        }
        /// <summary>
        /// Делает поиск покемона в БД по имени
        /// </summary>
        /// <param name="name">Имя покемона</param>
        /// <returns>Возвращает id покемона</returns>
        public Guid GetPokemonId(string name)
        {

            var select = new Select(UserConnection)
                .Column("Id")
                .From("DsnPokemons")
                .Where("DsnName").IsEqual(Column.Parameter(name)) as Select;
            return select.ExecuteScalar<Guid>();
        }
        /// <summary>
        /// Делает поиск покемона в БД по имени
        /// </summary>
        /// 
        /// <returns>Возвращает ссылку на API сервиса покемонов</returns>
        public string getUriApi()
        {
            var uri = (string)Terrasoft.Core.Configuration.SysSettings.GetValue(UserConnection, "DsnUriPokemon");
            return uri;
        }
        /// <summary>
        /// Делает поиск покемона в БД по имени
        /// </summary>
        /// <param name="name">Имя покемона</param>
        /// <param name="height">высота покемона</param>
        /// <param name="weight">вес покемона</param>
        /// <param name="imgPokemon">id картинки в таблице sysImage</param>
        public void CreatePokemon(string name, int height, int weight, Guid imgPokemon)
        {
            var pokemonSchema = UserConnection.EntitySchemaManager.GetInstanceByName("DsnPokemons");
            var pokemon = pokemonSchema.CreateEntity(UserConnection);
            pokemon.SetDefColumnValues();
            pokemon.SetColumnValue("Id", Guid.NewGuid());
            pokemon.SetColumnValue("DsnName", name);
            pokemon.SetColumnValue("DsnWeight", weight);
            pokemon.SetColumnValue("DsnHeight", height);
            pokemon.SetColumnValue("DsnLookupTypeId", Type);
            pokemon.SetColumnValue("DsnPokemonPhotoId", imgPokemon);
            pokemon.Save();
        }
        /// <summary>
        /// Получает изображение по ссылке 
        /// </summary>
        /// <param name="url">ссылка на картинку</param>
        /// <returns>Возвращает поток данных в котором содержится картинка с логотипом покемона</returns>
        public MemoryStream GetProfilePhotoIdByUrl(string url)
        {
            MemoryStream imageMemoryStream;
            WebRequest imageRequest = WebRequest.Create(url);
            WebResponse webResponse = imageRequest.GetResponse();
            Stream webResponseStream = webResponse.GetResponseStream();
            imageMemoryStream = new MemoryStream();
            webResponseStream.CopyTo(imageMemoryStream);

            return imageMemoryStream;
        }
        /// <summary>
        /// Сохраняет в бд(sysImage) картинку
        /// </summary>
        /// <param name="imageMemoryStream">Полученный поток изображения</param>
        /// <param name ="imageName">Наименование картинки в БД</param>
        /// <returns>Возвращает guid сохраненной картинки </returns>
        public Guid SaveImage(Stream imageMemoryStream, string imageName)
        {
            var imageId = Guid.NewGuid();
            _imageApi.Save(imageMemoryStream, "image/png", imageName, imageId);
            return imageId;
        }
        /// <summary>
        /// Сохраняет в бд(sysImage) картинку
        /// </summary>
        /// <param  name="nameShema">Имя схемы данных</param>
        /// <param  name="namestring">Наименование локализованной строки</param>
        /// <returns>Возвращает локазизованную строку </returns>
        public string GetLocallizableString(string nameShema, string namestring) {

            string title = UserConnection.GetLocalizableString(nameShema, namestring);

            return title;
        }

    }

}