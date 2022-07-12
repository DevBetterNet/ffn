using Dev.Api.Model.Plugin;
using Dev.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dev.Api.Controllers;

public class PluginController : PublicController
{
    #region Fields

    private readonly IDevFileProvider _fileProvider;

    #endregion Fields

    #region Ctor

    public PluginController(IDevFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    #endregion Ctor

    // GET: api/<ValuesController>
    [HttpGet]
    public List<LoadedAssemblyModel> Get()
    {
        List<LoadedAssemblyModel> loadedAssemblyModels = new List<LoadedAssemblyModel>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var loadedAssemblyModel = new LoadedAssemblyModel
            {
                FullName = assembly.FullName
            };

            //ensure no exception is thrown
            try
            {
                loadedAssemblyModel.Location = assembly.IsDynamic ? null : assembly.Location;
                loadedAssemblyModel.IsDebug = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false)
                    .FirstOrDefault() is DebuggableAttribute attribute && attribute.IsJITOptimizerDisabled;

                //https://stackoverflow.com/questions/2050396/getting-the-date-of-a-net-assembly
                //we use a simple method because the more Jeff Atwood's solution doesn't work anymore
                //more info at https://blog.codinghorror.com/determining-build-date-the-hard-way/
                loadedAssemblyModel.BuildDate = assembly.IsDynamic ? null : (DateTime?)TimeZoneInfo.ConvertTimeFromUtc(_fileProvider.GetLastWriteTimeUtc(assembly.Location), TimeZoneInfo.Local);
            }
            catch { }
            loadedAssemblyModels.Add(loadedAssemblyModel);
        }

        return loadedAssemblyModels;
    }
}
