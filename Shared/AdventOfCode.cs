using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared
{
    public class AdventOfCode
    {
        public static string GetCookie()
        {
            Console.WriteLine("To use this program you need to create an account at 'https://adventofcode.com/' and get your cookie. This can be done in chrome by bringing up the developer console, switching to the network tab and navigating to any page. Select the page call, navigate to the 'Headers' tab and copy the value of 'cookie: '.");
            Console.Write("Please paste login cookie: ");
            var userInput = Console.ReadLine();
            if (userInput != null)
            {
                return userInput;
            }
            
            Console.WriteLine("Sorry, nothing was supplied");
            throw new Exception("Failed to get the cookie from the user.");
        }        

        public static async Task<string> GetInput(string cookie, int day)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            return await httpClient.GetStringAsync($"https://adventofcode.com/2021/day/{day}/input");
        }
    }
}