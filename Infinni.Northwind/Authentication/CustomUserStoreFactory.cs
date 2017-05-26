using InfinniPlatform.Auth;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Identity;

namespace Infinni.Northwind.Authentication
{
    /// <summary>
    /// Фабрика, создающая экземпляры <see cref="CustomUserStore{TUser}" />.
    /// </summary>
    public class CustomUserStoreFactory : IUserStoreFactory
    {
        public IUserStore<TUser> Get<TUser>(IContainerResolver resolver) where TUser : AppUser
        {
            return new CustomUserStore<TUser>();
        }
    }
}