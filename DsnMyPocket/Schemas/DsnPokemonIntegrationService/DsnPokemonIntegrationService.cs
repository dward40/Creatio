namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
    using System;
    using Terrasoft.Core.ImageAPI;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.ServiceModel.Activation;
    using Terrasoft.Core;
    using Terrasoft.Web.Common;
    using Terrasoft.Core.Entities;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Terrasoft.Core.Factories;
    using Terrasoft.Core.DB;
    using System.Data;
    using global::Common.Logging;

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]

    public class DsnPokemonIntegrationService : BaseService
    {
        string uri;
        HttpClient httpClient = new HttpClient();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]



		// Я бы обернул весь метод или хотя бы запрос в API
		// в try/catch для логирования ошибки, в случае ее возникновения. 
		// Как пример банальной ошибки можно привести таймаут от api покемонов.
        public string GetPokemonInfo(string name)
        {
			//У типа данных string есть метод, проверяющий строку на пустоту. Используй его, вместо name == ""
            if (name == ""|| name.Contains("/") == true)
            {
			// Использовать локализуемую строку вместо текста
                return "Ââåäèòå èìÿ ïîêåìîíà"; 
            }
            var findPokemon = findPokemonInBase(name);
			// у типа данных Guid есть свойство равное "00000000-0000-0000-0000-000000000000". Используй его
			// В данном случае, чтобы не плодить лишний отступ для большого кол-ва кода - можно использовать отрицательное условие
			// Пример:
			// if(!findPokemon.IsEmpty()) ИЛИ if(findPokemon != Guid.Empty)
			//	{
			//		return "такой уже есть";
			//	}
            if (findPokemon == "00000000-0000-0000-0000-000000000000") 
            {
				
				// Получение ссылки вынести в отдельный метод
                uri = (string)Terrasoft.Core.Configuration.SysSettings.GetValue(UserConnection, "DsnUriPokemon");

					// Вынести запрос к API в отдельный класс DsnPokemonApiClient	
                    var result = httpClient.GetAsync(uri + name).Result;
					
					// Экземпляр логгера сделать глобальным
					// Настроить конфиги NLOG (помню, что лог писался в Common.log). Должен писаться в файл PokemonIntegration.log
                    var log = LogManager.GetLogger("API Pokemon.co");
                    log.Debug(uri);
                    log.Trace(name);


                    if (result.IsSuccessStatusCode)
                    {
                        var body = result.Content;
                        string responseString = body.ReadAsStringAsync().Result;
						//Класс Root переименовать в DsnPokemonDTO (DTO - Data Transfer Object)
                        var ability = JsonConvert.DeserializeObject<Root>(responseString);
                        var weight = ability.weight;
                        var height = ability.height;
                        var imgPokemon = ability.sprites.front_default;
                        CreatePokemon(name, height, weight, imgPokemon);
						// Использовать локализуемую строку вместо текста
                        return ("Ïîêåìîí ñîçäàí " + name);
                    }
                    else
                    {
					// Использовать локализуемую строку вместо текста
                        return "Ïîêåìîíà ñ òàêèì èìåíåì íåò";
                    }
                }
				// Использовать локализуемую строку вместо текста
                return "Òàêîé ïîêåìîí ó íàñ óæå åñòü";
            }

        public void CreatePokemon(string name, int height, int weight, string imgPokemon)
        {
            var pokemonSchema = UserConnection.EntitySchemaManager.GetInstanceByName("DsnPokemons");
            var pokemon = pokemonSchema.CreateEntity(UserConnection);
            var imgGiud = SavePokemonImgToBase64(imgPokemon, name);
            pokemon.SetDefColumnValues();
			// Тут нет смысла проставлять Id, т.к. он уже проставится в методе SetDefColumnValues
			// В крайнем случае он сгенерируется на уровне БД
            pokemon.SetColumnValue("Id", Guid.NewGuid());
            pokemon.SetColumnValue("DsnName", name);
            pokemon.SetColumnValue("DsnWeight", weight);
            pokemon.SetColumnValue("DsnHeight", height);
			// Id вынести в константу 
            pokemon.SetColumnValue("DsnLookupTypeId", "d7e5ab09-b2ed-45f5-8966-b69fad0c0b87");
            pokemon.SetColumnValue("DsnPokemonPhotoId", imgGiud);
            pokemon.Save();
        }

        public Guid SavePokemonImgToBase64(string imageUrl, string name)
        {
			// Тут конечно надо использовать ImageApi
			// Если вкратце, то нужно получить memoryStream, скачав изображение
			// Пример:
			// public Guid GetProfilePhotoIdByUrl(string url, string imageName) {
			// 	var imageId = Guid.NewGuid();
			// 	WebRequest imageRequest = WebRequest.Create(url);
			// 	using (WebResponse webResponse = imageRequest.GetResponse()) {
			// 		using (Stream webResponseStream = webResponse.GetResponseStream()) {
			// 			using (var imageMemoryStream = new MemoryStream())	{
			// 				webResponseStream.CopyTo(imageMemoryStream);
			// 				_imageApi.Save(imageMemoryStream, "image/png", imageName, imageId);
			// 			}
			// 		}
			// 	}
			// 	return imageId;
			// }
            WebClient downloader = new WebClient();
            var base64 = downloader.DownloadData(imageUrl);
            var imageSchema = UserConnection.EntitySchemaManager.GetInstanceByName("SysImage");
            var newImage = imageSchema.CreateEntity(UserConnection);
            var guid = Guid.NewGuid();
            newImage.SetDefColumnValues();
            newImage.SetColumnValue("Id", guid);
            newImage.SetColumnValue("Name", name + "Logo");
            newImage.SetColumnValue("Data", base64);
            newImage.SetColumnValue("MimeType", "image/png");
            newImage.Save();

            return guid;

        }

		//Правильнее было бы назвать метод GetPokemonId (глагол + название сущности + название колонки)
		// Возвращаемый тип данных должен быть Guid, так как запрос получает именно Id
        public string findPokemonInBase(string name)
        {
            var result = "";
            var select = new Select(UserConnection)
                    .Column("Id")
                .From("DsnPokemons")
                .Where("DsnName").IsEqual(Column.Parameter(name)) as Select;
				//тут не нужен ToString(), оставь Guid
				// Чтобы не создавать отдельную переменную result можно писать return select.ExecuteScalar<Guid>();
            result = select.ExecuteScalar<Guid>().ToString();
            return result;
        }











        public class Ability2
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Ability
        {
            public Ability ability { get; set; }
            public bool is_hidden { get; set; }
            public int slot { get; set; }
        }

        public class Form
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Version
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class GameIndice
        {
            public int game_index { get; set; }
            public Version version { get; set; }
        }

        public class Move2
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class MoveLearnMethod
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class VersionGroup
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class VersionGroupDetail
        {
            public int level_learned_at { get; set; }
            public MoveLearnMethod move_learn_method { get; set; }
            public VersionGroup version_group { get; set; }
        }

        public class Move
        {
            public Move move { get; set; }
            public List<VersionGroupDetail> version_group_details { get; set; }
        }

        public class Species
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class DreamWorld
        {
            public string front_default { get; set; }
            public object front_female { get; set; }
        }

        public class Home
        {
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class OfficialArtwork
        {
            public string front_default { get; set; }
        }

        public class Other
        {
            public DreamWorld dream_world { get; set; }
            public Home home { get; set; }

            [JsonProperty("official-artwork")]
            public OfficialArtwork OfficialArtwork { get; set; }
        }

        public class RedBlue
        {
            public string back_default { get; set; }
            public string back_gray { get; set; }
            public string back_transparent { get; set; }
            public string front_default { get; set; }
            public string front_gray { get; set; }
            public string front_transparent { get; set; }
        }

        public class Yellow
        {
            public string back_default { get; set; }
            public string back_gray { get; set; }
            public string back_transparent { get; set; }
            public string front_default { get; set; }
            public string front_gray { get; set; }
            public string front_transparent { get; set; }
        }

        public class GenerationI
        {
            [JsonProperty("red-blue")]
            public RedBlue RedBlue { get; set; }
            public Yellow yellow { get; set; }
        }

        public class Crystal
        {
            public string back_default { get; set; }
            public string back_shiny { get; set; }
            public string back_shiny_transparent { get; set; }
            public string back_transparent { get; set; }
            public string front_default { get; set; }
            public string front_shiny { get; set; }
            public string front_shiny_transparent { get; set; }
            public string front_transparent { get; set; }
        }

        public class Gold
        {
            public string back_default { get; set; }
            public string back_shiny { get; set; }
            public string front_default { get; set; }
            public string front_shiny { get; set; }
            public string front_transparent { get; set; }
        }

        public class Silver
        {
            public string back_default { get; set; }
            public string back_shiny { get; set; }
            public string front_default { get; set; }
            public string front_shiny { get; set; }
            public string front_transparent { get; set; }
        }

        public class GenerationIi
        {
            public Crystal crystal { get; set; }
            public Gold gold { get; set; }
            public Silver silver { get; set; }
        }

        public class Emerald
        {
            public string front_default { get; set; }
            public string front_shiny { get; set; }
        }

        public class FireredLeafgreen
        {
            public string back_default { get; set; }
            public string back_shiny { get; set; }
            public string front_default { get; set; }
            public string front_shiny { get; set; }
        }

        public class RubySapphire
        {
            public string back_default { get; set; }
            public string back_shiny { get; set; }
            public string front_default { get; set; }
            public string front_shiny { get; set; }
        }

        public class GenerationIii
        {
            public Emerald emerald { get; set; }

            [JsonProperty("firered-leafgreen")]
            public FireredLeafgreen FireredLeafgreen { get; set; }

            [JsonProperty("ruby-sapphire")]
            public RubySapphire RubySapphire { get; set; }
        }

        public class DiamondPearl
        {
            public string back_default { get; set; }
            public object back_female { get; set; }
            public string back_shiny { get; set; }
            public object back_shiny_female { get; set; }
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class HeartgoldSoulsilver
        {
            public string back_default { get; set; }
            public object back_female { get; set; }
            public string back_shiny { get; set; }
            public object back_shiny_female { get; set; }
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class Platinum
        {
            public string back_default { get; set; }
            public object back_female { get; set; }
            public string back_shiny { get; set; }
            public object back_shiny_female { get; set; }
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class GenerationIv
        {
            [JsonProperty("diamond-pearl")]
            public DiamondPearl DiamondPearl { get; set; }

            [JsonProperty("heartgold-soulsilver")]
            public HeartgoldSoulsilver HeartgoldSoulsilver { get; set; }
            public Platinum platinum { get; set; }
        }

        public class Animated
        {
            public string back_default { get; set; }
            public object back_female { get; set; }
            public string back_shiny { get; set; }
            public object back_shiny_female { get; set; }
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class BlackWhite
        {
            public Animated animated { get; set; }
            public string back_default { get; set; }
            public object back_female { get; set; }
            public string back_shiny { get; set; }
            public object back_shiny_female { get; set; }
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class GenerationV
        {
            [JsonProperty("black-white")]
            public BlackWhite BlackWhite { get; set; }
        }

        public class OmegarubyAlphasapphire
        {
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class XY
        {
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class GenerationVi
        {
            [JsonProperty("omegaruby-alphasapphire")]
            public OmegarubyAlphasapphire OmegarubyAlphasapphire { get; set; }

            [JsonProperty("x-y")]
            public XY XY { get; set; }
        }

        public class Icons
        {
            public string front_default { get; set; }
            public object front_female { get; set; }
        }

        public class UltraSunUltraMoon
        {
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
        }

        public class GenerationVii
        {
            public Icons icons { get; set; }

            [JsonProperty("ultra-sun-ultra-moon")]
            public UltraSunUltraMoon UltraSunUltraMoon { get; set; }
        }

        public class GenerationViii
        {
            public Icons icons { get; set; }
        }

        public class Versions
        {
            [JsonProperty("generation-i")]
            public GenerationI GenerationI { get; set; }

            [JsonProperty("generation-ii")]
            public GenerationIi GenerationIi { get; set; }

            [JsonProperty("generation-iii")]
            public GenerationIii GenerationIii { get; set; }

            [JsonProperty("generation-iv")]
            public GenerationIv GenerationIv { get; set; }

            [JsonProperty("generation-v")]
            public GenerationV GenerationV { get; set; }

            [JsonProperty("generation-vi")]
            public GenerationVi GenerationVi { get; set; }

            [JsonProperty("generation-vii")]
            public GenerationVii GenerationVii { get; set; }

            [JsonProperty("generation-viii")]
            public GenerationViii GenerationViii { get; set; }
        }

        public class Sprites
        {
            public string back_default { get; set; }
            public object back_female { get; set; }
            public string back_shiny { get; set; }
            public object back_shiny_female { get; set; }
            public string front_default { get; set; }
            public object front_female { get; set; }
            public string front_shiny { get; set; }
            public object front_shiny_female { get; set; }
            public Other other { get; set; }
            public Versions versions { get; set; }
        }

        public class Stat2
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Stat
        {
            public int base_stat { get; set; }
            public int effort { get; set; }
            public Stat stat { get; set; }
        }

        public class Type2
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Type
        {
            public int slot { get; set; }
            public Type type { get; set; }
        }

        public class Root
        {
            public List<Ability> abilities { get; set; }
            public int base_experience { get; set; }
            public List<Form> forms { get; set; }
            public List<GameIndice> game_indices { get; set; }
            public int height { get; set; }
            public List<object> held_items { get; set; }
            public int id { get; set; }
            public bool is_default { get; set; }
            public string location_area_encounters { get; set; }
            public List<Move> moves { get; set; }
            public string name { get; set; }
            public int order { get; set; }
            public List<object> past_types { get; set; }
            public Species species { get; set; }
            public Sprites sprites { get; set; }
            public List<Stat> stats { get; set; }
            public List<Type> types { get; set; }
            public int weight { get; set; }
        }


    }
}




