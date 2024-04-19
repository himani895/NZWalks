using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = dbContext.Regions.ToList();
            return Ok(regions);

            //using DTO

            var regionDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageURL = region.RegionImageURL
                });
            }
            return Ok(regionDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetbyId ([FromRoute]Guid id)
        {
            var region = dbContext.Regions.Find(id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);

        }
        [HttpPost]
        public IActionResult CreateRegion([FromBody]Region region)
        {
            var createRegion = new Region{
                Code = region.Code,
                RegionImageURL = region.RegionImageURL,
                Name = region.Name
            };
            dbContext.Regions.Add(createRegion);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetbyId), new {id=createRegion.Id},createRegion);
        }

        //update region
        [HttpPut]
        [Route("{id=Guid}")]
        public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest) {
            var region = dbContext.Regions.Find(id);
            if(region== null)
            {
                return NotFound();
            }
            region.Code = updateRegionRequest.Code;
            region.RegionImageURL = updateRegionRequest.RegionImageURL;
            region.Name = updateRegionRequest.Name;
            dbContext.SaveChanges();
            var regionDto = new RegionDto
            {
                Id = region.Id,
                RegionImageURL = region.RegionImageURL,
                Name = region.Name,
                Code = region.Code,
            };
            return Ok(region);
        }

        //delete
        [HttpDelete]
        [Route("{id=Guid}")]
        public IActionResult DeleteRegion([FromRoute] Guid id)
        {
            var region = dbContext.Regions.Find(id);
            if (region == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(region);
            dbContext.SaveChanges();
            return Ok(region);
        }
    }
}
