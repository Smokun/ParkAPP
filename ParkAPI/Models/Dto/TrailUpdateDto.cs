using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ParkAPI.Models.Trail;

namespace ParkAPI.Models.Dto
{
    public class TrailUpdateDto
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DifficulityType Difficulity { get; set; }
        public int NationalParkId { get; set; }
        

    }
}
