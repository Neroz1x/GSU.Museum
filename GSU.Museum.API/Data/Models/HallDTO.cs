﻿using System.Collections.Generic;

namespace GSU.Museum.API.Data.Models
{
    public class HallDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool State { get; set; }
        public PhotoInfoDTO Photo { get; set; }
        public List<StandDTO> Stands { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ GetStringHashCode(Id);
                hash = (hash * 16777619) ^ GetStringHashCode(Title);
                hash = (hash * 16777619) ^ State.GetHashCode();
                if(Photo != null)
                {
                    hash = (hash * 16777619) ^ Photo.GetHashCode();
                }
                if (Stands != null)
                {
                    foreach (var stand in Stands)
                    {
                        hash = (hash * 16777619) ^ (stand?.GetHashCode() ?? 1);
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
