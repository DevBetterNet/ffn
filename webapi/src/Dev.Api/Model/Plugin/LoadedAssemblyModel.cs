using System;

namespace Dev.Api.Model.Plugin
{
    public class LoadedAssemblyModel
    {
        public string FullName { get; set; }
        public string Location { get; set; }
        public bool IsDebug { get; set; }
        public DateTime? BuildDate { get; set; }
    }
}
