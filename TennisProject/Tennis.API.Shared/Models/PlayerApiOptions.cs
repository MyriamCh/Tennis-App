using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tennis.API.Shared.Models
{
    public class PlayerApiOptions
    {
        public const string SectionName = "PlayerApi"; 
        public string? JsonEndpoint { get; set; }
        public string? BaseAddress { get; set; }
    }
}
