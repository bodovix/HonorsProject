using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.Commands.IComands;
using HonorsProject.ViewModel.CoreVM;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class LecturerPageVM : BaseViewModel, IEnterNewModeCmd, ISaveVMFormCmd, IDeleteCmd, ICancelmd, INewPassHashCmd
    {
        #region Properties

        private int rowsToReturn;
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

        private Lecturer _user;

        public Lecturer User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string _searchTxt;

        public string SearchTxt
        {
            get { return _searchTxt; }
            set
            {
                _searchTxt = value;
                UpdateLecturersList(SearchTxt, rowsToReturn);
                OnPropertyChanged(nameof(SearchTxt));
            }
        }

        private Lecturer _selectedLecturer;

        public Lecturer SelectedLecturer
        {
            get { return _selectedLecturer; }
            set
            {
                if (value == null)
                    value = new Lecturer();
                FeedbackMessage = "";
                //if selected.id == 0 create else update
                FormContext = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedLecturer = value;
                OnPropertyChanged(nameof(SelectedLecturer));
            }
        }

        private ObservableCollection<Lecturer> _lecturers;

        public ObservableCollection<Lecturer> Lecturers
        {
            get { return _lecturers; }
            set
            {
                _lecturers = value;
                OnPropertyChanged(nameof(Lecturers));
            }
        }

        #endregion Properties

        #region Commands

        public NewModeCmd NewModeCmd { get; set; }
        public SaveCmd SaveFormCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public CancelCmd CancelCmd { get; set; }
        public NewPassHashCmd NewPassHashCmd { get; set; }

        #endregion Commands

        public LecturerPageVM(string dbcontextName, ISystemUser loggedInLectuer) : base(dbcontextName)
        {
            try
            {
                rowsToReturn = 20;
                IsConfirmed = false;
                SubgridContext = SubgridContext.Groups;
                //commands
                NewModeCmd = new NewModeCmd(this);
                SaveFormCmd = new SaveCmd(this);
                DeleteCmd = new DeleteCmd(this);
                CancelCmd = new CancelCmd(this);
                NewPassHashCmd = new NewPassHashCmd(this);
                //TODO: will likely need to attach lecturer to the DbContext..
                User = UnitOfWork.LecturerRepo.Get(loggedInLectuer.Id);
                SearchTxt = "";
                SelectedLecturer = new Lecturer();
                //TODO: figure out Async with EF and Pagination/ limit the results (limit probably best)
                List<Lecturer> results = UnitOfWork.LecturerRepo.GetTopXFromSearch(null, rowsToReturn).ToList();
                Lecturers = new ObservableCollection<Lecturer>(results);

                Mediator.Register(MediatorChannels.PoolingUpdate.ToString(), PoolingUpdate);
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
            }
        }

        private void PoolingUpdate(object obj)
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                UpdateLecturersList(SearchTxt, rowsToReturn);
            });
        }

        public void EnterNewMode()
        {
            if (!User.IsSuperAdmin)
            {
                ShowFeedback("Only Admin can create Lecturers.", FeedbackType.Error);
                return;
            }
            FormContext = FormContext.Create;
            SelectedLecturer = new Lecturer();
        }

        public bool Save()
        {
            ClearFeedback();
            if (!User.IsSuperAdmin)
            {
                ShowFeedback("Only Admin can Save Changes to lecturers.", FeedbackType.Error);
                return false;
            }
            bool result;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = User.AddNewLecturer(SelectedLecturer, UnitOfWork);
                    if (result)
                    {
                        UpdateLecturersList(SearchTxt, rowsToReturn);
                        ShowFeedback($"Created lecturer: {SelectedLecturer.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to create lecturer.", FeedbackType.Error);
                }
                else
                {
                    //Update
                    result = SelectedLecturer.Validate();
                    if (result)
                    {
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result)
                            ShowFeedback($"Updated lecturer: {SelectedLecturer.Id}.", FeedbackType.Success);
                        else
                        {
                            ShowFeedback($"lecturer not updated: {SelectedLecturer.Id}.", FeedbackType.Error);
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

        private void UpdateLecturersList(String searchTxt, int rows)
        {
            Lecturers = new ObservableCollection<Lecturer>(UnitOfWork.LecturerRepo.GetTopXFromSearch(searchTxt, rows));
        }

        public bool Delete(BaseEntity objToDelete)
        {
            ClearFeedback();
            if (!User.IsSuperAdmin)
            {
                ShowFeedback("Only Admin can create Lecturers.", FeedbackType.Error);
                return false;
            }
            bool result = false;
            Student studentToDelete = objToDelete as Student;
            if (studentToDelete == null)
            {
                ShowFeedback("No lecturer selected.", FeedbackType.Error);
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
                        UpdateLecturersList(SearchTxt, rowsToReturn);
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

        public bool Cancel()
        {
            ClearFeedback();
            if (FormContext == FormContext.Create)
                EnterNewMode();
            else
            {
                try
                {
                    UnitOfWork.Reload(SelectedLecturer);
                    UpdateLecturersList(SearchTxt, rowsToReturn);
                    OnPropertyChanged(nameof(SelectedLecturer));
                    ShowFeedback("Rolled back unsaved changes.", FeedbackType.Info);
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

        public bool GenerateNewPasswordHash(string optionalNewPassword)
        {
            ClearFeedback();
            if (!User.IsSuperAdmin)
            {
                ShowFeedback("Only Admin can generate new passwords.", FeedbackType.Error);
                return false;
            }
            bool result = false;
            try
            {
                //Confirmation Check
                Mediator.NotifyColleagues(MediatorChannels.LecturerPageGeneratePasswordCheck.ToString(), null);
                if (IsConfirmed)
                {
                    //randomly generate
                    result = SelectedLecturer.GenerateNewPasswordHash(ref optionalNewPassword, null);
                    OnPropertyChanged(nameof(SelectedLecturer));
                    if (result)
                        ShowFeedback("Password hash generated: \nRemember to save changes.", FeedbackType.Info);
                    else
                        ShowFeedback("Failed to generate new password hash.", FeedbackType.Error);
                    //Temporary display new password
                    Mediator.NotifyColleagues(MediatorChannels.LecturerPageNewPasswordDisplay.ToString(), optionalNewPassword);
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
    }
}