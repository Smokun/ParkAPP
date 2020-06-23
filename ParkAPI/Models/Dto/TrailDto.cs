using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ParkAPI.Models.Trail;

namespace ParkAPI.Models.Dto
{
    public class TrailDto
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DifficulityType Difficulity { get; set; }
        public int NationalParkId { get; set; }
        //in dtos we do not neet foreighn key refrences
        public NationalParkDto NationalPark { get; set; }

    }
}
