﻿
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class TreeWindow : Window
    {
        //======================================================================
        public TreeWindow()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;

            InitializeComponent();
        }

        //======================================================================
        public void Initialize(MainWindow mainWindow)
        {
            Top    = mainWindow.Top;
            Left   = mainWindow.Left;
            Height = mainWindow.Height;
            Width  = App.Model.Settings.TreeWidth;
        }

        //======================================================================
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow?.NotifyChildWindowClosing(this);
        }

        //======================================================================
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}