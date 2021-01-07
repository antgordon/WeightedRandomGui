using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WeightedRandom.core2
{
    public class Table : IEnumerable<Element>
    {

        private IList<Element> elements;

        public Table(int id, string name)
        {
            elements = new List<Element>();
            ID = id;
        }

        public int ID { get; }

        public string Name { get; set; }

        public Element GetElement(string name) {
   

            foreach (Element ele in elements) {
                if (name.Equals(ele.Name)) {
                    return ele;
                }
            
            }


            return null;

        }

        public Element GetElement(int id)
        {


            foreach (Element ele in elements)
            {
                if (id.Equals(ele.ID))
                {
                    return ele;
                }

            }

            return null;

        }


        public double GetWeight(string name)
        {


            Element element = GetElement(name);

            if (element == null) {
                throw new ArgumentException($"No element with name {name} exists in table {Name}");
            }


            return element.Weight;
           
        }

        public double GetWeight(int id)
        {

            Element element = GetElement(id);

            if (element == null)
            {
                throw new ArgumentException($"No element with id {id} exists in table {Name}");
            }


            return element.Weight;


        }



        public void AddElement(Element element) {

            Element other = GetElement(element.ID);

            if (other != null)
            {
                throw new ArgumentException($"element with id {element.ID} already exists in table {Name}");
            }


            elements.Add(element);

        }

        public void RemoveElement(int id)
        {

            List<Element> copy = new List<Element>(elements);

            foreach (Element ele in copy) {

                if (ele.ID == id) {
                    elements.Remove(ele);
                }
            }


        }

        public void RemoveElement(string name)
        {

            List<Element> copy = new List<Element>(elements);

            foreach (Element ele in copy)
            {

                if (ele.Name == name)
                {
                    elements.Remove(ele);
                }
            }


        }


        public void SwapElements(Element one, Element two)
        {

            Element copy = two;
            int oneIndex = elements.IndexOf(one);
            int twoIndex = elements.IndexOf(two);

            elements.Insert(twoIndex, one);
            elements.Insert(oneIndex, two);



        }

        public IEnumerator<Element> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

   

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }


        public double GetTotal()
        {
            double tots = 0.0;
            foreach (Element ele in elements)
            {

                tots += ele.Weight;
            }

            return tots;
        }


        public IDictionary<int,double> ComputePercentage(double foundation) {

            double total = GetTotal();

            IDictionary<int, double> value = new Dictionary<int, double>();
            foreach (Element ele in elements)
            {

                double percentage = ele.Weight / total;
                percentage *= foundation;

                value[ele.ID] = percentage;
            }


            return value;

        }
    }




}
