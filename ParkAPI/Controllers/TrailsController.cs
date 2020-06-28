using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkAPI.Models;
using ParkAPI.Models.Dto;
using ParkAPI.Repository.IRepository;

namespace ParkAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiVersion}/Trails")]
    [ApiController]
    //group for swagger documentation versioning
    //[ApiExplorerSettings(GroupName = "ParkOpenAPISpecTrails")]
    //this status code will be shown for all the requests
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
  
    {   //dependency injection
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;
        public TrailsController(ITrailRepository trailRepo, IMapper mapper) 
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get the list of trails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //specifying reponse types for swagger documentation
        [ProducesResponseType(200,Type = typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }
        /// <summary>
        /// Get individual trail.
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
           
            return Ok(objDto);

        }
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);

        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //we do not want to pass the required for NationalParkDto info so we use TrailUpsertDto but returning full TrailDto
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            //duplicate check
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }


            var trailObj = _mapper.Map<Trail>(trailDto);


            if (!_trailRepo.CreateTrail(trailObj))

            {

                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");

                return StatusCode(500, ModelState);

            }
            //route name -> route value -> final values
            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id}, trailObj);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            //we convert TrailUpsertDto to Trail so this requires mapping
            var trailObj = _mapper.Map<Trail>(trailDto);


            if (!_trailRepo.UpdateTrail(trailObj))

            {

                ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");

                return StatusCode(500, ModelState);

            }
            return NoContent();



        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }

            var trailObj = _trailRepo.GetTrail(trailId);



            if (!_trailRepo.UpdateTrail(trailObj))

            {

                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");

                return StatusCode(500, ModelState);

            }
            return NoContent();



        }

    }
}