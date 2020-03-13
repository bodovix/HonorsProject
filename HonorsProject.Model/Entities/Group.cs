using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using System.Collections.ObjectModel;

namespace HonorsProject.Model.Entities
{
    public class Group : BaseEntity
    {
        public virtual ObservableCollection<Student> Students { get; set; }
        public virtual ObservableCollection<Session> Sessions { get; set; }
        public int CreatedByLecturerId { get; set; }

        public Group()
        {
            Students = new ObservableCollection<Student>();
            Sessions = new ObservableCollection<Session>();
        }

        public Group(string name, ObservableCollection<Student> students, ObservableCollection<Session> sessions, DateTime createdOn, int createdByLecturerId)
        {
            Name = name;
            Students = students;
            Sessions = sessions;
            CreatedOn = createdOn;
            CreatedByLecturerId = createdByLecturerId;
        }

        public void ShallowCopy(Group selectedGroup)
        {
            Id = selectedGroup.Id;
            Name = selectedGroup.Name;
            Students = selectedGroup.Students;
            Sessions = selectedGroup.Sessions;
            CreatedOn = selectedGroup.CreatedOn;
            CreatedByLecturerId = selectedGroup.CreatedByLecturerId;
        }

        public bool ValidateGroup(UnitOfWork u)
        {
            //ID auto incremented by EF
            if (String.IsNullOrEmpty(Name))
                throw new ArgumentException("Group name required.");
            if (Name.Length > nameSizeLimit)
                throw new ArgumentException($"Group name cannot exceed {nameSizeLimit} chars.");
            if (CreatedOn == null)
                throw new ArgumentException("Group created on date required.");
            if (CreatedByLecturerId == 0)
                throw new ArgumentException("Group created by Id required.");
            if (u.GroupRepository.CheckGroupNameAlreadyExists(this))
                throw new ArgumentException("Group name must be Unique.");
            return true;
        }

        public bool RemoveStudent(Student studentToRemove, UnitOfWork u, ref string msg)
        {
            if (Students.Contains(studentToRemove))
            {
                Students.Remove(studentToRemove);
                int rows = u.Complete();
                if (rows > 0)
                    return true;
                else
                {
                    msg = "No record found to delete in database. \nRefresh to try again or contact support.";
                    return false;
                }
            }
            else
            {
                msg = "Student Not in Selected Group. Refresh and try again.";
                return false;
            }
        }
    }
}