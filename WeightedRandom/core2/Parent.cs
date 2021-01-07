using System;
using System.Collections.Generic;
using System.Text;

namespace WeightedRandom.core2
{
    public class Parent
    {

        public int TableId { get; }

        public int ElementId { get; }

        public Parent(int table, int element) {
            TableId = table;
            ElementId = element;
        }

        public Parent(Element element)
        {
            TableId = element.TableId;
            ElementId = element.ID;
        }
    }
}
