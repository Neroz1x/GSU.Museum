using System.Collections.Generic;

namespace GSU.Museum.Shared.Data.Models
{
    public class Hall
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool State { get; set; }
        public byte[] Photo { get; set; }
        public List<Stand> Stands { get; set; }
    }
}
