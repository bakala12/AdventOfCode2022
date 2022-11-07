using AdventOfCode2022;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) => {
                services.AddAoc();
            })
            .ConfigureLogging((_, l) => {
                l.SetMinimumLevel(LogLevel.None);
            })
            .Build();
host.Start();

try
{
    if (args.Length == 0)
    {
        Console.WriteLine("No parameters provided. Provide day name");
        return;
    }
    var day = args[0];
    var dayType = Type.GetType($"AdventOfCode2022.Days.{day}", false, true);
    if (dayType == null)
    {
        Console.WriteLine("Invalid day name argument.");
        return;
    }
    var dayInstance = host.Services.GetService(dayType);
    if (dayInstance == null)
        Console.WriteLine("Requested day was not found...");
    else if (dayInstance is IDay d)
        d.Solve(args[new Range(1, args.Length)]);
    else
        Console.WriteLine("Incorrectly got day instance...");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception is thrown: {ex}");
}