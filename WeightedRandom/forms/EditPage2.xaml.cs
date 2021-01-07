using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            public core2.Table table;
            public core2.Project project;
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
                UpdatePercentage();
                ViewModelTreeNode node = new ViewModelTreeNode(this, element);
                displayNodes.Add(node.GuiItem);
                nodes.Add(node);
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


       

        /// <summary>
        /// Represents an element in a weighted table
        /// </summary>
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
            private IList<ViewModelChild> viewModelChilds;

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

                viewModelChilds = new List<ViewModelChild>();
                double percentage = viewmodel.percentages[element.ID];

                //Display tables who consider the current table as a parent
                //under the tre node
                foreach (core2.Table table in viewmodel.project) 
                {

                    Parent par = table.GetParent();

                    if (par != null && par.TableId == viewmodel.table.ID && par.ElementId == element.ID) 
                    {
                        ViewModelChild model = new ViewModelChild(viewmodel, par, table);
                        model.Dispay();
                        viewModelChilds.Add(model);
                        GuiItem.Items.Add(model.GetUIElement());
                    }
           
                }


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


            /// <summary>
            /// Updates the name, weight and percentage weight for this element view model
            /// </summary>
            public void UpdateSelf() {
               
                nameBox.Text = Element.Name;
                weightBox.Text = $"{Element.Weight:F2}";
                double percentage = viewmodel.percentages[Element.ID];
                percentageLabel.Content = $"{percentage:P4}";

                foreach (ViewModelChild child in viewModelChilds)
                {
                    child.UpdatePercentage();
                    child.UpdateAll();
                }

            }

            public void Reweight() {
                //Parse weight
                //Apply to element in table
                viewmodel.UpdatePercentage();
                viewmodel.UpdateAll();
            }



            /// <summary>
            /// Remove display elements from the screen
            /// Clears references
            /// </summary>
            public void Clear() {
                stackPanel.Children.Clear();
                GuiItem.Header = null;
                viewmodel = null;
                Element = null;
                foreach (ViewModelChild child in viewModelChilds)
                {
                    child.Clear();
                }
                GuiItem.Items.Clear();
                viewModelChilds.Clear();


            }
        }


        /// <summary>
        /// Represents a table which is a child of another table
        /// All tree nodes present in the child table are immutable
        /// 
        /// Includes an "X" button to remove the element from the table
        /// A name and weight textbox
        /// A percentage level to show percentage weight
        /// </summary>
        class ViewModelChild
        {

            private core2.Table table;
            private ViewModel model;
            private Parent parent;
            public IDictionary<int, double> percentages;
            private IList<ViewModelTreeNodeChild> nodes;
            private ItemCollection displayNodes;
            private StackPanel stackPanel;
     


            public ViewModelChild(ViewModel model, Parent parent, core2.Table table)
            {
                this.table = table;
                nodes = new List<ViewModelTreeNodeChild>();
                this.parent = parent;
                this.model = model;

                
                Label label = new Label();
                label.Content = table.Name;
                TreeView tree = new TreeView();
                displayNodes = tree.Items;

                stackPanel = new StackPanel();
                stackPanel.Children.Add(label);
                stackPanel.Children.Add(tree);

            }

            //Gets the percentage weight of the parent element\
            //Is used to scale the percentage weight for tables in this display
            private double getBasePercentage() {
                return model.percentages[parent.ElementId];
            }

            /// <summary>
            /// Gets the ui element to display this child view model
            /// </summary>
            /// <returns></returns>
            public UIElement GetUIElement() 
            {
                return stackPanel;
            }


            /// <summary>
            /// Add display elements to the ui
            /// Adds immutable child nodes
            /// </summary>
            public void Dispay()
            {
                UpdatePercentage();

                foreach (Element ele in table)
                {
                    ViewModelTreeNodeChild node = new ViewModelTreeNodeChild(this, ele);
                    displayNodes.Add(node.GuiItem);
                    nodes.Add(node);
                }

                UpdateAll();
            }

            /// <summary>
            /// Tells all nodes to update their own display
            /// </summary>
            public void UpdateAll()
            {
                foreach (ViewModelTreeNodeChild tree in nodes)
                {
                    tree.UpdateSelf();
                }
            }

            /// <summary>
            /// Recalculates the percentage weight for element
            /// </summary>
            public void UpdatePercentage()
            {
                percentages = table.ComputePercentage(getBasePercentage());
            }

            /// <summary>
            /// Clears references of ui elements
            /// </summary>
            public void Clear()
            {
                foreach (ViewModelTreeNodeChild tree in nodes)
                {
                    tree.Clear();
                }
            }
        }


        /// <summary>
        /// An immutable tree node display to display the elements in child nodes.
        /// 
        /// Display the table name and then the tree of elements below the name
        /// 
        /// 
        /// </summary>
        class ViewModelTreeNodeChild
        {

            public Element Element { get; set; }
            public TreeViewItem GuiItem { get; }

            private ViewModelChild viewmodel;
        
            private Label nameBox;
            private Label weightBox;
            private Label percentageLabel;
            private StackPanel stackPanel;

            public ViewModelTreeNodeChild(ViewModelChild viewmodel, Element element)
            {
                this.viewmodel = viewmodel;
                this.Element = element;

                nameBox = new Label();
                nameBox.Margin = new Thickness(0, 5, 10, 0);

                weightBox = new Label();
                weightBox.Margin = new Thickness(0, 5, 10, 0);

                percentageLabel = new Label();
                percentageLabel.Margin = new Thickness(20, 5, 10, 0);

                stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(nameBox);
                stackPanel.Children.Add(weightBox);
                stackPanel.Children.Add(percentageLabel);

                GuiItem = new TreeViewItem();
                GuiItem.Header = stackPanel;

            }


 
            /// <summary>
            /// Updates the name, weight and percentage weight for this element view model
            /// </summary>
            public void UpdateSelf()
            {

                nameBox.Content = Element.Name;
                weightBox.Content = $"{Element.Weight:F2}";
                double percentage = viewmodel.percentages[Element.ID];
                percentageLabel.Content = $"{percentage:P4}";

            }


            /// <summary>
            /// Clears references to UI elements
            /// </summary>
            public void Clear()
            {
                stackPanel.Children.Clear();
                GuiItem.Header = null;
                viewmodel = null;
                Element = null;

            }
        }
    }
}
