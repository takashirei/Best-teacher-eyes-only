using System;
using System.Collections.Generic;
using CreaturesLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.Xml;

namespace CreatureTest
{
    [TestClass]
    public class CreatureTest
    {
        private static Random rnd = new Random();
        [TestMethod]
        public void ToStringTest()
        {
            double forTest = rnd.NextDouble();
            Creature testCreature = new Creature("Veselko", MovementType.Swimming, forTest);
            string testToString = $"{MovementType.Swimming} creature Veselko: Health = {forTest:F3}";
            Assert.AreEqual(testCreature.ToString(), testToString);
        }

        [TestMethod]
        public void OperatorEvenEvenSameLength()
        {
            Creature a = new Creature("Stalin", MovementType.Walking, 10.456);
            Creature b = new Creature("Hitler", MovementType.Walking, 14.88);
            Creature result = a * b;
            Creature expectedResult = new Creature("Staler", MovementType.Walking, 12.668);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void OperatorEvenOdd()
        {
            Creature a = new Creature("Depression", MovementType.Walking, 2.718281828);
            Creature b = new Creature("Boredom", MovementType.Walking, 1.2345278);
            Creature result = b * a;
            Creature expectedResult = new Creature("Depreedom", MovementType.Walking, (2.718281828 + 1.2345278)/2);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void OperatorOddEven()
        {
            Creature a = new Creature("OddNumber", MovementType.Walking, 9);
            Creature b = new Creature("EvenNumb", MovementType.Walking, 8);
            Creature result = b * a;
            Creature expectedResult = new Creature("OddNNumb", MovementType.Walking, 8.5);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void OperatorOddOddSameLength()
        {
            Creature a = new Creature("Tatatat", MovementType.Walking, 7);
            Creature b = new Creature("NoSense", MovementType.Walking, 181);
            Creature result = a * b;
            Creature expectedResult = new Creature("Tatense", MovementType.Walking, 94);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void OperatorOddOddSameName()
        {
            Creature a = new Creature("Shadrin", MovementType.Walking, Double.PositiveInfinity);
            Creature b = new Creature("Shadrin", MovementType.Walking, Double.PositiveInfinity);
            Creature result = b * a;
            Creature expectedResult = new Creature("Shadrin", MovementType.Walking, Double.PositiveInfinity);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void OperatorOddOddDifferentLength()
        {
            Creature a = new Creature("Slozhno", MovementType.Walking, 0);
            Creature b = new Creature("aFullHomo", MovementType.Walking, 0);
            Creature result = b * a;
            Creature expectedResult = new Creature("aFulzhno", MovementType.Walking, 0);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void SerializationTest()
        {
            List<Creature> randomGibberish = new List<Creature>();
            {
                new Creature("Heerejej", MovementType.Flying, rnd.Next());
                new Creature("yGGhhej", MovementType.Walking, rnd.NextDouble());
            }

            XmlWriterSettings set = new XmlWriterSettings { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create("creatures.xml", set))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<Creature>));
                serializer.WriteObject(xmlWriter, randomGibberish);
            }

            List<Creature> creaturesDeserialized = new List<Creature>();

            using (XmlReader xmlReader = XmlReader.Create("creatures.xml"))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<Creature>));
                creaturesDeserialized = serializer.ReadObject(xmlReader) as List<Creature>;
            }

            CollectionAssert.AreEqual(randomGibberish, creaturesDeserialized);
        }

        [TestMethod]
        public void IncorrectTest()
        {
            Assert.AreEqual(1, 2);
        }

        [TestMethod]
        public void ExceptionTest()
        {
            Assert.ThrowsException<ArgumentException>(delegate {
                Creature a = new Creature("Slozhno", MovementType.Walking, 0);
                Creature b = new Creature("aFullHomo", MovementType.Flying, 0);
                Creature result = b * a;
            });
        }


    }
}
