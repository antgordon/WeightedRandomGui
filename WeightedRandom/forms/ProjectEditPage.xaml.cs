using System;
using System.Collections;
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
        private LinkedList<TreeNode> viewModel;
        core.Table selected;


        public ProjectEditPage(core.Project project)
        {
            InitializeComponent();
            this.project = project;
            viewModel = new LinkedList<TreeNode>();
            //displayTree();
            SetTable("one");
            loadSideButtons();
    

        }

        private void loadSideButtons() {
            StackPanel stackPannel = new StackPanel();
            Grid.SetColumn(stackPannel, 0);
            Grid.SetRow(stackPannel, 1);

            Grid grid = (Grid)this.Content;

            foreach (core.Table table in project) {

           
                Button but = CreatButtonForTable(table);
             
                stackPannel.Children.Add(but);
               
            }

            grid.Children.Add(stackPannel);



        }

        private Button CreatButtonForTable(core.Table table) {
            Button button = new Button();
            button.Name = table.Name;
            button.Content = table.Name;
            button.Click += (obj, e) => { SetTable(button.Name);};
            return button;
        }

        public void SetTable(string name) {
            selected = project.GetTable(name);

            if (selected == null)
            {
                clearModel();

            }
            else {
                NormalTable tabe = selected.Normalize(project);
                modelTree(project, selected, tabe);
                displayTreeModel(selected, tabe, viewModel);
            }
        }


        private void modelTree(Project project, core.Table table, NormalTable normal)
        {
            clearModel();

            ICollection<TreeNode> convert = viewModel;
             foreach ((string name, _) in table) {
                recursiveModel(project, normal, table, null, name, convert );
            }

            
        }

        private void recursiveModel(Project project, NormalTable normal, core.Table currentTable, core.Key parentKey, string name, ICollection<TreeNode> children) {

      
            core.Key tableKey;

            if (parentKey == null)
            {
                tableKey = new core.Key(name);
            }
            else {
                tableKey = new core.Key(name, parentKey);
            }

            TreeNode node = new TreeNode(tableKey, currentTable);
            children.Add(node);

            if (currentTable.HasReference(name)) 
            {
                string refName = currentTable.GetReference(name);
                core.Table refTable = project.GetTable(refName);

                foreach ((string namesub, double weight) in refTable)
                {
                    recursiveModel(project, normal, refTable, tableKey, namesub, node.Children());

                }



            }
         
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

        private void displayTreeModel(core.Table table, NormalTable normal, ICollection<TreeNode> nodes)
        {

            projectTree.Items.Clear();
            foreach (TreeNode child in nodes)
            {
                TreeViewItem viewChild = displayTreeModelRecursive(project, normal, child);
                projectTree.Items.Add(viewChild);
            }



        }

        private TreeViewItem displayTreeModelRecursive(Project project, NormalTable normal, TreeNode node)
        {
            TreeViewItem item = createTreeViewItem(node.Name, node.GetRawWeight(), node.GetNormalWeight(normal));

            foreach (TreeNode child in node) {
                TreeViewItem viewChild = displayTreeModelRecursive(project, normal, child);
                item.Items.Add(viewChild);
            }

            return item;
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


        private void clearModel() {
            foreach (TreeNode node in viewModel) {
                node.ClearChildren();
            }

            viewModel.Clear();

        }

        class TreeNode : IEnumerable<TreeNode>
        {

            public string Name { get; } 

            public core.Key TableKey { get; }

            public core.Table TableRef { get; }

            public TreeNode(core.Key key, core.Table tableRef) 
            {
                this.TableKey = key;
                this.TableRef = tableRef;
                this.Name = key.ShortName;
                children = new LinkedList<TreeNode>();
            }

            public double GetNormalWeight(NormalTable table) 
            {
                return table[TableKey];
            }

            public double GetRawWeight()
            {
                return TableRef.GetWeight(Name);
            }

            public bool IsRoot() 
            {
                return parent != null;
            }


            public void AddChild(TreeNode node) {
                children.AddLast(node);
                node.parent = this;
            
            }

            public void RemoveChild(TreeNode node)
            {
                children.Remove(node);
                node.parent = null;

            }


            public void ClearChildren() {
                List < TreeNode > copy = new List<TreeNode>(children);

                foreach (TreeNode node in copy) {
                    RemoveChild(node);
                }
            }

            public ICollection<TreeNode> Children() {
                return children;
            }

            public IEnumerator<TreeNode> GetEnumerator()
            {
                return children.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return children.GetEnumerator();
            }

           


            private TreeNode parent;
            private LinkedList<TreeNode> children;


        }
    }
}
