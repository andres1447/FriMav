using FriMav.Domain;
using FriMav.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Repositories
{
    public class NumberSequenceRepository : BaseRepository<NumberSequence>, INumberSequenceRepository
    {
        public NumberSequenceRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {

        }
    }
}
