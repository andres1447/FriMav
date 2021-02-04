using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Infrastructure.Migrations
{
    public partial class Payroll
    {
        public void OnUp()
        {
            Sql("update dbo.Person set Balance = 0 where Type = 2");
            Sql("update dbo.Person set Salary = 0, JoinDate = getdate()");
        }

        public void OnDown()
        {
        }
    }
}
