using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Mrtn.Data
{
    public static class Storage
    {
        private static readonly object SyncRoot = new object();
        private static string _rootPath;

        public static void Initialize(string rootPath)
        {
            lock (SyncRoot)
            {
                _rootPath = rootPath;
                if (!Directory.Exists(_rootPath))
                {
                    Directory.CreateDirectory(_rootPath);
                }
            }
        }
        
        public static DataPackage GetPackage(string building, DateTime time)
        {
            lock (SyncRoot)
            {
                var path = Path.Combine(_rootPath, $"{building}", $"{time:yyyyMMdd}.json");
                if (!File.Exists(path))
                {
                    return null;
                }

                var json = File.ReadAllText(path, Encoding.UTF8);
                var package = JsonConvert.DeserializeObject<DataPackage>(json);
                return package;
            }
        }

        public static bool PackageExists(string building, DateTime time)
        {
            lock (SyncRoot)
            {
                var dir = Path.Combine(_rootPath, $"{building}");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var path = Path.Combine(dir, $"{time:yyyyMMdd}.json");
                return File.Exists(path);
            }
        }

        public static void StorePackage(string building, DateTime time, DataPackage package)
        {
            lock (SyncRoot)
            {
                var dir= Path.Combine(_rootPath, $"{building}");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var path = Path.Combine(dir, $"{time:yyyyMMdd}.json");

                var json = JsonConvert.SerializeObject(package);
                File.WriteAllText(path, json, Encoding.UTF8);
            }
        }

        public static string[] ListBuildings()
        {
            lock (SyncRoot)
            {
                var list = new List<string>();
                foreach (var dir in Directory.EnumerateDirectories(_rootPath))
                {
                    var building = Path.GetFileName(dir);
                    list.Add(building);
                }

                return list.ToArray();
            }
        }

        public static IDictionary<string, DateTime[]> ListPackages()
        {
            lock (SyncRoot)
            {
                var dict = new Dictionary<string, DateTime[]>();
                foreach (var dir in Directory.EnumerateDirectories(_rootPath))
                {
                    var building = Path.GetFileName(dir);
                    var times = ReadPackages(dir).OrderBy(_ => _).ToArray();
                    dict[building] = times;
                }

                return dict;
            }
        }

        public static DateTime[] ListPackages(string building)
        {
            lock (SyncRoot)
            {
                var dir = Path.Combine(_rootPath, building);
                if (!Directory.Exists(dir))
                {
                    return new DateTime[0];
                }

                var times = ReadPackages(dir).OrderBy(_ => _).ToArray();
                return times;
            }
        }

        private static IEnumerable<DateTime> ReadPackages(string dir)
        {
            foreach (var filename in Directory.EnumerateFiles(dir, "*.json"))
            {
                var name = Path.GetFileNameWithoutExtension(filename);
                DateTime time;
                if (DateTime.TryParseExact(
                    name, 
                    "yyyyMMdd", 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.AssumeLocal,
                    out time))
                {
                    yield return time;
                }
            }
        }
    }
}