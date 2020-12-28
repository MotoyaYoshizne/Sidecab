﻿
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class FileTreeWindow : Window
    {
        //======================================================================
        public FileTreeWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            InitializeComponent();

            this.DataContext = App.Presenter.Settings;
            App.Presenter.Settings.PropertyChanged += OnSettingWidthChanged;
        }

        //======================================================================
        public void ShowWithAnimation(Window parentWindow)
        {
            this.Top    = parentWindow.Top;
            this.Height = parentWindow.Height;
            this.Left   = parentWindow.Left;
            this.Width  = App.Presenter.Settings.TreeWidth;

            //------------------------------------------------------------------
            if (App.Presenter.Settings.DockPosition == Data.DockPosition.Left)
            {
                this.border_FileTree.RenderTransformOrigin = new Point(0, 0);
            }
            //------------------------------------------------------------------
            else
            {
                this.border_FileTree.RenderTransformOrigin = new Point(1, 0);
                this.Left -= App.Presenter.Settings.TreeWidth - App.Presenter.Settings.KnobWidth;
            }
            //------------------------------------------------------------------

            var storyboard = TryFindResource("animToShow") as Storyboard;
            if (storyboard != null) { storyboard.Begin(); }

            // Calling Hide() makes the window inactive, so avoid it.
            App.Current.MainWindow.Visibility = Visibility.Collapsed;

            Show();
        }

        //======================================================================
        public void HideWithAnimation()
        {
            var storyboard = TryFindResource("animToHide") as Storyboard;
            if (storyboard != null) { storyboard.Begin(); }
        }

        //======================================================================
        private void OnSettingWidthChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Presenter.Settings.TreeWidth))
            {
                Width = App.Presenter.Settings.TreeWidth;
            }
        }

        //======================================================================
        private void treeWindow_Closing(object sender, CancelEventArgs e)
        {
            // Colse this window if the MainWindow is lost
            var result = (App.Current.MainWindow as MainWindow)
                ?.NotifyChildWindowClosing(this) ?? MainWindow.WindowBehaviorRestriction.CanClose;

            if (result == MainWindow.WindowBehaviorRestriction.CanNotClose)
            {
                e.Cancel = true;
                HideWithAnimation();
            }
        }

        //======================================================================
        private void treeWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                HideWithAnimation();
            }
        }

        //======================================================================
        private void animToHide_Completed(object sender, EventArgs e)
        {
            App.Current.MainWindow.Show();
            Hide();
        }
    }
}
