using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ToolkitCore.Models;
using UnityEngine;
using Verse;

namespace ToolkitCore.Database
{
    [StaticConstructorOnStartup]
    public static class DatabaseController
    {
        static readonly string modFolder = "ToolkitCore";
        public static readonly string dataPath = Application.persistentDataPath + $"/{modFolder}/";

        static DatabaseController()
        {
            Main();
        }

        static void Main()
        {
            if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
        }

        public static void SaveToolkit()
        {
            ViewerManager.SaveViewers();
        }

        public static void LoadToolkit()
        {

        }

        public static void SaveObject(object obj, string fileName, Mod mod)
        {
            if (mod.Content.Name == null)
            {
                Log.Error("Mod has no name");
                return;
            }

            fileName = $"{mod.Content.Name.Replace(" ", "")}_{fileName}.json";

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            SaveFile(json, fileName);
        }

        public static bool LoadObject<T>(string fileName, Mod mod, out T obj)
        {
            obj = default;

            if (mod.Content.Name == null)
            {
                Log.Error("Mod has no name");
                return false;
            }

            fileName = $"{mod.Content.Name.Replace(" ", "")}_{fileName}.json";

            if (!LoadFile(fileName, out string json))
            {
                Log.Warning($"Tried to load {fileName} but could not find file");
                return false;
            }

            obj = JsonConvert.DeserializeObject<T>(json);

            return true;
        }

        public static bool SaveFile(string json, string fileName)
        {
            Log.Message(json);
            try
            {
                using (StreamWriter streamWriter = File.CreateText(Path.Combine(dataPath, fileName)))
                {
                    streamWriter.Write(json.ToString());
                }
            }
            catch (IOException e)
            {
                Log.Error(e.Message);
                return false;
            }

            return true;
        }

        public static bool LoadFile(string fileName, out string json)
        {
            json = null;

            var file = Path.Combine(dataPath, fileName);
            if (!File.Exists(file)) return false;

            try
            {
                using (StreamReader streamReader = File.OpenText(file))
                {
                    json = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
                return false;
            }

            return true;
        }
    }
}
