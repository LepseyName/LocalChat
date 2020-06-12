using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LocalChat
{
    static class Storage
    {
        static private string DefaultData = "Data";
        static private string DefaultFile = "File";
        static private string DefaultUserData = "Me.dat";
        static private string DefaultContactData = "Contact.dat";
        static private string DefaultLastDownloadInfo = "Down.dat";

        static private string InFileName = "Name: ";
        static private string InFileHostName = "Hostname: ";
        static private string InFileAatarNumber = "Ava: ";
        static private string InFileCrypt = ":Crypt";
        static private string InFileCryptSeparator = ":-|-:";

        static private User me = new User("", "", 0, null);
        static private int key = 0;

        static private int BLOCK_SIZE = 1024;
        static private bool isSave = false;
        
        static public bool Initialize(string nameUser)
        {
            Storage.key = nameUser.GetHashCode();

            if ( !Directory.Exists(DefaultData)) return false;

            if (!File.Exists(DefaultData + "/" + DefaultUserData)) return false;

            try
            {
                var MeData = new BinaryReader(File.OpenRead(DefaultData + "/" + DefaultUserData));
                byte[] array = new byte[1024];
                int count = 0;

                while (MeData.BaseStream.Position != MeData.BaseStream.Length)
                {
                    byte s = Storage.ReadByte(MeData);
                    if (s == (byte)'\n'){
                        byte[] normArray = new byte[count];
                        for(int i =0; i<count; i++)
                            normArray[i] = array[i];

                        int name = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileName));
                        if ( name >= 0){
                            byte[] buf = Program.SubBytes(normArray, Encoding.UTF8.GetBytes(InFileName).Length);
                            Storage.me.Name = Encoding.UTF8.GetString(buf);
                            count = 0;
                            continue;
                        }

                        int hostName = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileHostName));
                        if ( hostName >= 0){
                            byte[] buf = Program.SubBytes(normArray, Encoding.UTF8.GetBytes(InFileHostName).Length);
                            Storage.me.HostName = Encoding.UTF8.GetString(buf);
                            count = 0;
                            continue;
                        }

                        int avaNumber = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileAatarNumber));
                        if ( avaNumber >= 0){
                            byte[] buf = Program.SubBytes(normArray, Encoding.UTF8.GetBytes(InFileAatarNumber).Length);
                            Storage.me.avatarNumber = Convert.ToInt32(Encoding.UTF8.GetString(buf));
                            count = 0;
                            continue;
                        }

                        int crypt = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileCrypt));
                        if ( crypt >= 0){
                            byte[] buf = Program.SubBytes(normArray, 0, count - Encoding.UTF8.GetBytes(InFileCrypt).Length);
                            byte[][] cryptInfo = Program.Split(buf, Encoding.UTF8.GetBytes(InFileCryptSeparator), true);
                            Storage.me.crypt = new CrypterAES( Convert.ToInt32(Encoding.UTF8.GetString(cryptInfo[0])), Convert.ToInt32(Encoding.UTF8.GetString(cryptInfo[1])), cryptInfo[2], cryptInfo[3]);
                            count = 0;
                            continue;
                        }
                        array[count++] = s; 
                    }else{
                       array[count++] = s; 
                    }
                }
                MeData.Close();
                if (Storage.me.Name != null &&
                    Storage.me.HostName != null &&
                    Storage.me.avatarNumber >= 0 &&
                    Storage.me.crypt != null) return true;
                else return false;
                
            }
            catch(Exception e){
                throw e;
                return false;
            }

        }

        static public void SaveMe(User me){
            Storage.me = me;
            Storage.key = me.Name.GetHashCode();

            if ( !Directory.Exists(DefaultData)) Directory.CreateDirectory(DefaultData);

            try{
                var output = new BinaryWriter(File.Open(DefaultData + "/" + DefaultUserData, FileMode.Create));

                byte[] mess = Encoding.UTF8.GetBytes(InFileName + me.Name + "\n");
                for(int i=0; i<mess.Length; i++)
                    Storage.WriteByte(output, mess[i]);

                mess = Encoding.UTF8.GetBytes(InFileHostName + me.HostName + "\n");
                for(int i=0; i<mess.Length; i++)
                    Storage.WriteByte(output, mess[i]);

                mess = Encoding.UTF8.GetBytes(InFileAatarNumber + me.avatarNumber + "\n");
                for(int i=0; i<mess.Length; i++)
                    Storage.WriteByte(output, mess[i]);

                mess = Encoding.UTF8.GetBytes(CrypterAES.getMode() + InFileCryptSeparator + CrypterAES.getPadding() + InFileCryptSeparator);
                mess = Program.ConcatByte(mess, me.crypt.getKey());
                mess = Program.ConcatByte(mess, Encoding.UTF8.GetBytes(InFileCryptSeparator) );
                mess = Program.ConcatByte(mess, me.crypt.getIV());
                mess = Program.ConcatByte(mess, Encoding.UTF8.GetBytes(InFileCrypt + "\n") );
                for(int i=0; i<mess.Length; i++)
                    Storage.WriteByte(output, mess[i]);
                output.Close();
            }catch{

            }
        }

        static public void SaveContact(User contact)
        {
            if (!Directory.Exists(DefaultData)) Directory.CreateDirectory(DefaultData);
            if (!File.Exists(DefaultData + "/" + DefaultContactData)) File.Create(DefaultData + "/" + DefaultContactData);

            try
            {
                var output = new BinaryWriter(File.Open(DefaultData + "/" + DefaultContactData, FileMode.Append));

                byte[] mess = Encoding.UTF8.GetBytes(InFileName + contact.Name + "\n");
                for (int i = 0; i < mess.Length; i++)
                    Storage.WriteByte(output, mess[i]);

                mess = Encoding.UTF8.GetBytes(InFileHostName + contact.HostName + "\n");
                for (int i = 0; i < mess.Length; i++)
                    Storage.WriteByte(output, mess[i]);

                mess = Encoding.UTF8.GetBytes(InFileAatarNumber + contact.avatarNumber + "\n");
                for (int i = 0; i < mess.Length; i++)
                    Storage.WriteByte(output, mess[i]);

                if (contact.crypt != null)
                {
                    mess = Encoding.UTF8.GetBytes(contact.crypt.getMod() + InFileCryptSeparator + contact.crypt.getPaddin() + InFileCryptSeparator);
                    mess = Program.ConcatByte(mess, me.crypt.getKey());
                    mess = Program.ConcatByte(mess, Encoding.UTF8.GetBytes(InFileCryptSeparator));
                    mess = Program.ConcatByte(mess, me.crypt.getIV());
                    mess = Program.ConcatByte(mess, Encoding.UTF8.GetBytes(InFileCrypt + "\n"));
                    for (int i = 0; i < mess.Length; i++)
                        Storage.WriteByte(output, mess[i]);
                }
                output.Close();
            }
            catch
            {

            }
        }

        static public List<User> ReadContact()
        {
            if (!Directory.Exists(DefaultData)) return null;
            if (!File.Exists(DefaultData + "/" + DefaultContactData)) return null;

            //try
            //{
            var MeData = new BinaryReader(File.OpenRead(DefaultData + "/" + DefaultContactData));
            byte[] array = new byte[1024];
            int count = 0;
            var list = new List<User>();
            User contact = new User("", "", 0, null);

            while (MeData.BaseStream.Position != MeData.BaseStream.Length)
            {
                if (count >= 1024) return list;
                byte s = Storage.ReadByte(MeData);
                if (s == (byte)'\n')
                {
                    byte[] normArray = new byte[count];
                    for (int i = 0; i < count; i++)
                        normArray[i] = array[i];

                    int name = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileName));
                    if (name >= 0)
                    {
                        byte[] buf = Program.SubBytes(normArray, Encoding.UTF8.GetBytes(InFileName).Length + name);
                        if (contact.Name != null) 
                        {
                            list.Add(contact);
                            contact = new User();  
                        }
                        contact.Name = Encoding.UTF8.GetString(buf);
                        count = 0;
                        continue;
                    }

                    int hostName = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileHostName));
                    if (hostName >= 0)
                    {
                        byte[] buf = Program.SubBytes(normArray, Encoding.UTF8.GetBytes(InFileHostName).Length + hostName);
                        contact.HostName = Encoding.UTF8.GetString(buf);
                        count = 0;
                        continue;
                    }

                    int avaNumber = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileAatarNumber));
                    if (avaNumber >= 0)
                    {
                        byte[] buf = Program.SubBytes(normArray, Encoding.UTF8.GetBytes(InFileAatarNumber).Length + avaNumber);
                        contact.avatarNumber = Convert.ToInt32(Encoding.UTF8.GetString(buf));
                        count = 0;
                        continue;
                    }

                    int crypt = Program.FirstIndexOf(normArray, Encoding.UTF8.GetBytes(InFileCrypt));            
                    if (crypt >= 0)
                    {
                        byte[] buf = Program.SubBytes(normArray, 0, count - Encoding.UTF8.GetBytes(InFileCrypt).Length);
                        byte[][] cryptInfo = Program.Split(buf, Encoding.UTF8.GetBytes(InFileCryptSeparator), true);
                        contact.crypt = new CrypterAES(Convert.ToInt32(Encoding.UTF8.GetString(cryptInfo[0])), Convert.ToInt32(Encoding.UTF8.GetString(cryptInfo[1])), cryptInfo[2], cryptInfo[3]);

                        count = 0;
                        list.Add(contact);
                        contact = new User();
                        continue;
                    }
                    array[count++] = s;
                }
                else
                {
                    array[count++] = s;
                }
            }

            if(contact.Name != null) list.Add(contact);

            MeData.Close();
            return list;
            /*}
            catch(Exception e){
                throw e;
                //return false;
            }*/
        }

        static public byte ReadByte(BinaryReader input)
        {
            byte s;
            s = input.ReadByte();
            
            if (Storage.key != 0) s = Storage.decrypt(s, Storage.key); 
            return s;
        }

        static public void WriteByte(BinaryWriter output, byte s)
        {
            if (Storage.key != 0) s = Storage.crypt(s, Storage.key);
            output.Write(s);
        }

        static byte crypt(byte s, int i){
            return (byte)(s + (byte)i);
        }
        static byte decrypt(byte s, int i)
        {
            return (byte)(s - (byte)i);
        }

        static public void saveStatistic(AddFile file)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Storage.DefaultData + "\\" + DefaultLastDownloadInfo, false, System.Text.Encoding.Default))
                {
                        sw.WriteLine(DateTime.Now.Ticks + " " + file.getCountBlock() + " " + file.getBlockSize());                    
                }
            }
            catch { }   
        }

        static public void saveStatistic(double count)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Storage.DefaultData + "\\" + DefaultLastDownloadInfo, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(DateTime.Now.Ticks + " " + count);
                }
            }
            catch { }
        }

        static public void getStatistic(out long time, out long count, out long size, out long[] deltaTime, out long[] downLoad)
        {
            time = count = size = 0;
            deltaTime = downLoad = null;
            List<long> a = new List<long>(), b = new List<long>();
            try
            {
                string[] bufer;
                string s;
                using (StreamReader sw = new StreamReader(Storage.DefaultData + "\\" + DefaultLastDownloadInfo, System.Text.Encoding.Default))
                {
                    while (!sw.EndOfStream)
                    {
                        s = sw.ReadLine();
                        bufer = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (time == 0)
                        {
                            size = Convert.ToInt64(bufer[2]);
                            count = Convert.ToInt64(bufer[1]);
                            time = Convert.ToInt64(bufer[0]);
                        }
                        else
                        {
                            a.Add(Convert.ToInt64(bufer[0]));
                            b.Add(Convert.ToInt64(bufer[1]));
                        }
                    }
                }
            }
            catch { }
            deltaTime = a.ToArray();
            downLoad = b.ToArray();
        }

        static public User getMe(){
            return Storage.me;
        }

        public static string getNewName(string name)
        {
            while (File.Exists(Storage.DefaultFile + "\\" + name))
                name = "#" + name;
            return name;
        }

        public static void createFile(AddFile file)
        {
            if (!File.Exists(DefaultFile + "\\" + file.getNewName()))
            {
                if (!Directory.Exists(DefaultFile))
                    Directory.CreateDirectory(DefaultFile);

                var stream = new BinaryWriter(File.Create(DefaultFile + "\\" + file.getNewName()));
                for (ulong i = 0; i < file.getSize(); i++)
                    stream.Write((byte)1);
                stream.Close();
                stream.Dispose();
            }
            else
            {
                throw new Exception("File not found!");
            }
        }

        public static AddFile getAddFile(string FileName)
        {
            if (File.Exists(FileName))
            {
                if (!Directory.Exists(DefaultFile))
                    Directory.CreateDirectory(DefaultFile);

                
                string onlyName = FileName.Substring(FileName.LastIndexOf('\\') + 1);
                ulong size = (ulong)(new FileInfo(FileName)).Length;
                
                File.Copy(FileName, DefaultFile + '\\' + onlyName, true);
                AddFile file = new AddFile(Server.me, onlyName, size, Storage.BLOCK_SIZE);                

                return file;
            }
            else
            {
                throw new Exception("File not found!");
            }
        }

        public static AddFile getAddFile(string name, string newName, int blockSize, ulong countBlock)
        {
            if (File.Exists(DefaultFile + "\\" + name))
            {
                ulong size = (ulong)(new FileInfo(DefaultFile + "\\" + name)).Length;

                if (size <= countBlock * (ulong)blockSize && size > (countBlock - 1)* (ulong)blockSize )
                {
                    AddFile file = new AddFile(Server.me, name, size, blockSize, newName);
                    return file;
                }
                return null;
            }
            return null;
        }

        public static byte[] getBlockFile(string name, int blockSize, ulong number)
        {
            if(!File.Exists(DefaultFile + "\\" + name))
                throw new Exception("file not found");

            ulong size = (ulong)(new FileInfo(DefaultFile + "\\" + name)).Length;
            if(number*(ulong)blockSize > size)
                throw new Exception("out of file");

            byte[] block = new byte[blockSize];
            var fileStream = new BinaryReader(File.OpenRead(DefaultFile + "\\" + name)).BaseStream;
            ulong delta = number * (ulong)blockSize;

            fileStream.Position = (long)delta;
            int realSize = fileStream.Read(block, 0, blockSize);
            if (realSize < blockSize)
                Array.Resize(ref block, realSize);

            fileStream.Close();
            fileStream.Dispose();
            return block;
        }

        public static void setBlockFile(string name, int blockSize, ulong number, byte[] block)
        {
            if (!File.Exists(DefaultFile + "\\" + name))
                throw new Exception("file not found");

            ulong size = (ulong)(new FileInfo(DefaultFile + "\\" + name)).Length;
            if (number * (ulong)blockSize > size)
                throw new Exception("out of file");

            
            var fileStream = new BinaryWriter(File.OpenWrite(DefaultFile + "\\" + name)).BaseStream;
            ulong delta = number * (ulong)blockSize;

            fileStream.Position = (long)delta;
            fileStream.Write(block, 0, block.Length);

            fileStream.Close();
            fileStream.Dispose();
        }

        public static bool isHave(AddFile file)
        {
            string curentDir = Environment.CurrentDirectory;
            if (!File.Exists(curentDir + "\\" + DefaultFile + "\\" + file.getNewName()))
                return false;

            ulong size = (ulong)(new FileInfo(DefaultFile + "\\" + file.getNewName())).Length;
            if (size != file.getSize())
                return false;

            return true;
        }

        public static void open(AddFile file)
        {
            if (Storage.isHave(file))
            {
                string curentDir = Environment.CurrentDirectory;
                System.Diagnostics.Process.Start("\"" + curentDir + "\\" + DefaultFile + "\\" + file.getNewName() + "\"");
            }    
        }

        public static void openDownload()
        {
            if (!Directory.Exists(DefaultFile))
                Directory.CreateDirectory(DefaultFile);

            string open = "\"" + Environment.CurrentDirectory + "\\" + DefaultFile + "\"";
            System.Diagnostics.Process.Start(open);
        }

        public static void Clean()
        {
            if (Directory.Exists(DefaultFile))
                Directory.Delete(DefaultFile, true);
        }


        public static int getBlockSize()
        {
            return Storage.BLOCK_SIZE;
        }

        public static bool getIsSave()
        {
            return Storage.isSave;
        }

        public static void setIsSave(bool isS)
        {
            Storage.isSave = isS;
        }
    }
}
