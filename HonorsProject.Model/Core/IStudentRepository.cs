using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface IStudentRepository : IRepository<Student>
    {
        Student FindById(int id);

        List<Student> GetTopXFromSearch(string searchStudentTxt, int rows);

        List<Student> GetStudentsNotInGroup(Group group);

        List<Student> GetStudentsFromGroup(Group group);
    }
}