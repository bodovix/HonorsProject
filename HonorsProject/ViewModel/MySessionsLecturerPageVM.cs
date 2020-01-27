using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using HonorsProject.Model.HelperClasses;
using HonorsProject.ViewModel.Commands;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    public class MySessionsLecturerPageVM : BaseMySessionsPageVM
    {
        #region Properties

        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set
            {
                _user = (Lecturer)value;
                OnPropertyChanged(nameof(User));
            }
        }

        #endregion Properties

        public MySessionsLecturerPageVM(ISystemUser appUser, string dbcontextName) : base(dbcontextName)
        {
            //Initial setup
            User = (Lecturer)appUser;
            UserRole = Role.Lecturer;

            GetAllLecturers();
            SelectedSession = new Session();
            FormContext = FormContext.Create;
            GetAllGroups();
            //initially loads current sessions
            GetAllMyCurrentSessions();
        }

        public override bool Save()
        {
            ClearFeedback();
            bool result;
            try
            {
                if (FormContext == FormContext.Create)
                {
                    //Create New
                    result = User.AddNewSession(SelectedSession, UnitOfWork);
                    if (result)
                    {
                        UpdateMySessionsList();
                        ShowFeedback($"Successfully created session: {SelectedSession.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to create session. please try again or contact support.", FeedbackType.Error);
                    return result;
                }
                else
                {
                    //Update
                    result = SelectedSession.ValidateSession(UnitOfWork);
                    if (result)
                    {
                        UnitOfWork.Complete();
                        ShowFeedback($"Successfully updated session: {SelectedSession.Id}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to update session: {SelectedSession.Id}.", FeedbackType.Error);
                    return result;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.GetBaseException().Message, FeedbackType.Error);
                return false;
            }
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            ClearFeedback();
            bool result = false;
            Session sessionToDelte = objToDelete as Session;
            if (sessionToDelte == null)
            {
                ShowFeedback("No session selected.", FeedbackType.Error);
                return result;
            }
            try
            {
                Mediator.NotifyColleagues(MediatorChannels.DeleteSessionConfirmation.ToString(), null);
                if (IsConfirmationAccepted)
                {
                    int sId = sessionToDelte.Id;
                    UnitOfWork.SessionRepository.Remove(sessionToDelte);
                    result = (UnitOfWork.Complete() > 0) ? true : false;
                    if (result)
                    {
                        UpdateMySessionsList();
                        ShowFeedback($"Successfully deleted Session: {sId}.", FeedbackType.Success);
                    }
                    else
                        ShowFeedback($"Failed to delete Session: {sId}.", FeedbackType.Error);
                }

                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public override void EnterNewMode()
        {
            FormContext = FormContext.Create;
            SelectedSession = new Session();
        }

        public override bool AddLecturer()
        {
            ClearFeedback();
            try
            {
                //is lecturer selected
                if (SelectedLecturer == null)
                    throw new Exception("No lecturer selected.");
                //check if lecture already in session
                if (SelectedSession.Lecturers.Contains(SelectedLecturer))
                    throw new Exception("Lecturer already in session.");
                else
                {
                    SelectedSession.Lecturers.Add(SelectedLecturer);
                    ShowFeedback($"Added lecturer: {SelectedLecturer.Id}.", FeedbackType.Success);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public override bool Remove(BaseEntity entityToRemove)
        {
            ClearFeedback();
            bool result = false;
            Lecturer lecturer = (Lecturer)entityToRemove;
            if (SelectedSession.Lecturers != null)
                //Saving done on update Save button clicked
                if (SelectedSession.Lecturers.Contains(lecturer))
                {
                    int lId = lecturer.Id;
                    result = SelectedSession.Lecturers.Remove(lecturer);
                    if (result)
                        ShowFeedback($"Lecturer {lId} removed from session {SelectedSession.Id}.", FeedbackType.Success);
                    else
                        ShowFeedback($"Failed to remove lecture {lId} from ", FeedbackType.Error);
                    return result;
                }
                else
                {
                    ShowFeedback("Lecturer not found in session.", FeedbackType.Error);
                    return result;
                }
            else
                return result;
        }
    }
}