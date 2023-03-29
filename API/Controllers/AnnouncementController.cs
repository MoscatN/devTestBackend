using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Domain;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController : ControllerBase 
{
    private readonly IAnnouncementRepository repo;
    public AnnouncementController(IAnnouncementRepository repo) {
        this.repo = repo;
    }

    [HttpGet]
    [ProducesResponseType(200, Type =  typeof(IEnumerable<Announcement>))]
    public async Task<IEnumerable<Announcement>> GetAnnouncements() 
    {
        return await repo.RetrieveAllAsync();
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(IEnumerable<Announcement>))]
    public async Task<ActionResult<Announcement>> Create() 
    {
        try {
            var announcement = await repo.CreateAsync();
            return Ok(announcement);
        }
        catch (Exception e) 
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("AnnouncementByDate")]
    [ProducesResponseType(200, Type = typeof(List<Announcement>))]
    public async Task<IEnumerable<Announcement>> GetAnnouncementSortedByDate() {

        return await repo.RetrieveAllSortedByDate();
    }

    [HttpGet("{date}")]
    [ProducesResponseType(200, Type = typeof(List<Announcement>))]
    public async Task<IEnumerable<Announcement>> GetFilteredAnnouncements(DateTime date) {

        var filterRecords = await repo.RetrieveAllFilterByDate(date);

        return filterRecords; 
    }
}