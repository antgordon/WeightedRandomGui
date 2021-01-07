using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private ViewModel viewModel;

        public EditPage2(core2.Project project)
        {
            InitializeComponent();
            this.project = project;
            //SetTable("one");
            loadSideButtons();
        }

        private void loadSideButtons()
        {
            StackPanel stackPannel = new StackPanel();
            Grid.SetColumn(stackPannel, 0);
            Grid.SetRow(stackPannel, 1);

            Grid grid = (Grid)this.Content;

            foreach (core2.Table table in project)
            {


                Button but = creatButtonForTable(table);

                stackPannel.Children.Add(but);

            }

            grid.Children.Add(stackPannel);



        }

        private Button creatButtonForTable(core2.Table table)
        {
            Button button = new Button();
            button.Name = table.Name;
            button.Content = table.Name;
            button.Click += (obj, e) => { SetTable(button.Name); };
            return button;
        }

        public void SetTable(string name)
        {
            core2.Table table = project.GetTable(name);

            setTableRaw(table);
        }


        protected void setTableRaw(core2.Table table)
        {
            projectTree.Items.Clear();
            if (viewModel != null)
                viewModel.Clear();

            if (table != null)
            {
                viewModel = new ViewModel(project, table, 1.0, projectTree.Items);
                viewModel.Dispay();
            }
        }

    


        class ViewModel {

            private core2.Table table;
            private core2.Project project;
            public IDictionary<int, double> percentages;
            private IList<ViewModelTreeNode> nodes;
            private ItemCollection displayNodes;
            private double _basePercent;


            public ViewModel(core2.Project project, core2.Table table, double basePercent, ItemCollection display) {
                this.table = table;
                this.project = project;
                this._basePercent = basePercent;
                nodes = new List<ViewModelTreeNode>();
                this.displayNodes = display;
             

            }


            public void Dispay() {
                UpdatePercentage();

                foreach (Element ele in table)
                {
                    ViewModelTreeNode node = new ViewModelTreeNode(this, ele);
                    displayNodes.Add(node.GuiItem);
                    nodes.Add(node);
                }

                UpdateAll();
            }

            public void AddElement(string name, double weight) {
                Element element = project.Allocator.CreateElement(table, name, weight);
                table.AddElement(element);
                ViewModelTreeNode node = new ViewModelTreeNode(this, element);
                displayNodes.Add(node.GuiItem);
                nodes.Add(node);
                UpdatePercentage();
                UpdateAll();

            }


            public void RemoveElement(ViewModelTreeNode node)
            {

                table.RemoveElement(node.Element.ID);
                nodes.Remove(node);
                displayNodes.Remove(node.GuiItem);
                UpdateAll();
               
            }


            public void UpdateAll() {
                foreach (ViewModelTreeNode tree in nodes) {
                    tree.UpdateSelf();
                }
            }


            public void UpdatePercentage() {
                percentages = table.ComputePercentage(_basePercent);
            }

            public void Reorder(int index, int secondIndex) {
                //perform swap on table
                Element one = null;
                Element two = null;
                table.SwapElements(one, two);
                UpdateAll();
            }


            public void Clear() {
                foreach (ViewModelTreeNode tree in nodes)
                {
                    tree.Clear();
                }
            }

            private bool findTreeNode(int id, out ViewModelTreeNode node) {
                node = null;

                foreach (ViewModelTreeNode nodeSel in nodes) {

                    if (nodeSel.Element.ID == id) {
                        node = nodeSel;
                        return true;
                    }
                }


                return false;
            }

        }


        class ViewModelTreeNode
        {

            public Element Element { get; set; }
            public TreeViewItem GuiItem { get; }
           
            private ViewModel viewmodel;
            private Button closeButton;
            private TextBox nameBox;
            private TextBox weightBox;
            private Label percentageLabel;
            private StackPanel stackPanel;

            public ViewModelTreeNode(ViewModel viewmodel, Element element)
            {
                this.viewmodel = viewmodel;
                this.Element = element;
                
                closeButton = new Button();
                closeButton.Content = "XX";
                closeButton.Margin = new Thickness(0, 5, 10, 0);
                closeButton.Click += CloseButton_Click;

                nameBox = new TextBox();
                nameBox.AcceptsReturn = false;
                nameBox.AcceptsTab = false;
                nameBox.KeyDown += Unfocus_KeyDown;
                nameBox.LostKeyboardFocus += NameBox_LostKeyboardFocus;
                nameBox.Margin = new Thickness(0, 5, 10, 0);

                weightBox = new TextBox();
                weightBox.AcceptsReturn = false;
                weightBox.AcceptsTab = false;
                weightBox.KeyDown += Unfocus_KeyDown;
                weightBox.LostKeyboardFocus += WeightBox_LostKeyboardFocus;
                weightBox.Margin = new Thickness(0, 5, 10, 0);

                percentageLabel = new Label();
                percentageLabel.Margin = new Thickness(20, 5, 10, 0);

                stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(closeButton);
                stackPanel.Children.Add(nameBox);
                stackPanel.Children.Add(weightBox);
                stackPanel.Children.Add(percentageLabel);

                GuiItem = new TreeViewItem();
                GuiItem.Header = stackPanel;

            }


            /// <summary>
            /// Parse namebox and set name
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void NameBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
            {
                Element.Name = nameBox.Text;
                UpdateSelf();
            }

            /// <summary>
            /// Parse weight and set weight
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void WeightBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
            {
                double value;

                if (double.TryParse(weightBox.Text, out value))
                {
                    Element.Weight = value;
                    Reweight();


                }
            }


            //
            //
            //Unfocus a textbox when enter or excape is pressed
            //
            private void Unfocus_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == System.Windows.Input.Key.Enter || e.Key == System.Windows.Input.Key.Escape)
                {

                    Keyboard.ClearFocus();
                }
            }


            /// <summary>
            /// Close the button removes the tree
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void CloseButton_Click(object sender, RoutedEventArgs e)
            {
                viewmodel.RemoveElement(this);
            }



            public void UpdateSelf() {
               
                nameBox.Text = Element.Name;
                weightBox.Text = $"{Element.Weight:F2}";
                double percentage = viewmodel.percentages[Element.ID];
                percentageLabel.Content = $"{percentage:F4}";

            }

            public void Reweight() {
                //Parse weight
                //Apply to element in table
                viewmodel.UpdatePercentage();
                viewmodel.UpdateAll();
            }


            public void Clear() {
                stackPanel.Children.Clear();
                GuiItem.Header = null;
                viewmodel = null;
                Element = null;

            }
        }
    }
}
