using GSU.Museum.CommonClassLibrary.Data.Models;
using System.Collections.Generic;

namespace GSU.Museum.CommonClassLibrary.Models
{
    public class HallDTO : MuseumItemDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public PhotoInfoDTO Photo { get; set; }
        public List<StandDTO> Stands { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)InitialHash;
                hash = (hash * Multiplier) ^ GetStringHashCode(Id);
                hash = (hash * Multiplier) ^ GetStringHashCode(Title);
                if(Photo != null)
                {
                    hash = (hash * Multiplier) ^ Photo.GetHashCode();
                }
                if (Stands != null)
                {
                    foreach (var stand in Stands)
                    {
                        hash = (hash * Multiplier) ^ (stand?.GetHashCode() ?? 1);
                    }
                }
                return hash;
            }
        }
    }
}
