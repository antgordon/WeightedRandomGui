using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WeightedRandom.core2
{
    public class Project : IEnumerable<Table>
    {

        private IList<Table> tables;

        public Project(string name) 
        {
            tables = new List<Table>();
        }

        public string Name { get; set; }

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




}
