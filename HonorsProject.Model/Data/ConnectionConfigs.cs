using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public static class ConnectionConfigs
    {
        public static readonly string LiveConfig = "name=LabAssistantContext";
        public static readonly string TestConfig = "TestDBContext";
    }
}