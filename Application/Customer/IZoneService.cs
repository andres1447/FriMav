using FriMav.Domain;
using FriMav.Domain.Entities;
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

        [Transactional]
        void Create(Zone zone);

        [Transactional]
        void Update(Zone zone);

        [Transactional]
        void Delete(int id);
    }
}
