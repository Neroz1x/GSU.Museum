using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GSU.Museum.API.Data.Models
{
    public class PhotoInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionBe { get; set; }
        public byte[] Photo { get; set; }
    }
}
