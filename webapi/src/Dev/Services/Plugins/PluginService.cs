using Dev.Core;
using Dev.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dev.Services.Plugins
{
    /// <summary>
    /// Represents the plugin service implementation
    /// </summary>
    public partial class PluginService : IPluginService
    {
        #region Fields

        private readonly IDevFileProvider _fileProvider;
        private readonly IPluginsInfo _pluginsInfo;

        #endregion Fields

        #region Ctor

        public PluginService(IDevFileProvider fileProvider)
        {
            _fileProvider = fileProvider;

            _pluginsInfo = Singleton<IPluginsInfo>.Instance;
        }

        #endregion Ctor

        #region Utilities

        /// <summary>
        /// Check whether to load the plugin based on the load mode passed
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="loadMode">Load plugins mode</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByLoadMode(PluginDescriptor pluginDescriptor, LoadPluginsMode loadMode)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            switch (loadMode)
            {
                case LoadPluginsMode.All:
                    return true;

                case LoadPluginsMode.InstalledOnly:
                    return pluginDescriptor.Installed;

                case LoadPluginsMode.NotInstalledOnly:
                    return !pluginDescriptor.Installed;

                default:
                    throw new NotSupportedException(nameof(loadMode));
            }
        }

        /// <summary>
        /// Check whether to load the plugin based on the plugin group passed
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="group">Group name</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByPluginGroup(PluginDescriptor pluginDescriptor, string group)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            if (string.IsNullOrEmpty(group))
                return true;

            return group.Equals(pluginDescriptor.Group, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Check whether to load the plugin based on the store identifier passed
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="webAppId">WebApp identifier</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByWebApp(PluginDescriptor pluginDescriptor, Guid webAppId)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            //no validation required
            if (webAppId == Guid.Empty)
                return true;

            if (!pluginDescriptor.LimitedToWebApps.Any())
                return true;

            return pluginDescriptor.LimitedToWebApps.Contains(webAppId);
        }

        /// <summary>
        /// Check whether to load the plugin based on dependency from other plugin
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="dependsOnSystemName">Other plugin system name</param>
        /// <returns>Result of check</returns>
        protected virtual bool FilterByDependsOn(PluginDescriptor pluginDescriptor, string dependsOnSystemName)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            if (string.IsNullOrEmpty(dependsOnSystemName))
                return true;

            return pluginDescriptor.DependsOn?.Contains(dependsOnSystemName) ?? false;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <typeparam name="TPlugin">The type of plugins to get</typeparam>
        /// <param name="loadMode">Filter by load plugins mode</param>
        /// <param name="customer">Filter by  customer; pass null to load all records</param>
        /// <param name="storeId">Filter by store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <param name="dependsOnSystemName">System name of the plugin to define dependencies</param>
        /// <returns>Plugin descriptors</returns>
        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors<TPlugin>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
             Guid webAppId = new Guid(), string group = null, string dependsOnSystemName = "") where TPlugin : class, IPlugin
        {
            var pluginDescriptors = _pluginsInfo.PluginDescriptors;

            //filter plugins
            pluginDescriptors = pluginDescriptors.Where(descriptor =>
                FilterByLoadMode(descriptor, loadMode) &&
                FilterByWebApp(descriptor, webAppId) &&
                FilterByPluginGroup(descriptor, group) &&
                FilterByDependsOn(descriptor, dependsOnSystemName)).ToList();

            //filter by the passed type
            if (typeof(TPlugin) != typeof(IPlugin))
                pluginDescriptors = pluginDescriptors.Where(descriptor => typeof(TPlugin).IsAssignableFrom(descriptor.PluginType)).ToList();

            //order by group name
            pluginDescriptors = pluginDescriptors.OrderBy(descriptor => descriptor.Group)
                .ThenBy(descriptor => descriptor.DisplayOrder).ToList();

            return pluginDescriptors;
        }

        /// <summary>
        /// Get a plugin descriptor by the system name
        /// </summary>
        /// <typeparam name="TPlugin">The type of plugin to get</typeparam>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="loadMode">Load plugins mode</param>
        /// <param name="customer">Filter by  customer; pass null to load all records</param>
        /// <param name="storeId">Filter by store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <returns>>Plugin descriptor</returns>
        public virtual PluginDescriptor GetPluginDescriptorBySystemName<TPlugin>(string systemName,
            LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
             Guid webAppId = new Guid(), string group = null) where TPlugin : class, IPlugin
        {
            return GetPluginDescriptors<TPlugin>(loadMode, webAppId, group)
                .FirstOrDefault(descriptor => descriptor.SystemName.Equals(systemName));
        }

        /// <summary>
        /// Get plugins
        /// </summary>
        /// <typeparam name="TPlugin">The type of plugins to get</typeparam>
        /// <param name="loadMode">Filter by load plugins mode</param>
        /// <param name="customer">Filter by customer; pass null to load all records</param>
        /// <param name="storeId">Filter by store; pass 0 to load all records</param>
        /// <param name="group">Filter by plugin group; pass null to load all records</param>
        /// <returns>Plugins</returns>
        public virtual IEnumerable<TPlugin> GetPlugins<TPlugin>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
             Guid webAppId = new Guid(), string group = null) where TPlugin : class, IPlugin
        {
            return GetPluginDescriptors<TPlugin>(loadMode, webAppId, group)
                .Select(descriptor => descriptor.Instance<TPlugin>());
        }

        /// <summary>
        /// Find a plugin by the type which is located into the same assembly as a plugin
        /// </summary>
        /// <param name="typeInAssembly">Type</param>
        /// <returns>Plugin</returns>
        public virtual IPlugin FindPluginByTypeInAssembly(Type typeInAssembly)
        {
            if (typeInAssembly == null)
                throw new ArgumentNullException(nameof(typeInAssembly));

            //try to do magic
            var pluginDescriptor = _pluginsInfo.PluginDescriptors.FirstOrDefault(descriptor =>
               descriptor.ReferencedAssembly?.FullName.Equals(typeInAssembly.Assembly.FullName, StringComparison.InvariantCultureIgnoreCase) ?? false);

            return pluginDescriptor?.Instance<IPlugin>();
        }

        /// <summary>
        /// Get plugin logo URL
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor</param>
        /// <returns>Logo URL</returns>
        public virtual string GetPluginLogoUrl(PluginDescriptor pluginDescriptor)
        {
            //var pluginDirectory = _fileProvider.GetDirectoryName(pluginDescriptor.OriginalAssemblyFile);
            //if (string.IsNullOrEmpty(pluginDirectory))
            //    return null;

            ////check for supported extensions
            //var logoExtension = DevPluginDefaults.SupportedLogoImageExtensions
            //    .FirstOrDefault(ext => _fileProvider.FileExists(_fileProvider.Combine(pluginDirectory, $"{DevPluginDefaults.LogoFileName}.{ext}")));
            //if (string.IsNullOrWhiteSpace(logoExtension))
            //    return null;

            //var storeLocation = _webHelper.GetStoreLocation();
            //var logoUrl = $"{storeLocation}{DevPluginDefaults.PathName}/" +
            //    $"{_fileProvider.GetDirectoryNameOnly(pluginDirectory)}/{DevPluginDefaults.LogoFileName}.{logoExtension}";

            //return logoUrl;
            return string.Empty;
        }

        /// <summary>
        /// Prepare plugin to the installation
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="customer">Customer</param>
        /// <param name="checkDependencies">Specifies whether to check plugin dependencies</param>
        public virtual void PreparePluginToInstall(string systemName, bool checkDependencies = true)
        {
            //add plugin name to the appropriate list (if not yet contained) and save changes
            if (_pluginsInfo.PluginNamesToInstall.Any(item => item.SystemName == systemName))
                return;

            var pluginsAfterRestart = _pluginsInfo.InstalledPlugins.Select(pd => pd.SystemName).Where(installedSystemName => !_pluginsInfo.PluginNamesToUninstall.Contains(installedSystemName)).ToList();
            pluginsAfterRestart.AddRange(_pluginsInfo.PluginNamesToInstall.Select(item => item.SystemName));

            if (checkDependencies)
            {
                var descriptor = GetPluginDescriptorBySystemName<IPlugin>(systemName, LoadPluginsMode.NotInstalledOnly);

                if (descriptor.DependsOn?.Any() ?? false)
                {
                    var dependsOn = descriptor.DependsOn
                        .Where(dependsOnSystemName => !pluginsAfterRestart.Contains(dependsOnSystemName)).ToList();

                    if (dependsOn.Any())
                    {
                        var errorMessage = "Admin.Plugins.Errors.InstallDependsOn";

                        throw new DevException(errorMessage);
                    }
                }
            }

            _pluginsInfo.PluginNamesToInstall.Add((systemName, Guid.NewGuid()));
            _pluginsInfo.Save();
        }

        /// <summary>
        /// Prepare plugin to the uninstallation
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public virtual void PreparePluginToUninstall(string systemName)
        {
            //add plugin name to the appropriate list (if not yet contained) and save changes
            if (_pluginsInfo.PluginNamesToUninstall.Contains(systemName))
                return;

            var dependentPlugins = GetPluginDescriptors<IPlugin>(dependsOnSystemName: systemName).ToList();
            var descriptor = GetPluginDescriptorBySystemName<IPlugin>(systemName);

            if (dependentPlugins.Any())
            {
                var dependsOn = new List<string>();

                foreach (var dependentPlugin in dependentPlugins)
                {
                    if (!_pluginsInfo.InstalledPlugins.Select(pd => pd.SystemName).Contains(dependentPlugin.SystemName))
                        continue;
                    if (_pluginsInfo.PluginNamesToUninstall.Contains(dependentPlugin.SystemName))
                        continue;

                    dependsOn.Add(string.IsNullOrEmpty(dependentPlugin.FriendlyName)
                        ? dependentPlugin.SystemName
                        : dependentPlugin.FriendlyName);
                }

                if (dependsOn.Any())
                {
                    var dependsOnSystemNames = dependsOn.Aggregate((all, current) => $"{all}, {current}");
                    throw new DevException("Admin.Plugins.Errors.UninstallDependsOn");
                }
            }

            var plugin = descriptor?.Instance<IPlugin>();
            plugin?.PreparePluginToUninstall();

            _pluginsInfo.PluginNamesToUninstall.Add(systemName);
            _pluginsInfo.Save();
        }

        /// <summary>
        /// Prepare plugin to the removing
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public virtual void PreparePluginToDelete(string systemName)
        {
            //add plugin name to the appropriate list (if not yet contained) and save changes
            if (_pluginsInfo.PluginNamesToDelete.Contains(systemName))
                return;

            _pluginsInfo.PluginNamesToDelete.Add(systemName);
            _pluginsInfo.Save();
        }

        /// <summary>
        /// Reset changes
        /// </summary>
        public virtual void ResetChanges()
        {
            //clear lists and save changes
            _pluginsInfo.PluginNamesToDelete.Clear();
            _pluginsInfo.PluginNamesToInstall.Clear();
            _pluginsInfo.PluginNamesToUninstall.Clear();
            _pluginsInfo.Save();

            //display all plugins on the plugin list page
            _pluginsInfo.PluginDescriptors.ToList().ForEach(pluginDescriptor => pluginDescriptor.ShowInPluginsList = true);
        }

        /// <summary>
        /// Clear installed plugins list
        /// </summary>
        public virtual void ClearInstalledPluginsList()
        {
            _pluginsInfo.InstalledPlugins.Clear();
        }

        /// <summary>
        /// Install plugins
        /// </summary>
        public virtual void InstallPlugins()
        {
            //get all uninstalled plugins
            var pluginDescriptors = _pluginsInfo.PluginDescriptors.Where(descriptor => !descriptor.Installed).ToList();

            //filter plugins need to install
            pluginDescriptors = pluginDescriptors.Where(descriptor => _pluginsInfo.PluginNamesToInstall
                .Any(item => item.SystemName.Equals(descriptor.SystemName))).ToList();
            if (!pluginDescriptors.Any())
                return;

            //install plugins
            foreach (var descriptor in pluginDescriptors.OrderBy(pluginDescriptor => pluginDescriptor.DisplayOrder))
            {
                //try to install an instance
                descriptor.Instance<IPlugin>().Install();

                //remove and add plugin system name to appropriate lists
                var pluginToInstall = _pluginsInfo.PluginNamesToInstall
                    .FirstOrDefault(plugin => plugin.SystemName.Equals(descriptor.SystemName));
                _pluginsInfo.InstalledPlugins.Add(descriptor.GetBaseInfoCopy);
                _pluginsInfo.PluginNamesToInstall.Remove(pluginToInstall);

                //mark the plugin as installed
                descriptor.Installed = true;
                descriptor.ShowInPluginsList = true;
            }

            //save changes
            _pluginsInfo.Save();
        }

        /// <summary>
        /// Uninstall plugins
        /// </summary>
        public virtual void UninstallPlugins()
        {
            //get all installed plugins
            var pluginDescriptors = _pluginsInfo.PluginDescriptors.Where(descriptor => descriptor.Installed).ToList();

            //filter plugins need to uninstall
            pluginDescriptors = pluginDescriptors
                .Where(descriptor => _pluginsInfo.PluginNamesToUninstall.Contains(descriptor.SystemName)).ToList();
            if (!pluginDescriptors.Any())
                return;

            //uninstall plugins
            foreach (var descriptor in pluginDescriptors.OrderByDescending(pluginDescriptor => pluginDescriptor.DisplayOrder))
            {
                //try to uninstall an instance
                var plugin = descriptor.Instance<IPlugin>();
                //try to uninstall an instance
                plugin.Uninstall();

                //clear plugin data on the database
                //DeletePluginData(descriptor.PluginType);

                //remove plugin system name from appropriate lists
                _pluginsInfo.InstalledPlugins.Remove(descriptor);
                _pluginsInfo.PluginNamesToUninstall.Remove(descriptor.SystemName);

                //mark the plugin as uninstalled
                descriptor.Installed = false;
                descriptor.ShowInPluginsList = true;
            }

            //save changes
            _pluginsInfo.Save();
        }

        /// <summary>
        /// Delete plugins
        /// </summary>
        public virtual void DeletePlugins()
        {
            //get all uninstalled plugins (delete plugin only previously uninstalled)
            var pluginDescriptors = _pluginsInfo.PluginDescriptors.Where(descriptor => !descriptor.Installed).ToList();

            //filter plugins need to delete
            pluginDescriptors = pluginDescriptors
                .Where(descriptor => _pluginsInfo.PluginNamesToDelete.Contains(descriptor.SystemName)).ToList();
            if (!pluginDescriptors.Any())
                return;

            //delete plugins
            foreach (var descriptor in pluginDescriptors)
            {
                //try to delete a plugin directory from disk storage
                var pluginDirectory = _fileProvider.GetDirectoryName(descriptor.OriginalAssemblyFile);
                if (_fileProvider.DirectoryExists(pluginDirectory))
                    _fileProvider.DeleteDirectory(pluginDirectory);

                //remove plugin system name from the appropriate list
                _pluginsInfo.PluginNamesToDelete.Remove(descriptor.SystemName);
            }

            //save changes
            _pluginsInfo.Save();
        }

        /// <summary>
        /// Check whether application restart is required to apply changes to plugins
        /// </summary>
        /// <returns>Result of check</returns>
        public virtual bool IsRestartRequired()
        {
            //return true if any of lists contains items
            return _pluginsInfo.PluginNamesToInstall.Any()
                || _pluginsInfo.PluginNamesToUninstall.Any()
                || _pluginsInfo.PluginNamesToDelete.Any();
        }

        /// <summary>
        /// Get names of incompatible plugins
        /// </summary>
        /// <returns>List of plugin names</returns>
        public virtual IList<string> GetIncompatiblePlugins()
        {
            return _pluginsInfo.IncompatiblePlugins;
        }

        /// <summary>
        /// Get all assembly loaded collisions
        /// </summary>
        /// <returns>List of plugin loaded assembly info</returns>
        public virtual IList<PluginLoadedAssemblyInfo> GetAssemblyCollisions()
        {
            return _pluginsInfo.AssemblyLoadedCollision;
        }

        #endregion Methods
    }
}