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

        public List<Group> GetGroupsNotContainingStudent(Student student)
        {
            return _entities.Where(g => !g.Students.Any(s => s.Id == student.Id)).ToList();
        }
    }
}