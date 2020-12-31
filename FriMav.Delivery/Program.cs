using FriMav.Delivery.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FriMav.Delivery
{
    class Program
    {
        static int Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.DependsOnMsSql();
                x.Service<HostConfig>();
                x.RunAsLocalSystem();
                x.SetDisplayName("Frimav.Api");
                x.SetServiceName("Frimav.Api");
            });
            return (int)exitCode;
        }
    }
}
