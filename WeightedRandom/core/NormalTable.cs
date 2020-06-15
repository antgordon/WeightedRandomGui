using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WeightedRandom.core
{
    public class NormalTable : IEnumerable<KeyValuePair<Key, double>>
    {
        private IDictionary<Key, double> entries;

        public double Total { get; }


     
        public NormalTable(Project project, Table table, Key parent)
        {
            entries = new Dictionary<Key, double>();
            Total = table.GetTotal();

            foreach (KeyValuePair<string, double> pair in table)
            {

                Key key;

                if (parent == null) {
                    key = new Key(pair.Key);
                } else {
                    key = new Key(pair.Key, parent);
                }

                double val = pair.Value / Total;
                entries[key] = val;

                if (table.HasReference(pair.Key))
                {
                    string tableName = table.GetReference(pair.Key);
                    Table tref = project.GetTable(tableName);
                    NormalTable subTable = new NormalTable(project, tref, key);
                    foreach (KeyValuePair<Key, double> subPair in subTable)
                    {
                        Key subKey = subPair.Key;
                        double subVal = subPair.Value * val;
                        entries[subKey] = subVal;

                    }
                }
            }
        }


        public bool HasKey(Key key)
        {
            return entries.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<Key, double>> GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entries.GetEnumerator();
        }

     

        public double this[Key key]
        {
            get
            {
                if (HasKey(key))
                {
                    return entries[key];
                }
                else
                {
                    throw new ArgumentException("Unknown key " + key.FullName);
                }

            }
          

        }
    }
}
