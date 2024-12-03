using System.IO;
using System.Windows;
using System.Windows.Controls;
//スコアページ
namespace pyramid
{
    public partial class SCORE : Page
    {
        //ファイルパスをクラス内で渡す
        private string _file_path;
        private TimeSpan _time;
        private int insert_index = -1; // ランキング挿入位置
        public SCORE(TimeSpan timespan, string param)
        {
            InitializeComponent();
            _time = timespan; // プロパティに値を設定
            time_text_block.Text = $"タイム: {_time.TotalSeconds:F2} 秒";
            
            _file_path = SelectionFile(param);
            LoadRanking(_file_path);
        }

        //難易度に応じてファイルパスを変更
        private string SelectionFile(string param)
        {
            switch (param)
            {
                case "easy": return "./ranking_easy.txt";
                case "medium": return "./ranking_medium.txt";
                case "hard": return "./ranking_hard.txt";
                default:
                    throw new InvalidOperationException("無効な難易度指定です。");
            }
        }

        // ランキングを読み込んで表示
        private void LoadRanking(string file_path)
        {
            if (!File.Exists(file_path))
            {
                File.WriteAllText(file_path, "1位 {name} {Time}秒");
                username_panel.Visibility = Visibility.Visible;
                
            }

            // ランキングデータを読み込む
            var rankings = new List<(int Rank, string Name, TimeSpan _time)>();
            foreach (var line in File.ReadLines(file_path))
            {
                var parts = line.Split(' ');
                if (parts.Length >= 3 && TimeSpan.TryParse(parts[2].Replace("秒", ""), out var parsedTime))
                {
                    rankings.Add((int.Parse(parts[0].Replace("位", "")), parts[1], parsedTime));
                }
            }

            // ランキングをソートして上位10位まで取得
            rankings = rankings.OrderBy(r => r._time).Take(10).ToList();
            ranking_list_box.ItemsSource = rankings.Select(r => $"{r.Rank}位 {r.Name} {r._time.TotalSeconds:F2}秒");

            // 新しいタイムがランキングに入るか判定
            if (rankings.Count < 10 || _time < rankings.Last()._time)
            {
                insert_index = rankings.FindIndex(r => _time < r._time);
                insert_index = insert_index == -1 ? rankings.Count : insert_index;
                username_panel.Visibility = Visibility.Visible;
            }
            else
            {
                username_panel.Visibility = Visibility.Collapsed;
                return_panel.Visibility = Visibility.Visible;
            }
        }

        // 保存ボタンのクリックイベント
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            string username = username_text_box.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("名前を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 現在のランキングを取得
                var lines = File.Exists(_file_path) ? File.ReadAllLines(_file_path).ToList() : new List<string>();
                var rankings = new List<(int Rank, string Name, TimeSpan _time)>();

                foreach (var line in lines)
                {
                    var parts = line.Split(' ');
                    if (parts.Length >= 3 && TimeSpan.TryParse(parts[2].Replace("秒", ""), out var parsedTime))
                    {
                        rankings.Add((int.Parse(parts[0].Replace("位", "")), parts[1], parsedTime));
                    }
                }

                // 新しいタイムを挿入
                rankings.Insert(insert_index, (insert_index + 1, username, _time));

                // 上位10位までに絞る
                rankings = rankings.OrderBy(r => r._time).Take(10).ToList();

                // 順位を振り直してファイルに保存
                var updatedLines = rankings.Select((r, index) => $"{index + 1}位 {r.Name} {r._time}秒").ToList();
                File.WriteAllLines(_file_path, updatedLines);
                
                // ランキングを再読み込み
                LoadRanking(_file_path);
                username_panel.Visibility = Visibility.Collapsed;
                return_panel.Visibility = Visibility.Visible;
                status_text_block.Text = "ランキングを更新しました！";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ランキングの保存に失敗しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private void UsernameTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            // プレースホルダーの表示を切り替え
            placeholder_text_block.Visibility = string.IsNullOrEmpty(username_text_box.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void ReturnButtonClick(object sender, RoutedEventArgs e)
        {       
            if (NavigationService != null)
            {
                e.Handled = true;
                NavigationService.Navigate(new START());
            }
            else
            {
                MessageBox.Show("ナビゲーションサービスが利用できません。");
            }
        }
    }
}
