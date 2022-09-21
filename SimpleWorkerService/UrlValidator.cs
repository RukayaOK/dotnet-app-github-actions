using System;

namespace URLValidation
{

    public class ValidateURL
    {

        public static bool IsValid(string url)
        {if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return true;
            }
            throw new NotImplementedException("Not fully implemented.");
        }
    }
}
