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
    public class StudentsPageVM : BaseViewModel, IRemoveEntityCmd, IEnterNewModeCmd, ISaveVMFormCmd
    {
        #region Properties

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
                if (value == null)
                    value = new Student();
                //if selected.id == 0 create else update
                FormContext = (value.Id == 0) ? FormContext.Create : FormContext.Update;
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

        #endregion Properties

        #region Commands

        public RemoveEntityCmd RemoveEntityCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }
        public SaveCmd SaveFormCmd { get; set; }

        #endregion Commands

        public StudentsPageVM(string dbcontextName) : base(dbcontextName)
        {
            try
            {
                //commands
                RemoveEntityCmd = new RemoveEntityCmd(this);
                NewModeCmd = new NewModeCmd(this);
                SaveFormCmd = new SaveCmd(this);
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
            try
            {
                FeedbackMessage = "";
                string msg = "";
                SelectedStudent.RemoveGroup((Group)entity, UnitOfWork, ref msg);
                FeedbackMessage = msg;
                return true;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public void EnterNewMode()
        {
            FormContext = FormContext.Create;
            SelectedStudent = new Student();
        }

        public bool Save()
        {
            FeedbackMessage = "";
            bool result;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = Lecturer.AddNewStudent(SelectedStudent, UnitOfWork);
                    if (result)
                        UpdateMyStudentsList();
                }
                else
                {
                    //Update
                    result = SelectedStudent.Validate();
                    if (result)
                        UnitOfWork.Complete();
                }
                return result;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.GetBaseException().Message;
                return false;
            }
        }

        private void UpdateMyStudentsList()
        {
            int rows = 10;
            Students = new ObservableCollection<Student>(UnitOfWork.StudentRepo.GetTopXFromSearch(SearchStudentTxt, rows));
        }
    }
}