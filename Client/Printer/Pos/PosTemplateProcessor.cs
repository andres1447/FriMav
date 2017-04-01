using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FriMav.Client.Printer.Pos.Command;

namespace FriMav.Client.Printer.Pos
{
    public class PosTemplateProcessor
    {
        public static IEnumerable<string> Print(string template)
        {
            var pages = new List<string>();
            XmlSerializer serializer = new XmlSerializer(typeof(PosTemplate));
            using (var stream = new StringReader(template))
            {
                var data = (PosTemplate)serializer.Deserialize(stream);
                return data.Execute();
            }
        }
    }
}
