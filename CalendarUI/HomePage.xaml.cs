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

namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window, HomePageViewInterface
    {
        private readonly Presenter presenter;
        public HomePage(Presenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            presenter.InitializeHomePageView(this);

            ChangeBackground(Presenter.BackgroundColor);
            ChangeFontColor(Presenter.FontColor);
            ChangeBorderColor(Presenter.BorderColor);
            ChangeForegroundColor(Presenter.ForegroundColor);
        }

        public void AddEventBtnClick(object sender, RoutedEventArgs e)
        {
            OpenAddEventWindow();
        }
        public void AddCategoryBtnClick(object sender, RoutedEventArgs e)
        {
            OpenAddCategoryWindow();
        }

        private void PersonalizeBtnClick(object sender, RoutedEventArgs e)
        {
            OpenPersonalizationWindow();
        }

        public void OpenAddCategoryWindow()
        {
            CategoriesWindow categories = new CategoriesWindow(presenter);
            categories.Show();
        }

        public void OpenAddEventWindow()
        {
            EventsWindow events = new EventsWindow(presenter);
            events.Show();
        }

        public void OpenPersonalizationWindow()
        {
            PersonalizationWindow personalization = new PersonalizationWindow(presenter);
            personalization.Show();
        }

        public void ChangeBackground(System.Windows.Media.Color color)
        {
            this.Background = new SolidColorBrush(color);
        }

        public void ChangeFontColor(System.Windows.Media.Color color)
        {
            this.Foreground = new SolidColorBrush(color);

            foreach (var child in mainGrid.Children)
            {

                if (child is GroupBox groupBox)
                {
                    groupBox.Foreground = new SolidColorBrush(color);

                    if (groupBox.Content is Grid grid)
                    {
                        foreach (var child2 in grid.Children)
                        {
                            if (child2 is Button button2)
                            {
                                button2.Foreground = new SolidColorBrush(color);
                            }
                        }
                    }
                }
            }
        }

        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            this.BorderBrush = new SolidColorBrush(color);

            foreach (var child in mainGrid.Children)
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

        public void ChangeForegroundColor(System.Windows.Media.Color color)
        {
            foreach (var child in mainGrid.Children)
            {
                if (child is GroupBox groupBox)
                {
                    groupBox.Background = new SolidColorBrush(color);

                    if (groupBox.Content is Grid grid)
                    {
                        foreach (var child2 in grid.Children)
                        {
                            if (child2 is Button button2)
                            {
                                button2.Background = new SolidColorBrush(color);
                            }
                        }
                    }
                }
            }
        }

    }
}
