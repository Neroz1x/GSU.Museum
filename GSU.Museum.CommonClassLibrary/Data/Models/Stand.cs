using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class Stand
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
        public PhotoInfo Photo { get; set; }
        public List<Exhibit> Exhibits { get; set; }

        public Stand()
        {
            Photo = new PhotoInfo();
            Exhibits = new List<Exhibit>();
        }
    }
}
