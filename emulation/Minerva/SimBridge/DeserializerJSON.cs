﻿using System;
using Newtonsoft.Json;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace SimBridge
{
    class DeserializerJSON
    {
        public void deserializeJSON()
        {
            //This constructor arbitrarily assigns the local port number.
            UdpClient udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 11000));
            try
            {
                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                string strJSON = String.Empty;
                do
                {
                    //Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    strJSON = Encoding.ASCII.GetString(receiveBytes);
                    var jItems = JsonConvert.DeserializeObject<JSONdata>(strJSON);
                }
                while (strJSON[strJSON.Length] != '\0');
                udpClient.Close();
            }

            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
        }
    }
}

    //var json = JsonConvert.DeserializeObject<dynamic>(strJSON);
    /*
    using (StreamReader r = new StreamReader("file.json"))
    {
        string strJSON = r.ReadToEnd();
        var jItems = JsonConvert.DeserializeObject<JSONdata>(strJSON);
    }
    */