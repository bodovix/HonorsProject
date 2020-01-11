using HonorsProject.Model.Data;
using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.CoreVM
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected string dbConName;
        private string _feedbackMessage;

        public string FeedbackMessage
        {
            get { return _feedbackMessage; }
            set
            {
                _feedbackMessage = value;
                OnPropertyChanged(nameof(FeedbackMessage));
            }
        }

        private FeedbackType _feedbackType;

        public FeedbackType FeedbackType
        {
            get { return _feedbackType; }
            set
            {
                _feedbackType = value;
                OnPropertyChanged(nameof(FeedbackType));
            }
        }

        public UnitOfWork UnitOfWork { get; set; }

        private Role _userRole;

        public Role UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                OnPropertyChanged(nameof(UserRole));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseViewModel(string dbcontextName)
        {
            this.dbConName = dbcontextName;
            UnitOfWork = new UnitOfWork(new LabAssistantContext(dbConName));
        }

        protected void ClearFeedback()
        {
            FeedbackMessage = "";
        }

        protected void InfoMessage(string message)
        {
            FeedbackMessage = message;
            FeedbackType = FeedbackType.Info;
        }

        protected bool ErrorFeedback(string message)
        {
            FeedbackMessage = message;
            FeedbackType = FeedbackType.Error;
            return false;
        }

        protected bool SuccessFeedback(string message)
        {
            FeedbackMessage = message;
            FeedbackType = FeedbackType.Success;
            return true;
        }
    }
}