using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(LabAssistantContext context) : base(context)
        {
        }

        public List<Group> GetForStudent(Student student, int rowLimit)
        {
            if (rowLimit > 0)
                return _entities.Where(g => g.Students.Any(s => s.Id == student.Id)).Take(rowLimit).ToList();
            else
                return _entities.Where(g => g.Students.Any(s => s.Id == student.Id)).ToList();
        }

        public List<Group> GetForStudentSearch(Student student, string groupSearchTxt, int rowLimit)
        {
            if (rowLimit > 0)
                return _entities.Where(g => g.Students.Any(s => s.Id == student.Id)
                            && (g.Name.Contains(groupSearchTxt)
                            || g.Id.ToString().Contains(groupSearchTxt))).Take(rowLimit).ToList();
            else
                return _entities.Where(g => g.Students.Any(s => s.Id == student.Id)).ToList();
        }

        public List<Group> GetGroupsNotContainingStudent(Student student)
        {
            return _entities.Where(g => !g.Students.Any(s => s.Id == student.Id)).ToList();
        }

        public List<Group> GetTopXFromSearch(string searchGroupTxt, int rows)
        {
            //if no search return all
            if (String.IsNullOrEmpty(searchGroupTxt))
                return _entities.Take(rows).ToList();
            else
                return _entities.Where(g =>
                        g.Name.Contains(searchGroupTxt)
                        || g.Id.ToString().Contains(searchGroupTxt)).Take(rows).ToList();
        }
    }
}