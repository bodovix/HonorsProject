using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Lecturer : IEntity
    {
        public int Id { get; set; }
        public List<Answer> Answers { get; set; }

        public Lecturer()
        {
            Answers = new List<Answer>();
        }
    }
}