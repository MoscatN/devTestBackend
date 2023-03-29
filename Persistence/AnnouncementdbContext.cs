using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class AnnouncementdbContext : DbContext
{
    public virtual DbSet<Announcement> Announcements { get; set; }

    public AnnouncementdbContext() 
    {
    }

    public AnnouncementdbContext(DbContextOptions<AnnouncementdbContext> options) 
        : base(options) 
    { 
    }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=Announcement; Integrated Security=True" );

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
