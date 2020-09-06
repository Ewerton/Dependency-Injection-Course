using System.Collections.Generic;
using System.IO;
using System.Linq;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;

namespace StudentsManager.CSV
{
    public class StudentsCsvProvider : IDataProvider<Student>
    {
        private readonly string _filePath;
        private readonly List<Student> _students;

        public StudentsCsvProvider(string filePath)
        {
            _filePath = filePath;
            var content = File.ReadAllLines(filePath);
            _students = content.Select(line =>
            {
                var parts = line.Split(';');
                const int id = 0;
                const int name = 1;
                const int lastName = 2;
                const int age = 3;
                const int email = 4;

                return new Student
                {
                    Age = int.Parse(parts[age]),
                    EmailAddress = parts[email],
                    FirstName = parts[name],
                    Id = int.Parse(parts[id]),
                    LastName = parts[lastName]
                };
            }).ToList();
        }

        public Student GetById(int id)
        {
            return _students.Single(x => x.Id == id);
        }

        public void Change(Student student)
        {
            var s = GetById(student.Id);
            s.Age = student.Age;
            s.FirstName = student.FirstName;
            s.LastName = student.LastName;
            s.EmailAddress = student.EmailAddress;
        }

        public void Add(Student student)
        {
            _students.Add(student);
        }

        public void Remove(int id)
        {
            _students.RemoveAll(x => x.Id == id);
        }

        public void SubmitChanges()
        {
            var content = _students.Select(x => string.Join(";", x.Id, x.FirstName, x.LastName, x.Age, x.EmailAddress));
            File.WriteAllLines(_filePath, content);
        }

        public IEnumerable<Student> GetAll()
        {
            return _students;
        }
    }
}