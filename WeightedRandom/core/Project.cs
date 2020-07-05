using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Annotations;

namespace WeightedRandom.core
{
    public class Project: IEnumerable<Table>
    {
        private IDictionary<string, Table> entries;

        public Project() {
            entries = new Dictionary<string, Table>();
        }

        public Table GetTable(string name)
        {
            return entries[name];
        }

        public bool IsRegistered(string name) {
            return entries.ContainsKey(name);
        }

        public bool IsRegistered(Table table)
        {
            return IsRegistered(table.Name);
        }

        public void RegisterTable(Table table)
        {
            entries.Add(table.Name, table);
        }

        public void UnregisterTable(string name)
        {
            entries.Remove(name);
        }

        public void UnregisterTable(Table table)
        {
            entries.Remove(table.Name);
        }


        public IEnumerator<Table> GetEnumerator()
        {
            return entries.Values.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return entries.Values.GetEnumerator();
        }
    }
}
