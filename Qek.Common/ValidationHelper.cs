using System.Text.RegularExpressions;
using System;

namespace Qek.Common
{
    /// <summary>
    /// DateTime:   2012/1/19
    /// Author:     Sam.SH_Chang#21978
    /// Validation Tool Class
    /// </summary>
    public class ValidationHelper
    {
        // Function to test for Positive Integers. 
        public static bool IsNaturalNumber(String strNumber)
        {
            Regex objNotNaturalPattern = new Regex("[^0-9]");
            Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");
            return !objNotNaturalPattern.IsMatch(strNumber) &&
            objNaturalPattern.IsMatch(strNumber);
        }
        // Function to test for Positive Integers with zero inclusive 
        public static bool IsWholeNumber(String strNumber)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strNumber);
        }
        // Function to Test for Integers both Positive & Negative 
        public static bool IsInteger(String strNumber)
        {
            Regex objNotIntPattern = new Regex("[^0-9-]");
            Regex objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");
            return !objNotIntPattern.IsMatch(strNumber) && objIntPattern.IsMatch(strNumber);
        }
        // Function to Test for Positive Number both Integer & Real 
        public static bool IsPositiveNumber(String strNumber)
        {
            Regex objNotPositivePattern = new Regex("[^0-9.]");
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            return !objNotPositivePattern.IsMatch(strNumber) &&
            objPositivePattern.IsMatch(strNumber) &&
            !objTwoDotPattern.IsMatch(strNumber);
        }
        // Function to test whether the string is valid number or not
        public static bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
            return !objNotNumberPattern.IsMatch(strNumber) &&
            !objTwoDotPattern.IsMatch(strNumber) &&
            !objTwoMinusPattern.IsMatch(strNumber) &&
            objNumberPattern.IsMatch(strNumber);
        }
        // Function To test for Alphabets. 
        public static bool IsAlpha(String strToCheck)
        {
            Regex objAlphaPattern = new Regex("[^a-zA-Z]");
            return !objAlphaPattern.IsMatch(strToCheck);
        }
        // Function to Check for AlphaNumeric.
        public static bool IsAlphaNumeric(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }
        // Function to Check for AlphaNumeric.
        public static bool IsSpecifyLengthWholeNumber(String strNumber, int length)
        {
            Regex objNotWholePattern = new Regex("[0-9]{" + Convert.ToString(length) + "}");
            return objNotWholePattern.IsMatch(strNumber);
        }
        /// <summary>
        /// Determines whether [is valid email] [the specified email address].
        /// For single email address
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid email] [the specified email address]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmail(string emailAddress)
        {
            //string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //Regex reLenient = new Regex(patternLenient);
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                  + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                  + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                  + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                  + @"[a-zA-Z]{2,}))$";
            Regex reStrict = new Regex(patternStrict);

            bool isStrictMatch = reStrict.IsMatch(emailAddress);
            return isStrictMatch;
        }
        /// <summary>
        /// Determines whether [is valid multi email separate by specify char] [the specified STR email].
        /// For multiple email addresses 
        /// </summary>
        /// <param name="strEmail">The STR email.</param>
        /// <param name="specifyChar">The specify char.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid multi email separate by specify char] [the specified STR email]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidMultiEmailSeparateBySpecifyChar(string strEmail, char specifyChar)
        {
            bool flag = true;
            string[] emailAddresses = strEmail.Split(new char[] { specifyChar }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string email in emailAddresses)
            {
                if (!IsValidEmail(email))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
    }
}
