using System.Collections.ObjectModel;
using System.Windows.Input;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;

namespace StudentsManager.ViewModels
{
    public class MainViewModel
    {
        private readonly IDataProvider<Student> _studentsXmlProvider;
        public ObservableCollection<Student> Students { get; }
        
        public ICommand SubmitChangesCommand { get; }

        public ICommand RemoveCommand { get; }

        public MainViewModel(IDataProvider<Student> provider)
        {            
            _studentsXmlProvider = provider;
            Students = new ObservableCollection<Student>();

            SubmitChangesCommand = new RelayCommand(SubmitChanges);
            RemoveCommand = new RelayCommand(RemoveStudent, CanRemoveStudent);

            ReloadStudents();
        }

        public Student SelectedStudent { get; set; }

        public bool CanRemoveStudent()
        {
            return SelectedStudent != null;
        }

        private void RemoveStudent()
        {
            _studentsXmlProvider.Remove(SelectedStudent.Id);
            ReloadStudents();
        }

        private void SubmitChanges()
        {
            _studentsXmlProvider.SubmitChanges();
        }        

        private void ReloadStudents()
        {
            Students.Clear();
            var students = _studentsXmlProvider.GetAll();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }
    }
}
