using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSU.Museum.Web.Models
{
    public class ExhibitViewModel
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

        [Required(ErrorMessage = "Не указано описание")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessage = "Не указано описание")]
        public string DescriptionEn { get; set; }

        [Required(ErrorMessage = "Не указано описание")]
        public string DescriptionBe { get; set; }

        [Required(ErrorMessage = "Не указан текст")]
        public string TextRu { get; set; }

        [Required(ErrorMessage = "Не указан текст")]
        public string TextEn { get; set; }

        [Required(ErrorMessage = "Не указан текст")]
        public string TextBe { get; set; }
        
        public List<PhotoInfo> Photos { get; set; }
    }
}
