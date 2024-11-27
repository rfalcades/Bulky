using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            this._db = db;
            this.Category = new CategoryRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }
     
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
