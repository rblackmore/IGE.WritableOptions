using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGE.ConsoleTest
{
    public class UserPreferences
    {
        public static string FileName = "UserPreferences";

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public int Rank { get; set; }
    }
}
