using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GSU.Museum.Web.Models
{
    public class ExhibitViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TitleRu { get; set; }
        public string TitleEn { get; set; }
        public string TitleBe { get; set; }
        public string TextRu { get; set; }
        public string TextEn { get; set; }
        public string TextBe { get; set; }
        public bool State { get; set; }
        
        public List<PhotoInfo> Photos { get; set; }
    }
}
