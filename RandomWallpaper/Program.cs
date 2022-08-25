using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using Console = Colorful.Console;
using System.Drawing;
using System.Threading;
using SunCore.Ultralight.NT62x;
namespace RandomWallpaper
{
    class Program
    {
        
    
    public static void WriteLogo()
        {
            Console.WriteLine(@"______                _                   _    _       _ _                             
| ___ \              | |                 | |  | |     | | |                            
| |_/ /__ _ _ __   __| | ___  _ __ ___   | |  | | __ _| | |_ __   __ _ _ __   ___ _ __ 
|    // _` | '_ \ / _` |/ _ \| '_ ` _ \  | |/\| |/ _` | | | '_ \ / _` | '_ \ / _ \ '__|
| |\ \ (_| | | | | (_| | (_) | | | | | | \  /\  / (_| | | | |_) | (_| | |_) |  __/ |     Squatch#0850
\_| \_\__,_|_| |_|\__,_|\___/|_| |_| |_|  \/  \/ \__,_|_|_| .__/ \__,_| .__/ \___|_|   
                                                          | |         | |              
                                                          |_|         |_|              ", Color.HotPink);
        }
        public static void Menu()
        {
            for(; ; )
            {
                Console.Clear();
                WriteLogo();
                Say("1", "Select random wallpaper and apply.");
                Say("2", "Select 5 random wallpaper and store in folder.");
                Console.WriteLine("#####################################################", Color.Pink);
                Console.Write("* ");
                string selectednum = Console.ReadLine();
                if (selectednum == "1")
                {
                    string path;
                    Console.WriteLine("* Please choose the category. Ex: Anime", Color.Pink);
                    Console.Write("* ");
                    string category = Console.ReadLine();
                    Console.WriteLine("* Please choose the Resolution. Ex: 1920x1080", Color.Pink);
                    Console.Write("* ");
                    string resolution = Console.ReadLine();
                    Console.WriteLine("* Alright, wallpaper is getting ready to apply please wait.", Color.Pink);
                    for(; ; )
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create($"https://wallhaven.cc/api/v1/search?q={category}&sorting=random&resolutions={resolution}");
                        webRequest.Proxy = null;
                        using (var response = webRequest.GetResponse())
                        using (var content = response.GetResponseStream())
                        using (var reader = new StreamReader(content))
                        {
                            var strContent = reader.ReadToEnd();
                            dynamic readWeb = JsonConvert.DeserializeObject(strContent);
                            path = readWeb["data"][0]["path"];
                        }
                        using (var client = new WebClient())
                        {
                            string pathfull = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\randomwallpaper.png";
                            client.DownloadFile(path, pathfull);
                            Wallpaper.Set(pathfull, WallpaperStyle.Fill);
                            File.Delete(pathfull);
                        }
                        Console.WriteLine("* Done!, Do it again ? (y/n)", Color.Pink);
                        Console.Write("* ");
                        string againyn = Console.ReadLine();
                        if (againyn == "n")
                        {
                            break;
                        }
                    }
                    Thread.Sleep(500);
                }
                else if (selectednum == "2")
                {
                    Console.WriteLine("* Please choose the category. Ex: Anime", Color.Pink);
                    Console.Write("* ");
                    string category = Console.ReadLine();
                    Console.WriteLine("* Please choose the Resolution. Ex: 1920x1080", Color.Pink);
                    Console.Write("* ");
                    string resolution = Console.ReadLine();
                    Console.WriteLine(@"* Please choose the Folder. Ex: C:\Users\user\Documents\MyWallpapers", Color.Pink);
                    Console.Write("* ");
                    string folder = Console.ReadLine();
                    if(Directory.Exists(folder) == false)
                    {
                        for (; ; )
                        {
                            Console.WriteLine(@"* Error! Please write a valid folder! Ex: C:\Users\user\Documents\MyWallpapers", Color.DeepPink);
                            Console.Write("* ");
                            string folder2 = Console.ReadLine();
                            if (Directory.Exists(folder2) == false)
                            {
                                Console.WriteLine(@"* Error! Please write a valid folder! Ex: C:\Users\user\Documents\MyWallpapers", Color.DeepPink);
                            }
                            else
                            {
                                folder = folder2;
                                break;
                            }
                        }
                    }
                    Console.WriteLine(@"* How much wallpaper do you want ? Ex: 5", Color.Pink);
                    Console.Write("* ");
                    int wallnum = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("* Alright, wallpaper is getting ready to apply please wait.", Color.Pink);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create($"https://wallhaven.cc/api/v1/search?q={category}&sorting=random&resolutions={resolution}");
                    webRequest.Proxy = null;
                    using (var response = webRequest.GetResponse())
                    using (var content = response.GetResponseStream())
                    using (var reader = new StreamReader(content))
                    {
                        var strContent = reader.ReadToEnd();
                        dynamic readWeb = JsonConvert.DeserializeObject(strContent);
                        using (var client = new WebClient())
                        {
                            int statwallnum = wallnum;
                            wallnum++;
                            for (int i = 1; i < wallnum; i++)
                            {
                                string path = readWeb["data"][i]["path"];
                                string pathfull = folder + $@"\randomwallpaper{i}.png";
                                client.DownloadFile(path, pathfull);
                                Console.WriteLine($"* Done! {i}/{statwallnum}", Color.Pink);
                            }
                        }
                    }
                    Console.WriteLine("* Everything is done. Check out the folder.", Color.Pink);
                }
                else
                {
                    Console.WriteLine("* Error! Please choose valid option!", Color.DeepPink);
                    Thread.Sleep(1500);
                }
            }
        }
        public static void Say(string prefix, string message)
        {
            Console.Write("[", Color.Pink);
            Console.Write(prefix, Color.MediumSpringGreen);
            Console.WriteLine("] " + message, Color.Pink);
        }
        static void Main(string[] args)
        {
            Console.Title = "Random Wallpaper";
            Menu();
            Console.ReadKey();
        }
    }
}
