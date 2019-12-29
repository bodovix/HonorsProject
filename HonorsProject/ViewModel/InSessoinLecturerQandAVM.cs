﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.ViewModel.CoreVM;

namespace HonorsProject.ViewModel
{
    class InSessoinLecturerQandAVM : BaseQandAPageVM
    {

        public InSessoinLecturerQandAVM(string dbcontextName) : base(dbcontextName)
        {
            
        }

        private ISystemUser _user;

        public override ISystemUser User
        {
            get { return _user; }
            set { _user = (Lecturer)value;
                OnPropertyChanged(nameof(User));
            }
        }

        public override bool Cancel()
        {
            throw new NotImplementedException();
        }

        public override bool Delete(BaseEntity objToDelete)
        {
            throw new NotImplementedException();
        }

        public override bool MarkQuestion(Question questionToMark)
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
    }
}
