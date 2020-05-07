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
            Project pj = new Project();
            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);
            NormalMap normal = pj.Normalize();

            double expected = 4.0;
            double actual = normal.Total;


            Assert.IsTrue(floatEqual(expected, actual), $"Acual is {actual}");

        }

        [TestMethod]
        public void TestMethod1()
        {
            Project pj = new Project();
            pj.AddKey("one", 1.0);
            pj.AddKey("two", 1.0);
            pj.AddKey("three", 1.0);
            pj.AddKey("four", 1.0);

            NormalMap normal = pj.Normalize();

            double expected = 0.25;
            double actual = normal["one"];

            Assert.IsTrue(floatEqual(expected, actual), $"Acual is {normal["one"]}");
        }

        private bool floatEqual(double expected, double actual)
        {

            double diff = Math.Abs(expected - actual);
            return diff < 0.00001;
        }
    }

}
