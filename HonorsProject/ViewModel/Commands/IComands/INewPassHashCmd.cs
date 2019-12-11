using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface INewPassHashCmd
    {
        NewPassHashCmd NewPassHashCmd { get; set; }

        bool GenerateNewPasswordHash(string optionalNewPassword);
    }
}