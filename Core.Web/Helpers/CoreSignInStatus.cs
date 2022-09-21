
namespace Core.Web.Helpers
{
    public enum CoreSignInStatus
    {
        // Summary:
        //     Sign in was successful
        Success = 0,
        //
        // Summary:
        //     User is locked out
        LockedOut = 1,
        //
        // Summary:
        //     Sign in requires addition verification (i.e. two factor)
        RequiresVerification = 2,
        //
        // Summary:
        //     Sign in failed
        Failure = 3,
        //
        // Summary:
        //     User tidak terdaftar
        NotRegistered = 4,
        //
        // Summary:
        //     User tidak aktif
        InActive = 5,
        //
        // Summary:
        //     User expired
        Expired = 6,
        //
        // Summary:
        //     Password salah
        IncorrectPassword = 7,
    }
}
