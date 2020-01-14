using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.CoreVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel
{
    public class MyAnswersQandAVM : BaseLecturerQandAPageVM
    {
        public MyAnswersQandAVM(ISystemUser appUser, Answer selectedAnswer, string dbcontextName) : base(appUser, dbcontextName)
        {
        }

        protected override bool UpdateQuestionsList(BaseEntity entToSearchFrom, string questionSearchTxt)
        {
            throw new NotImplementedException();
        }
    }
}