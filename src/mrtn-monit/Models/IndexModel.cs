using System;
using System.Collections.Generic;
using Mrtn.Data;

namespace Mrtn.Models
{
    public class IndexModel
    {
        public IndexModel(string[] buildings)
        {
            Buildings = buildings;
        }

        public string[] Buildings { get; }
    }

    public class BuildingModel
    {
        public BuildingModel(string id, DateTime[] times)
        {
            Id = id;
            Times = times;
        }

        public string Id { get; }
        public DateTime[] Times { get; }
    }

    public class PackageModel
    {
        public PackageModel(string id, DataPackage package)
        {
            Id = id;
            Package = package;
        }

        public string Id { get; }
        public DataPackage Package { get; }
    }

    public class HistoryModel
    {
        public HistoryModel(string id, AggregatedInfoKey[] keys, HistoryModelRow[] rows)
        {
            Id = id;
            Keys = keys;
            Rows = rows;
        }

        public string Id { get; }
        public AggregatedInfoKey[] Keys { get; }
        public HistoryModelRow[] Rows { get; }
    }
    
    public class HistoryModelRow
    {
        public HistoryModelRow(DateTime time, Dictionary<AggregatedInfoKey, AggregatedInfo> infoByKey)
        {
            Time = time;
            InfoByKey = infoByKey;
        }

        public DateTime Time { get; }

        public Dictionary<AggregatedInfoKey, AggregatedInfo> InfoByKey { get; }
    }
}
