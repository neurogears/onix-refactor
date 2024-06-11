using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OpenEphys.Onix.Design
{
    public static class DesignHelper
    {
        public static T DeserializeString<T>(string channelLayout)
        {
            return JsonConvert.DeserializeObject<T>(channelLayout);
        }

        public static void SerializeObject(object _object, string filepath)
        {
            var stringJson = JsonConvert.SerializeObject(_object);

            File.WriteAllText(filepath, stringJson);
        }

        public static IEnumerable<Control> GetAllChildren(this Control root)
        {
            var stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Any())
            {
                var next = stack.Pop();
                foreach (Control child in next.Controls)
                    stack.Push(child);
                yield return next;
            }
        }

        public static void AddMenuItemsFromDialog(this Form thisForm, Form form, string menuName)
        {
            if (form != null)
            {
                var menuStrips = form.GetAllChildren()
                                     .OfType<MenuStrip>()
                                     .ToList();

                if (menuStrips != null && menuStrips.Count > 0)
                {
                    foreach (var menuStrip in menuStrips)
                    {
                        var toolStripMenuItem = new ToolStripMenuItem(menuName);

                        toolStripMenuItem.DropDownItems.AddRange(menuStrip.Items);

                        if (thisForm.Controls["menuStrip"] is MenuStrip thisMenuStrip)
                        {
                            thisMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem });
                        }
                    }
                }
            }
        }

    }
}
