using FriMav.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application
{
    public interface IZoneService
    {
        IEnumerable<Zone> GetAll();
        Zone Get(int zoneId);
        void Create(Zone zone);
        void Update(Zone zone);
        void Delete(Zone zone);
    }
}
