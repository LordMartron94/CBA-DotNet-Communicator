using System.Diagnostics.CodeAnalysis;

namespace MD.CBACommunicator.DotNet.Model;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public struct Argument
{
    public string type { get; set; }
    public string? value { get; set; }

    public override string ToString()
    {
        return $"type: {type}, value: {value}";
    }
}