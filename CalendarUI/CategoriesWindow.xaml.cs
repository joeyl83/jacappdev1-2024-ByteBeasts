﻿using System;
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
using CalendarUI.Interfaces;
namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for CategoriesWindow.xaml
    /// </summary>
    public partial class CategoriesWindow : Window, CategoriesViewInterface
    {
        private readonly Presenter _presenter;
        public CategoriesWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializeCategoryView(this);
            _presenter.LoadCategoryTypes();

            LoadPersonalization();
        }

        public void LoadCategoryTypes(List<string> categoryTypes)
        {
            foreach (string type in categoryTypes)
            {
                CategoryType.Items.Add(type);
            }
        }
        public void AddCategory()
        {
           Success.Visibility = Visibility.Visible;
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryType.Text) && string.IsNullOrWhiteSpace(CategoryName.Text))
            {
                ShowError("Please select a value for Category Type and input a name for Category Name.");
            }
            else if (string.IsNullOrWhiteSpace(CategoryType.Text))
            {
                ShowError("Please select a value for Category Type.");
            }
            else if(string.IsNullOrWhiteSpace(CategoryName.Text))
            {
                ShowError("Please input a value for Category Name.");
            }
            else
            {
                int typenumber = 0;
                string[] array = CategoryType.Text.Split(':');
                typenumber = Int32.Parse(array[1].Trim());
                Category.CategoryType type = (Category.CategoryType)typenumber;
                Success.Visibility = Visibility.Collapsed;
                _presenter.ProcessAddCategory(CategoryName.Text, type);
                _presenter.LoadCategories(1);
            }
          
        }
        public void ChangeBackground(System.Windows.Media.Color color)
        {
            this.Background = new SolidColorBrush(color);
        }
        public void ChangeFontColor(System.Windows.Media.Color color)
        {
            this.Foreground = new SolidColorBrush(color);

            foreach (var child in InputGrid.Children)
            {

                if (child is Panel panel)
                {

                    foreach (var child2 in panel.Children)
                    {
                        if (child2 is Button button)
                        {
                            button.Foreground = new SolidColorBrush(color);
                        }   
                    }
                    
                }
            }

        }
        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            this.BorderBrush = new SolidColorBrush(color);

            foreach (var child in InputGrid.Children)
            {
                if (child is Button button)
                {
                    button.BorderBrush = new SolidColorBrush(color);

                }

                if (child is GroupBox groupBox)
                {
                    groupBox.BorderBrush = new SolidColorBrush(color);

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
                            if (child2 is TextBox textBox)
                            {
                                textBox.BorderBrush = new SolidColorBrush(color);
                            }
                        }
                    }
                }

                if (child is Panel panel2)
                {
                    foreach (var child2 in panel2.Children)
                    {
                        if (child2 is Button button3)
                        {
                            button3.BorderBrush = new SolidColorBrush(color);
                        }
                    }
                }


            }
        }

        public void ChangeForegroundColor(System.Windows.Media.Color color)
        {
            foreach (var child in InputGrid.Children)
            {
                if (child is GroupBox groupBox)
                {
                    groupBox.Background = new SolidColorBrush(color);

                }

                if (child is Panel panel)
                {
                    foreach (var child2 in panel.Children)
                    {
                        if (child2 is Button button)
                        {
                            button.Background = new SolidColorBrush(color);
                        }
                    }
                }

            }
        }

        public void LoadPersonalization()
        {
            ChangeBackground(Presenter.BackgroundColor);
            ChangeFontColor(Presenter.FontColor);
            ChangeBorderColor(Presenter.BorderColor);
            ChangeForegroundColor(Presenter.ForegroundColor);
        }

    }
}
