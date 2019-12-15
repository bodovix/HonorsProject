using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
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
    public class StudentsPageVM : BaseViewModel, IRemoveEntityCmd, IEnterNewModeCmd, ISaveVMFormCmd, INewPassHashCmd, IMoveEntityInList
    {
        #region Properties

        public bool IsConfirmed { get; set; }
        private SubgridContext _subgridContext;

        public SubgridContext SubgridContext
        {
            get { return _subgridContext; }
            set
            {
                _subgridContext = value;
                OnPropertyChanged(nameof(SubgridContext));
            }
        }

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
                FeedbackMessage = "";
                //if selected.id == 0 create else update
                FormContext = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedStudent = value;
                RefreshAvailableGroups(SelectedStudent);
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
        public NewPassHashCmd NewPassHashCmd { get; set; }
        public MoveEntityOutOfListCmd MoveEntityOutOfListCmd { get; set; }
        public MoveEntityInToListCmd MoveEntityInToListCmd { get; set; }

        #endregion Commands

        public StudentsPageVM(string dbcontextName) : base(dbcontextName)
        {
            try
            {
                IsConfirmed = false;
                SubgridContext = SubgridContext.Groups;
                //commands
                RemoveEntityCmd = new RemoveEntityCmd(this);
                NewModeCmd = new NewModeCmd(this);
                SaveFormCmd = new SaveCmd(this);
                NewPassHashCmd = new NewPassHashCmd(this);
                MoveEntityOutOfListCmd = new MoveEntityOutOfListCmd(this);
                MoveEntityInToListCmd = new MoveEntityInToListCmd(this);

                //TODO: will likely need to attach lecturer to the DbContext..
                Lecturer = (Lecturer)App.AppUser;
                SearchStudentTxt = "";
                FormContext = FormContext.Create;
                SelectedStudent = new Student();

                //TODO: figure out Async with EF and Pagination/ limit the results (limit probably best)
                RefreshAvailableGroups(SelectedStudent);
                List<Student> results = UnitOfWork.StudentRepo.GetAll().ToList();
                Students = new ObservableCollection<Student>(results);
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
            }
        }

        private void RefreshAvailableGroups(Student student)
        {
            List<Group> groups;
            if (student == null || student.Id == 0)
                groups = UnitOfWork.GroupRepository.GetAll().ToList();
            else
                groups = UnitOfWork.GroupRepository.GetGroupsNotContainingStudent(student).ToList();
            AvailableGroups = new ObservableCollection<Group>(groups);
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
            RefreshAvailableGroups(SelectedStudent);
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
                    {
                        int rowsChanged = UnitOfWork.Complete();
                        if (rowsChanged == 0)
                        {
                            FeedbackMessage = $"Student not updated: {SelectedStudent.Id}.";
                            result = false;
                        }
                    }
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

        public bool GenerateNewPasswordHash(string optionalNewPassword)
        {
            FeedbackMessage = "";
            try
            {
                IsConfirmed = false;
                //Confirmation Check
                Mediator.NotifyColleagues(MediatorChannels.StudentsPageGeneratePasswordCheck.ToString(), null);
                if (IsConfirmed)
                {
                    //randomly generate
                    SelectedStudent.GenerateNewPasswordHash(ref optionalNewPassword);
                    //have to set form context back when in create as this updates the id
                    if (FormContext == FormContext.Create)
                    {
                        //have to update at property level so view updates....
                        SelectedStudent = SelectedStudent;
                        FormContext = FormContext.Create;
                    }
                    else
                        SelectedStudent = SelectedStudent;
                    //Temporary display new password
                    Mediator.NotifyColleagues(MediatorChannels.StudentsPageNewPasswordDisplay.ToString(), optionalNewPassword);
                    return true;
                }
                else
                {
                    FeedbackMessage = "Generation of new password Canceled.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public bool MoveEntityOutOfList(BaseEntity entityToRemove)
        {
            FeedbackMessage = "";
            try
            {
                bool result = false;
                if (entityToRemove is Group group)
                {
                    SelectedStudent.Groups.Remove(group);
                    UnitOfWork.Complete();
                    RefreshAvailableGroups(SelectedStudent);
                    return true;
                }
                return result;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }

        public bool MoveEntityInToList(BaseEntity entityToAdd)
        {
            FeedbackMessage = "";
            try
            {
                bool result = false;
                if (entityToAdd is Group group)
                {
                    if (SelectedStudent.Groups.Contains(entityToAdd))
                    {
                        FeedbackMessage = $"Student already belongs to group: {entityToAdd.Name}";
                        return false;
                    }
                    SelectedStudent.Groups.Add(group);
                    UnitOfWork.Complete();
                    RefreshAvailableGroups(SelectedStudent);
                    return true;
                }
                return result;
            }
            catch (Exception ex)
            {
                FeedbackMessage = ex.Message;
                return false;
            }
        }
    }
}