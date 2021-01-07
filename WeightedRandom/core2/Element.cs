using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;

namespace WeightedRandom.core2
{


    /// <summary>
    /// Represents a single entry in a weighted table
    /// </summary>
    public class Element
    {


        public Element(int id, int tableId,  string name, double weight) {
            ID = id;
            TableId = tableId;
            Name = name;
            Weight = weight;
        
        }


        /// <summary>
        /// the unique id for this element
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// The unique id of the table this element belongs to
        /// </summary>
        public int TableId { get; }

        /// <summary>
        /// The name of this element
        /// </summary>
        public string Name { set; get; }


        /// <summary>
        /// The weight of the element
        /// </summary>
        public double Weight { set; get; }

    }
}
