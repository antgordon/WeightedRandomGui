using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WeightedRandom.core
{
    public class NormalMap : IEnumerable<KeyValuePair<string, double>>
    {
        private IDictionary<string, double> entries;

        public double Total { get; }


        public NormalMap(Project project) {
            entries = new Dictionary<string, double>();
            Total = project.GetTotal();

            foreach (KeyValuePair<string,double> pair in project)
            {

                entries[pair.Key] = pair.Value / Total;
            }

   
        }


        public bool HasKey(string key)
        {
            return entries.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entries.GetEnumerator();
        }

     

        public double this[string key]
        {
            get
            {
                if (HasKey(key))
                {
                    return entries[key];
                }
                else
                {
                    return 0.0;
                }

            }
          

        }
    }
}
