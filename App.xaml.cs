﻿using Kasyno.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Kasyno
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User User { get; set; }
    }

}
