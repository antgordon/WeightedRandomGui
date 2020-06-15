using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using WeightedRandom.core;

namespace WeightedRandom.forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Project project = new Project();

            core.Table pj = new core.Table("one");
            project.RegisterTable(pj);
            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);

            Page page = new ProjectEditPage(project);

            NavigationService server = NavigationService.GetNavigationService(this);
            server.Navigate(page);
        }
    }
}
