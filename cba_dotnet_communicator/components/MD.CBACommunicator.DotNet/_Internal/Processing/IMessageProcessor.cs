using MD.CBACommunicator.DotNet.Model;

namespace MD.CBACommunicator.DotNet._Internal.Processing;

/// <summary>
/// Interface for processing incoming messages.
/// </summary>
public interface IMessageProcessor
{
    /// <summary>
    /// Processes the given message.
    /// </summary>
    /// <param name="incomingMessage">The message to process.</param>
    void ProcessIncomingMessage(Message incomingMessage);
}