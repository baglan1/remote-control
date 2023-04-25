using System.Net;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class ClientBehavior : MonoBehaviour
{
    [SerializeField] string ServerAddres;
    [SerializeField] BuildLogger logger;

    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public bool Done;

    void Start()
    {
        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);

        IPAddress serverAddress = IPAddress.Parse(ServerAddres);
        NativeArray<byte> nativeArrayAddress;
        // Convert that into a NativeArray of byte data
        nativeArrayAddress = new NativeArray<byte>(serverAddress.GetAddressBytes().Length, Allocator.Temp);
        nativeArrayAddress.CopyFrom(serverAddress.GetAddressBytes());
        // Set to AnyIpv4
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.SetRawAddressBytes(nativeArrayAddress);       
        endpoint.Port = 8000;
        m_Connection = m_Driver.Connect(endpoint);
    }

    public void OnDestroy() 
    { 
        m_Driver.Dispose();
    }

    void Update() 
    { 
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            if (!Done){
                Debug.Log("Something went wrong during connect");
                logger.AddText("Something went wrong during connect");
            }
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");
                logger.AddText("We are now connected to the server");

                uint value = 1;
                m_Driver.BeginSend(m_Connection, out var writer);
                writer.WriteUInt(value);
                m_Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                uint value = stream.ReadUInt();
                Debug.Log("Got the value = " + value + " back from the server");
                logger.AddText("Got the value = " + value + " back from the server");
                Done = true;
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                logger.AddText("Client got disconnected from server");
                m_Connection = default(NetworkConnection);
            }
        }
    }
}
