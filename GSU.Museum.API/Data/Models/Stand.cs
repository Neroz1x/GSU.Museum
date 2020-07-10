using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GSU.Museum.API.Data.Models
{
    public class Stand
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TitleRu { get; set; }
        public string TitleEn { get; set; }
        public string TitleBe { get; set; }
        public List<string> TextRu { get; set; }
        public List<string> TextEn { get; set; }
        public List<string> TextBe { get; set; }
        public bool State { get; set; }
        public byte[] Photo { get; set; }
        public List<Exhibit> Exhibits { get; set; }
    }
}
