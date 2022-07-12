namespace Dev.Core.Configuration;

public class DatabaseConfig
{
    public string DataProvider { get; set; } = "mysql";

    public string DataConnectionString { get; set; } = "server=127.0.0.1;database=plugindb;allowuservariables=True;user id=root;password=2wsxXSW@";
}