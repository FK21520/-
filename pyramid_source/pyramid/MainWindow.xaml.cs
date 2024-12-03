using System.Windows;

//メイン画面
namespace pyramid
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 最初に表示するページを設定
            MainFrame.Navigate(new START());
        }
    }
}