using System.Net;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Events;

public class ClientBehavior : MonoBehaviour
{
    public UnityEvent OnConnectionEvent = new UnityEvent();

    NetworkDriver m_Driver;
    NetworkConnection m_Connection;
    bool isConnecting;

    string IpAddress;
    bool isIpSet;

    void Start()
    {
        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);
    }

    public void CreateConnection(string ipAddress) {
        if (m_Connection != default(NetworkConnection)) return;
        // if (isConnecting) return;

        isConnecting = true;
        
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
        Debug.Log("Connected");
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

        if (!m_Connection.IsCreated)
        {
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                OnConnectionEvent.Invoke();
                isConnecting = false;
                // uint value = 1;
                // m_Driver.BeginSend(m_Connection, out var writer);
                // writer.WriteUInt(value);
                // m_Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                // uint value = stream.ReadUInt();

            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                m_Connection = default(NetworkConnection);
            }
        }
    }
}
