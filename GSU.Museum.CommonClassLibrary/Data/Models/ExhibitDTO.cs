using GSU.Museum.CommonClassLibrary.Data.Enums;
using System.Collections.Generic;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class ExhibitDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public ExhibitType ExhibitType { get; set; }
        public List<PhotoInfoDTO> Photos { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ GetStringHashCode(Id);
                hash = (hash * 16777619) ^ GetStringHashCode(Title);
                hash = (hash * 16777619) ^ GetStringHashCode(Description);
                hash = (hash * 16777619) ^ GetStringHashCode(Text);
                hash = (hash * 16777619) ^ (int)ExhibitType;
                if (Photos != null)
                {
                    foreach (var photo in Photos)
                    {
                        if (photo != null)
                        {
                            hash = (hash * 16777619) ^ photo.GetHashCode();
                        }
                    }
                }
                return hash;
            }
        }

        private int GetStringHashCode(string str)
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
            return 1;
        }
    }
}
