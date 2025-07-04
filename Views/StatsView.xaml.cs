﻿using Kasyno.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Kasyno.Views
{
    /// <summary>
    /// Logika interakcji dla klasy StatsView.xaml
    /// </summary>
    public partial class StatsView : Window
    {
        public StatsView()
        {
            InitializeComponent();
            this.Closing += Stats_Closing;
        }

        private void Stats_Closing(object? sender, CancelEventArgs e)
        {
            if (DataContext is StatsViewModel vm)
            {
                vm.OnWindowClosing();
            }
        }
    }
}
