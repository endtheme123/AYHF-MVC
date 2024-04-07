using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYHF.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepo { get; }
        IProductRepository ProductRepo { get; }
        ICartRepository CartRepo { get; }
        IApplicationUserRepository ApplicationUserRepo { get; }
        IOrderDetailRepository OrderDetailRepo { get; }
        IOrderHeaderRepository OrderHeaderRepo { get; }

        void Save();
    }
}
