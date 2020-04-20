using System;
using System.Runtime.Serialization;

namespace CreaturesLibrary
{
    /// <summary>
    /// Перечисление со способами передвижения.
    /// </summary>
    public enum MovementType
    {
        Swimming,
        Walking,
        Flying
    }

    [DataContract(Name = "Creature")]
    public class Creature
    { 
        /// <summary>
        /// Свойство имени. Я не стал делать проверку, потому что генерирую значения в нужном диапазоне.
        /// И считаю, что нам гарантируют правильность имени.
        /// 
        /// </summary>
        [DataMember]
        public string Name { get; private set; }
        
        /// <summary>
        /// Свойство для способа передвижения.
        /// </summary>
        [DataMember]
        public MovementType MovementType { get; private set; }

        /// <summary>
        /// Свойство для здоровья существа.
        /// </summary>
        [DataMember]
        public double Health { get; private set; }

        public Creature(string name, MovementType movType, double health)
        {
            Name = name;
            MovementType = movType;
            Health = health;
        }

        public override string ToString()
        {
            return $"{MovementType} creature {Name}: Health = {Health:F3}";
        }

        /// <summary>
        /// Создаём оператор умножения для членов этого класса.
        /// </summary>
        /// <param name="a"> First creature </param>
        /// <param name="b"> Second creature </param>
        /// <returns></returns>
        public static Creature operator *(Creature a, Creature b)
        {
            if (a.MovementType != b.MovementType)
            {
                throw new ArgumentException();
            }

            if (a.Name.Length >= b.Name.Length)
            {
                return new Creature(a.Name.Substring(0, a.Name.Length / 2) + b.Name.Substring(b.Name.Length / 2), a.MovementType, (a.Health + b.Health) / 2.0);
            }
            else
            {
                return new Creature(b.Name.Substring(0, b.Name.Length / 2) + a.Name.Substring(a.Name.Length / 2), a.MovementType, (a.Health + b.Health) / 2.0);
            }
        }

        // Переопределяем Equals, чтобы тесты работали правильно.
        public bool Equals(Creature other)
        {
            return Name.Equals(other.Name) && MovementType.Equals(other.MovementType) && Health.Equals(other.Health);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Creature);
        }
    }
}
