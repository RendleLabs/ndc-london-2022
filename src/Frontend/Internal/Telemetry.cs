using System.Diagnostics;

namespace Frontend;

internal static class Telemetry
{
    private static readonly ActivitySource Activities = new(typeof(Telemetry).Namespace);

    public static Activity? StartActivity(string name, ActivityKind kind) =>
        Activities.StartActivity(name, kind);
}