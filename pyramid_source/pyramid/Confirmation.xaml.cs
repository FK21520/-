using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Input;

//データを消すための確認画面
namespace pyramid
{
    public partial class DELETECONFIRMATION : Page
    {
        private string _file_path;
        private string characters_disolay; //表示するための文字変換
        public DELETECONFIRMATION(string param)
        {
            InitializeComponent();
            characters_disolay = ConversinCharacters(param);
            text_block.Text = $"{characters_disolay}のデータを消去しますか?";
            SelectionFile(param);
            _file_path = SelectionFile(param);
        }

        private string SelectionFile(string param)
        {
            //難易度の選択
            switch (param)
            {
                case "easy": return "./ranking_easy.txt";
                case "medium": return "./ranking_medium.txt";
                case "hard": return "./ranking_hard.txt";
                default:
                    throw new InvalidOperationException("無効な難易度指定です。");
            }
        }

        private string ConversinCharacters(string param)
        {
            switch (param)
            {
                case "easy": return "初級";
                case "medium": return "中級";
                case "hard": return "上級";
                default:
                    throw new InvalidOperationException("無効な難易度指定です。");
            }
        }

        private void YesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // ファイルが存在するか確認
                if (File.Exists(_file_path))
                {
                    // ファイルを削除
                    File.Delete(_file_path);
                    text_block.Text = $"{characters_disolay}のデータを消去しました";                   
                }
                else
                {
                    text_block.Text = $"{characters_disolay}のデータ削除済みです";
                }
            }
            catch (Exception ex)
            {
                // エラーメッセージを表示
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        //戻るボタン
        private void NoClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DELETEWINDOWPAGE());
        }
        
        private void ReturnButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DELETEWINDOWPAGE());
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                // このイベントを他に伝播させない
                e.Handled = true;
            }
        }
    }
}
