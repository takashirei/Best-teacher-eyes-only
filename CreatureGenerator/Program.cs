using System;
using System.Collections.Generic;
using System.Linq;
using CreaturesLibrary;
using System.Runtime.Serialization;
using System.Xml;

namespace CreatureGenerator
{
    class Program
    {
        /// <summary>
        /// Генератор рандомных чисел. Он генерирует случайные числа, которые мы потом используем в программе.
        /// </summary>
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            // Создаём список и добавляем в него 30 элементов.
            List<Creature> creatures = new List<Creature>();
            for (int i = 0; i < 30; i++)
            {
                creatures.Add(new Creature(GenerateName(), (MovementType)rnd.Next(0, 3), 10 * rnd.NextDouble()));
            }

            try
            {
                XmlWriterSettings set = new XmlWriterSettings {Indent = true};
                using (XmlWriter xmlWriter = XmlWriter.Create("../../../creatures.xml", set))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(List<Creature>));
                    serializer.WriteObject(xmlWriter, creatures);
                }

                List<Creature> creaturesDeserialized = new List<Creature>();

                using (XmlReader xmlReader = XmlReader.Create("../../../creatures.xml"))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(List<Creature>));
                    creaturesDeserialized = serializer.ReadObject(xmlReader) as List<Creature>;
                }

                var swimmingCreatures = creaturesDeserialized.Count(a => a.MovementType == MovementType.Swimming);
                Console.WriteLine(swimmingCreatures);
                Console.Write(Environment.NewLine);
                var creaturesByHealth = creatures.OrderBy(creature => creature.Health
                ).Reverse().Take(10);
                foreach (var creature in creaturesByHealth)
                {
                    Console.WriteLine(creature);
                }

                Console.Write(Environment.NewLine);
                var grouppedCreatures = creatures.GroupBy(creature
                    => creature.MovementType).Select(g =>
                    new {Name = g.Key, Count = g.Count(), Creatures = g.Select(p => p)});
                List<Creature> top3 = new List<Creature>();
                foreach (var group in grouppedCreatures)
                {
                    Console.WriteLine(group.Creatures.Aggregate((a, b) => (a * b)));
                    top3.Add(group.Creatures.Aggregate((a, b) => (a * b)));
                }

                Console.Write(Environment.NewLine);
                var top3Sorted = top3.OrderBy(creature => creature.Health).Reverse().Take(10);
                foreach (var creature in top3Sorted)
                {
                    Console.WriteLine(creature);
                }
            }
            catch (SerializationException)
            {
                Console.WriteLine("Something happened during serialization. I don't know why," +
                                  " I don't know what, but perhaps stop breaking my XML file? Or try again.");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Something got nulled. Wasn't supposed to happen. Try again.");
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong. Try again.");
            }
            
        }

        public static string GenerateName()
        {
            string name = "";
            name += (char)rnd.Next('A', 'Z' + 1);
            int length = rnd.Next(6, 11);
            for (int i = 1; i < length; i++)
            {
                name += (char)rnd.Next('a', 'z' + 1);
            }

            return name;
        }
    }
}
