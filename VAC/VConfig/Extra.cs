﻿using BepInEx;
using IniParser;
using IniParser.Model;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VAC.VTools;

namespace VConfig
{
    public class Extra
    {
        public static string GetServerHashFor(Configuration config)
        {
            var serialized = "";
            foreach (var prop in typeof(Configuration).GetProperties())
            {
                var keyName = prop.Name;
                var method = prop.PropertyType.GetMethod("ServerSerializeSection", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
                
                if (method != null)
                {
                    var instance = prop.GetValue(config, null);
                    string result = (string)method.Invoke(instance, new object[] { });
                    serialized += result;
                }
            }

            return Helper.CreateMD5(serialized);
        }

        public static string ConfigIniPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + Path.DirectorySeparatorChar + "vac.cfg";

        public static bool LoadSettings()
        {
            try
            {
                if (File.Exists(ConfigIniPath))
                    Configuration.Current = LoadFromIni(ConfigIniPath);
                else
                {
                    Debug.LogError("Error: Configuration not found. Plugin not loaded.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unable to load configuration file: {ex}");
                return false;
            }

            return true;
        }

        public static Configuration LoadFromIni(string filename)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData configdata = parser.ReadFile(filename);

            Configuration conf = new Configuration();
            foreach (var prop in typeof(Configuration).GetProperties())
            {
                string keyName = prop.Name;
                MethodInfo method = prop.PropertyType.GetMethod("LoadIni", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (method != null)
                {
                    var result = method.Invoke(null, new object[] { configdata, keyName });
                    prop.SetValue(conf, result, null);
                }
            }

            return conf;
        }

        public static Configuration LoadFromIni(Stream iniStream)
        {
            using (StreamReader iniReader = new StreamReader(iniStream))
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData configdata = parser.ReadData(iniReader);

                Configuration conf = new Configuration();
                foreach (var prop in typeof(Configuration).GetProperties())
                {
                    string keyName = prop.Name;
                    MethodInfo method = prop.PropertyType.GetMethod("LoadIni",
                        BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                    if (method != null)
                    {
                        object result = method.Invoke(null, new object[] {configdata, keyName});
                        prop.SetValue(conf, result, null);
                    }
                }

                return conf;
            }
        }
    }
    public static class IniDataExtensions
    {
        public static float GetFloat(this KeyDataCollection data, string key, float defaultVal)
        {
            if (float.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result)) { 
                return result;
            }

            Debug.LogWarning($" [Float] Could not read {key}, using the default value: {defaultVal}");
            return defaultVal;
        }

        public static bool GetBool(this KeyDataCollection data, string key)
        {
            var truevals = new[] { "y", "yes", "true" };
            return truevals.Contains($"{data[key]}".ToLower());
        }

        public static int GetInt(this KeyDataCollection data, string key, int defaultVal)
        {
            if (int.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result)) { 
                return result;
            }

            Debug.LogWarning($" [Int] Could not read {key}, using the default value: {defaultVal}");
            return defaultVal;
        }

        public static KeyCode GetKeyCode(this KeyDataCollection data, string key, KeyCode defaultVal)
        {
            if (Enum.TryParse<KeyCode>(data[key], out var result)) {
                return result;
            }

            Debug.LogWarning($" [KeyCode] Could not read {key}, using the default value: {defaultVal}");
            return defaultVal;
        }

        public static T LoadConfiguration<T>(this IniData data, string key) where T : BaseConfig<T>, new()
        {
            // this function gives null reference error
            KeyDataCollection idata = data[key];
            return (T)typeof(T).GetMethod("LoadIni").Invoke(null, new[] { idata });
        }
    }
}