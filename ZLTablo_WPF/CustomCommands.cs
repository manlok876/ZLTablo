using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZLTablo_WPF
{
    public class MyAppCommands
    {
        public static RoutedUICommand RestartCmd = new RoutedUICommand("Начать матч заново",
                                                  "RestartCmd",
                                                  typeof(MyAppCommands));
        public static RoutedUICommand SecondWindowCmd = new RoutedUICommand("Показать второе окно",
                                                  "SecondWindowCmd",
                                                  typeof(MyAppCommands));
        public static RoutedUICommand MaxShowWindowCmd = new RoutedUICommand("Развернуть второе окно",
                                                  "MaxShowWindowCmd",
                                                  typeof(MyAppCommands));


        public static RoutedUICommand ClassicTimeCmd = new RoutedUICommand("Начать матч заново",
                                                  "ClassicTimeCmd",
                                                  typeof(MyAppCommands));
        public static RoutedUICommand RapierTimeCmd = new RoutedUICommand("Начать матч заново",
                                                  "RapierTimeCmd",
                                                  typeof(MyAppCommands));
        public static RoutedUICommand ExitCmd = new RoutedUICommand("Выйти",
                                                  "ExitCmd",
                                                  typeof(MyAppCommands));

        public static RoutedUICommand HelpCmd = new RoutedUICommand("Помощь",
                                                  "HelpCmd",
                                                  typeof(MyAppCommands));
    }
}
