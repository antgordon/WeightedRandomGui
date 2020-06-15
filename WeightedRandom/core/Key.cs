using System;
using System.Collections.Generic;
using System.Text;

namespace WeightedRandom.core
{
    public class Key
    {
        public string FullName { get; }
        private string[] segments;

        public Key(string name) {
            this.FullName = name;
            this.segments = new string[] { name };
        }

        public Key(string name, Key parent)
        {

            StringBuilder builder = new StringBuilder();
          
            this.segments = new string[parent.segments.Length + 1];

            for (int i = 0; i < parent.segments.Length; i += 1) {
                this.segments[i] = parent.segments[i];
               
                builder.Append(parent.segments[i]);

                if (i != segments.Length-1)
                {
                    builder.Append('.');
                }
            }

            builder.Append(name);
            this.segments[this.segments.Length - 1] = name;
            this.FullName = builder.ToString();
        }



        public string this[int index] { get => segments[index]; }


        public override bool Equals(object obj)
        {
            // If this and obj do not refer to the same type, then they are not equal.
            if (obj.GetType() != this.GetType()) return false;

  
            var other = (Key)obj;
            return FullName.Equals(other.FullName);
        }


        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

       
        public override string ToString()
        {
            return $"Key ({FullName})";
        }


        public int Depth { get => segments.Length; }

        public string ShortName { get => this.segments[this.segments.Length - 1]; }




    }
}
