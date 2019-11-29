using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface ISystemUser<T> where T : BaseSystemUser
    {
        T Login(int userId, string password, string conName);

        bool Register(T user, string conName);
    }
}