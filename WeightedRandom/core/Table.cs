using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Media;

namespace WeightedRandom.core
{
    public class Table : IEnumerable<KeyValuePair<string, double>>
    {
        private IDictionary<string, double> entries;
        private IDictionary<string, string> references;

        public string Name { get; }

        public Table(string name) {
            entries = new Dictionary<string, double>();
            references = new Dictionary<string, string>();
            Name = name;
        }


        public void AddReference(string key, string table) {
            if (HasKey(key))
            {
                references.Add(key, table);
            }
            else {
                throw new ArgumentException("Reference key is not present");
            }
        }

        public string GetReference(string key)
        {
            if (HasKey(key))
            {
                return references[key];
            }
            else
            {
                throw new ArgumentException("Reference key is not present");
            }
        }

        public bool HasReference(string key)
        {
            if (HasKey(key))
            {
                return references.ContainsKey(key);
            }
            else
            {
                throw new ArgumentException("Reference key is not present");
            }
        }


        public void RemoveReference(string key)
        {
            if (HasKey(key))
            {
                references.Remove(key);
            }
            else
            {
                throw new ArgumentException("Reference key is not present");
            }
        }

        public bool HasKey(string key) 
        {
            return entries.ContainsKey(key);
        }

        public void AddKey(string key, double value)
        {
            entries[key] = value;
        }

        public void RemoveKey(string key)
        {
            RemoveReference(key);
            entries.Remove(key);
            
        }

        public double GetWeight(string key) {
            return entries[key];
        }


        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return  entries.GetEnumerator();
        }



        public double GetTotal() {
            double tots = 0.0;
            foreach ((string _, double value) in entries)
            {

                tots += value;
            }

            return tots;
        }


        public NormalTable Normalize( Project project) { 
        
            return new NormalTable(project, this, null);
        }
    }
}
