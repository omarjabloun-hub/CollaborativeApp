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

namespace CollaborativeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // on focus try to take access from rabbitmq
        // on lost focus try to release access from rabbitmq
        // on close try to release access from rabbitmq
        public void OnFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("OnFocus");
            
        }
        public void OnLostFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("OnLostFocus");
        }
        public void OnClose(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("OnClose");
        }


        
    }
}