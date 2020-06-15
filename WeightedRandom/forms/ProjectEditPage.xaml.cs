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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeightedRandom.core;

namespace WeightedRandom.forms
{
    /// <summary>
    /// Interaction logic for ProjectEditPage.xaml
    /// </summary>
    public partial class ProjectEditPage : Page
    {

        private core.Project project;


        public ProjectEditPage(core.Project project)
        {
            InitializeComponent();
            this.project = project;
            displayTree();

        }

        private TreeViewItem createTreeViewItem(string key, double raw, double normal) {
            TreeViewItem treeViewItem = new TreeViewItem();
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;


            treeViewItem.IsExpanded = true;
            TextBlock nameText = new TextBlock();
            nameText.Text = key;
            nameText.Margin = new Thickness(0, 5, 10, 0);

            TextBox rawText = new TextBox();
            rawText.Text = Convert.ToString(raw);
            rawText.Margin = new Thickness(0, 5, 10, 0);

            Label normalText = new Label();
            normalText.Content = Convert.ToString(normal);
            normalText.Margin = new Thickness(20, 5, 10, 0);

            stackPanel.Children.Add(nameText);
            stackPanel.Children.Add(rawText);
            stackPanel.Children.Add(normalText);
            treeViewItem.Header = stackPanel;
            //treeViewItem.Items.Add( stackPanel);

            return treeViewItem;

        }


        public void displayTree() {
            core.Table table = project.GetTable("one");
            NormalTable map = table.Normalize(project);
            foreach ((core.Key key, double val) in map) {
                double shortVal = table.GetWeight(key.ShortName);
                TreeViewItem item = createTreeViewItem(key.ShortName, shortVal, val);
                projectTree.Items.Add(item);
            }

        }
    }
}
