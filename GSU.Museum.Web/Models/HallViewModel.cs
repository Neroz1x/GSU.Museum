using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSU.Museum.Web.Models
{
    public class HallViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string TitleRu { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string TitleEn { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string TitleBe { get; set; }
        public PhotoInfo Photo { get; set; }
        public List<StandViewModel> Stands { get; set; }
    }
}
