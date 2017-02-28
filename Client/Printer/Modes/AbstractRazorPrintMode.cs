using FriMav.Client.Models;
using FriMav.Domain;
using RazorTemplates.Core;

namespace FriMav.Client.Printer
{
    public abstract class AbstractRazorPrintMode : IPrintMode
    {
        public abstract string Name { get; }
        
        public abstract void PrintTemplate(string dest, string template);

        public bool Applies(string mode)
        {
            return mode == Name;
        }

        public void Print(string dest, object model, string template)
        {
            var view = Template.Compile(template);
            string res = view.Render(model);
            PrintTemplate(dest, res);
        }
    }
}
