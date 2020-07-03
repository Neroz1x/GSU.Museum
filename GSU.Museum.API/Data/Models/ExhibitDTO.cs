using System.Collections.Generic;

namespace GSU.Museum.API.Data.Models
{
    public class ExhibitDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool State { get; set; }
        public List<byte[]> Photos { get; set; }
    }
}
