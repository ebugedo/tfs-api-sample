using Winton.Extensions.Configuration.Consul;

public static class ConsulExtensions
{
    public static void AddCustomConsul(this IConfigurationBuilder configBuilder, string environment)
    {
        configBuilder.AddConsul(
            $"tfsapi/{environment.ToLower()}/config",
            options => {
                options.ConsulConfigurationOptions = cko => {
                    cko.Address = new Uri("http://consul:8500");
                };
                options.Optional = false;
                options.ReloadOnChange = true;
                options.OnLoadException = context => context.Ignore = true;
                options.Parser = new Winton.Extensions.Configuration.Consul.Parsers.JsonConfigurationParser();
                options.OnLoadException = context => {
                    Console.WriteLine($"[CONSUL ERROR]: {context.Exception.Message}");
                    context.Ignore = false;
                };
            }
        );
    }
}