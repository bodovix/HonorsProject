using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class LabAssistantContext : DbContext
    {
        //define tables
        public LabAssistantContext() : base("name=DefaultConnection")
        {
        }
    }
}