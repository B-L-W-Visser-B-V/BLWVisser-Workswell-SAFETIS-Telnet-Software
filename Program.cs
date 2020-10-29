using blwvisserSafetis.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Timers;

namespace MinimalisticTelnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
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
            Console.WriteLine("\nInteraction Menu: \n1 - Connect with certain ip address \n2 - Connect with serial number \n3 - Open commands documentation \n4 - Connect to Thermal Frame Stream \n5 - Quit");
            Console.WriteLine("\nSelect option:");
            
            string menurespone = Console.ReadLine();

            if(menurespone == "1")
            {
                Console.Clear();
                Console.WriteLine("Enter SAFETIS ip:");
                string safetisip = Console.ReadLine();
                Console.WriteLine("Enter SAFETIS port:");
                int safetisport = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                ConnectTelnet(safetisip, safetisport);
            }
            if (menurespone == "2")
            {
                Console.Clear();
                Console.WriteLine("Enter the the first 4 numbers of the SAFETIS serial number:");
                string safetis_sn = Console.ReadLine();
                string safetisip = "10.0." + safetis_sn.Insert(2, ".");
                Console.WriteLine("Enter SAFETIS port:");
                int safetisport = Convert.ToInt32(Console.ReadLine());
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
                Console.WriteLine("Enter SAFETIS ip::");
                string safetisip = Console.ReadLine();
                int safetisport = 2252;
                Console.Clear();
                ConnectTelnet(safetisip, safetisport);
            }
            if (menurespone == "5")
            {
                System.Environment.Exit(1);
            }
            if (menurespone == "6")
            {
                System.Environment.Exit(1);
            }
            Console.Clear();
            Menu();
        }

        static void ConnectTelnet(string safetisip, int safetisport)
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

            while (tc.IsConnected && prompt.Trim() != "exit")
            {
                Console.Write(tc.Read());
                File.AppendAllText(@"c:\blwvisser\test.png", tc.Read());
                prompt = Console.ReadLine();
                tc.WriteLine(prompt);

                Console.Write(tc.Read());
            }
        }
        static void finddevices()
        {

        }
    }
}
