using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;

namespace StudentsManager.DataAccess {
    public class StudentsXmlProvider : IDataProvider<Student> {
        private readonly XDocument _doc;
        private readonly string _filePath;

        public StudentsXmlProvider(string filePath) {
            _filePath = filePath;
            _doc = XDocument.Load(filePath);
        }

        public Student GetById(int id) {
            return GetElementById(id)?.FromXElement<Student>();
        }

        public void Change(Student newEntity) {
            var entityById = GetElementById(newEntity.Id);
            entityById.Attribute("FirstName").Value = newEntity.FirstName;
            entityById.Attribute("LastName").Value = newEntity.LastName;
            entityById.Attribute("Age").Value = newEntity.Age.ToString();
            entityById.Attribute("EmailAddress").Value = newEntity.EmailAddress;
        }

        public void Add(Student entity) {
            var element = entity.ToXElement<Student>();
            _doc.Root.Add(element);
        }

        public void Remove(int id) {
            var entityById = GetElementById(id);
            entityById.Remove();
        }

        public IEnumerable<Student> GetAll() {
            return _doc.Descendants("Student").Select(element => element.FromXElement<Student>());
        }

        public void SubmitChanges() {
            _doc.Save(_filePath);
        }

        private XElement GetElementById(int id) {
            var result = _doc.Descendants("Student")
                .SingleOrDefault(element => (int) element.Attribute("Id") == id);
            return result;
        }
    }
}