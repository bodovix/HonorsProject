using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    internal class DbContextFactory : IDbContextFactory<LabAssistantContext>
    {
        public LabAssistantContext Create()
        {
            //return new LabAssistantContext("name=TestDBContext");
            return new LabAssistantContext("name=LabAssistantContext");
        }
    }
}