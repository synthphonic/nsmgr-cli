namespace Nautilus.SolutionExplorer.Core;

public class AppFactory
{
    private static Dictionary<string, Type> _reports;

    public static void Register<TReport>(string key)
    {
        if (_reports == null)
        {
            _reports = new Dictionary<string, Type>();
        }

        _reports[key] = typeof(TReport);
    }

    public static TType GetRequestor<TType>(string key) where TType : class
    {
        if (_reports.ContainsKey(key))
        {
            var type = _reports[key];
            var instance = Activator.CreateInstance(type);
            return (TType)instance;
        }

        return default;
    }
}
