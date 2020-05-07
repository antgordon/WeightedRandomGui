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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeightedRandom.core;
using WeightedRandom.forms;

namespace WeightedRandom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Project pj = new Project();
            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);

            Page page = new ProjectEditPage(pj);
            mainFrame.Navigate(page);
        }
    }
}
