using System;
using Newtonsoft.Json;

public class Command
{
    [JsonProperty]
	string name;
    [JsonProperty]
    string description;

    [JsonIgnore]
    Action action;

    [JsonConstructor]
    public Command(string name, string description) {
        this.name = name;
        this.description = description;
    }

    public void SetAction(Action action) {
        this.action = action;
    }
}
