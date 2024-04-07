using AYHF.DataAccess.Data;
using AYHF.DataAccess.Repository.IRepository;
using AYHF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AYHF.DataAccess.Repository
{
    public class CartRepository : Repository<ShoppingCart>, ICartRepository
    {

        private ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}
