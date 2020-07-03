using System.Collections.Generic;

namespace GSU.Museum.API.Data.Models
{
    public class HallDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool State { get; set; }
        public byte[] Photo { get; set; }
        public List<StandDTO> Stands { get; set; }
    }
}
