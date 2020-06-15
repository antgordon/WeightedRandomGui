using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;
using System;
using WeightedRandom.core;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod2()
        {
            Project project = new Project();
            Table pj = new Table("test");
            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);
            NormalTable normal = pj.Normalize(project);

            double expected = 4.0;
            double actual = normal.Total;


            Assert.IsTrue(floatEqual(expected, actual), $"Acual is {actual}");

        }

        [TestMethod]
        public void TestKey()
        {
            Key key = new Key("apple", new Key("one"));

            string expected = "one.apple";
            string actual = key.FullName;


            Assert.IsTrue(expected.Equals(actual), $"Actual is {actual} Expected is {expected}");

        }

        [TestMethod]
        public void TestMethod1()
        {
            Project project = new Project();
            Table pj = new Table("test");
            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);

            NormalTable normal = pj.Normalize(project);

            double expected = 0.25;
            double actual = normal[new Key("one")];

            Assert.IsTrue(floatEqual(expected, actual), $"Acual is {normal[new Key("one")]}");
        }

        [TestMethod]
        public void TestTableReferences()
        {
            Project project = new Project();
            Table pj = new Table("dtest111");
            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);

            Table pj2 = new Table("test2");
            pj2.AddKey("apple", 1.0);
            pj2.AddKey("orange", 1.0);
            pj2.AddKey("banana", 1.0);
            pj2.AddKey("peach", 1.0);

        

            project.RegisterTable(pj);
            project.RegisterTable(pj2);
           pj.AddReference("one", "test2");
            NormalTable normal = pj.Normalize(project);

            Key key = new Key("apple", new Key("one"));
            Key keyOne = new Key("one");
            Key keyApple = new Key("apple");

            Assert.IsTrue(normal.HasKey(keyOne), $"{keyOne.FullName} is not present");
            Assert.IsFalse(normal.HasKey(keyApple), $"{keyApple.FullName} is present");
            Assert.IsTrue(normal.HasKey(key), $"{key.FullName} is not present");

            double expected = 0.25 * 0.25;
            double actual = normal[key];

           

            Assert.IsTrue(floatEqual(expected, actual), $"Acual is {actual} Expected is {expected}");
        }

        private bool floatEqual(double expected, double actual)
        {

            double diff = Math.Abs(expected - actual);
            return diff < 0.00001;
        }
    }

}
