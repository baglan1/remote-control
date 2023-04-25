using System.Collections.Generic;
using Newtonsoft.Json;

public class CommandsListMessage : NetworkMessage
{
	List<Command> commands;

    [JsonConstructor]
    public CommandsListMessage(List<Command> commands) {
        this.commands = commands;
    }
}
