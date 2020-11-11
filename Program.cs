using System;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;

namespace MinimalisticTelnet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Workswell SAFETIS Software | BLWVisser";
            Console.Beep();
            Menu();
        }

        public static void Menu()
        {
            Console.ForegroundColor = GetRandomConsoleColor();
            string blwvisser = @"
    ____  __ _       ___    ___                    
   / __ )/ /| |     / / |  / (_)____________  _____
  / __  / / | | /| / /| | / / / ___/ ___/ _ \/ ___/
 / /_/ / /__| |/ |/ / | |/ / (__  |__  )  __/ /    
/_____/_____/__/|__/  |___/_/____/____/\___/_/     
                                                   
";
            Console.WriteLine(blwvisser);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("BLWVisser SAFETIS Software - WARNING! All commands must be executed in CAPS");
            Console.ResetColor();
            Console.WriteLine("\nInteraction Menu: \n1 - Connect with certain ip address \n2 - Connect with serial number \n3 - Open commands documentation \n4 - Connect to Thermal Frame Stream\n5 - Device Availability Check \n6 - Quit");
            Console.WriteLine("Enter the button and press enter.");
            Console.WriteLine("\nSelect option:");

            string menurespone = Console.ReadLine();

            if (menurespone == "1")
            {
                Console.Clear();
                Console.WriteLine("Enter SAFETIS ip:");
                string safetisip = Console.ReadLine();
                int safetisport = 2245;
                Console.Clear();
                ConnectTelnet(safetisip, safetisport);
            }
            if (menurespone == "2")
            {
                Console.Clear();
                Console.WriteLine("Enter the the first 4 numbers of the SAFETIS serial number:");
                string safetis_sn = Console.ReadLine();
                string safetisip = "10.0." + safetis_sn.Insert(2, ".");
                int safetisport = 2245;
                Console.Clear();
                ConnectTelnet(safetisip, safetisport);
            }
            if (menurespone == "3")
            {
                Console.Clear();
                Console.WriteLine("Opening Guide in default browser...");
                System.Threading.Thread.Sleep(1000);
                System.Diagnostics.Process.Start("http://www.blwvisser.nl/safetis_ethernet_sdk_usermanual_1-4/");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
                Console.Clear();
            }
            if (menurespone == "4")
            {
                Console.Clear();
                Console.WriteLine("Enter SAFETIS ip:");
                string safetisip = Console.ReadLine();
                int safetisport = 2252;
                Console.Clear();
                ConnectTelnet(safetisip, safetisport);
            }
            if (menurespone == "5")
            {
                Console.Clear();
                Console.WriteLine("Enter device IP:");
                string safetisip = Console.ReadLine();
                finddevices(safetisip);
            }
            if (menurespone == "6")
            {
                System.Environment.Exit(1);
            }
            Console.Clear();
            Menu();
        }

        private static void ConnectTelnet(string safetisip, int safetisport)
        {
            try
            {
                TelnetConnection tc = new TelnetConnection(safetisip, safetisport);

                string s = tc.Login("root", "rootpassword", 100);
                Console.Write(s);

                string prompt = s.TrimEnd();
                prompt = s.Substring(prompt.Length - 1, 1);

                prompt = "";

                if (tc.IsConnected == true)
                {
                    Console.WriteLine("\n\nConnected to " + safetisip);
                    Console.WriteLine("Type 'exit' to disconnect");
                }

                while (tc.IsConnected && prompt.Trim() != "exit" && prompt.Trim() != "leave" && prompt.Trim() != "disconnect")
                {
                    Console.Write(tc.Read());
                    prompt = Console.ReadLine();
                    tc.WriteLine(prompt);
                    Console.Write(tc.Read());
                    File.AppendAllText(@"c:\blwvisser\test.txt", tc.Read());
                    File.AppendAllText(@"c:\blwvisser\test.png", tc.Read());
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Interne Fout!");
                Menu();
            }
        }

        private static void finddevices(string safetisip)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingresult = ping.Send(safetisip);
                if (pingresult.Status.ToString() == "Success")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ONLINE!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("OFFLINE!");
                    Console.ResetColor();
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ip ongeldig!");
                Console.ResetColor();
            }
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }

        private static Random _random = new Random();
        private static ConsoleColor GetRandomConsoleColor()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)consoleColors.GetValue(_random.Next(consoleColors.Length));
        }
    }
}
