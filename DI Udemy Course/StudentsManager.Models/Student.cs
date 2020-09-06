using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsManager.Models
{
    [XmlRoot]
    public class Student
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string FirstName { get; set; }

        [XmlAttribute]
        public string LastName { get; set; }

        [XmlAttribute]
        public int Age { get; set; }

        [XmlAttribute]
        public string EmailAddress { get; set; }
    }
}
