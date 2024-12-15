using System.Reflection;
using MD.CBACommunicator.DotNet.Model;
using Newtonsoft.Json;

namespace MD.CBACommunicator.DotNet;

public static class Utilities
{
    public static string GetEmbeddedResourceString(string resourceName, Assembly associatedAssembly)
    {
        using Stream stream = associatedAssembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"{resourceName} not found in {associatedAssembly.FullName}");
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static Message GetMessageFromSharedResource(string resourceName, Assembly associatedAssembly, MessagePayload? messagePayload = null)
    {
        string json = GetEmbeddedResourceString(resourceName, associatedAssembly); 
        Message message = JsonConvert.DeserializeObject<Message>(json);
        message = new Message(message.payload);

        if (messagePayload != null)
            message.payload = messagePayload.Value;

        return message;
    }

    /// <summary>
    /// Builds a MessagePayload object with the given action.
    /// Mostly used for default special actions (shutdown, unregister, keep_alive).
    /// </summary>
    /// <param name="action">The associated parameterless action.</param>
    /// <param name="args">Optional argument specification.</param>
    /// <returns>MessagePayload object</returns>
    public static MessagePayload BuildPayload(string action, Argument[]? args = null)
    {
        Argument[] arguments = args ?? Array.Empty<Argument>();
        
        return new MessagePayload
        {
            action = action,
            args = arguments
        };
    }
}