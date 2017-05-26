using InfinniPlatform.Auth;

namespace Infinni.Northwind.Authentication
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class CustomUser : AppUser
    {
        public int Age { get; set; }
    }
}