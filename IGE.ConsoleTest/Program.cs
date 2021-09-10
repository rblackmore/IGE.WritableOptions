using IGE.ApplicationPreferences;
using System;

namespace IGE.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var prefs = Preferences<UserPreferences>.Create(UserPreferences.FileName);

            Console.WriteLine(prefs.Value.DOB); 
            Console.WriteLine(prefs.Value.FirstName); 
            Console.WriteLine(prefs.Value.LastName);
            Console.WriteLine(prefs.Value.Rank); 
            Console.WriteLine(prefs.Value.UserName);

        }
    }
}
