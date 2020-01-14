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
    public class StudentsPageVM : BaseViewModel, IRemoveEntityCmd, IEnterNewModeCmd, ISaveVMFormCmd, INewPassHashCmd, IMoveEntityInList, IChangeSubgridCmd, IDeleteCmd, ICancelmd, IGoToEntityCmd
    {
        #region Properties

        private int studentRowsToReturn;
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
                UpdateMyStudentsList(studentRowsToReturn);
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

        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get
            {
                return _selectedGroup
;
            }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        private Question _selectedQuestion;

        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));
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
        public ChangeSubgridContextCmd ChangeSubgridContextCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }
        public GoToEntityCmd GoToEntityCmd { get; set; }

        #endregion Commands

        public StudentsPageVM(string dbcontextName, Student selectedStudent, ISystemUser loggedInLectuer) : base(dbcontextName)
        {
            try
            {
                studentRowsToReturn = 10;
                IsConfirmed = SetFalse();
                SubgridContext = SubgridContext.Groups;
                //commands
                RemoveEntityCmd = new RemoveEntityCmd(this);
                NewModeCmd = new NewModeCmd(this);
                SaveFormCmd = new SaveCmd(this);
                NewPassHashCmd = new NewPassHashCmd(this);
                MoveEntityOutOfListCmd = new MoveEntityOutOfListCmd(this);
                MoveEntityInToListCmd = new MoveEntityInToListCmd(this);
                ChangeSubgridContextCmd = new ChangeSubgridContextCmd(this);
                DeleteCmd = new DeleteCmd(this);
                CancelCmd = new CancelCmd(this);
                GoToEntityCmd = new GoToEntityCmd(this);
                //TODO: will likely need to attach lecturer to the DbContext..
                Lecturer = (Lecturer)loggedInLectuer;
                SearchStudentTxt = "";
                if (selectedStudent.Id == 0)
                    SelectedStudent = new Student();
                else
                    SelectedStudent = UnitOfWork.StudentRepo.Get(selectedStudent.Id);
                //TODO: figure out Async with EF and Pagination/ limit the results (limit probably best)
                RefreshAvailableGroups(SelectedStudent);
                List<Student> results = UnitOfWork.StudentRepo.GetAll().ToList();
                Students = new ObservableCollection<Student>(results);
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        private void RefreshAvailableGroups(Student student)
        {
            List<Group> groups;
            if (student == null || student.Id == 0) // If in New Mode
                groups = UnitOfWork.GroupRepository.GetAll().ToList();
            else // if student already selected
                groups = UnitOfWork.GroupRepository.GetGroupsNotContainingStudent(student).ToList();
            AvailableGroups = new ObservableCollection<Group>(groups);
        }

        public bool Remove(BaseEntity entity)
        {
            bool result = false;
            if (entity == null)
            {
                ShowFeedback("No valid group selected. Canceling.", FeedbackType.Error);
                return false;
            }
            try
            {
                ClearFeedback();
                string msg = "";
                int gId = entity.Id;
                result = SelectedStudent.RemoveGroup((Group)entity, UnitOfWork, ref msg);
                if (result)
                {
                    RefreshAvailableGroups(SelectedStudent);
                    ShowFeedback($"Removed Student {SelectedStudent.Id} from Group {gId}.", FeedbackType.Success);
                    result = true;
                }
                else
                    ShowFeedback(msg, FeedbackType.Error);

                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
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
            ClearFeedback();
            bool result;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = Lecturer.AddNewStudent(SelectedStudent, UnitOfWork);
                    if (result)
                    {
                        UpdateMyStudentsList(studentRowsToReturn);
                        ShowFeedback($"Created Student: {SelectedStudent.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to create student.", FeedbackType.Error);
                }
                else
                {
                    //Update
                    result = SelectedStudent.Validate();
                    if (result)
                    {
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result)
                            ShowFeedback($"Updated Student: {SelectedStudent.Id}.", FeedbackType.Success);
                        else
                        {
                            ShowFeedback($"Student not updated: {SelectedStudent.Id}.", FeedbackType.Error);
                            result = false;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.GetBaseException().Message, FeedbackType.Error);
                return false;
            }
        }

        private void UpdateMyStudentsList(int rows)
        {
            Students = new ObservableCollection<Student>(UnitOfWork.StudentRepo.GetTopXFromSearch(SearchStudentTxt, rows));
        }

        public bool GenerateNewPasswordHash(string optionalNewPassword)
        {
            ClearFeedback();
            bool result = false;
            try
            {
                //Confirmation Check
                Mediator.NotifyColleagues(MediatorChannels.StudentsPageGeneratePasswordCheck.ToString(), null);
                if (IsConfirmed)
                {
                    //randomly generate
                    result = SelectedStudent.GenerateNewPasswordHash(ref optionalNewPassword, null);
                    OnPropertyChanged(nameof(SelectedStudent));
                    if (result)
                        ShowFeedback("Password hash generated: \nRemember to save changes.", FeedbackType.Info);
                    else
                        ShowFeedback("Failed to generate new password hash.", FeedbackType.Error);
                    //Temporary display new password
                    Mediator.NotifyColleagues(MediatorChannels.StudentsPageNewPasswordDisplay.ToString(), optionalNewPassword);
                    return true;
                }
                else
                {
                    ShowFeedback("Generation of new password Canceled.", FeedbackType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public bool MoveEntityOutOfList(BaseEntity entityToRemove)
        {
            ClearFeedback();
            try
            {
                bool result = false;
                if (entityToRemove is Group group)
                {
                    int gId = group.Id;
                    SelectedStudent.Groups.Remove(group);
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    if (result)
                    {
                        RefreshAvailableGroups(SelectedStudent);
                        ShowFeedback($"Student {SelectedStudent.Id} removed from group {gId}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to remove Student {SelectedStudent.Id} from group {gId}.", FeedbackType.Error);
                }
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public bool MoveEntityInToList(BaseEntity entityToAdd)
        {
            ClearFeedback();
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
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    if (result)
                    {
                        RefreshAvailableGroups(SelectedStudent);
                        ShowFeedback($"Student {SelectedStudent.Id} added to Group {group.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to add Student {SelectedStudent.Id} to Group {group.Id}.", FeedbackType.Error);
                }
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public bool ChangeSubgridContext(SubgridContext context)
        {
            bool result = false;
            ClearFeedback();

            switch (context)
            {
                case SubgridContext.Groups:
                    //Student Groups should be lazy loaded into form
                    SubgridContext = SubgridContext.Groups;
                    Mediator.NotifyColleagues(MediatorChannels.LoadGroupsSubgridForStudents.ToString(), null);
                    break;

                case SubgridContext.Questions:
                    //Student Questions should be lazy loaded into form
                    SubgridContext = SubgridContext.Questions;
                    Mediator.NotifyColleagues(MediatorChannels.LoadQuestionsSubgridForStudents.ToString(), null);
                    break;

                default:
                    ShowFeedback("Sub-grid type not supported. Contact support.", FeedbackType.Error);
                    break;
            }
            return result;
        }

        public bool Delete(BaseEntity objToDelete)
        {
            ClearFeedback();
            bool result = false;
            Student studentToDelete = objToDelete as Student;
            if (studentToDelete == null)
            {
                ShowFeedback("No student selected.", FeedbackType.Error);
                return result;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteStudentConfirmation.ToString(), studentToDelete);
                if (IsConfirmed)
                {
                    int id = studentToDelete.Id;
                    UnitOfWork.StudentRepo.Remove(studentToDelete);
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    if (result)
                    {
                        UpdateMyStudentsList(studentRowsToReturn);
                        ShowFeedback($"Deleted Student: {id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to delete Student: {id}.", FeedbackType.Error);
                }

                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return result;
            }
        }

        public bool SetFalse()
        {
            return false;
        }

        public bool Cancel()
        {
            ClearFeedback();
            if (FormContext == FormContext.Create)
                EnterNewMode();
            else
            {
                try
                {
                    UnitOfWork.Reload(SelectedStudent);
                    UpdateMyStudentsList(studentRowsToReturn);
                    OnPropertyChanged(nameof(SelectedStudent));
                }
                catch
                {
                    EnterNewMode();
                    ShowFeedback("Unable to re-load selected Group. \n Going back to new mode.", FeedbackType.Error);
                    return false;
                }
            }
            return true;
        }

        public bool GoToEntity(BaseEntity entity)
        {
            if (entity is Group)
            {
                Mediator.NotifyColleagues(MediatorChannels.GoToThisGroup.ToString(), entity);
                return true;
            }
            else if (entity == null)
            {
                ShowFeedback("Cannot go to a null entity", FeedbackType.Error);
                return false;
            }
            return false;
        }
    }
}