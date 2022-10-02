using System.Globalization;
using System.Text.RegularExpressions;

namespace APICatalogo.Functions
{
    public class Funcoes
    {
        public static bool IsValidEmail(string email)
        {
            //string regex = @"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$";
            string regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
    }
}
