using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mrtn.Data;
using Mrtn.Models;

namespace Mrtn.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("~/")]
        public IActionResult Index()
        {
            var buildings = Storage.ListBuildings();
            return View(new IndexModel(buildings));
        }

        [HttpGet("~/{id}")]
        public IActionResult Building(string id)
        {
            var times = Storage.ListPackages(id);
            var model = new BuildingModel(id, times);
            return View(model);
        }

        [HttpGet("~/{id}/package/{time}")]
        public IActionResult Package(string id, string time)
        {
            var t = DateTime.ParseExact(time, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var package = Storage.GetPackage(id, t);
            if (package == null)
            {
                return RedirectToAction("Building", new {id});
            }

            return View(new PackageModel(id, package));
        }

        [HttpGet("~/{id}/history")]
        public IActionResult History(string id)
        {
            var times = Storage.ListPackages(id);
            var packages = times.Select(t => Storage.GetPackage(id, t)).Where(_ => _ != null).ToArray();

            var keys = new HashSet<AggregatedInfoKey>();
            foreach (var package in packages)
            {
                foreach (var aggr in package.AggregatedInfos)
                {
                    var key = new AggregatedInfoKey
                    {
                        Type = aggr.Type,
                        Square = aggr.Square,
                        HasBalcony = aggr.HasBalcony
                    };

                    keys.Add(key);
                }
            }

            var rows = new List<HistoryModelRow>();
            foreach (var package in packages)
            {
                var row = new HistoryModelRow(package.Time, new Dictionary<AggregatedInfoKey, AggregatedInfo>());
                rows.Add(row);

                foreach (var aggr in package.AggregatedInfos)
                {
                    var key = new AggregatedInfoKey
                    {
                        Type = aggr.Type,
                        Square = aggr.Square,
                        HasBalcony = aggr.HasBalcony
                    };

                    row.InfoByKey[key] = aggr;
                }
            }

            return View(new HistoryModel(id, keys.ToArray(), rows.ToArray()));
        }

        [HttpGet("~/{id}/history/specific")]
        public IActionResult SpecificHistory(string id, FlatType t, int sq, bool b)
        {
            var times = Storage.ListPackages(id);
            var packages = times.Select(time => Storage.GetPackage(id, time)).Where(_ => _ != null).ToArray();
            var square = sq/100f;

            var keys = new HashSet<AggregatedInfoKey>();
            foreach (var package in packages)
            {
                foreach (var aggr in package.AggregatedInfos)
                {
                    var key = new AggregatedInfoKey
                    {
                        Type = aggr.Type,
                        Square = aggr.Square,
                        HasBalcony = aggr.HasBalcony
                    };

                    if (key.Type != t)
                    {
                        continue;
                    }

                    if (key.Square != square)
                    {
                        continue;
                    }

                    if (key.HasBalcony != b)
                    {
                        continue;
                    }

                    keys.Add(key);
                }
            }

            var rows = new List<HistoryModelRow>();
            foreach (var package in packages)
            {
                var row = new HistoryModelRow(package.Time, new Dictionary<AggregatedInfoKey, AggregatedInfo>());
                rows.Add(row);

                foreach (var aggr in package.AggregatedInfos)
                {
                    var key = new AggregatedInfoKey
                    {
                        Type = aggr.Type,
                        Square = aggr.Square,
                        HasBalcony = aggr.HasBalcony
                    };

                    if (!keys.Contains(key))
                    {
                        continue;
                    }

                    row.InfoByKey[key] = aggr;
                }
            }

            return View(new HistoryModel(id, keys.ToArray(), rows.ToArray()));
        }
    }
}
