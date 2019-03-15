using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Media;

namespace ZLTablo_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int T = 100;
        private int leftScore;
        private int rightScore;
        private int doubleHits;

        private DispatcherTimer timer;
        private DateTime lastTick;
        private TimeSpan timeLeft;
        private bool matchInProgress;

        private int arena;

        private SoundPlayer sound;

        private Dictionary<string, Gamemode> gamemodes;
        private Gamemode currentGamemode;

        public ShowWindow showWindow;
        public int LeftScore { get { return leftScore; } }
        public int RightScore { get { return rightScore; } }
        public int DoubleHits { get { return doubleHits; } }
        public Gamemode Gamemode { get { return currentGamemode; } }
        public TimeSpan TimeLeft { get { return timeLeft; } }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            WindowState = WindowState.Maximized;

            _restartCmd = new RestartCmd(this);
            _soundChangeCmd = new SoundChangeCmd(this);
            _gamemodeChangeCmd = new GamemodeChangeCmd(this);
            _secondWindowCmd = new SecondWindowCmd(this);
            _arenaChangeCmd = new ArenaChangeCmd(this);
            _exitCmd = new ExitCmd(this);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(T);
            timer.Tick += TimerTick;

            gamemodes = new Dictionary<string, Gamemode>();
            gamemodes.Add("Рапира", new Gamemode("Рапира", 180, true, 15, 3));
            gamemodes.Add("Меч", new Gamemode("Меч", 180, true, 8, 3));

            _gamemodeChangeCmd.Execute("Рапира");

            sound = new SoundPlayer("Sound/timeout2.wav");
            sound.Load();
            leftScore = 0;
            rightScore = 0;
            UpdateScore();

            arena = 1;

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void SecondWindow ()
        {
            if (showWindow != null)
            {
                showWindow.Close();
            }
            showWindow = new ShowWindow
            {
                Owner = this
            };
            showWindow.Show();
            showWindow.UpdateColor();
            showWindow.UpdateScore();
            showWindow.UpdateTimer();
        }

        public void swapColor ()
        {
            Brush b = Left.Background;
            Left.Background = Right.Background;
            Right.Background = b;
            b = ScoreLeftTextBlock.Foreground;
            ScoreLeftTextBlock.Foreground = ScoreRightTextBlock.Foreground;
            ScoreRightTextBlock.Foreground = b;
            if (showWindow != null) showWindow.UpdateColor();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan delta = now - lastTick;
            lastTick = now;
            if (timeLeft < delta)
            {
                timeLeft = TimeSpan.Zero;
                matchInProgress = false;
                timer.Stop();
                sound.Play();
            }
            else
            {
                timeLeft -= delta;
            }
            UpdateTimer();
        }
        private void UpdateTimer ()
        {
            TimerTextBlock.Text = String.Format("{0}:{1:00},{2:00}", 
                                                timeLeft.Minutes, 
                                                timeLeft.Seconds, 
                                                timeLeft.Milliseconds / 10);
            if (showWindow != null) showWindow.UpdateTimer();
        }
        private void UpdateScore ()
        {
            ScoreLeftTextBlock.Text = leftScore.ToString();
            ScoreRightTextBlock.Text = rightScore.ToString();
            if (currentGamemode.CountScoreGap)
            {
                if (leftScore >= rightScore + currentGamemode.MaxScoreGap)
                {
                    Right.Background = Brushes.Black;
                }
                else if (rightScore >= leftScore + currentGamemode.MaxScoreGap)
                {
                    Left.Background = Brushes.Black;
                }
                else
                {
                    Left.Background = Brushes.Transparent;
                    Right.Background = Brushes.Transparent;
                }
            }
            else
            {
                Left.Background = Brushes.Transparent;
                Right.Background = Brushes.Transparent;
            }
            if (showWindow != null) showWindow.UpdateScore();
        }
        private void UpdateDoubleHits()
        {
            if (currentGamemode.CountDoubleHits)
            {
                DoubleHitsTextBlock.Visibility = Visibility.Visible;
                DoubleHitsTextBlock.Text = String.Format("Обоюдки: {0}", doubleHits);
                if (doubleHits >= currentGamemode.MaxDoubleHits)
                {
                    DoubleHitsTextBlock.Background = Brushes.Red;
                }
                else
                {
                    DoubleHitsTextBlock.Background = Brushes.White;
                }
            }
            else
            {
                DoubleHitsTextBlock.Visibility = Visibility.Hidden;
            }
            if (showWindow != null) showWindow.UpdateDoubleHits();
        }

        #region Commands

        RestartCmd _restartCmd;
        public RestartCmd RestartCommand
        {
            get { return _restartCmd; }
        }
        public class RestartCmd : ICommand
        {
            MainWindow w;
            public RestartCmd(MainWindow window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                w.leftScore = w.rightScore = w.doubleHits = 0;
                w.timeLeft = w.currentGamemode.TotalTime;
                w.timer.Stop();
                w.matchInProgress = true;
                w.UpdateScore();
                w.UpdateTimer();
                w.UpdateDoubleHits();
            }
        }


        private void True_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        SecondWindowCmd _secondWindowCmd;
        public SecondWindowCmd SecondWindowCommand
        {
            get { return _secondWindowCmd; }
        }
        public class SecondWindowCmd : ICommand
        {
            MainWindow w;
            public SecondWindowCmd(MainWindow window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                if (w.showWindow != null)
                {
                    w.showWindow.Close();
                }
                w.showWindow = new ShowWindow
                {
                    Owner = w
                };
                w.showWindow.Show();
                w.showWindow.UpdateColor();
                w.showWindow.UpdateScore();
                w.showWindow.UpdateTimer();
                w.showWindow.UpdateDoubleHits();
            }
        }

        private void MaxShowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (showWindow != null)
            {
                showWindow.WindowState = System.Windows.WindowState.Maximized;
            }
        }


        GamemodeChangeCmd _gamemodeChangeCmd;
        public GamemodeChangeCmd GamemodeChangeCommand
        {
            get { return _gamemodeChangeCmd; }
        }
        public class GamemodeChangeCmd : ICommand
        {
            MainWindow w;
            public GamemodeChangeCmd(MainWindow window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                w.currentGamemode = w.gamemodes[(string) parameter];
                w._restartCmd.Execute(null);
            }
        }

        SoundChangeCmd _soundChangeCmd;
        public SoundChangeCmd SoundChangeCommand
        {
            get { return _soundChangeCmd; }
        }
        public class SoundChangeCmd : ICommand
        {
            MainWindow w;
            public SoundChangeCmd(MainWindow window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public string SecretSound { get { return "Sound/timeout_secret.wav"; } }
            public void Execute(object parameter)
            {
                w.sound = new SoundPlayer((String)parameter);
                /*
                int num = (int) parameter;
                if (num == 0)
                {
                    w.sound = new SoundPlayer("Sound/timeout_secret.wav");
                }
                else if(num == 1)
                {
                    w.sound = new SoundPlayer("Sound/timeout1.wav");
                }
                else if (num == 2)
                {
                    w.sound = new SoundPlayer("Sound/timeout2.wav");
                }
                else if (num == 3)
                {
                    w.sound = new SoundPlayer("Sound/timeout3.wav");
                }
                else if (num == 4)
                {
                    w.sound = new SoundPlayer("Sound/timeout4.wav");
                }
                */
                w.sound.Load();
            }
        }

        ArenaChangeCmd _arenaChangeCmd;
        public ArenaChangeCmd ArenaChangeCommand
        {
            get { return _arenaChangeCmd; }
        }
        public class ArenaChangeCmd : ICommand
        {
            MainWindow w;
            public ArenaChangeCmd(MainWindow window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                w.arena = (Int32)parameter;
                w.ArenaTextBlock.Text = String.Format("Ристалище {0}", w.arena);
                if (w.showWindow != null)
                {
                    w.showWindow.ArenaTextBlock.Text = String.Format("Ристалище {0}", w.arena);
                }
            }
        }

        ExitCmd _exitCmd;
        public ExitCmd ExitCommand
        {
            get { return _exitCmd; }
        }
        public class ExitCmd : ICommand
        {
            MainWindow w;
            public ExitCmd(MainWindow window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                if (w.showWindow != null) w.showWindow.Close();
                w.Close();
            }
        }

        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }


        #endregion

        #region Controls
        /*
         *  F1,F2,F3 или Q,W,E - прибавить левому бойцу 1, 2, или 3 балла
            F4 или Z - снять 1 балл
            F12,F11,F10 или P,O,I - прибавить правому бойцу 1, 2, или 3 балла
            F9 или M- снять 1 балл

            F5 - отметить обоюдку
            F6 - убрать обоюдку
            F8 - перезапуск матча
            F7 или Y - последний сход

            Пробел - пауза

            F - прибавить 1 секунду
            G - вычесть 1 секунду
            J - добавить 1 минуту
            T - поменять местами цвета

            Все буквы латинские
         * */
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (matchInProgress)
                {
                    if (timer.IsEnabled)
                    {
                        timer.Stop();
                    }
                    else
                    {
                        timer.Start();
                        lastTick = DateTime.Now;
                    }
                }
                else if (timeLeft > TimeSpan.Zero)
                {
                    timer.Start();
                    lastTick = DateTime.Now;
                }
            }
            else if (e.Key == Key.Q || e.Key == Key.F1)
            {
                leftScore += 1;
                UpdateScore();
            }
            else if (e.Key == Key.W || e.Key == Key.F2)
            {
                leftScore += 2;
                UpdateScore();
            }
            else if (e.Key == Key.E || e.Key == Key.F3)
            {
                leftScore += 3;
                UpdateScore();
            }
            else if (e.Key == Key.Z || e.Key == Key.F4)
            {
                if (leftScore > 0)
                {
                    leftScore--;
                    UpdateScore();
                }
            }
            else if (e.Key == Key.I || e.Key == Key.System)
            {
                rightScore += 3;
                UpdateScore();
            }
            else if (e.Key == Key.O || e.Key == Key.F11)
            {
                rightScore += 2;
                UpdateScore();
            }
            else if (e.Key == Key.P || e.Key == Key.F12)
            {
                rightScore += 1;
                UpdateScore();
            }
            else if (e.Key == Key.M || e.Key == Key.F9)
            {
                if (rightScore > 0)
                {
                    rightScore--;
                    UpdateScore();
                }
            }
            else if (e.Key == Key.T)
            {
                swapColor();
            }
            else if (e.Key == Key.F)
            {
                timeLeft += new TimeSpan(0, 0, 1);
                matchInProgress = true;
                UpdateTimer();
            }
            else if (e.Key == Key.G)
            {
                timeLeft -= new TimeSpan(0, 0, 1);
                matchInProgress = true;
                UpdateTimer();
            }
            else if (e.Key == Key.J)
            {
                timeLeft += new TimeSpan(0, 1, 0);
                matchInProgress = true;
                UpdateTimer();
            }
            else if (e.Key == Key.F8)
            {
                _restartCmd.Execute(null);
            }
            else if (e.Key == Key.F5)
            {
                doubleHits++;
                UpdateDoubleHits();
            }
            else if (e.Key == Key.F6)
            {
                if (doubleHits > 0)
                {
                    doubleHits--;
                    UpdateDoubleHits();
                }
            }
            else if (e.Key == Key.Y || e.Key == Key.F7)
            {
                timeLeft = new TimeSpan(0, 1, 0);
                matchInProgress = true;
                UpdateTimer();
            }
        }
        #endregion
    }
}
