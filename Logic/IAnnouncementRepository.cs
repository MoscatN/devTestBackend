using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;
using Domain;

namespace Logic;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> RetrieveAllAsync();
    Task<IEnumerable<Announcement>> CreateAsync();
    Task<IEnumerable<Announcement>> RetrieveAllSortedByDate();
    Task<IEnumerable<Announcement>> RetrieveAllFilterByDate(DateTime date);

}
