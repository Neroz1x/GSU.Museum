using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GSU.Museum.API.Data.Models
{
    public class Exhibit
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
        
        public List<byte[]> Photos { get; set; }
    }
}
