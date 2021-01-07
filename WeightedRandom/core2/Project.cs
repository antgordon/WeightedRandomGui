using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WeightedRandom.core2
{
    public class Project : IEnumerable<Table>
    {

        private IList<Table> tables;
        public string Name { get; }
        public IdAllocator Allocator { get; }
     

        public Project(string name, IdAllocator allocator) 
        {
            tables = new List<Table>();
            this.Allocator = allocator;
            this.Name = name;
        }



        public Table GetTable(string name) {
   

            foreach (Table tab in tables) {
                if (name.Equals(tab.Name))
                {
                    return tab;
                }
            }


            return null;

        }

        public Table GetTable(int id)
        {


            foreach (Table tab in tables)
            {
                if (id.Equals(tab.ID))
                {
                    return tab;
                }

            }

            return null;

        }


        public void AddTable(Table tab) {

            Table other = GetTable(tab.ID);

            if (other != null)
            {
                throw new ArgumentException($"table with id {tab.ID} already exists in project {Name}");
            }


            tables.Add(tab);

        }



        public void RemoveTable(int id)
        {

            List<Table> copy = new List<Table>(tables);

            foreach (Table tab in copy) {

                if (tab.ID == id) {
                    tables.Remove(tab);
                }
            }


        }


        public void RemoveTable(string name)
        {

            List<Table> copy = new List<Table>(tables);

            foreach (Table tab in copy)
            {

                if (tab.Name == name)
                {
                    tables.Remove(tab);
                }
            }
        }


        public IEnumerator<Table> GetEnumerator()
        {
            return tables.GetEnumerator();
        }

   

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tables.GetEnumerator();
        }


   
    }


    public class IdAllocator { 
        public int TableId { get; set; }
        public int ElementId { get; set; }

        public int IncrementTable() {
            int num = TableId;
            TableId += 1;
            return num;
        }

        public int IncrementElement()
        {
            int num = ElementId;
            ElementId += 1;
            return num;
        }


        public Table CreateTable(string name) {
            return new Table(IncrementTable(), name);
        }

        public Element CreateElement(Table table,string  name, double weight)
        {
            return new Element(IncrementElement(), table.ID, name, weight);
        }
    }



}
