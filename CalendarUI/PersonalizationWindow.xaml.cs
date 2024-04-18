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
            _presenter.ProcessColor(colorPicker.SelectedColor);
        }

        public void ChangeColor()
        {
            throw new NotImplementedException();
        }

    }
}
