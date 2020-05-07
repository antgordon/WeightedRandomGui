using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;

namespace WeightedRandom.core
{
    public class Project : IEnumerable<KeyValuePair<string, double>>
    {
        private IDictionary<string, double> entries;

        public Project() {
            entries = new Dictionary<string, double>();
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
            entries.Remove(key);
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


        public NormalMap Normalize() { 
        
            return new NormalMap(this);
        }
    }
}
