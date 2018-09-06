﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PicDB.Models;

namespace PicDB.Xaml
{
    /// <summary>
    /// Interaktionslogik für PhotographerWindow.xaml
    /// </summary>
    public partial class PhotographerWindow : Window
    {
        private MainWindowViewModel Controller { get; set; } = null; 

        public PhotographerWindow(MainWindowViewModel controller)
        {
            Controller = controller; 
            InitializeComponent();
        }
    }
}