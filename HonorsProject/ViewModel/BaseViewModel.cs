using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected LabAssistantContext _labAssistantContext;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseViewModel(LabAssistantContext labAssistantContext)
        {
            _labAssistantContext = labAssistantContext;
        }
    }
}