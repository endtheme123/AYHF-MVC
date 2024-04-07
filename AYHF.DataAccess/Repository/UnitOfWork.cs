﻿using AYHF.DataAccess.Data;
using AYHF.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYHF.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository CategoryRepo { get; private set; }
        public IProductRepository ProductRepo { get; private set; }
        public ICartRepository CartRepo { get; private set; }
        public IApplicationUserRepository ApplicationUserRepo { get; private set; }
        public IOrderDetailRepository OrderDetailRepo {  get; private set; }
        public IOrderHeaderRepository OrderHeaderRepo {  get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ProductRepo = new ProductRepository(_db);
            CategoryRepo = new CategoryRepository(_db);
            CartRepo = new CartRepository(_db);
            ApplicationUserRepo = new ApplicationUserRepository(_db);
            OrderHeaderRepo = new OrderHeaderRepository(_db);
            OrderDetailRepo = new OrderDetailRepository(_db);
        }
        

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
