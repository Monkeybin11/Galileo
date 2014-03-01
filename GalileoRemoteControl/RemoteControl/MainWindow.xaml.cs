﻿using System.Windows;
using RemoteControl.Galileo;

namespace RemoteControl
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GalileoClient client = new GalileoClient("GalileoEndpoint", HostTextBox.Text);
            client.Open();
            StateTextBlock.Text = client.State.ToString();
            if (client.Start())
            {
                StateTextBlock.Text = "Ok";
            }
        }
    }
}