using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class StudentsPageVM : BaseViewModel
    {
        private Lecturer _lecturer;

        public Lecturer Lecturer
        {
            get { return _lecturer; }
            set
            {
                _lecturer = value;
                OnPropertyChanged(nameof(Lecturer));
            }
        }

        private string _searchStudentTxt;

        public string SearchStudentTxt
        {
            get { return _searchStudentTxt; }
            set
            {
                _searchStudentTxt = value;
                OnPropertyChanged(nameof(SearchStudentTxt));
            }
        }

        private Student _selectedStudent;

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private List<Student> _students;

        public List<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public StudentsPageVM(string dbcontextName) : base(dbcontextName)
        {
        }
    }
}