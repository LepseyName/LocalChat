using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Linq;
using System.Net.NetworkInformation;

namespace LocalChat
{
    static class Server
    {
        public static TcpListener Listener;
        public static List<Client> clients;
        public static bool isWork = false;
        public static User me;
        private const string IfHaveServer = "\tIt is Local Chat?\n";  //Сообщение опросник всех адресов локальной сети
        private static Chat form;
        private static List<Thread> threads;
        private static List<Thread> threadsClient;
        private static int Port;

        public static void Start(int port, Chat Form)
        {
            Server.form = Form;
            Server.Port = port;
            clients = new List<Client>();
            Listener = new TcpListener(System.Net.IPAddress.Any, port);
            Listener.Start();
            isWork = true;
            threadsClient = new List<Thread>();

            Server.form.SendToFormMessage("start server");
            while (isWork)
            {
                try
                {
                    Server.form.SendToFormMessage("sr");
                    TcpClient Client = Listener.AcceptTcpClient();
                    Server.form.SendToFormMessage("sr2");

                    Server.OnConnect(Client);
                }
                catch (Exception e)
                {
                    Server.OnError(e);
                    break;
                }
            }
            Server.form.SendToFormMessage("Server stopped");
            //Server.Stop();
        }

        public static void Stop()
        {
            Server.isWork = false;

            if (Listener != null)
            {
                Listener.Stop();
            }

            if (Server.clients != null)
            {
                foreach (var i in Server.clients)
                    i.Close();
                Server.clients = null;
            }
            
            if (threads != null)
                foreach (Thread i in threads)
                    if (i != null && i.IsAlive)
                        i.Abort();

            if (threadsClient != null)
                foreach (Thread i in threadsClient)
                    if (i != null && i.IsAlive)
                        i.Abort();
        }

        public static void Connect(TcpClient connect)
        {
            Server.OnConnect(connect);
        }

        static void OnError(Exception e)
        {
            Server.form.SendToFormMessage("server error!");
            Server.Stop();
        }

        static void OnConnect(object info){
            Server.form.SendToFormMessage("sr3");

            foreach (var i in Server.clients)
                if (i.tcpYou((TcpClient)info)) return;

            Client client = new Client( (TcpClient)info , Server.form);
            Server.clients.Add( client );
            Server.form.SendToFormMessage("sr4");

            Thread thread = new Thread(new ThreadStart(client.OnStart));
            thread.Start();
            threadsClient.Add(thread);

            Server.form.SendToFormMessage("sr5");
        } 
        
        public static void OnDisconnect(Client client){
            Server.form.SendToFormMessage("sr6");

            if (Server.clients == null) return;
            if (client != null)
            {
                Server.clients.Remove( client );
            }
            else
            {

                var newList = new List<Client>(Server.clients.Count);
                foreach (var i in Server.clients)
                {
                    try
                    {
                        if (i.isGood()) newList.Add(i);
                    }
                    catch (Exception)
                    {
                    }
                }
                Server.clients.Clear();
                Server.clients.AddRange(newList);
            }

        } 


        public static void Send(Client client, string Message){
            if ( clients.IndexOf(client) >= 0) client.Send( Message );
        }

        public static void Send(Client client, MESSAGE Message)
        {
            if (clients.IndexOf(client) >= 0) client.Send(Message);
        }

        public static void Send(Client client, AddFile Message)
        {
            if (clients.IndexOf(client) >= 0) client.Send(Message);
        }
 
        public static void SendToAll(string Message){
            foreach ( Client i in Server.clients)
                i.Send( Message );
        }

        public static void SendToAll(MESSAGE Message)
        {
            if (Server.clients == null) return;
            foreach (Client i in Server.clients)
                i.Send(Message);
        }

        public static void SendToAll(AddFile Message)
        {
            if (Server.clients == null) return;
            foreach (Client i in Server.clients)
                i.Send(Message);
        }

        public static void startDownload(Client client, AddFile file)
        {
            if (clients.IndexOf(client) >= 0) client.Download(file);
        }

        public static bool isDownloadable(Client client, AddFile file)
        {
            if (clients.IndexOf(client) >= 0) return client.isDownloadable(file);
            return false;
        }

        public static int getPort()
        {
            return Server.Port;
        }

    }
}
