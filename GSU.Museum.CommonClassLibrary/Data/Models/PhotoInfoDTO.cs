using GSU.Museum.CommonClassLibrary.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class PhotoInfoDTO : MuseumItemDTO
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
                    int hash = (int)InitialHash;

                    for (int i = 0; i < Photo.Length; i++)
                    {
                        hash = (hash ^ Photo[i]) * Multiplier;
                    }

                    hash += hash << 13;
                    hash ^= hash >> 7;
                    hash += hash << 3;
                    hash ^= hash >> 17;
                    hash += hash << 5;
                    return hash;
                }
            }
            return DefaultHashValue;
        }
    }
}
