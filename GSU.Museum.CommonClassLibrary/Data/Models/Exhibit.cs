using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class Exhibit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TitleRu { get; set; }
        public string TitleEn { get; set; }
        public string TitleBe { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionBe { get; set; }
        public string TextRu { get; set; }
        public string TextEn { get; set; }
        public string TextBe { get; set; }
        
        public List<PhotoInfo> Photos { get; set; }

        public Exhibit()
        {
            Photos = new List<PhotoInfo>();
        }
    }
}
