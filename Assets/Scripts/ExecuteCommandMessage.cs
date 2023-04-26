using Newtonsoft.Json;

public class ExecuteCommandMessage : NetworkMessage
{
    [JsonProperty]
	string name;

    [JsonIgnore]
    public string Name {
        get {
            return name;
        }
    }

    [JsonConstructor]
    public ExecuteCommandMessage(string name) {
        this.name = name;
    }
}
