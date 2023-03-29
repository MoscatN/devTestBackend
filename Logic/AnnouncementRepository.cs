using Persistence;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public class AnnouncementRepository : IAnnouncementRepository {

    //Make a const string with the API endpoint
    private const string URL_API = "https://www.bitmex.com/api/v1/announcement";
    private AnnouncementdbContext _context;


    public AnnouncementRepository(AnnouncementdbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Announcement>> CreateAsync() {
        var apiRes = await GetApiResponse(URL_API);
        //Save all the records found
        await _context.AddRangeAsync(apiRes);
        await _context.SaveChangesAsync();
        return apiRes;
    }

    public async Task<IEnumerable<Announcement>> RetrieveAllAsync() {
        var apiRes = await GetApiResponse(URL_API);
        return apiRes is null ? Enumerable.Empty<Announcement>() : apiRes;
    }

    public async Task<IEnumerable<Announcement>> RetrieveAllSortedByDate() {
        var sortedResponse = await _context
                             .Announcements
                             .OrderByDescending(x => x.Date)
                             .ToListAsync();

        return sortedResponse;
    }

    public async Task<IEnumerable<Announcement>> RetrieveAllFilterByDate(DateTime date) 
    {
        //Compare the given date with every date found in the database.
        var filteredResponse = await _context.Announcements
                                     .Where(x => x.Date.Date == date)
                                     .OrderByDescending(x => x.Date)
                                     .ToListAsync();

        return filteredResponse;
    }

    //Method to reuse the api's call
    private async Task<IEnumerable<Announcement>> GetApiResponse(string endpoint) {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        HttpResponseMessage response = await client.GetAsync(endpoint);
        string apiResponse = await response.Content.ReadAsStringAsync();
        
        var result = JsonConvert.DeserializeObject<IEnumerable<Announcement>>(apiResponse);
        return result;
    }
}
