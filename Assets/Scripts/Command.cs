using System;
using Newtonsoft.Json;

public class Command
{
    [JsonProperty]
	string name;

    [JsonIgnore]
    public string Name {
        get {
            return name;
        }
    }

    [JsonProperty]
    string description;

    [JsonIgnore]
    public string Description {
        get {
            return description;
        }
    }

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
