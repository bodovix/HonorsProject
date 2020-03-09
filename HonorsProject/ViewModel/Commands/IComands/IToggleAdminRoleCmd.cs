using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IToggleAdminRoleCmd
    {
        ToggleAdminRoleCmd ToggleAdminRoleCmd { get; set; }

        bool ToggleAdminRole(Lecturer lecturerToToggle);
    }
}