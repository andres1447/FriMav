using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Domain.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        bool Exists(string code);
    }
}
