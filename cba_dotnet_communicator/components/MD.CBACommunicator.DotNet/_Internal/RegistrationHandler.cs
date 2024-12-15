using System.Net.Sockets;
using System.Reflection;
using MD.CBACommunicator.DotNet._Internal.Connectivity;
using MD.CBACommunicator.DotNet.Model;

namespace MD.CBACommunicator.DotNet._Internal;

/// <summary>
/// Handles the logic of registering the component to the server.
/// </summary>
internal class RegistrationHandler
{
    private readonly Connector _connector;
    private readonly Socket _socket;
    
    public RegistrationHandler(Connector connector, Socket socket)
    {
        _connector = connector;
        _socket = socket;
    }
    
    public void Register(string registrationEmbeddedResourcePath, Assembly registrationAssembly)
    {
        Message message = Utilities.GetMessageFromSharedResource(registrationEmbeddedResourcePath, registrationAssembly);
        _connector.SendMessage(_socket, message);
        _connector.StartKeepAliveMessageLoop(_socket);
    }

    public void Unregister()
    {
        MessagePayload payload = PayloadFactory.BuildPayload(
            action: "unregister",
            args: []);
        
        Message message = new Message(payload);
        
        _connector.SendMessage(_socket, message);
    }
}