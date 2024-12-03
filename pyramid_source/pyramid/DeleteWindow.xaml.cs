using System.Windows;

//データ所虚画面の基本ページ
namespace pyramid
{
    public partial class DELETEWINDOW: Window
    {
        public  DELETEWINDOW()
        { 
            InitializeComponent();
            delete_frame.Navigate(new DELETEWINDOWPAGE());
        }
    }
}
