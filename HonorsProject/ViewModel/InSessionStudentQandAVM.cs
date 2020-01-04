using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    class InSessionStudentQandAVM : BaseQandAPageVM
    {
        public InSessionStudentQandAVM(ISystemUser appUser, Session selectedSession, string dbcontextName) : base(dbcontextName)
        {
        }

        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set { _user = value;
                OnPropertyChanged(nameof(User)); }
        }

        public override bool Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            throw new NotImplementedException();
        }

        public override void EnterNewMode()
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool UploadImage(Image imageToUpload)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateAnswersList(BaseEntity entToSearchFrom, string AnswerSearchTxt)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateQuestionsList(BaseEntity entToSearchFrom, string QuestionSearchTxt)
        {
            throw new NotImplementedException();
        }
    }
}
