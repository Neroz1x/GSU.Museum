using System.Collections.Generic;

namespace GSU.Museum.Shared.Data.Models
{
    public class Stand
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Text { get; set; }
        public bool State { get; set; }
        public byte[] Photo { get; set; }
        public List<Exhibit> Exhibits { get; set; }
    }
}
