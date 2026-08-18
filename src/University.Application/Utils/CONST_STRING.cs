using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Utils
{
    internal static class CONST_STRING
    {
        public static string PROPERTY_ERROR_EMPTY = "{PropertyName} can not be empty";
        public static string PROPERTY_ERROR_NULL = "{PropertyName} can not be null";
        public static string PROPERTY_ERROR_MAX_LENGTH = "{PropertyName} can not exceed 50 characters";
        public static string PROPERTY_ERROR_LETTERS_ONLY = "{PropertyName can contains only letters}";
        public static string PROPERTY_ERROR_ALPHA_NUMERIC_ONLY = "The {PropertyName} can contains only alphanumeric characters";
        public static string PROPERTY_ERROR_VALID_EMAIL = "{PropertyName} is not a valid email address";
    }
}
