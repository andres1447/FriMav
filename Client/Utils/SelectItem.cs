using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Client.Utils
{
    public class SelectItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public SelectItem(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}
