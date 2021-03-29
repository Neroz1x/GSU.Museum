using GSU.Museum.CommonClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace GSU.Museum.CommonClassLibrary.Data.Models
{
    public class MuseumItemDTO : IMuseumItemDTO
    {
        [JsonIgnore]
        public readonly uint InitialHash = 2166136261;
        [JsonIgnore]
        public readonly int Multiplier = 16777619;
        [JsonIgnore]
        public readonly int DefaultHashValue = 1;

        public int GetStringHashCode(string str)
        {
            if (str != null)
            {
                unchecked
                {
                    int hash1 = (5381 << 16) + 5381;
                    int hash2 = hash1;

                    for (int i = 0; i < str.Length; i += 2)
                    {
                        hash1 = ((hash1 << 5) + hash1) ^ str[i];
                        if (i == str.Length - 1)
                        {
                            break;
                        }
                        hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                    }

                    return hash1 + (hash2 * 1566083941);
                }
            }
            return DefaultHashValue;
        }
    }
}
