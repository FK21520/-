using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
//ゲーム画面
namespace pyramid
{
    public partial class GAME : Page
    {
        //難易度をクラス内で渡す
        private string _param;

        private int rows = 5; // ピラミッドの行数
        private List<List<TextBox>> text_boxes = new List<List<TextBox>>();//テキストボックス
        private int current_row;
        private int current_col;
        private int this_box_num; //残りのボックスの数
        private int box_num = 1; //ボックスの数
        private int ans; //判定ボックス 
        private int count_box_num = 0;
        private DispatcherTimer? timer;
        private int seconds_elapsed;
        private DateTime start_time;

        public GAME(string param)
        {
            InitializeComponent();
            _param = param; // フィールドに格納
            ans = CalculateSum(rows - 1); // コンストラクタで初期化
            CreatePilamid(param);
        }

        //n*(n+1)/2の計算
        private int CalculateSum(int n)
        {
            return n * (n + 1) / 2;
        }

        private void CreatePilamid(string param)
        {
            text_boxes = new List<List<TextBox>>(); // リストの初期化
            int rmin, rmax;
            
            //難易度
            switch (param)
            {
                case "easy": rmin = 1; rmax = 9; break;
                case "medium": rmin = 10; rmax = 99; break;
                case "hard": rmin = 100; rmax = 250; break;
                default: throw new InvalidOperationException();
            }

            var row_text_boxes = new List<TextBox>();

            for (int i = 0; i<rows; i++)
            {
                //テキストボックスの作成
                StackPanel row_panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 0)
                };

                //かぶせるテキストボックスの作成
                StackPanel coverRowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 0)
                };

                for (int j = 0; j<=i; j++)
                {
                    TextBox text_box = new TextBox
                    {
                        Width = 70,
                        Height = 50,
                        Margin = new Thickness(2),
                        TextAlignment = TextAlignment.Center,
                        IsReadOnly = true  // 全てのテキストボックスを初期は入力不可に
                    };

                    text_box.PreviewTextInput += NumOnly;
                    row_panel.Children.Add(text_box);
                    row_text_boxes.Add(text_box);

                    // カバー用のテキストボックス
                    TextBox cover_box = new TextBox
                    {
                        Width = 70,
                        Height = 50,
                        Margin = new Thickness(2),
                        Background = Brushes.Gray, // 見えないようにカバー
                        IsReadOnly = true
                    };

                    coverRowPanel.Children.Add(cover_box);
                    text_box.FontSize = 30; // フォントサイズを設定

                    Random rand = new Random(); 
                    if(i == rows -1)
                    {
                        text_box.Text = rand.Next(rmin, rmax).ToString();
                    }
                    else
                    {
                        box_num++;
                    }
                }
                game_panel.Children.Add(row_panel);
                text_boxes.Add(row_text_boxes);
                cover_panel.Children.Add(coverRowPanel);
            }
            // 初期の入力可能テキストボックスを設定
            current_row = rows - 1; //行
            current_col = CalculateSum(rows - 2); //列
            this_box_num = current_row;
        }
        private void PageKeyDown(object sender, KeyEventArgs e)
        {
            // タイマーの初期化
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 1秒間隔で更新
            timer.Tick += Timer_Tick;
            
            if (e.Key == Key.Space)
            {
                start_label.Visibility = Visibility.Collapsed;
                EnableTextBox(text_boxes[current_row][current_col]);
                start_time = DateTime.Now;
                // タイマー開始
                timer.Start();
                cover_panel.Visibility = Visibility.Collapsed;
               
            }
        }

        // 入力可能なテキストボックスを有効化
        private void EnableTextBox(TextBox text_box)
        {
            text_box.IsReadOnly = false;
            text_box.Focus();
            text_box.KeyDown += CheckInput;
        }

        // 入力内容を確認し、条件が合えば次のテキストボックスを有効化
        private void CheckInput(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                TextBox? text_box = sender as TextBox;

                if (int.TryParse(text_box?.Text, out int userInput))
                {
                    // 下の行の現在の列の左右にある値を取得
                    int leftValue = int.Parse(text_boxes[current_row][ans].Text);
                    int rightValue = int.Parse(text_boxes[current_row][ans + 1].Text);
                    int sumExpected = leftValue + rightValue;
                    
                    if (userInput == sumExpected) // 正しい値が入力された場合
                    {
                        text_box.IsReadOnly = true;

                        text_box.KeyDown -= CheckInput;
                     
                        // 列インデックスを次に進める
                        if (this_box_num != 1)
                        {
                            current_col++;
                            ans++;
                            count_box_num++;
                            this_box_num--;
                        }
                        else
                        {
                            //インデックスを上に進める
                            current_row--;
                            ans = current_col - count_box_num;
                            current_col = ans - current_row;
                            this_box_num = current_row;
                            count_box_num = 0;
                        }
                        

                        if (current_row > 0) // まだ残っている行がある場合
                        {
                            EnableTextBox(text_boxes[current_row][current_col]);
                        }
                        else
                        {
                            //スコア画面に移動
                            timer?.Stop();
                            TimeSpan finalElapsedTime = DateTime.Now - start_time;
                            NavigationService.Navigate(new SCORE(finalElapsedTime, _param));                          
                        }
                    }
                    else // 不正な値の場合は入力をクリア
                    {
                        text_box.Clear();
                    }
                }
                else
                {
                    text_box?.Clear();
                }
            }
        }

        //数値のみの入力を許可する関数
        private void NumOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$"); // 数値以外を拒否
        }

        // タイマーが1秒ごとに呼び出されるイベント
        private void Timer_Tick(object? sender, EventArgs e)
        {
            seconds_elapsed++;
            time_label.Content = $"経過時間: {seconds_elapsed} 秒";
        }
    }
}