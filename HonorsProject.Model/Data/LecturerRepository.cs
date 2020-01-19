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

        public List<Lecturer> GetTopXFromSearch(string searchTxt, int rows)
        {
            //if no search return all
            if (String.IsNullOrEmpty(searchTxt))
                return _entities.Take(rows).ToList();
            else
                return _entities.Where(s =>
                        s.Name.Contains(searchTxt)
                        || s.Email.Contains(searchTxt)
                        || s.Id.ToString().Contains(searchTxt)).Take(rows).ToList();
        }
    }
}