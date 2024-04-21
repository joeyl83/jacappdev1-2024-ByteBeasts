using CalendarUI.Interfaces;
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
using Calendar;
using Haley.Enums;
using Haley.Models;
using Haley.Abstractions;
using Haley.Utils;
using Haley.Events;
using Haley.MVVM;
using Haley.Services;
using System.Drawing;
using Haley.WPF.Controls;


namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for PersonalizationWindow.xaml
    /// </summary>
    public partial class PersonalizationWindow : Window, PersonalizationInterface
    {
        private Presenter _presenter;
        public PersonalizationWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializePersonalizationWindow(this);
        }

        public void GetColor()
        {
            //_presenter.ProcessColor(colorPicker.SelectedColor);


        }

        private void btn_ChangeBackground(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Color backgroundColor = colorPicker_Background.SelectedColor;
            _presenter.ProcessBackgroundColor(backgroundColor);

        }

        private void btn_ChangeFontColor(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Color backgroundColor = colorPicker_Font.SelectedColor;
            _presenter.ProcessFontColor(backgroundColor);

        }

        private void btn_ChangeBorderColor(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Color backgroundColor = colorPicker_Border.SelectedColor;
            _presenter.ProcessBorderColor(backgroundColor);

        }

        public void ChangeBackground(System.Windows.Media.Color color)
        {
            this.Background = new SolidColorBrush(color);   
        }

        public void ChangeFontColor(System.Windows.Media.Color color)
        {
            this.Foreground = new SolidColorBrush(color);
        }
        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            this.BorderBrush = new SolidColorBrush(color);

            foreach (var child in mainPanel.Children)
            {
                if (child is Button button)
                {
                    button.BorderBrush = new SolidColorBrush(color);
                }

                if (child is GroupBox groupBox)
                {
                    if (groupBox.Content is Button buttonInGroupBox)
                    {
                        buttonInGroupBox.BorderBrush = new SolidColorBrush(color);
                    }
                    else if (groupBox.Content is Panel panel)
                    {
                        foreach (var child2 in panel.Children)
                        {
                            if (child2 is Button button2)
                            {
                                button2.BorderBrush = new SolidColorBrush(color);
                            }
                        }
                    }
                    else if (groupBox.Content is Grid grid)
                    {
                        foreach (var child2 in grid.Children)
                        {
                            if (child2 is Button button2)
                            {
                                button2.BorderBrush = new SolidColorBrush(color);
                            }
                        }
                    }
                }

                
            }
        }



    }
}
