using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Relay.Models
{    
    public class Whitelist
    {
        [Key]
        public string PubKey { get; set; }
    }
}
    