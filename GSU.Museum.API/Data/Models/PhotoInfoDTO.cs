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

        public override int GetHashCode()
        {
            if (Photo != null)
            {
                unchecked
                {
                    const int p = 16777619;
                    int hash = (int)2166136261;

                    for (int i = 0; i < Photo.Length; i++)
                        hash = (hash ^ Photo[i]) * p;

                    hash += hash << 13;
                    hash ^= hash >> 7;
                    hash += hash << 3;
                    hash ^= hash >> 17;
                    hash += hash << 5;
                    return hash;
                }
            }
            return 1;
        }
    }
}
