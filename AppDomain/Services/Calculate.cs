using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.AppDomain.Services
{
    public class Calculate
    {
        public static double CalculateSalary(int rank, string role)
        {
            switch(role)
            {
                case "employee":
                    return 1.125 * rank;

                case "manager":
                    return 1.725 * rank;
                case "ceo":
                    return 2.725 * rank;
                default:
                    return 0;
            }
        }
    }
}
