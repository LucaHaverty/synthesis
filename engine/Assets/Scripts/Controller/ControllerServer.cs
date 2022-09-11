using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#nullable enable

public class ControllerServer : IDisposable {

    private class Inner {

        public int Port;
        public Socket Listener;
        private Thread _listenerThread;
        public Dictionary<string, ControllerClient> _clients;

        public Inner(int port) {
            Port = port;
            _clients = new Dictionary<string, ControllerClient>();

            Listener = new Socket(SocketType.Stream, ProtocolType.Udp);
            _listenerThread = new Thread(() => {
                Listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port));
                Listener.Listen(3);
                var socketTask = Listener.AcceptAsync();
                while (true) {
                    if (socketTask.Wait(1000)) {
                        var socket = socketTask.Result;
                        var client = new ControllerClient(Guid.NewGuid().ToString(), socket);
                        _clients.Add(client.Guid, client);
                    }
                }
            });
        }

        ~Inner() {
            _listenerThread.Interrupt();
            _clients.ForEach(x => {
                x.Value.Dispose();
            });
            _clients.Clear();

            Listener.Close();

            _clients = null!;
            Listener = null!;
            _listenerThread = null!;
        }
    }

    private Inner? _inner = null;

    public ControllerServer(int port) {
        _inner = new Inner(port);
    }

    public void Update() {

    }

    public void Dispose() {
        _inner = null;
        GC.Collect();
    }
}

public class ControllerClient : IDisposable {

    private class Inner {

        public string ClientGuid;
        public Socket ClientSocket;
        private Thread _clientThread;

        public Inner(string clientGuid, Socket clientSocket) {
            ClientGuid = clientGuid;
            ClientSocket = clientSocket;

            _clientThread = new Thread(() => {
                ArraySegment<byte>? buff = null;
                Task<int>? receiveTask = null;
                while (true) {

                    if (receiveTask == null) {
                        buff = new ArraySegment<byte>(new byte[1024]);
                        receiveTask = ClientSocket.ReceiveAsync(buff.Value, SocketFlags.None);
                    }

                    if (receiveTask.Wait(1000)) {
                        var bytesRead = receiveTask.Result;

                        // Parse and Handle Message

                        receiveTask = null;
                    }
                }
            });
        }

        ~Inner() {
            _clientThread.Interrupt();
            ClientSocket.Close();

            _clientThread = null!;
            ClientSocket = null!;
        }
    }

    public string Guid => _inner?.ClientGuid ?? string.Empty;

    private Inner? _inner;

    public ControllerClient(string guid, Socket socket) {
        _inner = new Inner(guid, socket);
    }

    public void Dispose() {
        _inner = null;
        GC.Collect();
    }

}
