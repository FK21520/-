using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
//消去する難易度を選択する画面
namespace pyramid
{
    public partial class DELETEWINDOWPAGE : Page
    {
        public DELETEWINDOWPAGE()
        {
            InitializeComponent();
        }
        private void StartEasyClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DELETECONFIRMATION("easy"));
        }

        private void StartMediumClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DELETECONFIRMATION("medium"));
        }

        private void StartHardClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DELETECONFIRMATION("hard"));
        }
    }
}
