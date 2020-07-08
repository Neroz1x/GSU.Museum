using System.Collections.Generic;

namespace GSU.Museum.Shared.Data.Models
{
    public class Exhibit
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool State { get; set; }
        public List<byte[]> Photos { get; set; }
    }
}
