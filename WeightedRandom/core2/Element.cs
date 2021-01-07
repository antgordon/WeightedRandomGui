using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;

namespace WeightedRandom.core2
{
    public class Element
    {


        public Element(int id, int tableId,  string name, double weight) {
            ID = id;
            TableId = tableId;
            Name = name;
            Weight = weight;
        
        }

        public int ID { get; }

        public int TableId { get; }

        public string Name { set; get; }

        public double Weight { set; get; }

    }
}
