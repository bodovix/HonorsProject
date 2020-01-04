using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.DTO
{
    public class QuestionStateConverterDTO 
    {
		private ISystemUser _user;

		public ISystemUser User
		{
			get { return _user; }
			set { _user = value;
			}
		}
		private Question _question;

		public Question Question
		{
			get { return _question; }
			set { _question = value; }
		}
		public QuestionStateConverterDTO()
		{

		}
	}
}
