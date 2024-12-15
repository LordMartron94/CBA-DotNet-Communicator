using System.Net.Sockets;
using MD.CBACommunicator.DotNet._Internal;
using MD.CBACommunicator.DotNet._Internal.Connectivity;
using MD.CBACommunicator.DotNet._Internal.Processing;
using MD.CBACommunicator.DotNet.Model;

namespace MD.CBACommunicator.DotNet;

/// <summary>
/// Communicates with the CBA server.
/// </summary>
public class Communicator
{
    private readonly Connector _connector;
    private readonly Socket? _socket;

    private readonly RegistrationHandler _registrationHandler;
    private SettingsContext? _context;
    
    private IMessageHandler _messageHandler;
    
    /// <remarks>
    /// Don't forget to call <see cref="Initialize"/> after constructing this instance.
    /// </remarks>
    public Communicator(int componentPort = 50002)
    {
        MessageProcessorSelector messageProcessor = new MessageProcessorSelector();
        _connector = new Connector(messageProcessor);
        _socket = _connector.ConnectToRemote(Constants.ServerHost, Constants.ServerPort, componentPort);

        if (_socket == null)
            throw new Exception("Failed to connect to server");

        IMessageHandler messageHandler = new MessageHandler(_connector, _socket);
        messageProcessor.Initialize(messageHandler);
        _messageHandler = messageHandler;
            
        _registrationHandler = new RegistrationHandler(_connector, _socket);
    }

    /// <summary>
    /// Initializes the communicator with the provided settings.
    /// </summary>
    /// <param name="context"></param>
    public void Initialize(SettingsContext context)
    {
        _context = context;
        _registrationHandler.Register(_context.Value.RegistrationEmbeddedResourcePath);
    }
    
    /// <summary>
    /// Shuts down the communicator.
    /// </summary>
    /// <exception cref="InvalidOperationException">Throws an InvalidOperationException when the socket is not initialized.</exception>
    public void Shutdown()
    {
        EnsureInitialized();
        
        if (!_context!.Value.KeepServerAlive)
            SendShutdownMessage();
        
        _registrationHandler?.Unregister();
        
        _connector.Shutdown();
        _socket?.Close();
    }

    /// <summary>
    /// Sends a request to the server and returns the UUID.
    /// </summary>
    /// <param name="action">The requested action.</param>
    /// <param name="args">The arguments for the payload.</param>
    /// <param name="onResponseReceived">A callback for when a response is received for the request.</param>
    /// <returns>The UUID of the sent message.</returns>
    /// <remarks>
    /// The response received callback is invoked when either a client response or a server error response is received.
    /// </remarks>
    public string SendMessage(string action, IEnumerable<(string, string)> args, Action<Message>? onResponseReceived = null)
    {
        EnsureInitialized();
        return _messageHandler.SendRequest(action, args, onResponseReceived);
    }

    /// <summary>
    /// Sends a response to the server and returns the UUID.
    /// </summary>
    /// <returns>The UUID of the sent message.</returns>
    public string SendResponse(int responseCode, string responseMessage, string targetID)
    {
        EnsureInitialized();
        return _messageHandler.SendResponse(responseCode, responseMessage, targetID);
    }
    
    private void SendShutdownMessage()
    {
        EnsureInitialized();
        
        MessagePayload payload = PayloadFactory.BuildPayload(
            action: "shutdown",
            args: []);
        
        Message message = new Message(payload);
            
        _connector.SendMessage(_socket!, message);
    }

    private void EnsureInitialized()
    {
        if (_socket == null)
            throw new InvalidOperationException("Socket is not initialized");
    }
}