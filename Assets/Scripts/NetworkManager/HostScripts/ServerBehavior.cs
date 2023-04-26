using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Events;

public class ServerBehavior : MonoBehaviour
{
    public UnityEvent OnConnectionEvent = new UnityEvent();
    public UnityEvent<NetworkMessage> OnMessageReceiveEvent = new UnityEvent<NetworkMessage>();

    NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;

    Queue<NetworkMessage> sendMessageQueue = new Queue<NetworkMessage>();

    void Start()
    {
        m_Driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = Constants.MESSAGE_PORT;
        if (m_Driver.Bind(endpoint) != 0) {
            Debug.Log($"Failed to bind to port {Constants.MESSAGE_PORT}");
        }
        else
            m_Driver.Listen();

        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        // Clean up connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            OnConnectionEvent.Invoke();
            m_Connections.Add(c);
        }

        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
                continue;

            // Send messages
            if (sendMessageQueue.Count > 0) {
                var msg = sendMessageQueue.Dequeue();
                var jsonSerializerSettings = new JsonSerializerSettings() { 
                    TypeNameHandling = TypeNameHandling.All
                };
                var json = JsonConvert.SerializeObject(msg, jsonSerializerSettings);
                var bytes = Encoding.UTF8.GetBytes(json);

                NativeArray<byte> nativeArrayBytes;
                nativeArrayBytes = new NativeArray<byte>(bytes.Length, Allocator.Temp);
                nativeArrayBytes.CopyFrom(bytes);

                m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i], out var writer);
                writer.WriteBytes(nativeArrayBytes);
                m_Driver.EndSend(writer);
            }

            // receive messages
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    byte[] recBuffer = new byte[stream.Length];
                    var array = new NativeArray<byte>(recBuffer, Allocator.Temp);
                    stream.ReadBytes(array);

                    recBuffer = array.ToArray();

                    var jsonStr = Encoding.UTF8.GetString(recBuffer);
                    Debug.Log(jsonStr);

                    var jsonSerializerSettings = new JsonSerializerSettings() { 
                        TypeNameHandling = TypeNameHandling.All
                    };
                    var msg = JsonConvert.DeserializeObject<NetworkMessage>(jsonStr, jsonSerializerSettings);
                    OnMessageReceiveEvent.Invoke(msg);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }
            }
        }
    }

    public void SendMessage(NetworkMessage message) {
        sendMessageQueue.Enqueue(message);
    }

    void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }
}
