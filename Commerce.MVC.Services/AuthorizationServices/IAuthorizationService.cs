namespace Commerce.Services {
    public interface IAuthorizationService {

        /// <summary>
        /// User can do anything to the application
        /// </summary>
        bool IsSuperAdmin(string userName);
    }
}
