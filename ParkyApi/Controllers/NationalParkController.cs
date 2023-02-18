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
    [Route("api/v{version:apiVersion}/nationalparks")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpec")]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objList = _npRepo.GetNationalParks();
            var objDto = new List<NationalParksDtos>();
            foreach (var item in objList)
            {
                objDto.Add(_mapper.Map<NationalParksDtos>(item));
            }
            return Ok(objList);
        }
        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
        [Authorize]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);
            if (obj == null)
            {
               return NotFound();
            }
            var objDto = _mapper.Map<NationalParksDtos>(obj);
            return Ok(objDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParksDtos nationalParksDtos)
        {
            if(nationalParksDtos == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.NationalParkExists(nationalParksDtos.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }
            
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParksDtos);
            
            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark",new { nationalParkId = nationalParkObj.Id },nationalParkObj);
        }
        [HttpPatch("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId,[FromBody] NationalParksDtos nationalParksDtos)
        {
            if(nationalParksDtos==null || nationalParkId!= nationalParksDtos.Id)
            {
                return BadRequest(ModelState);
            }
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParksDtos);

            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }
            var obj = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{obj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
