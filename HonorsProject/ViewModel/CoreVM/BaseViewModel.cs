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
    }
}