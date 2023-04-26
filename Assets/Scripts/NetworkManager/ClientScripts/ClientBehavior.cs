using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Events;

public class ClientBehavior : MonoBehaviour
{
    public UnityEvent OnConnectionEvent = new UnityEvent();
    public UnityEvent<NetworkMessage> OnMessageReceiveEvent = new UnityEvent<NetworkMessage>();

    NetworkDriver m_Driver;
    NetworkConnection m_Connection;

    string IpAddress;
    bool isIpSet;

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
        m_Connection = default(NetworkConnection);
    }

    public void CreateConnection(string ipAddress) {
        if (m_Connection != default(NetworkConnection)) return;
        
        IpAddress = ipAddress;
        isIpSet = true;
    }

    void CheckForCreateConnection() {
        if (!isIpSet) return;

        Debug.Log("Connect called");

        IPAddress serverAddress = IPAddress.Parse(IpAddress);
        NativeArray<byte> nativeArrayAddress;
        // Convert that into a NativeArray of byte data
        nativeArrayAddress = new NativeArray<byte>(serverAddress.GetAddressBytes().Length, Allocator.Temp);
        nativeArrayAddress.CopyFrom(serverAddress.GetAddressBytes());
        // Set to AnyIpv4
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.SetRawAddressBytes(nativeArrayAddress);       
        endpoint.Port = Constants.MESSAGE_PORT;
        m_Connection = m_Driver.Connect(endpoint);
    }

    public void OnDestroy() 
    { 
        m_Driver.Dispose();
    }

    void Update() 
    { 
        CheckForCreateConnection();
        isIpSet = false;

        m_Driver.ScheduleUpdate().Complete();

        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            OnConnectionEvent.Invoke();
            m_Connection = c;
            Debug.Log("Connection is set");
        }

        if (!m_Connection.IsCreated)
        {
            return;
        }

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

            m_Driver.BeginSend(NetworkPipeline.Null, m_Connection, out var writer);
            writer.WriteBytes(nativeArrayBytes);
            m_Driver.EndSend(writer);
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                OnConnectionEvent.Invoke();

                
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                byte[] recBuffer = new byte[stream.Length];
                var array = new NativeArray<byte>(recBuffer, Allocator.Temp);
                stream.ReadBytes(array);

                recBuffer = array.ToArray();

                var jsonStr = Encoding.UTF8.GetString(recBuffer);

                var jsonSerializerSettings = new JsonSerializerSettings() { 
                    TypeNameHandling = TypeNameHandling.All
                };
                var msg = JsonConvert.DeserializeObject<NetworkMessage>(jsonStr, jsonSerializerSettings);
                OnMessageReceiveEvent.Invoke(msg);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                m_Connection = default(NetworkConnection);
            }
        }
    }

    public void Disconnect() {
        m_Driver.Disconnect(m_Connection);
        m_Connection = default(NetworkConnection);
    }

    public void SendMessage(NetworkMessage message) {
        sendMessageQueue.Enqueue(message);
    }
}
