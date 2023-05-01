using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TextEditor2
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

        private void CreateUI()
        {
            // Create the main grid
            Grid mainGrid = new Grid();
            mainGrid.Margin = new Thickness(0, 0, 0, 29);

            // Define the row and column definitions for the grid
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            // Create the first TextBlock
            TextBlock textBlock1 = new TextBlock();
            textBlock1.Text = "Section 1";
            textBlock1.FontSize = 16;
            textBlock1.FontWeight = FontWeights.Bold;
            textBlock1.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock1.Margin = new Thickness(10);
            Grid.SetRow(textBlock1, 0);
            Grid.SetColumn(textBlock1, 0);
            mainGrid.Children.Add(textBlock1);

            // Create the second TextBlock
            TextBlock textBlock2 = new TextBlock();
            textBlock2.Text = "Section 2";
            textBlock2.FontSize = 16;
            textBlock2.FontWeight = FontWeights.Bold;
            textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock2.Margin = new Thickness(10);
            Grid.SetRow(textBlock2, 0);
            Grid.SetColumn(textBlock2, 1);
            mainGrid.Children.Add(textBlock2);

            // Create the first TextBox
            TextBox textBox1 = new TextBox();
            textBox1.Name = "TextBox1";
            textBox1.Margin = new Thickness(10, 10, 10, 60);
            textBox1.TextWrapping = TextWrapping.Wrap;
            textBox1.AcceptsReturn = true;
            textBox1.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            textBox1.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
            textBox1.Foreground = Brushes.White;
            textBox1.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 122, 204));
            textBox1.BorderThickness = new Thickness(1);
            textBox1.TextChanged += TextBox_TextChanged;
            Grid.SetRow(textBox1, 1);
            Grid.SetColumn(textBox1, 0);
            mainGrid.Children.Add(textBox1);
            // Create the second TextBox
            TextBox textBox2 = new TextBox();
            textBox2.Name = "TextBox2";
            textBox2.Margin = new Thickness(10, 10, 10, 60);
            textBox2.TextWrapping = TextWrapping.Wrap;
            textBox2.AcceptsReturn = true;
            textBox2.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            textBox2.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
            textBox2.Foreground = Brushes.White;
            textBox2.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 122, 204));
            textBox2.BorderThickness = new Thickness(1);

            Grid.SetRow(textBox2, 1);
            Grid.SetColumn(textBox2, 1);
            mainGrid.Children.Add(textBox2);

            // Set the main window's content to the main grid
            this.Content = mainGrid;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // send message to RabbitMQ queue
            // example code here
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "task_queue_2",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = TextBox2.Text;
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: string.Empty,
                routingKey: "task_queue_2",
                basicProperties: properties,
                body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "task_queue_2",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Dispatcher.Invoke(() => TextBox2.Text = message);
                Console.WriteLine($" [x] Received {message}");
            };
            channel.BasicConsume(queue: "task_queue_2",
                                    autoAck: true,
                                    consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            
        }
    }
}