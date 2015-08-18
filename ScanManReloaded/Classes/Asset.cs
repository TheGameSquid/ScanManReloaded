using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ScanManReloaded
{
    public enum AssetState
    { 
        Incomplete,
        Unknown,
        Known
    }

    public class Asset
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string ADPath { get; set; }
        public AssetState State { get; set; }

        public Asset()
        {
            this.Type = "";
            this.Name = "";
            this.ADPath = "";
        }

        public Asset(string type)
        {
            this.Type = type;
            this.Name = "";
            this.ADPath = "";
            this.State = AssetState.Incomplete;
        }

        public Asset(string type, AssetState state)
        {
            this.Type = type;
            this.Name = "";
            this.ADPath = "";
            this.State = state;
        }

        public Asset(string type, string name)
        {
            this.Type = type;
            this.Name = name;
            this.ADPath = "";
            this.State = AssetState.Incomplete;
        }
    }
}
