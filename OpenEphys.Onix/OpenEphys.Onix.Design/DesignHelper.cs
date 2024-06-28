using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenEphys.ProbeInterface;

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

                var thisMenuStrip = thisForm.GetAllChildren()
                                            .OfType<MenuStrip>()
                                            .FirstOrDefault();

                if (menuStrips != null && menuStrips.Count > 0)
                {
                    foreach (var menuStrip in menuStrips)
                    {
                        var toolStripMenuItem = new ToolStripMenuItem(menuName);

                        toolStripMenuItem.DropDownItems.AddRange(menuStrip.Items);

                        thisMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem });
                    }
                }
            }
        }

        /// <summary>
        /// Given two forms, take all menu items that are in the "File" MenuItem of the child form, and copy them directly to the 
        /// "File" MenuItem for the parent form
        /// </summary>
        /// <param name="thisForm"></param>
        /// <param name="form"></param>
        public static void AddMenuItemsFromDialogToFileOption(this Form thisForm, Form form)
        {
            const string FileString = "File";

            if (form != null)
            {
                var menuStrips = form.GetAllChildren()
                                     .OfType<MenuStrip>()
                                     .ToList();

                var thisMenuStrip = thisForm.GetAllChildren()
                                            .OfType<MenuStrip>()
                                            .FirstOrDefault();

                ToolStripMenuItem existingMenuItem = null;

                foreach (ToolStripMenuItem menuItem in thisMenuStrip.Items)
                {
                    if (menuItem.Text == FileString)
                    {
                        existingMenuItem = menuItem;
                    }
                }

                if (menuStrips != null && menuStrips.Count > 0)
                {
                    foreach (var menuStrip in menuStrips)
                    {
                        foreach (ToolStripMenuItem menuItem in menuStrip.Items)
                        {
                            if (menuItem.Text == "File")
                            {
                                while (menuItem.DropDownItems.Count > 0)
                                {
                                    existingMenuItem.DropDownItems.Add(menuItem.DropDownItems[0]);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Given two forms, take all menu items that are in the "File" MenuItem of the child form, and copy them to the 
        /// sub-menu name given, nested under the "File" MenuItem for the parent form
        /// </summary>
        /// <param name="thisForm"></param>
        /// <param name="form"></param>
        /// <param name="subMenuName"></param>
        public static void AddMenuItemsFromDialogToFileOption(this Form thisForm, Form form, string subMenuName)
        {
            const string FileString = "File";

            if (form != null)
            {
                var menuStrips = form.GetAllChildren()
                                     .OfType<MenuStrip>()
                                     .ToList();

                var thisMenuStrip = thisForm.GetAllChildren()
                                            .OfType<MenuStrip>()
                                            .FirstOrDefault();

                ToolStripMenuItem existingMenuItem = null;

                foreach (ToolStripMenuItem menuItem in thisMenuStrip.Items)
                {
                    if (menuItem.Text == FileString)
                    {
                        existingMenuItem = menuItem;
                    }
                }

                ToolStripMenuItem newItems = new()
                {
                    Text = subMenuName
                };

                if (menuStrips != null && menuStrips.Count > 0)
                {
                    foreach (var menuStrip in menuStrips)
                    {
                        foreach (ToolStripMenuItem menuItem in menuStrip.Items)
                        {
                            if (menuItem.Text == "File")
                            {
                                while (menuItem.DropDownItems.Count > 0)
                                {
                                    newItems.DropDownItems.Add(menuItem.DropDownItems[0]);
                                }
                            }
                        }
                    }

                    existingMenuItem.DropDownItems.Add(newItems);
                }
            }
        }
    }
}
