using GSU.Museum.CommonClassLibrary.Data.Models;
using System.Collections.Generic;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class StandDTO : MuseumItemDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PhotoInfoDTO Photo { get; set; }
        public List<ExhibitDTO> Exhibits { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)InitialHash;
                hash = (hash * Multiplier) ^ GetStringHashCode(Id);
                hash = (hash * Multiplier) ^ GetStringHashCode(Title);
                hash = (hash * Multiplier) ^ GetStringHashCode(Description);
                if(Photo != null)
                {
                    hash = (hash * Multiplier) ^ Photo.GetHashCode();
                }
                if (Exhibits != null)
                {
                    foreach (var exhibit in Exhibits)
                    {
                        hash = (hash * Multiplier) ^ (exhibit?.GetHashCode() ?? 1);
                    }
                }
                return hash;
            }
        }
    }
}
