using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GSU.Museum.Web.Models
{
    public class StandViewModel
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
        public bool State { get; set; }
        public PhotoInfo Photo { get; set; }
        public List<ExhibitViewModel> Exhibits { get; set; }
    }
}
