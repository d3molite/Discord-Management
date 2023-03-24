namespace BotModule.DI;

public interface IConfigProvider
{
    public bool TryGetConfig<T>(int id, out T config);

    public bool TryGetConfig<T>(string name, out T config);

    public bool TryGetConfig<T>(ulong snowflake, out T config);
}