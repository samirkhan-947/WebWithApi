using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _ITrailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository ITrailRepository, IMapper mapper)
        {
            _ITrailRepository = ITrailRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            var objList = _ITrailRepository.GetTrails();
            var objDto = new List<TrailDtos>();
            foreach (var item in objList)
            {
                objDto.Add(_mapper.Map<TrailDtos>(item));
            }
            return Ok(objList);
        }
        [HttpGet("{trailsid:int}", Name = "GetTrail")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetTrail(int trailsid)
        {
            var obj = _ITrailRepository.GetTrail(trailsid);
            if (obj == null)
            {
               return NotFound();
            }
            var objDto = _mapper.Map<TrailDtos>(obj);
            return Ok(objDto);
        }
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDtos))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _ITrailRepository.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDtos>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDtos>(obj));
            }


            return Ok(objDto);

        }

        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailCreateDtos TrailDtos)
        {
            if(TrailDtos == null)
            {
                return BadRequest(ModelState);
            }
            if (_ITrailRepository.TrailExists(TrailDtos.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }
            
            var TrailObj = _mapper.Map<Trail>(TrailDtos);
            
            if (!_ITrailRepository.CreateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{TrailDtos.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailsid = TrailObj.Id }, TrailObj);
        }
        [HttpPatch("{trailsId:int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int trailsId,[FromBody] TrailUpdateDtos TrailDtos)
        {
            if(TrailDtos == null || trailsId != TrailDtos.Id)
            {
                return BadRequest(ModelState);
            }
            var TrailObj = _mapper.Map<Trail>(TrailDtos);

            if (!_ITrailRepository.UpdateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{TrailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{trailsId:int}", Name = "DeleteTrail")]
        public IActionResult DeleteTrail(int trailsId)
        {
            if (!_ITrailRepository.TrailExists(trailsId))
            {
                return NotFound();
            }
            var obj = _ITrailRepository.GetTrail(trailsId);
            if (!_ITrailRepository.DeleteTrail(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{obj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
