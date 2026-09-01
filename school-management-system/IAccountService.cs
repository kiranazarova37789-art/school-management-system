using school_management_system.DbModels;

namespace school_management_system
{
    public interface IAccountService
    {
        Task<User?> LoginAcc(
            string name,
            string password
        );

        Task<bool> RegistrationAcc(
            string name,
            string password
        );
    }
}
