namespace MD.CBACommunicator.DotNet.Model;

public struct SettingsContext(string rootModuleSeparator, bool keepServerAlive, string registrationEmbeddedResourcePath)
{
    public string RootModuleSeparator { get; } = rootModuleSeparator;
    public bool KeepServerAlive { get; } = keepServerAlive;
    public string RegistrationEmbeddedResourcePath { get; } = registrationEmbeddedResourcePath;
}