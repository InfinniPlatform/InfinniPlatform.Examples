using InfinniPlatform.Auth.Identity;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Identity;

namespace Infinni.Northwind.Auth
{
    /// <summary>
    /// Фабрика, создающая экземпляры <see cref="CustomUserStore{TUser}" />.
    /// </summary>
    public class CustomUserStoreFactory : ICustomUserStoreFactory
    {
        public IUserStore<TUser> GetUserStore<TUser>(IContainerResolver resolver) where TUser : AppUser
        {
            return new CustomUserStore<TUser>();
        }
    }
}