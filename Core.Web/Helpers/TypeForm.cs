
namespace Core.Web.Helpers
{
    public enum TypeForm
    {
        Html,
        MultilineText,
        boolean,
        Time,
        datetime,
        text,
        Email,
        Phone,
        dacimal,
        integer
    }
    public class PropLenght
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class PropRange
    {
        public int Max { get; set; }
        public int Min { get; set; }
    }
}
