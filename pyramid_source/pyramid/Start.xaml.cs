using System.Windows;
using System.Windows.Controls;

//スタートページ//
namespace pyramid
{
    public partial class START : Page
    
    {
        public START()
        {
            InitializeComponent();
        }

        //難易度を渡す
        private void StartEasyClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GAME("easy"));
        }

        private void StartMediumClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GAME("medium"));
        }

        private void StartHardClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GAME("hard"));
        }

        //消去ウィンドウに移動
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            var delete_window = new DELETEWINDOW();
            delete_window.Show();
        }
    }
}