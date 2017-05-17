using InfinniPlatform.Auth;

namespace Infinni.Northwind.Auth
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class CustomUser : AppUser
    {
        public int Age { get; set; }
    }
}