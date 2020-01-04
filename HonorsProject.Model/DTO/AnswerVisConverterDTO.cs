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
    public class AnswerVisConverterDTO 
    {
		private ISystemUser _user;

		public ISystemUser User
		{
			get { return _user; }
			set { _user = value;
			}
		}
		private Question _answer;

		public Question Answer
		{
			get { return _answer; }
			set { _answer = value; }
		}
		public AnswerVisConverterDTO()
		{

		}
	}
}
