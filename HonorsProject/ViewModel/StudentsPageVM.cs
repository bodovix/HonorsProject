using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.Commands.IComands;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class StudentsPageVM : BaseViewModel, IRemoveEntityCmd
    {
        private FormContext _formContext;

        public FormContext FormContext
        {
            get { return _formContext; }
            set
            {
                _formContext = value;
                OnPropertyChanged(nameof(FormContext));
            }
        }

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

        private ObservableCollection<Group> _availableGroups;

        public ObservableCollection<Group> AvailableGroups
        {
            get { return _availableGroups; }
            set
            {
                _availableGroups = value;
                OnPropertyChanged(nameof(AvailableGroups));
            }
        }

        private Group _group;

        public Group SelectedGroup
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        #region Commands

        public RemoveEntityCmd RemoveEntityCmd { get; set; }

        #endregion Commands

        public StudentsPageVM(string dbcontextName) : base(dbcontextName)
        {
            try
            {
                //commands
                RemoveEntityCmd = new RemoveEntityCmd(this);
                //TODO: will likely need to attach lecturer to the DbContext..
                Lecturer = (Lecturer)App.AppUser;
                SearchStudentTxt = "";
                FormContext = FormContext.Create;
                SelectedStudent = new Student();

                //TODO: figure out Async with EF and Pagination/ limit the results (limit probably best)
                List<Group> groups = UnitOfWork.GroupRepository.GetAll().ToList();
                AvailableGroups = new ObservableCollection<Group>(groups);
                List<Student> results = UnitOfWork.StudentRepo.GetAll().ToList();
                Students = new ObservableCollection<Student>(results);
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
            }
        }

        public bool Remove(BaseEntity entity)
        {
            FeedbackMessage = "Not implemented remove from grup yet";
        }
    }
}