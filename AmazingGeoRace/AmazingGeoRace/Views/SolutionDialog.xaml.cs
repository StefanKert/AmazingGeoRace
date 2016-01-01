using Windows.UI.Xaml.Controls;

namespace AmazingGeoRace.Views
{
    public sealed partial class SolutionDialog : ContentDialog
    {
        public string Solution => TbSolution.Text;

        public SolutionDialog() {
            this.InitializeComponent();
        }
    }
}
