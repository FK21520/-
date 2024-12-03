using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace pyramid
{
    public partial class App : Application
    {
       
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            //// 特定のページを表示する
            //TimeSpan testTime = TimeSpan.FromSeconds(127);
            //string con = "easy";

            //var myPage = new SCORE(testTime, con); // MyPage は別のクラス
            ////var myPage = new DisplayRank();
            //var window = new Window
            //{
            //    Content = myPage,
            //    Title = "My Custom Page",
            //    Width = 800,  // ウィンドウの幅
            //    Height = 550, // ウィンドウの高さ
            //};
            //window.Show();
        }
    }

}
