using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class LecturerRepository : Repository<Lecturer>, ILecturerRepository
    {
        public LecturerRepository(LabAssistantContext context) : base(context)
        {
        }

        //Lecturer specific Query's to be populated below
        public IEnumerable<Lecturer> GetLecturerWhereX(int id)
        {
            throw new NotImplementedException();
        }
    }
}