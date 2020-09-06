using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;

namespace StudentsManager.ViewModels.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        public void CanRemoveStudent_NoSelectedStudents_ReturnsFalse()
        {
            var sut = new MainViewModel(new DataAccessMock());
            bool result = sut.CanRemoveStudent();
            Assert.IsFalse(result);
        }
    }

    public class DataAccessMock : IDataProvider<Student>
    {
        private List<Student> _students = new List<Student>()
        {
            new Student(), new Student(), new Student()
        };
        public Student GetById(int id)
        {
            return _students[id];
        }

        public void Change(Student newEntity)
        {            
        }

        public void Add(Student entity)
        {         
        }

        public void Remove(int id)
        {            
        }

        public IEnumerable<Student> GetAll()
        {
            return _students;
        }

        public void SubmitChanges()
        {            
        }
    }
}
