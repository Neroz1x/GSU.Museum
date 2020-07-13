﻿using System.Collections.Generic;

namespace GSU.Museum.API.Data.Models
{
    public class StandDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Text { get; set; }
        public bool State { get; set; }
        public byte[] Photo { get; set; }
        public List<ExhibitDTO> Exhibits { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ GetStringHashCode(Id);
                hash = (hash * 16777619) ^ GetStringHashCode(Title);
                if (Text != null)
                {
                    foreach (var str in Text)
                    {
                        hash = (hash * 16777619) ^ GetStringHashCode(str);
                    }
                }
                hash = (hash * 16777619) ^ State.GetHashCode();
                hash = (hash * 16777619) ^ GetByteHashCode(Photo);
                if (Exhibits != null)
                {
                    foreach (var exhibit in Exhibits)
                    {
                        hash = (hash * 16777619) ^ (exhibit?.GetHashCode() ?? 1);
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

        private int GetByteHashCode(byte[] byteArray)
        {
            if (byteArray != null)
            {
                unchecked
                {
                    const int p = 16777619;
                    int hash = (int)2166136261;

                    for (int i = 0; i < byteArray.Length; i++)
                        hash = (hash ^ byteArray[i]) * p;

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
