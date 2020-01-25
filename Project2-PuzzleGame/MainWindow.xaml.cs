using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Project2_PuzzleGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board Puzzle = new Board();       
        int size = 3;
        DispatcherTimer timer = new DispatcherTimer();
        static public int time; // seconds
        MediaPlayer mediaPlayer = new MediaPlayer();
        MediaPlayer soundtrack = new MediaPlayer();
        BrushConverter bc = new BrushConverter();
        static public string filename = "save";
        

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Load
        private void LoadBoard()
        {
            int ButtonWidth = 450 / size; // 450
            int ButtonHeight = 360 / size; // 360
            int Padding = 0;
            int TopOffset = 20;
            int LeftOffset = 25;

            Puzzle.Init(size, size);

            for (int i = 0; i < Puzzle.Rows; i++)
            {
                for (int j = 0; j < Puzzle.Columns; j++)
                {
                    Button btn = Puzzle.showBtn[i, j];
                    btn.Click += Button_Click;
                    btn.Width = ButtonWidth;
                    btn.Height = ButtonHeight;
                    btn.Padding = new Thickness(Padding);

                    // Dua vao UI
                    uiCanvas.Children.Add(btn);
                    Canvas.SetLeft(btn, LeftOffset + j * (ButtonWidth + Padding));
                    Canvas.SetTop(btn, TopOffset + i * (ButtonHeight + Padding));
                }
            }
        }

        private int chooseRandomButton(int i, int j)
        {
            List<int> ar = new List<int>();
            //-2: trai, -1: tren, 1: phai, 2: duoi
            if (i == 0)
            {
                if (j == 0)
                {
                    ar.Add(1);
                    ar.Add(2);
                }
                else if (j == Puzzle.Columns - 1)
                {
                    ar.Add(-2);
                    ar.Add(2);
                }
                else
                {
                    ar.Add(1);
                    ar.Add(2);
                    ar.Add(-2);
                }
            }
            else if (i == Puzzle.Rows - 1)
            {
                if (j == 0)
                {
                    ar.Add(1);
                    ar.Add(-1);
                }
                else if (j == Puzzle.Columns - 1)
                {
                    ar.Add(-2);
                    ar.Add(-1);
                }
                else
                {
                    ar.Add(1);
                    ar.Add(-1);
                    ar.Add(-2);
                }
            }
            else
            {
                if (j == 0)
                {
                    ar.Add(1);
                    ar.Add(-1);
                    ar.Add(2);
                }
                else if (j == Puzzle.Columns - 1)
                {
                    ar.Add(-2);
                    ar.Add(-1);
                    ar.Add(2);
                }
                else
                {
                    ar.Add(1);
                    ar.Add(-1);
                    ar.Add(-2);
                    ar.Add(2);
                }
            }
            //-2: trai, -1: tren, 1: phai, 2: duoi
            Random r = new Random();
            return ar[r.Next(0, ar.Count())];
        }

        private void randomThePuzzle()
        {
            int i = Puzzle.Rows - 1;
            int j = Puzzle.Columns - 1;
            int soLanTron = Puzzle.Rows * 15;
            for (int index = 0; index < soLanTron; index++)
            {
                clickNextPuzzleButton(ref i, ref j);
            }
        }

        private void clickNextPuzzleButton(ref int i, ref int j)
        {
            int randomButtonIndex = chooseRandomButton(i, j);
            //-2: trai, -1: tren, 1: phai, 2: duoi
            if (randomButtonIndex == -2)
            {
                j--;
            }
            else if (randomButtonIndex == -1)
            {
                i--;
            }
            else if (randomButtonIndex == 1)
            {
                j++;
            }
            else if (randomButtonIndex == 2)
            {
                i++;
            }
            performClick(i, j,false);
        }

        private void LoadImage()
        {

            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image a = System.Drawing.Image.FromFile(path + "original_1.png");
            a = MyImage.ResizeImage(a, 450, 360);

            System.Drawing.Image null_image = System.Drawing.Image.FromFile(path + "null-image.jpg");
            null_image = MyImage.ResizeImage(null_image, 150, 120);

            int width = 450 / size;
            int height = 360 / size;

            Random ra = new Random();
            int min = 1;
            int max = size * size - 1;
            List<int> flag = new List<int>();

            for(int i = min; i <= max; i++)
            {
                flag.Add(i);
            }

            //flag = Utils.ArrangeRandomArray(flag);


            for (int i = 0; i < flag.Count; i++)
            {
                int temp_1 = flag[i] -1;
                int i_i = temp_1 / size;
                int i_j = temp_1 % size;



                System.Drawing.Rectangle b1 = new System.Drawing.Rectangle(i_j * width, i_i * height, width - 2, height - 2);
                System.Drawing.Image child1 = MyImage.CropImage(a, b1);
                BitmapImage bm1 = MyImage.ConvertImageToBitmap(child1);
                Image _img = new Image();
                Puzzle.chess[i / size, i % size] = flag[i];
                _img.Source = bm1;


                StackPanel stackPnl = new StackPanel();
                stackPnl.Children.Add(_img);

                Puzzle.showBtn[i / size, i % size].Content = stackPnl;

            }

            System.Drawing.Rectangle b = new System.Drawing.Rectangle(0, 0, width - 2, height - 2);
            System.Drawing.Image child = MyImage.CropImage(null_image, b);
            BitmapImage bm = MyImage.ConvertImageToBitmap(child);

            StackPanel temp = new StackPanel();
            Image img = new Image(); img.Source = bm;
            temp.Children.Add(img);
            Puzzle.showBtn[size - 1, size - 1].Content = temp;

            Puzzle.chess[size - 1, size - 1] = 0;
            Puzzle.showBtn[size - 1, size - 1].IsEnabled = false;
        }

        private void LoadOriginalImage()
        {
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image a = System.Drawing.Image.FromFile(path + "original_1.png");
            a = MyImage.ResizeImage(a, 250, 220);
            BitmapImage bm1 = MyImage.ConvertImageToBitmap(a);

            original_image.Source = bm1;
        }

        private void LoadSoundTrack()
        {
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path_audio = absolute_path + "Audio\\";

            soundtrack.Open(new Uri(path_audio + "soundtrack.mp3"));
            soundtrack.Play();
            soundtrack.MediaEnded += SoundTrackEnd;
        }

        private void LoadGame()
        {
            SetMode();
            LoadBoard();
            LoadImage();
            randomThePuzzle();
            LoadOriginalImage();
            LoadSoundTrack();
            SetTimer();
        }

        private void SetMode()
        {          
            string type = mode.SelectedItem.ToString();

            if (type == "Easy")
            {
                size = 3;
            }
            else if (type == "Normal")
            {
                size = 4;
            }
            else
            {
                size = 5;
            }
        }
        #endregion

        private void performClick(int i, int j, bool check)
        {
            int width = 450 / size;
            int height = 360 / size;

            // set image
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image null_image = System.Drawing.Image.FromFile(path + "null-image.jpg");
            null_image = MyImage.ResizeImage(null_image, 150, 120);

            System.Drawing.Rectangle b = new System.Drawing.Rectangle(0, 0, width - 2, height - 2);
            System.Drawing.Image child = MyImage.CropImage(null_image, b);
            BitmapImage bm = MyImage.ConvertImageToBitmap(child);

            // set audio
            string path_audio = absolute_path + "Audio\\";
            mediaPlayer.Open(new Uri(path_audio + "click.mp3"));

            if (check == true)
            {
                mediaPlayer.Play();
            }


            // kiểm tra 4 hướng          
            //up
            if (i > 0)
            {
                int n_i = i - 1,
                    n_j = j;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }
            // down
            if (i < size - 1)
            {
                int n_i = i + 1,
                    n_j = j;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }
            // right
            if (j < size - 1)
            {
                int n_i = i,
                    n_j = j + 1;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }
            // left
            if (j > 0)
            {
                int n_i = i,
                    n_j = j - 1;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }

        End:
            if (Puzzle.checkWin() && check == true)
            {
                timer_label.Content = "You Win !!!";
                timer_label.Foreground = Brushes.Red;

                timer.Stop();
                soundtrack.Stop();

                MediaPlayer win = new MediaPlayer();
                win.Open(new Uri(path_audio + "win.mp3"));
                win.Play();

                for (int ii = 0; ii < size; ii++)
                {
                    for (int jj = 0; jj < size; jj++)
                    {
                        Puzzle.showBtn[ii, jj].IsEnabled = false;
                    }
                }

                startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
                pauseBtn.Background = Brushes.SkyBlue;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var (i, j) = btn.Tag as Tuple<int, int>;

            performClick(i, j, true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> lines = new List<string>(); lines.Add("Easy"); lines.Add("Normal"); lines.Add("Hard");
            mode.ItemsSource = lines;
            mode.SelectedIndex = 0;

            LoadGame();

            soundtrack.Stop();

            // Pause game     
            timer.Stop();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Puzzle.showBtn[i, j].IsEnabled = false;
                }
            }
            upBtn.IsEnabled = false;
            downBtn.IsEnabled = false;
            leftBtn.IsEnabled = false;
            rightBtn.IsEnabled = false;

            startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
            pauseBtn.Background = Brushes.SkyBlue;
 
        }
     
        private void SetTimer()
        {
            timer_label.Content = "03:00";
            timer = new DispatcherTimer();
            timer_label.Foreground = Brushes.Black;
            time = 180;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Stop();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if(time <=15)
            {
                timer_label.Foreground = Brushes.Red;
            }

            if(time > -1)
            {
                string temp = "";
                if (time % 60 >= 10)
                {
                    temp = string.Format($"0{time / 60}:{time % 60}");
                }
                else
                {
                    temp = string.Format($"0{time / 60}:0{time % 60}");
                }
                timer_label.Content = temp;
                time--;
            }
            else
            {
                soundtrack.Stop();
                timer.Stop();

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Puzzle.showBtn[i, j].IsEnabled = false;
                    }
                }

                MessageBox.Show("Time was out!\nYou Lose", "Time out", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            soundtrack.Play();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (Puzzle.chess[i, j] != 0)
                    {
                        Puzzle.showBtn[i, j].IsEnabled = true;
                    }
                }
            }
            upBtn.IsEnabled = true;
            downBtn.IsEnabled = true;
            leftBtn.IsEnabled = true;
            rightBtn.IsEnabled = true;

            startBtn.Background =  Brushes.SkyBlue;
            pauseBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            soundtrack.Stop();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Puzzle.showBtn[i, j].IsEnabled = false;
                }
            }
            upBtn.IsEnabled = false;
            downBtn.IsEnabled = false;
            leftBtn.IsEnabled = false;
            rightBtn.IsEnabled = false;

            startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
            pauseBtn.Background = Brushes.SkyBlue;
        }

        private void SoundTrackEnd(object sender, EventArgs e)
        {
            MediaPlayer media = sender as MediaPlayer;
            media.Position = TimeSpan.Zero;
            media.Play();
        }

        private void plagainBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadGame();

            // Pause game     
            timer.Stop();
            soundtrack.Stop();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Puzzle.showBtn[i, j].IsEnabled = false;
                }
            }
            upBtn.IsEnabled = false;
            downBtn.IsEnabled = false;
            leftBtn.IsEnabled = false;
            rightBtn.IsEnabled = false;

            startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
            pauseBtn.Background = Brushes.SkyBlue;

            timer_label.Content = "03:00";
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //pause
            timer.Stop();
            soundtrack.Stop();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Puzzle.showBtn[i, j].IsEnabled = false;
                }
            }

            startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
            pauseBtn.Background = Brushes.SkyBlue;

            Puzzle.Save(filename, time);
            MessageBox.Show("Save sucessfully!!!", "Save", MessageBoxButton.OK);
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            Puzzle.Load(filename);

            //time
            if (time <= 15)
            {
                timer_label.Foreground = Brushes.Red;
            }

            if (time > -1)
            {
                string temp = "";
                if (time % 60 >= 10)
                {
                    temp = string.Format($"0{time / 60}:{time % 60}");
                }
                else
                {
                    temp = string.Format($"0{time / 60}:0{time % 60}");
                }
                timer_label.Content = temp;
            }

            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image a = System.Drawing.Image.FromFile(path + "original_1.png");
            a = MyImage.ResizeImage(a, 450, 360);

            size = Puzzle.Rows;
            int width = 450 / size;
            int height = 360 / size;

            // copy chess to temp
            int[,] temp_a = new int[size, size];
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    temp_a[i, j] = Puzzle.chess[i, j];
                }
            }

            LoadBoard();

            // copy chess to chess
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Puzzle.chess[i, j] = temp_a[i, j];
                }
            }


            System.Drawing.Image null_image = System.Drawing.Image.FromFile(path + "null-image.jpg");
            null_image = MyImage.ResizeImage(null_image, width, height);

        

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int temp = Puzzle.chess[i, j] - 1;
                    int ii = temp / size;
                    int jj = temp % size;

                    if (Puzzle.chess[i,j] == 0)
                    {
                        System.Drawing.Rectangle b = new System.Drawing.Rectangle(0, 0, width-2, height-2);
                        System.Drawing.Image child = MyImage.CropImage(null_image, b);
                        BitmapImage bm = MyImage.ConvertImageToBitmap(child);

                        StackPanel temp1 = new StackPanel();
                        Image img = new Image(); img.Source = bm;
                        temp1.Children.Add(img);
                        Puzzle.showBtn[i, j].Content = temp1;

                        Puzzle.showBtn[i, j].IsEnabled = false;
                    }
                    else
                    {                     
                        System.Drawing.Rectangle b1 = new System.Drawing.Rectangle(jj * width, ii * height, width-2, height-2);
                        System.Drawing.Image child1 = MyImage.CropImage(a, b1);
                        BitmapImage bm1 = MyImage.ConvertImageToBitmap(child1);

                        StackPanel temp1 = new StackPanel();
                        Image img = new Image(); img.Source = bm1;
                        temp1.Children.Add(img);
                        Puzzle.showBtn[i, j].Content = temp1;

                        Puzzle.showBtn[i, j].IsEnabled = true;
                    }
                }
            }

            //pause         
            timer.Stop();
            soundtrack.Stop();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Puzzle.showBtn[i, j].IsEnabled = false;
                }
            }
            upBtn.IsEnabled = false;
            downBtn.IsEnabled = false;
            leftBtn.IsEnabled = false;
            rightBtn.IsEnabled = false;

            startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
            pauseBtn.Background = Brushes.SkyBlue;

            // update mode
            if (size == 3) mode.SelectedIndex = 0;
            else if (size == 4) mode.SelectedIndex = 1;
            else mode.SelectedIndex = 2;

            MessageBox.Show("Loadgame sucessfully!!!", "Load", MessageBoxButton.OK);
        }

        private void mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetMode();
            LoadBoard();
            LoadImage();
            randomThePuzzle();
            LoadOriginalImage();
            LoadSoundTrack();
            time = 180; timer_label.Content = "03:00";
            timer.Stop();

            soundtrack.Stop();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Puzzle.showBtn[i, j].IsEnabled = false;
                }
            }

            startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
            pauseBtn.Background = Brushes.SkyBlue;
        }

        #region Control Button

        private (int,int) FindNullImage()
        {
            int ii = 0,
                jj = 0;

            for(int i = 0; i < Puzzle.Rows; i++)
            {
                for(int j = 0; j < Puzzle.Columns; j++)
                {
                    if (Puzzle.chess[i, j] == 0)
                        return (i, j);
                }
            }

            return (ii, jj);
        }

        private void upBtn_Click(object sender, RoutedEventArgs e)
        {
            var (ii, jj) = FindNullImage();

            int i = ii + 1,
                j = jj;

            int width = 450 / size;
            int height = 360 / size;

            // set image
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image null_image = System.Drawing.Image.FromFile(path + "null-image.jpg");
            null_image = MyImage.ResizeImage(null_image, 150, 120);

            System.Drawing.Rectangle b = new System.Drawing.Rectangle(0, 0, width - 2, height - 2);
            System.Drawing.Image child = MyImage.CropImage(null_image, b);
            BitmapImage bm = MyImage.ConvertImageToBitmap(child);

            // set audio
            string path_audio = absolute_path + "Audio\\";
            mediaPlayer.Open(new Uri(path_audio + "click.mp3"));
            mediaPlayer.Play();


            // kiểm tra 4 hướng          
            //up
            if (i > 0 && i < size)
            {
                int n_i = ii,
                    n_j = jj;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }

        End:
            if (Puzzle.checkWin())
            {
                timer_label.Content = "You Win !!!";
                timer_label.Foreground = Brushes.Red;

                timer.Stop();
                soundtrack.Stop();

                MediaPlayer win = new MediaPlayer();
                win.Open(new Uri(path_audio + "win.mp3"));
                win.Play();

                for (int iii = 0; iii < size; iii++)
                {
                    for (int jjj = 0; jjj < size; jjj++)
                    {
                        Puzzle.showBtn[iii, jjj].IsEnabled = false;
                    }
                }

                startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
                pauseBtn.Background = Brushes.SkyBlue;
            }
        }

        private void downBtn_Click(object sender, RoutedEventArgs e)
        {
            var (ii, jj) = FindNullImage();

            int i = ii - 1,
                j = jj;

            int width = 450 / size;
            int height = 360 / size;

            // set image
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image null_image = System.Drawing.Image.FromFile(path + "null-image.jpg");
            null_image = MyImage.ResizeImage(null_image, 150, 120);

            System.Drawing.Rectangle b = new System.Drawing.Rectangle(0, 0, width - 2, height - 2);
            System.Drawing.Image child = MyImage.CropImage(null_image, b);
            BitmapImage bm = MyImage.ConvertImageToBitmap(child);

            // set audio
            string path_audio = absolute_path + "Audio\\";
            mediaPlayer.Open(new Uri(path_audio + "click.mp3"));
            mediaPlayer.Play();


            // kiểm tra 4 hướng          
            //up
            if (i >= 0 && i < size - 1)
            {
                int n_i = ii,
                    n_j = jj;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }

        End:
            if (Puzzle.checkWin())
            {
                timer_label.Content = "You Win !!!";
                timer_label.Foreground = Brushes.Red;

                timer.Stop();
                soundtrack.Stop();

                MediaPlayer win = new MediaPlayer();
                win.Open(new Uri(path_audio + "win.mp3"));
                win.Play();

                for (int iii = 0; iii < size; iii++)
                {
                    for (int jjj = 0; jjj < size; jjj++)
                    {
                        Puzzle.showBtn[iii, jjj].IsEnabled = false;
                    }
                }

                startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
                pauseBtn.Background = Brushes.SkyBlue;
            }
        }

        private void leftBtn_Click(object sender, RoutedEventArgs e)
        {
            var (ii, jj) = FindNullImage();

            int i = ii,
                j = jj + 1;

            int width = 450 / size;
            int height = 360 / size;

            // set image
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image null_image = System.Drawing.Image.FromFile(path + "null-image.jpg");
            null_image = MyImage.ResizeImage(null_image, 150, 120);

            System.Drawing.Rectangle b = new System.Drawing.Rectangle(0, 0, width - 2, height - 2);
            System.Drawing.Image child = MyImage.CropImage(null_image, b);
            BitmapImage bm = MyImage.ConvertImageToBitmap(child);

            // set audio
            string path_audio = absolute_path + "Audio\\";
            mediaPlayer.Open(new Uri(path_audio + "click.mp3"));
            mediaPlayer.Play();


            // kiểm tra 4 hướng          
            //up
            if (j > 0 && j < size)
            {
                int n_i = ii,
                    n_j = jj;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }

        End:
            if (Puzzle.checkWin())
            {
                timer_label.Content = "You Win !!!";
                timer_label.Foreground = Brushes.Red;

                timer.Stop();
                soundtrack.Stop();

                MediaPlayer win = new MediaPlayer();
                win.Open(new Uri(path_audio + "win.mp3"));
                win.Play();

                for (int iii = 0; iii < size; iii++)
                {
                    for (int jjj = 0; jjj < size; jjj++)
                    {
                        Puzzle.showBtn[iii, jjj].IsEnabled = false;
                    }
                }

                startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
                pauseBtn.Background = Brushes.SkyBlue;
            }
        }

        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {
            var (ii, jj) = FindNullImage();

            int i = ii,
                j = jj - 1;

            int width = 450 / size;
            int height = 360 / size;

            // set image
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Image\\";
            System.Drawing.Image null_image = System.Drawing.Image.FromFile(path + "null-image.jpg");
            null_image = MyImage.ResizeImage(null_image, 150, 120);

            System.Drawing.Rectangle b = new System.Drawing.Rectangle(0, 0, width - 2, height - 2);
            System.Drawing.Image child = MyImage.CropImage(null_image, b);
            BitmapImage bm = MyImage.ConvertImageToBitmap(child);

            // set audio
            string path_audio = absolute_path + "Audio\\";
            mediaPlayer.Open(new Uri(path_audio + "click.mp3"));
            mediaPlayer.Play();


            // kiểm tra 4 hướng          
            //up
            if (j >= 0 && j < size - 1)
            {
                int n_i = ii,
                    n_j = jj;
                if (Puzzle.chess[n_i, n_j] == 0)
                {
                    StackPanel stackPnl = Puzzle.showBtn[i, j].Content as StackPanel;
                    Puzzle.showBtn[n_i, n_j].Content = stackPnl;

                    StackPanel temp = new StackPanel();
                    Image img = new Image(); img.Source = bm;
                    temp.Children.Add(img);
                    Puzzle.showBtn[i, j].Content = temp;

                    Puzzle.chess[n_i, n_j] = Puzzle.chess[i, j];
                    Puzzle.chess[i, j] = 0;

                    Puzzle.showBtn[i, j].IsEnabled = false;
                    Puzzle.showBtn[n_i, n_j].IsEnabled = true;
                    goto End;
                }
            }

        End:
            if (Puzzle.checkWin())
            {
                timer_label.Content = "You Win !!!";
                timer_label.Foreground = Brushes.Red;

                timer.Stop();
                soundtrack.Stop();

                MediaPlayer win = new MediaPlayer();
                win.Open(new Uri(path_audio + "win.mp3"));
                win.Play();

                for (int iii = 0; iii < size; iii++)
                {
                    for (int jjj = 0; jjj < size; jjj++)
                    {
                        Puzzle.showBtn[iii, jjj].IsEnabled = false;
                    }
                }

                startBtn.Background = (Brush)bc.ConvertFrom("#E8E8E8");
                pauseBtn.Background = Brushes.SkyBlue;
            }
        }

        #endregion
    }
}
