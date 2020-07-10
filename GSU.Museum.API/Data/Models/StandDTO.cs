using System.Collections.Generic;

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
    }
}
