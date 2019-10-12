using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LabAssistantContext _context;
        public ILecturerRepository LecturerRepo { get; private set; }

        public UnitOfWork(LabAssistantContext context)
        {
            _context = context;
            LecturerRepo = new LecturerRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}