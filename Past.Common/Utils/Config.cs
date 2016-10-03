﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Past.Common.Utils
{
    public class Config
    {
        public static string LoginServer_Address { get { return GetValue("LOGIN", "Address"); } }
        public static int LoginServer_Port { get { return int.Parse(GetValue("LOGIN", "Port")); } }
        public static string GameServer_Address { get { return GetValue("GAME", "Address"); } }
        public static int GameServer_Port { get { return int.Parse(GetValue("GAME", "Port")); } }
        public static string LoginDatabase_Host { get { return GetValue("LOGIN_DATABASE", "Host"); } }
        public static string LoginDatabase_Name { get { return GetValue("LOGIN_DATABASE", "Name"); } }
        public static string LoginDatabase_Username { get { return GetValue("LOGIN_DATABASE", "Username"); } }
        public static string LoginDatabase_Password { get { return GetValue("LOGIN_DATABASE", "Password"); } }
        public static string GameDatabase_Host { get { return GetValue("GAME_DATABASE", "Host"); } }
        public static string GameDatabase_Name { get { return GetValue("GAME_DATABASE", "Name"); } }
        public static string GameDatabase_Username { get { return GetValue("GAME_DATABASE", "Username"); } }
        public static string GameDatabase_Password { get { return GetValue("GAME_DATABASE", "Password"); } }
        public static bool Debug { get { return bool.Parse(GetValue("OTHERS", "Debug")); } }
        private static Dictionary<string, Dictionary<string, string>> Elements = new Dictionary<string, Dictionary<string, string>>();
        private static Dictionary<string, string> ConfigEntries;
        #region default ini file
        private static string DefaultConfig = @"[LOGIN]
Address = 127.0.0.1		; Address for the login server
Port = 443              ; Port for the login server

[GAME]
Address = 127.0.0.1     ; Address for the game server
Port = 5555				; Port for the game server

[LOGIN_DATABASE]
Host = localhost
Name = past_login
Username = root
Password =

[GAME_DATABASE]
Host = localhost
Name = past_game
Username = root
Password =

[OTHERS]
Debug = true            ; Display in the console message received and sent";
        #endregion

        public static void ReadConfig()
        {
            string path = String.Format(@"{0}\Config.ini", Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName));
            if (!File.Exists(path))
            {
                File.WriteAllText(path, DefaultConfig);
            }
            foreach (var line in File.ReadAllLines(path))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("["))
                    {
                        string section = line.Replace("[", "").Replace("]", "");
                        ConfigEntries = new Dictionary<string, string>();
                        Elements.Add(section, ConfigEntries);
                    }
                    else if (ConfigEntries != null)
                    {
                        string[] data = line.Trim().Split('=', ';');
                        string key = data[0].Trim();
                        string value = data[1].Trim();
                        ConfigEntries.Add(key, value);
                    }
                }
            }
        }

        public static string GetValue(string section, string key)
        {
            return Elements.Where(x => x.Key == section).First().Value.Where(x => x.Key == key).First().Value;
        }
    }
}