using System.Collections.Generic;
using Newtonsoft.Json;

public class CommandsListMessage : NetworkMessage
{
    [JsonProperty]
	List<Command> commands;

    [JsonConstructor]
    public CommandsListMessage(List<Command> commands) {
        this.commands = commands;
    }
}
