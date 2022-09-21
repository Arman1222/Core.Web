
namespace Core.Web.Security
{
    public class CoreLicense
    {
        /*** Check if a valid license file is available. ***/
        public static bool IsValidLicenseAvailable()
        {
            return License.Status.Licensed;
        }

        /*** Invalidate the license. Please note, your protected software does not accept a license file anymore! ***/
        public static string InvalidateLicense()
        {
            string confirmation_code = License.Status.InvalidateLicense();
            return confirmation_code;
        }

        /*** Check if a confirmation code is valid ***/
        public static bool CheckConfirmationCode(string confirmation_code)
        {
            return License.Status.CheckConfirmationCode(License.Status.HardwareID,
            confirmation_code);
        }

        /*** Get Hardware ID of the current machine ***/
        public static string GetHardwareID()
        {
            return License.Status.HardwareID;
        }

        /*** Compare current Hardware ID with Hardware ID stored in License File ***/
        public static bool CompareHardwareID()
        {
            if (License.Status.HardwareID == License.Status.License_HardwareID)
                return true;
            else
                return false;
        }
    }
}
