using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly IVillaStoreRepository _villaRepository;
        private readonly IMapper _mapper;

        public VillaAPIController(ILogging logger, IVillaStoreRepository villaStoreRepository, IMapper mapper)
        {
            _logger = logger;
            _villaRepository = villaStoreRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetAll()
        {
            _logger.Log("Getting all villas", LogType.Information);
            var villas = await _villaRepository.GetAllVillas();
            return Ok(_mapper.Map<VillaDTO>(villas));
        }

        [HttpGet("{id}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Villa>> GetVilla(int id) 
        {
            var villa = await _villaRepository.GetVilla(id);
            return Ok(villa);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpPost]
        //public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (VillaStore.villas.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
        //    {
        //        ModelState.AddModelError("Cutstom Error", "Villa already Exists!");
        //        return BadRequest(ModelState);
        //    }

        //    if (villaDTO is null)
        //    {
        //        return BadRequest(villaDTO);
        //    }

        //    if (villaDTO.Id > 0)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //    villaDTO.Id = VillaStore.villas.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
        //    VillaStore.villas.Add(villaDTO);

        //    //return Ok(villaDTO);
        //    return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        //}


        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[HttpDelete("{id}", Name = "DeleteVilla")]
        //public ActionResult DeleteVilla(int id)
        //{
        //    if(id == 0)
        //    {
        //        return BadRequest();
        //    }

        //    var villa = VillaStore.villas.FirstOrDefault(v => v.Id == id);
        //    if(villa == null)
        //    {
        //        return NotFound();
        //    }

        //    VillaStore.villas.Remove(villa);
        //    return NoContent();
        //}

        [HttpPut("{id}", Name ="UpdateVilla")]
        public ActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }

            var villa = VillaStore.villas.FirstOrDefault(v => v.Id == id);
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{id}", Name ="UpdatePartialVilla")]
        public ActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO )
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villas.FirstOrDefault(v => v.Id == id);

            if(villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
