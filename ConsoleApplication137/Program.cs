using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApplication137
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            using (FileStream fileStream = new FileStream("People.xml", FileMode.Open))
            {
                Root stuff = serializer.Deserialize(fileStream) as Root;
                Console.WriteLine(stuff.People.Persons[0].Cars[0].Make);
            }

            Console.ReadKey(true);
        }
    }

    [XmlRoot("Root")]
    public class Root
    {
        [XmlElement("People")]
        public People People { get; set; } 
    }

    public class People
    {
        [XmlElement("Person")]
        public Person[] Persons { get; set; }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        [XmlArray("Cars")]
        public Car[] Cars { get; set; }
    }

    public class Car
    {
        [XmlAttribute("car_make")]
        public string Make { get; set; }

        [XmlAttribute("Model")]
        public string Model { get; set; }

        [XmlAttribute("Year")]
        public int Year { get; set; }
    }
}
