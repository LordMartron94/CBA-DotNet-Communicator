namespace MD.CBACommunicator.DotNet.Model;

public struct SettingsContext
{
    public string RootModuleSeparator { get; }
    public bool KeepServerAlive { get; }
    public string RegistrationEmbeddedResourcePath { get; }
}