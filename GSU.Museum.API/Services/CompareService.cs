using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Interfaces;
using GSU.Museum.CommonClassLibrary.Models;
using System.Collections.Generic;

namespace GSU.Museum.API.Services
{
    public class CompareService : ICompareService
    {
        public bool IsEquals(int hashCode, IMuseumItemDTO item)
        {
            return hashCode == item.GetHashCode();
        }

        public bool IsListEquals(int hashCode, List<HallDTO> halls)
        {
            return hashCode == GetHash(halls);
        }

        public int GetHash(List<HallDTO> halls)
        {
            unchecked
            {
                int hash = (int)2166136261;
                foreach (var hall in halls)
                {
                    hash = (hash * 16777619) ^ (hall?.GetHashCode() ?? 1);
                }
                return hash;
            }
        }
    }
}
