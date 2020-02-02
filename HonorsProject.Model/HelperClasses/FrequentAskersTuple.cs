using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.HelperClasses
{
    public class FrequentAskersTuple
    {
        public Student Student;
        public int Count;

        public override string ToString()
        {
            if (Student != null)
                return Student.Name + "\n" + Student.Id + "\nCount:" + Count;
            else
                return "0";
        }
    }
}