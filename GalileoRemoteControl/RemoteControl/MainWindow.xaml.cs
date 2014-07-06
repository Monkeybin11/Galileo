using System.Windows;
using RemoteControl.Galileo;

namespace RemoteControl
{
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private GalileoClient client;
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                client = new GalileoClient("GalileoEndpoint", HostTextBox.Text);
                client.Open();
                StateTextBlock.Text = client.State.ToString();
                if (client.Start())
                {
                    StateTextBlock.Text = "Ok";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int speed = int.Parse(SpeedTextBox.Text);
                client.Move(speed, 0);
                SpeedTextBox.Text = (speed + 1).ToString();
            }
            catch (Exception)
            {
                
                throw;
            }
             
        }
    }
}
