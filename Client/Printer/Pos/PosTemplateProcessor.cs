using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FriMav.Client.Printer.Pos
{
    public class PosTemplateProcessor
    {
        public static string Build(string template)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PosTemplate));
            using (var stream = new StringReader(template))
            {
                var data = (PosTemplate)serializer.Deserialize(stream);
                var helper = new EpsonCommander(40);
                helper.Init();
                foreach (var command in data.Commands)
                {
                    command.Execute(helper);
                }
                helper.FormFeed();
                helper.Init();
                return helper.Build();
            }
        }
    }
}
