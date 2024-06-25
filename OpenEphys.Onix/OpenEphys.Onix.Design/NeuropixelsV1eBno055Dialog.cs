using System;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eBno055Dialog : GenericDeviceDialog
    {
        public ConfigureNeuropixelsV1eBno055 ConfigureNode
        {
            get => (ConfigureNeuropixelsV1eBno055)propertyGrid.SelectedObject;
            set => propertyGrid.SelectedObject = value;
        }

        public NeuropixelsV1eBno055Dialog(ConfigureNeuropixelsV1eBno055 configureNode)
        {
            InitializeComponent();
            Shown += FormShown;

            ConfigureNode = new(configureNode);
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
