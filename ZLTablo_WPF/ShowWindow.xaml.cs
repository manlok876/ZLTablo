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
            int leftScore = ((MainWindow)Owner).LeftScore;
            int rightScore = ((MainWindow)Owner).RightScore;
            ScoreRightTextBlock.Text = leftScore.ToString();
            ScoreLeftTextBlock.Text = rightScore.ToString();
        }
        public void UpdateColor()
        {
            Left.Background = ((MainWindow)Owner).Right.Background;
            ScoreLeftTextBlock.Foreground = ((MainWindow)Owner).ScoreRightTextBlock.Foreground;
            Right.Background = ((MainWindow)Owner).Left.Background;
            ScoreRightTextBlock.Foreground = ((MainWindow)Owner).ScoreLeftTextBlock.Foreground;
        }

        public void Restart()
        {
            UpdateScore();
            UpdateTimer();
        }
    }
}
