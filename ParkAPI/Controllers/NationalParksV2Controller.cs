using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkAPI.Models;
using ParkAPI.Models.Dto;
using ParkAPI.Repository.IRepository;

namespace ParkAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    //group for swagger documentation versioning
    [ApiExplorerSettings(GroupName = "ParkOpenAPISpecNP")]
    [ApiController]
    //this status code will be shown for all the requests
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper) 
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get the list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //specifying reponse types for swagger documentation
        [ProducesResponseType(200,Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var obj = _npRepo.GetNationalParks().FirstOrDefault();
          
            return Ok(obj);
        }
        
    }
}