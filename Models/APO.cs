using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GetDataFromApi.Models
{
 
    public class APO
    {
        [Key]
        public int apoID { get; set; }
        public string iata_code { get; set; }
        public string name { get; set; }
        public string icao_code { get; set; }
    }
}
