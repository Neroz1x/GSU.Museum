using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GSU.Museum.API.Data.Models
{
    public class PhotoInfoDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
    }
}
