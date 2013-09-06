using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Core.Organization
{
    public class PasswordComplexValidator
    {
        const int CHAR_A = (int)'A';
        const int CHAR_Z = (int)'Z';
        const int CHAR_a = (int)'a';
        const int CHAR_z = (int)'z';
        const int CHAR_0 = (int)'0';
        const int CHAR_9 = (int)'9';

        public static bool Validate(string password)
        {
            bool contains_A_Z = false;
            bool contains_a_z = false;
            bool contains_0_9 = false;
            foreach(char pchar in password)
            {
                if(pchar >= CHAR_A && pchar <= CHAR_Z)
                {
                    contains_A_Z = true;
                }
                else if (pchar >= CHAR_a && pchar <= CHAR_z)
                {
                    contains_a_z = true;
                }
                else if (pchar >= CHAR_0 && pchar <= CHAR_9)
                {
                    contains_0_9 = true;
                }
            }

            int dd = 0;
            if(contains_A_Z)
            {
                dd += 1;
            }
            if(contains_a_z)
            {
                dd += 2;
            }
            if(contains_0_9)
            {
                dd += 4;
            }
            if (dd == 3 || dd == 5 || dd == 6 || dd == 7)
            {
                return true;
            }
            return false;
        }
    }
}
