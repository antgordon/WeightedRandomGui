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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeightedRandom.core;
using WeightedRandom.core2;
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
            startCore2Page();
        }



        private void startCore1Page() {
            core.Project project = new core.Project();

            core.Table pj = new core.Table("one");

            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);

            core.Table pj2 = new core.Table("test2");
            pj2.AddKey("apple", 1.0);
            pj2.AddKey("orange", 1.0);
            pj2.AddKey("banana", 1.0);
            pj2.AddKey("peach", 1.0);


            core.Table pj3 = new core.Table("test3");
            pj3.AddKey("Toyota", 0.5);
            pj3.AddKey("Ford", 5.0);
            pj3.AddKey("General Motors", 1.0);
            pj3.AddKey("Honda", 10.0);



            project.RegisterTable(pj);
            project.RegisterTable(pj2);
            project.RegisterTable(pj3);
            pj.AddReference("one", "test2");
            pj2.AddReference("orange", "test3");
            pj.AddReference("three", "test3");

            Page page = new ProjectEditPage(project);
            mainFrame.Navigate(page);
        }

        private void startCore2Page()
        {
            core2.Project project = new core2.Project("test", new IdAllocator());
            IdAllocator allocator = project.Allocator;

            core2.Table pj = allocator.CreateTable("one");
            project.AddTable(pj);
            pj.AddElement(allocator.CreateElement(pj,"one", 1.0));
            pj.AddElement(allocator.CreateElement(pj, "two", 1.0));
            pj.AddElement(allocator.CreateElement(pj, "three", 1.0));
            pj.AddElement(allocator.CreateElement(pj, "four", 1.0));

            core2.Table pj2 = allocator.CreateTable("test2");
            project.AddTable(pj2);
            pj2.AddElement(allocator.CreateElement(pj2,"apple", 1.0));
            pj2.AddElement(allocator.CreateElement(pj2, "orange", 1.0));
            pj2.AddElement(allocator.CreateElement(pj2, "banana", 1.0));
            pj2.AddElement(allocator.CreateElement(pj2, "peach", 1.0));


            core2.Table pj3 = allocator.CreateTable("test3");
            project.AddTable(pj3);
            pj3.AddElement(allocator.CreateElement(pj3, "Toyota", 0.5));
            pj3.AddElement(allocator.CreateElement(pj3, "Ford", 5.0));
            pj3.AddElement(allocator.CreateElement(pj3, "General Motors", 1.0));
            pj3.AddElement(allocator.CreateElement(pj3, "Honda", 10.0));


            pj2.SetParent(project, new Parent(pj.GetElement("one")));
            pj3.SetParent(project, new Parent(pj2.GetElement("orange")));
            pj3.SetParent(project, new Parent(pj.GetElement("three")));

            // pj.AddReference("one", "test2");
            // pj2.AddReference("orange", "test3");
            // pj.AddReference("three", "test3");

            Page page = new EditPage2(project);
            mainFrame.Navigate(page);
        }
    }
}
