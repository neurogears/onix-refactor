namespace OpenEphys.Onix.Design
{
    public partial class Bno055Dialog : GenericDeviceDialog
    {
        public ConfigureBno055 ConfigureBno055
        {
            get => (ConfigureBno055)propertyGrid.SelectedObject;
            set => propertyGrid.SelectedObject = value;
        }

        public Bno055Dialog(ConfigureBno055 configureNode)
        {
            InitializeComponent();

            ConfigureBno055 = new(configureNode);
        }
    }
}
