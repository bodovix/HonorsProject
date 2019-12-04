using HonorsProject.Model.Data;
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
        public UnitOfWork UnitOfWork { get; set; }

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