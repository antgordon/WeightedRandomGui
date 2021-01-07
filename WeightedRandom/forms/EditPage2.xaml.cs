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
using WeightedRandom.core2;

namespace WeightedRandom.forms
{
    /// <summary>
    /// Interaction logic for EditPage2.xaml
    /// </summary>
    public partial class EditPage2 : Page
    {

        private core2.Project project;

        public EditPage2(core2.Project project)
        {
            this.project = project;
        }



        class ViewModel {

            private core2.Table table;
            public IDictionary<int, double> percentages;
            private IList<ViewModelTreeNode> nodes;


            public ViewModel(core2.Table table) {
                this.table = table;
                percentages = table.ComputePercentage(1.0);
                nodes = new List<ViewModelTreeNode>();

            }

            public void AddElement(string name, double weight) {
                int newId = 0; //TODO assign some other way
                Element element = new Element(newId, table.ID, name, weight);
                table.AddElement(element);
                ViewModelTreeNode node = createTreeItem(nodes.Count);
                nodes.Add(node);
                UpdatePercentage();
                UpdateAll();

            }


            public void RemoveElement(ViewModelTreeNode element)
            {
          
                if (nodes.Count > 0) {
                    //taable remove inddex
                    nodes.RemoveAt(nodes.Count - 1);
                    UpdateAll();
                }
               
            }


            public void UpdateAll() {
                foreach (ViewModelTreeNode tree in nodes) {
                    tree.updateSelf();
                }
            }


            public void UpdatePercentage() {
                percentages = table.ComputePercentage(1.0);
            }

            public void Reorder(int index, int secondIndex) {
                //perform swap on table
                Element one = null;
                Element two = null;
                table.SwapElements(one, two);
                UpdateAll();
            }


            public ViewModelTreeNode createTreeItem(int index) {

                ViewModelTreeNode tree = new ViewModelTreeNode(this, index);
                return tree;
            }

        }


        class ViewModelTreeNode
        {

            private int index;
            private ViewModel viewmodel;

            private Button closeButton;
            private TextBox nameBox;
            private TextBox weightBox;
            private Label percentageLabel;
            StackPanel stackPanel;

            public ViewModelTreeNode(ViewModel viewmodel, int index)
            {
                this.viewmodel = viewmodel;
                stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                closeButton = new Button();
                nameBox = new TextBox();
                weightBox = new TextBox();
                percentageLabel = new Label();
                this.index = index;


                closeButton.Click += CloseButton_Click;

            }

            private void CloseButton_Click(object sender, RoutedEventArgs e)
            {
                viewmodel.RemoveElement(this);
            }

            public void updateSelf() {
                Element element = null;
                nameBox.Text = element.Name;
                weightBox.Text = $"{element.Weight:F2}";
                double percentage = viewmodel.percentages[element.ID];
                percentageLabel.Content = $"{percentage:F2}";

            }

            public void reweight() {
                //Parse weight
                //Apply to element in table
                viewmodel.UpdatePercentage();
                viewmodel.UpdateAll();
            }
        }
    }
}
