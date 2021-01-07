using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WeightedRandom.core2
{

    /// <summary>
    /// Stores multiple elements to compare the chance of appearence in a uniform distribution.
    /// </summary>
    public class Table : IEnumerable<Element>
    {

        private IList<Element> elements;


        /// <summary>
        /// The unique id for this table
        /// </summary>
        public int ID { get; }



        /// <summary>
        /// The name of this table
        /// </summary>
        public string Name { get; set; }

        private Parent _Parent;

        public Table(int id, string name)
        {
            elements = new List<Element>();
            ID = id;
            Name = name;
        }


        public void SetParent(Project project, Parent parent) {
            //Do some cycle detecting stuff
            _Parent = parent;
        }

        public Parent GetParent()
        {
            return _Parent;
        }


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

        /// <summary>
        /// Get the weight of an element in the table with the given name.
        /// Will throw and exeception if no element with that name exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetWeight(string name)
        {


            Element element = GetElement(name);

            if (element == null) {
                throw new ArgumentException($"No element with name {name} exists in table {Name}");
            }


            return element.Weight;
           
        }


        /// <summary>
        /// Get the weight of an element in the table with the given id.
        /// Will throw and exeception if no element with that id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
