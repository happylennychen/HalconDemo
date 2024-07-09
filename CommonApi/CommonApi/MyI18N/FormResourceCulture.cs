using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace CommonApi.MyI18N {
    public sealed class FormResourceCulture : ResourceCulture {
        public FormResourceCulture(string fullFormClassName, Assembly assembly) : base(fullFormClassName, assembly) {
        }

        public void ExcludedControls(HashSet<Control> controls) {
            this.excludedControls.UnionWith(controls);
        }

        public void ExcludedToolStripStatusLabels(HashSet<ToolStripStatusLabel> toolStripStatusLabels) {
            this.excludedToolStripStatusLabels.UnionWith(toolStripStatusLabels);
        }

        public void EnumerateControl(Control parentControl) {
            if (!excludedControls.Contains(parentControl)) {
                parentControl.Text = GetString(parentControl.Name);
            }

            foreach (Control control in parentControl.Controls) {
                if (excludedControls.Contains(control)) {
                    continue;
                }

                string controlType = control.GetType().Name;
                switch (controlType) {
                    //---------------------------------------------------------------
                    // atom control
                    case "Label":
                    case "LinkLabel":
                    case "Button":
                    case "RadioButton":
                    case "CheckBox":
                    case "ToolStripMenuItem":
                    case "ToolStripStatusLabel":
                        control.Text = GetString(control.Name);
                        break;

                    //----------------------------------------------------------------
                    // composited control
                    case "MenuStrip":
                        MenuStrip menuStrip = control as MenuStrip;
                        EnumrateMenu(menuStrip);
                        break;

                    case "ContextMenuStrip":
                        ContextMenuStrip contextMenuStrip = control as ContextMenuStrip;
                        EnumerateContextMenus(contextMenuStrip);
                        break;

                    case "StatusStrip":
                        StatusStrip statusStrip = control as StatusStrip;
                        EnumerateStatusBar(statusStrip);
                        break;

                    case "GroupBox":
                        GroupBox groupBox = control as GroupBox;
                        EnumerateControl(groupBox);
                        break;

                    case "TabControl":
                        TabControl tabControl = control as TabControl;
                        EnumerateTabControl(tabControl);
                        break;

                    case "Panel":
                        Panel panel = control as Panel;
                        EnumerateControl(panel);
                        break;

                    case "SplitterPanel":
                        SplitterPanel splitterPanel = control as SplitterPanel;
                        EnumerateControl(splitterPanel);
                        break;

                    case "SplitContainer":
                        SplitContainer splitContainer = control as SplitContainer;
                        EnumerateControl(splitContainer.Panel1);
                        EnumerateControl(splitContainer.Panel2);
                        break;

                    //Form
                    default:
                        break;
                }
            }
        }

        public void EnumerateContextMenus(ContextMenuStrip contextMenu) {
            foreach (ToolStripItem item in contextMenu.Items) {
                if (item is ToolStripSeparator) {
                    continue;
                } else if (item is ToolStripMenuItem) {
                    ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                    menuItem.Text = GetString(menuItem.Name);
                    EnumerateMenuItem(menuItem);
                }
            }
        }

        #region private methods
        private void EnumrateMenu(MenuStrip menuStrip) {
            foreach (ToolStripMenuItem item in menuStrip.Items) {
                item.Text = GetString(item.Name);
                EnumerateMenuItem(item);
            }
        }

        private void EnumerateMenuItem(ToolStripMenuItem parentMenuItem) {
            foreach (ToolStripItem item in parentMenuItem.DropDownItems) {
                if (item is ToolStripSeparator) {
                    continue;
                }
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                menuItem.Text = GetString(menuItem.Name);
                EnumerateMenuItem(menuItem);
            }
        }

        private void EnumerateTabControl(TabControl tabControl) {
            string controlText = string.Empty;
            foreach (TabPage tabPage in tabControl.TabPages) {
                controlText = GetString(tabPage.Name);
                tabPage.Text = controlText;
                EnumerateControl(tabPage);
            }
        }

        private void EnumerateStatusBar(StatusStrip statusStrip) {
            foreach (ToolStripItem item in statusStrip.Items) {
                if (item is ToolStripStatusLabel) {
                    ToolStripStatusLabel label = item as ToolStripStatusLabel;
                    if (excludedToolStripStatusLabels.Contains(label)) {
                        continue;
                    }
                    label.Text = GetString(label.Name);
                } else if (item is ToolStripDropDownButton) {
                    ToolStripDropDownButton dropDownButton = item as ToolStripDropDownButton;
                    dropDownButton.Text = GetString(dropDownButton.Name);
                    for (int i = 0; i < dropDownButton.DropDownItems.Count; i++) {
                        ToolStripMenuItem menuItem = dropDownButton.DropDownItems[i] as ToolStripMenuItem;
                        menuItem.Text = GetString(menuItem.Name);
                    }
                } else if (item is ToolStripSplitButton) {
                    ToolStripSplitButton splitButton = item as ToolStripSplitButton;
                    splitButton.Text = GetString(splitButton.Name);
                    for (int i = 0; i < splitButton.DropDownItems.Count; i++) {
                        ToolStripMenuItem menuItem = splitButton.DropDownItems[i] as ToolStripMenuItem;
                        menuItem.Text = GetString(menuItem.Name);
                    }
                }
            }
        }
        #endregion

        private HashSet<Control> excludedControls = new HashSet<Control>();
        private HashSet<ToolStripStatusLabel> excludedToolStripStatusLabels = new HashSet<ToolStripStatusLabel>();
    }
}
