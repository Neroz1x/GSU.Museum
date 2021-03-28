using GSU.Museum.CommonClassLibrary.Data.Enums;
using GSU.Museum.CommonClassLibrary.Data.Models;
using System.Collections.Generic;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class ExhibitDTO : MuseumItemDTO
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
                int hash = (int)InitialHash;
                hash = (hash * Multiplier) ^ GetStringHashCode(Id);
                hash = (hash * Multiplier) ^ GetStringHashCode(Title);
                hash = (hash * Multiplier) ^ GetStringHashCode(Description);
                hash = (hash * Multiplier) ^ GetStringHashCode(Text);
                hash = (hash * Multiplier) ^ (int)ExhibitType;
                if (Photos != null)
                {
                    foreach (var photo in Photos)
                    {
                        if (photo != null)
                        {
                            hash = (hash * Multiplier) ^ photo.GetHashCode();
                        }
                    }
                }
                return hash;
            }
        }
    }
}
