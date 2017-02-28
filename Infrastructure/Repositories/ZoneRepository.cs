using FriMav.Domain;
using FriMav.Domain.Repositories;
using System.Data.Entity;
using System.Linq;

namespace FriMav.Infrastructure.Repositories
{
    public class ZoneRepository : BaseRepository<Zone>, IZoneRepository
    {
        public ZoneRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
