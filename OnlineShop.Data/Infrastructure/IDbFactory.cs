using System;

namespace OnlineShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        OnlineShopDbContext Init();
    }
}