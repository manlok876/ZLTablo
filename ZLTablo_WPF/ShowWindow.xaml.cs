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
using System.Windows.Shapes;

namespace ZLTablo_WPF
{
    /// <summary>
    /// Interaction logic for ShowWindow.xaml
    /// </summary>
    public partial class ShowWindow : Window
    {
        public ShowWindow()
        {
            InitializeComponent();
        }
        
        public void UpdateTimer()
        {
            TimerTextBlock.Text = ((MainWindow)Owner).TimerTextBlock.Text;
        }
        public void UpdateScore()
        {
            MainWindow mw = ((MainWindow)Owner);
            int leftScore = mw.LeftScore;
            int rightScore = mw.RightScore;
            ScoreRightTextBlock.Text = leftScore.ToString();
            Right.Background = mw.Left.Background;
            ScoreLeftTextBlock.Text = rightScore.ToString();
            Left.Background = mw.Right.Background;
        }
        public void UpdateDoubleHits()
        {
            MainWindow mw = ((MainWindow)Owner);
            if (mw.Gamemode.CountDoubleHits)
            {
                DoubleHitsTextBlock.Visibility = Visibility.Visible;
                DoubleHitsTextBlock.Text = String.Format("Обоюдки: {0}", mw.DoubleHits);
                if (mw.DoubleHits >= mw.Gamemode.MaxDoubleHits)
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
        }
        public void UpdateColor()
        {
            MainWindow mw = ((MainWindow)Owner);
            Left.Background = mw.Right.Background;
            ScoreLeftTextBlock.Foreground = mw.ScoreRightTextBlock.Foreground;
            Right.Background = mw.Left.Background;
            ScoreRightTextBlock.Foreground = mw.ScoreLeftTextBlock.Foreground;
        }

        public void Restart()
        {
            UpdateScore();
            UpdateTimer();
        }
    }
}
