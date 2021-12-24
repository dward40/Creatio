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
        /// Поиск покемона в БД по имени
        /// </summary>
        /// <param name="name">Имя покемона</param>
        /// <param name="height">высота покемона</param>
        /// <param name="weight">вес покемона</param>
        /// <param name="imgPokemon">id картинки в таблице sysImage</param>
        public void CreatePokemon(string name, int height, int weight, Guid Type, string imgUrl)
        {
            var pokemonSchema = UserConnection.EntitySchemaManager.GetInstanceByName("DsnPokemons");
            var pokemon = pokemonSchema.CreateEntity(UserConnection);
            pokemon.SetDefColumnValues();
            pokemon.SetColumnValue("Id", Guid.NewGuid());
            pokemon.SetColumnValue("DsnName", name);
            pokemon.SetColumnValue("DsnWeight", weight);
            pokemon.SetColumnValue("DsnHeight", height);
            pokemon.SetColumnValue("DsnLookupTypeId", Type);
            pokemon.Save();
            Guid imgGuid = GetProfilePhotoIdByUrl(imgUrl, name);
            pokemon.SetColumnValue("DsnPokemonPhotoId", imgGuid);
            pokemon.Save();


        }

        /// <summary>
        /// Получает изображение по ссылке и сохраняет в базу
        /// </summary>
        /// <param name="url">Ссылка на картинку</param>
        /// <param name="imageName">Наименование картинки</param>
        /// <returns>Возвращает guid картинки, которая была получена</returns>
        public Guid GetProfilePhotoIdByUrl(string url, string imageName)
        {
            var imageId = Guid.NewGuid();
            WebRequest imageRequest = WebRequest.Create(url);
            using (WebResponse webResponse = imageRequest.GetResponse())
            {
                using (Stream webResponseStream = webResponse.GetResponseStream())
                {
                    using (var imageMemoryStream = new MemoryStream())
                    {
                        webResponseStream.CopyTo(imageMemoryStream);
                        _imageApi.Save(imageMemoryStream, "image/png", imageName, imageId);
                    }
                }
            }
            return imageId;
        }

    }

}