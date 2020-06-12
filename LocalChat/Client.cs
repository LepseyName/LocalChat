using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LocalChat
{
    public class Client
    {
        private TcpClient tcp;
        private System.Net.Sockets.NetworkStream stream;
        private User user;
        private Chat form;
        private CrypterAES perhaps;
        private CrypterRSA rsa;
        private List<AddFile> downloadFile;
        private volatile List<AddFile> startdownloadFile;
        private List<AddFile> uploadFile;
        private volatile List<byte[]> quenSend;
        private Thread downloadFileThread;
        private Thread uploadFileThread;
        private Thread sendThread;
        private volatile bool stopSendThread;
        private static int MaxLenQuen = 5;
        /*
        private const string EndMessages = "\r\n\r\n";                //конец потока инфы
        private const string Separator   = "\n/SEP/n";                //разделитель  инфы
        private const string ReplaceSeparator = "\n";                 //замена, если разделитель есть в тексте
        private const string HeadMessage = "MESS";                    //начало сообщения
        private const string HeadUser = "User";                       //начало данных пользователя
        private const string IfHaveServer = "\tIt is Local Chat?\n";  //Сообщение опросник всех адресов локальной сети
        private const string YesItIsServer= "\tSTO XYEB\n";           //Сообщение ответ на вопрос о наличие сервера
        private const string GetUserPliace = "\tGet Me Faken User\n"; //Сообщение запрос на предоставления данных
        */

        private const string EndSeans = "s_ebal";                //конец потока 
        private const string Good = "saebok";                //конец потока 
        private const string EndMessages = "ENDend";                //конец потока инфы
        private const string Separator = "|sepSEP|";                //разделитель  инфы
        private const string ReplaceSeparator = "?";                 //замена, если разделитель есть в тексте
        private const string HeadMessage = "MESS";                    //начало сообщения
        private const string HeadAddFile = "FILE";                    //начало прикрепленного файла
        private const string HeadFile = "FILE_DOWNLOAD";              //начало файла
        private const string HeadFileDop = "FILE_DOWNLOAD_DOP";       //начало дополнений файла 
        private const string HeadUser = "User";                       //начало данных пользователя
        private const string HeadSecret = "SECRET";                   //начало секретного ключа пользователя
        private const string IfHaveServer = "It is Local Chat?";  //Сообщение опросник всех адресов локальной сети
        private const string YesItIsServer = "STO_XYEB";           //Сообщение ответ на вопрос о наличие сервера
        private const string GetUserPliace = "Get_Me_Faken_User"; //Сообщение запрос на предоставления данных
        private const string GetSecretKey = "Get_Me_Faken_Key"; //Сообщение запрос на предоставления ключа шифрования

        private byte[] EndMessageByte = Encoding.UTF8.GetBytes(EndMessages);

        public Client(TcpClient tcpClient, Chat Form = null) {
            this.tcp = tcpClient;
            this.stream = tcpClient.GetStream();
            this.form = Form;
            this.startdownloadFile = new List<AddFile>();
            this.downloadFile = new List<AddFile>();
            this.uploadFile = new List<AddFile>();
            this.quenSend = new List<byte[]>();
            this.downloadFileThread = null;
            this.uploadFileThread = null;
            this.sendThread = null;
            this.stopSendThread = false;
            this.perhaps = null;
            this.rsa = null;
        }

        ~Client()
        {
            tcp.Close();
        }

        public bool isGood()
        {
            return this.tcp.Connected;
        }

        public bool tcpYou(TcpClient a)
        {
            string meIP = ((IPEndPoint)this.tcp.Client.RemoteEndPoint).Address.ToString();
            string anyIP = ((IPEndPoint)a.Client.RemoteEndPoint).Address.ToString();
            int mePort = ((IPEndPoint)this.tcp.Client.RemoteEndPoint).Port;
            int anyPort = ((IPEndPoint)a.Client.RemoteEndPoint).Port;
            return meIP.Equals(anyIP) && mePort == anyPort && this.isGood() ;
        }

        public bool isCrypted()
        {
            return (this.user != null && (this.user.crypt != null || this.perhaps != null));
        }

        public bool isDownloadable(AddFile file)
        {
            if (this.downloadFile != null && this.downloadFile.IndexOf(file) >= 0)
                return true;
            return false;
        }

        public bool openSecretLine(){
            try
            {
                if (this.perhaps != null) return false;

                this.form.SendToFormMessage("SecretLine");
                this.rsa = new CrypterRSA();
                byte[] buf = this.rsa.getPublicKey();
                this.form.SendToFormMessage("SecretKey: " + Program.HechBytes(buf));

                byte[] mess = new byte[0] { };
                mess = Program.ConcatByte(mess, Encoding.UTF8.GetBytes(GetSecretKey + Separator));
                mess = Program.ConcatByte(mess, buf);

                this.Send(mess);

                this.form.SendToFormMessage("SecretLineMessage: " + buf.Length);
            }
            catch(Exception e)
            {
                this.OnError(e);
                return false;
            }
            return true;
        }

        public void OnStart()
        {
            byte[] buffer = new byte[1024];
            byte[] Data = new byte[0]{};
            int count;
            this.Send(IfHaveServer);
            this.form.SendToFormMessage("sr4.1");
            
            try 
            {
                while(true)
                {
                    count = this.stream.Read(buffer, 0, buffer.Length);
                    if (count > 0)
                    {
                        Data = Program.ConcatByte(Data, buffer, count);
                        //this.form.SendToFormMessage("Data: " + Encoding.UTF8.GetString(Data));
                        
                        while (Program.FirstIndexOf(Data, EndMessageByte) >= 0)
                        {
                            int index = Program.FirstIndexOf(Data, EndMessageByte);
                            //this.form.SendToFormMessage("index: " + index + ": " + EndMessageByte.Length);
                            
                            byte[] message = Program.SubBytes(Data, 0, index);

                            if (this.user != null && this.user.crypt != null)
                                try
                                {
                                    message = this.user.crypt.DecoderBytes(message);
                                }
                                catch(Exception e)
                                {
                                    this.form.SendToFormMessage("Eror Decode: " + e.Message);
                                }
                            
                            this.OnMessage(message);
                            Data = Program.SubBytes(Data, index + EndMessageByte.Length);
                        }
                    }
                    if (Data.Length > 4096)
                    {
                        Data = new byte[0] { };
                    } 
                    //Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                this.form.SendToFormMessage("sr4.E: " + e.StackTrace);
                this.OnError(e);
            }

        }

        public void Download(AddFile file)
        {
            bool isHave = false;
            if (!Storage.getIsSave())
            {
                Storage.setIsSave(true);
                file.isSave = true;
                Storage.saveStatistic(file);
            }
            foreach (var i in this.startdownloadFile)
                if (i.Equals(file)) isHave = true;

            if (!isHave)
                this.startdownloadFile.Add(file);

            if (this.downloadFileThread == null || !this.downloadFileThread.IsAlive)
            {
                this.downloadFileThread = new Thread(new ThreadStart(this.startDownload));
                this.downloadFileThread.Start();
            }
        }

        private void startDownload()
        {
            while (this.startdownloadFile.Count > 0)
            {
                var file = this.startdownloadFile[0];
                this.startdownloadFile.RemoveAt(0);
                try
                {
                    Storage.createFile(file);
                    this.downloadFile.Add(file);
                    this.Send(false, HeadFile, file.getName(), file.getNewName(), Convert.ToString(file.getBlockSize()), Convert.ToString(file.getCountBlock()));
                }
                catch (Exception e)
                {
                    file.setError(e.Message);
                    this.form.downloadError(file);
                }
            }
        }

        private void finishDownload(AddFile file)
        {
            bool isGoot = true;

            for(ulong i = 0; i<file.getCountBlock(); i++)
                if (file.flagBlock[i] == 0)
                {
                    isGoot = false;
                    this.Send(false, HeadFileDop, file.getName(), file.getNewName(), Convert.ToString(i), Convert.ToString(file.getBlockSize()));
                }
            if (isGoot)
            {
                if (file.isSave)
                    Storage.setIsSave(false);
                this.downloadFile.Remove(file);
                this.form.UpdateDownloadInfo(file);
            }
            else
                this.Send(false, HeadFileDop, file.getName(), file.getNewName(), Convert.ToString(file.getCountBlock() - 1), Convert.ToString(file.getBlockSize()));
            
        }

        private void startUpload(AddFile file)
        {
            this.form.SendToFormMessage("start upload: " + file.getName());

            bool isHave = false;
            foreach (var i in this.uploadFile)
                if (i.Equals(file)) isHave = true;

            if (!isHave)
                this.uploadFile.Add(file);

            if (this.uploadFileThread != null && this.uploadFileThread.IsAlive)
            {

            }
            else
            {
                this.uploadFileThread = new Thread(new ThreadStart(this.uploadFiles));
                this.uploadFileThread.Start();
            }
        }

        private void uploadFiles()
        {

            while (this.uploadFile.Count > 0)
            {
                var file = this.uploadFile[0];
                this.uploadFile.RemoveAt(0);

                string fileName = file.getName(), newName = file.getNewName(); 
                int blockSize = file.getBlockSize();
                ulong countBlock = file.getCountBlock(), block100 = countBlock/100;

                for (ulong i = 0; i < countBlock; i++)
                {
                    try
                    {
                        //while(this.stream.DataAvailable);
                        if (block100 != 0 && i%block100 == 0)
                            this.form.SendToFormMessage("upload: " + i / block100 + "%");

                        byte[] block = Storage.getBlockFile(fileName, blockSize, i);
                        int hash = Program.HechBytes(block);

                        string header = HeadFile + Separator + fileName + Separator + newName + Separator + Convert.ToString(i) + Separator + Convert.ToString(hash) + Separator;
                        byte[] buffer = Encoding.UTF8.GetBytes(header);

                        this.Send(Program.ConcatByte(buffer, block));
                        
                        //Thread.Sleep(5);
                        while (this.quenSend.Count > Client.MaxLenQuen) Thread.Sleep(1);
                    }
                    catch (Exception e)
                    {
                        this.form.SendToFormMessage("upload: " + e.StackTrace);
                        //this.form.ErrorUpload(e.Message);
                    }
                }
            }
            this.form.SendToFormMessage("upload has finished");
        }

        public void Close()
        {
            try{
                byte[] buffer = Encoding.UTF8.GetBytes(EndSeans);
                
                if (this.user.crypt != null)
                    buffer = this.user.crypt.CoderBytes(buffer);
                
                this.stream.Write( buffer, 0, buffer.Length );
                this.stream.Write(EndMessageByte, 0, EndMessageByte.Length);
            }
            catch{}

            if (this.downloadFileThread != null && this.downloadFileThread.IsAlive)
                this.downloadFileThread.Abort();

            if (this.uploadFileThread != null && this.uploadFileThread.IsAlive)
                this.uploadFileThread.Abort();

            if (this.sendThread != null && this.sendThread.IsAlive)
                this.sendThread.Abort();

            this.stream.Close();
            this.tcp.Close();
        }


        private void OnMessage(byte[] Message, bool isRsa = false)
        {
            byte[][] mess = Program.Split(Message, Encoding.UTF8.GetBytes(Separator), true);
            if (mess.Length == 0) return;

            //this.form.SendToFormMessage(((this.perhaps != null) ? "aes_per" : "null") + ((!isRsa) ? "aes_per" : "null"));
            //for(int i = 0; i<mess.Length; i++)
            //    this.form.SendToFormMessage("sr4.M: " + i + ": " + mess[i].Length + ": " + Encoding.UTF8.GetString(mess[i]));

            string head = Encoding.UTF8.GetString(mess[0]);
            
            
            switch (head)
            {
                case HeadFileDop :{
                    if ((mess.Length == 5) && (this.user != null))
                        {
                            string name = Encoding.UTF8.GetString(mess[1]), newName = Encoding.UTF8.GetString(mess[2]), blockNumber = Encoding.UTF8.GetString(mess[3]), blockSize = Encoding.UTF8.GetString(mess[4]);
                            ulong number = Convert.ToUInt64(blockNumber);
                            int size = Convert.ToInt32(blockSize);

                            try
                            {
                                byte[] block = Storage.getBlockFile(name, size, number);
                                int hash = Program.HechBytes(block);

                                string header = HeadFile + Separator + name + Separator + newName + Separator + Convert.ToString(number) + Separator + Convert.ToString(hash) + Separator;
                                byte[] buffer = Encoding.UTF8.GetBytes(header);

                                this.Send(Program.ConcatByte(buffer, block));
                                this.form.SendToFormMessage("Dop file block: " + number);
                            }
                            catch(Exception e)
                            {
                                this.form.SendToFormMessage("Dop file block eror: " + e.Message + "\n" + e.StackTrace);
                            }                                                   
                        }
                    break;
                }
                case HeadFile:
                    {
                        if ((mess.Length == 4) && (this.user != null))
                        {
                            string name = Encoding.UTF8.GetString(mess[1]), newName = Encoding.UTF8.GetString(mess[2]), Error = Encoding.UTF8.GetString(mess[3]);
                            var file = new AddFile(this.user, name, 0, 0, newName);
                            file.setError(Error);
                            this.form.downloadError(file);
                        }
                        if ((mess.Length == 5) && (this.user != null))
                        {
                            string name = Encoding.UTF8.GetString(mess[1]), newName = Encoding.UTF8.GetString(mess[2]), blockSize = Encoding.UTF8.GetString(mess[3]), countBlock = Encoding.UTF8.GetString(mess[4]);

                            var file = Storage.getAddFile(name, newName, Convert.ToInt32(blockSize), Convert.ToUInt64(countBlock));
                            if (file != null)
                            {
                                this.startUpload(file);
                            }
                            else
                            {
                                this.Send(false, HeadFile, name, newName, "file not found");
                            }
                        }
                        else if ((mess.Length == 6) && (this.user != null))
                        {
                            this.Send(Client.Good);
                            string name = Encoding.UTF8.GetString(mess[1]), newName = Encoding.UTF8.GetString(mess[2]);
                            ulong blockNumber = Convert.ToUInt64(Encoding.UTF8.GetString(mess[3]));

                            AddFile file = null;
                            foreach (var i in this.downloadFile)
                                if (i.getNewName() == newName)
                                {
                                    file = i;
                                    break;
                                }

                            if (file != null){
                                if (mess[5].Length > file.getBlockSize())
                                    System.Array.Resize(ref mess[5], file.getBlockSize());

                                if (blockNumber == file.getCountBlock() - 1 && file.getSize()%(ulong)file.getBlockSize() != 0)
                                    System.Array.Resize(ref mess[5], (int)( file.getSize() % (ulong)file.getBlockSize()) );

                                int hash2 = Program.HechBytes(mess[5]);
                                if (Convert.ToInt32(Encoding.UTF8.GetString(mess[4])) == hash2)
                                {
                                    try
                                    {
                                        Storage.setBlockFile(file.getNewName(), file.getBlockSize(), blockNumber, mess[5]);
                                        file.flagBlock[blockNumber] = 1;
                                        if (blockNumber > 100 && blockNumber % (file.getCountBlock() / 100) == 0)
                                        {
                                            if (file.isSave)
                                                Storage.saveStatistic(blockNumber / (file.getCountBlock() / 100));
                                            this.form.UpdateDownloadInfo(file);
                                            this.form.SendToFormMessage("download: " + blockNumber / (file.getCountBlock() / 100) + "%");
                                        }     
                                    }
                                    catch (Exception e)
                                    {
                                        this.form.SendToFormMessage("download error: " + e.StackTrace);
                                    }       
                                }
                                if( blockNumber == file.getCountBlock() - 1)
                                    this.finishDownload(file);
                            }
                        }
                        break;
                    }
                
                case HeadAddFile:
                    {
                        if ((mess.Length == 5) && (this.user != null))
                        {
                            string name = Encoding.UTF8.GetString(mess[1]), size = Encoding.UTF8.GetString(mess[2]), time = Encoding.UTF8.GetString(mess[4]);
                            string newName = Storage.getNewName(name);
                            bool isPrivate = Convert.ToBoolean(Encoding.UTF8.GetString(mess[3]));
                            
                            var file = new AddFile(this.user, name, Convert.ToUInt64(size), Storage.getBlockSize(), Convert.ToDateTime(time), newName);
                            file.isPrivate = isPrivate;
                            this.form.SendToFormMessage(file);
                        }
                        break;
                    }
                case HeadMessage:
                    {
                        if ( (mess.Length != 4) && (this.user.client != null) ) break;
                        
                        string text = Encoding.UTF8.GetString(mess[2]), time = Encoding.UTF8.GetString(mess[3]);
                        bool isPrivate = Convert.ToBoolean(Encoding.UTF8.GetString(mess[1]));
                        MESSAGE newMessage = new MESSAGE(this.user, Server.me, text, Convert.ToDateTime( time ), isPrivate);
                        
                        this.form.SendToFormMessage(newMessage);
                        break;
                    }
                case HeadUser:
                    {
                        if (mess.Length == 3)
                        {
                            string HostName = Dns.GetHostEntry( ((IPEndPoint)this.tcp.Client.RemoteEndPoint).Address ).HostName;
                            string name = Encoding.UTF8.GetString(mess[1]);
                            string avatar = Encoding.UTF8.GetString(mess[2]);
                            if (this.user ==null) 
                                this.user = new User(name, HostName, System.Convert.ToInt32(avatar), this);
                            else
                                this.user.update(System.Convert.ToInt32(avatar));
                            this.form.SendToFormMessage("sr4.User");
                            Storage.SaveContact(this.user);
                            this.form.OnConnect(this.user);
                        }
                        else this.form.SendToFormMessage("sr4.NOT_USER");
                        
                        break;
                    }
                case IfHaveServer:
                    {
                        this.Send(YesItIsServer);
                        if (this.user == null) this.Send(GetUserPliace);
                        break;
                    }
                case YesItIsServer:
                    {
                        if (this.user == null) this.Send(GetUserPliace);
                        break;
                    }

                case GetUserPliace:
                    {
                        while (true)
                        {
                            if (Server.me != null)
                            {
                                this.Send(Server.me);
                                break;
                            }
                            else
                                Thread.Sleep(200);   
                        }
                        break;
                    }
                case EndSeans:
                    {
                        this.OnError(new Exception("Cancel"));
                        break;
                    }
                case GetSecretKey:
                    {
                        if (mess.Length == 2)
                        {
                            mess[1] = Program.SubBytes(mess[1], 0, 148);
                            
                            this.form.SendToFormMessage("Create Secret Data:" + mess[1].Length);
                            this.form.SendToFormMessage("SecretKey: " + Program.HechBytes(mess[1]));
                            
                            rsa = new CrypterRSA( mess[1] );
                            var aes = new CrypterAES();
                            byte[] message = new byte[0] { };
                            message = Program.ConcatByte(message, Encoding.UTF8.GetBytes(HeadSecret + Separator + CrypterAES.getMode() + Separator + CrypterAES.getPadding() + Separator));
                            message = Program.ConcatByte(message, aes.getKey());
                            message = Program.ConcatByte(message, Encoding.UTF8.GetBytes(Separator));
                            message = Program.ConcatByte(message, aes.getIV());
                            this.perhaps = aes;

                            this.Send(rsa.Coder(message));
                            this.form.SendToFormMessage("hech: " + aes.GetHashCode());
                        }
                        break;
                    }
                case HeadSecret:
                    {
                        
                        if (mess.Length == 5)
                        { 
                            this.form.SendToFormMessage("Create Secret Line");
                            this.user.crypt = new CrypterAES(Convert.ToInt32(Encoding.UTF8.GetString(mess[1])), Convert.ToInt32(Encoding.UTF8.GetString(mess[2])), mess[3], mess[4]);
                            this.rsa = null;
                            this.Send(true, HeadSecret, Client.Good);
                            this.form.SendToFormMessage("hech: " + this.user.crypt.GetHashCode());
                        }
                        else if (mess.Length == 2)
                        {
                            this.form.SendToFormMessage("1: " + Encoding.UTF8.GetString(mess[1]) + " 2: " + (string)Client.Good);
                            if (Encoding.UTF8.GetString(mess[1]) == Client.Good || true)
                            {
                                this.user.crypt = this.perhaps;
                                this.perhaps = null;
                                this.form.SendToFormMessage("secret good");
                            }
                        }
                        break;
                    }

                default: {
                    if (this.perhaps != null && !isRsa)
                    {
                        try
                        {
                            this.OnMessage(this.perhaps.DecoderBytes(Message), true);
                        }
                        catch(Exception e) {
                            this.form.SendToFormMessage("eror pizdaaaaaaaaaaa: " + e.Message + "\n" + e.StackTrace);
                        }
                    }

                    if (this.rsa != null && !isRsa && this.rsa.isPrivate)
                    {
                        try
                        {
                            this.OnMessage(this.rsa.Decoder(Message), true);
                        }
                        catch
                        {
                            return;
                        }

                    }
                    break;
                }

            }
        }

        private void Send()
        {
            while (this.quenSend.Count > 0 && !this.stopSendThread)
            {
                byte[] message = this.quenSend[0];
                this.quenSend.RemoveAt(0);
                this.sendPrivate(message);
            }   
        }

        private void sendPrivate(byte[] Message)
        {
            try
            {
                if (Message == null || Message.Length == 0)
                    return;
                if (this.user != null && this.user.crypt != null)
                    Message = this.user.crypt.CoderBytes(Message);
                
                this.stream.Write(Message, 0, Message.Length);
                this.stream.Write(EndMessageByte, 0, EndMessageByte.Length);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(System.Threading.ThreadAbortException)) throw e;
                //this.OnError(e);
            }
        }

        private void Send(byte[] Message, bool isOne = false)
        {

            if (isOne)
            {
                this.stopSendThread = true;
                this.quenSend.Insert(0, Message);
                if (this.sendThread != null)
                    while (this.sendThread.IsAlive) ;
            }
            else
            {
                //while (this.quenSend.Count > 100) ;
                this.quenSend.Add(Message);
            }
                            
            if (this.sendThread == null || !this.sendThread.IsAlive)
            {
                this.sendThread = new Thread(new ThreadStart(this.Send));
                this.stopSendThread = false;
                this.sendThread.Start();
            }
            
        }
        
        public void Send(string Message, bool isOne = false)
        {
            byte[] buffer;
            buffer = Encoding.UTF8.GetBytes(Message);
            // Отправим его клиенту
            if (Message != Client.Good) 
                this.form.SendToFormMessage("isOne: " + isOne + ";SEND: " + Message);                                                                            //<---------------------------------
            this.Send(buffer, isOne);
        }

        public void Send(bool isOne, params string[] Messages)
        {
            string message = "";
            foreach (string i in Messages)
                message += i + Separator;
            message = message.Substring(0, message.Length - Separator.Length);
            this.Send(message, isOne);
        }

        public void Send(MESSAGE Message, bool isOne = true)
        {
            if (Message.text.IndexOf(Separator) >= 0) {
                Message.text = Message.text.Replace(Separator, ReplaceSeparator);
            }
            this.Send( isOne,  HeadMessage, Convert.ToString(Message.isPrivate), Message.text, Message.time.ToString());
        }

        public void Send(AddFile file, bool isOne = true)
        {
            this.Send(isOne, HeadAddFile, file.getName(), System.Convert.ToString(file.getSize()), System.Convert.ToString(file.isPrivate), System.Convert.ToString(file.getTime()));
        }

        public void Send(User user, bool isOne = true)
        {
            this.Send(isOne,  HeadUser, user.Name, System.Convert.ToString(user.avatarNumber));
        }



        private void OnError(Exception e)
        {
            this.form.SendToFormMessage("Error: " + e.Message + "\n" + e.StackTrace);
            if (this.user != null)
            {
                this.form.OnDisconnect(this.user);
                if (Server.isWork) Server.OnDisconnect(this);
            }
            this.Close();
            
                        
        }

        public User getUser()
        {
            return this.user;
        }

        public void setUser(User newUser)
        {
            if (this.user.Name.Equals(newUser.Name) && this.user.HostName.ToLower().Equals(newUser.HostName.ToLower()))
            {
                this.user = newUser;
                this.user.setClient(this);
                this.rsa = null;
                this.perhaps = null;
                this.uploadFile = new List<AddFile>();
                this.quenSend = new List<byte[]>();
                this.uploadFileThread = null;
                this.sendThread = null;
                this.stopSendThread = false;
            }
        }
    }
}