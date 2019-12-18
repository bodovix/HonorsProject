using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface IGroupRepository : IRepository<Group>
    {
        List<Group> GetGroupsNotContainingStudent(Student student);

        List<Group> GetTopXFromSearch(string searchGroupTxt, int rows);
    }
}