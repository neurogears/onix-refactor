using System;

namespace OpenEphys.Onix1.Design
{
    public partial class Rhs2116Dialog : GenericDeviceDialog
    {
        public ConfigureRhs2116 ConfigureNode
        {
            get => (ConfigureRhs2116)propertyGrid.SelectedObject;
            set => propertyGrid.SelectedObject = value;
        }

        public Rhs2116Dialog(ConfigureRhs2116 configureRhs2116)
        {
            InitializeComponent();
            Shown += FormShown;

            ConfigureNode = configureRhs2116;
        }

        private void FormShown(object sender, EventArgs e)
        {
            if (!TopLevel)
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();
            }
        }
    }
}
