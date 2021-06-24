namespace Dev.Core.Configuration
{
    public class PluginConfig
    {
        public bool ClearPluginShadowDirectoryOnStartup { get; set; } = true;
        public bool CopyLockedPluginAssembilesToSubdirectoriesOnStartup { get; set; } = false;
        public bool UsePluginsShadowCopy { get; set; } = true;
        public bool UseUnsafeLoadAssembly { get; set; } = true;
    }
}