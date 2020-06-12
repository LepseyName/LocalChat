using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ComServer;

namespace LocalChat
{
    public partial class Chat : Form
    {
        private readonly SynchronizationContext syncContext;
        private int lastY = 20;
        private List<User> contacts = new List<User>();
        private User privateUser = null;
        private volatile List<string> QueueAddFile = new List<string>();
        private List<AddFile> LoadedAddFile = new List<AddFile>();
        private Thread LoadAddFileThread = null;
        private bool Consol = false;
        
        public Chat()
        {
            InitializeComponent();
            this.syncContext = SynchronizationContext.Current;

            // Debug
          
            //var file = new AddFile(null, "load.gif", 9462, DataStorage.getBlockSize(), "load.gif");

            //this.SendToFormMessage(Convert.ToString(DataStorage.isHave(file)));
            //DataStorage.open(file);
            // End Debug


            var t = Storage.ReadContact();
            if (t != null)
            {
                this.SendToFormMessage(Convert.ToString(t.Count));
                foreach (User i in t)
                    this.NewUser(i);
            }

            openFileDialog1.AddExtension = false;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Title = "Add File";

            this.button2.Controls.Add(this.loadBox1);
            this.loadBox1.BackColor = Color.Transparent;
            this.loadBox1.Location = new Point(this.button2.Width / 2 - this.loadBox1.Width / 2, this.button2.Height / 2 - this.loadBox1.Height / 2);
            this.loadBox1.Image = LocalChat.Properties.Resources.load;
    
        }

        public void SendToFormMessage(object actionText)
        {
            syncContext.Post(OnMessage, actionText);
        }

        public void UpdateDownloadInfo(object downFile)
        {
            syncContext.Post(updateDownloadInfo, downFile);
        }

        private void updateDownloadInfo(object downFile)
        {
            if (downFile.GetType() == typeof(AddFile))
            {
                AddFile file = (AddFile)downFile;
                if (file.getPanel() == null)
                    return;

                ulong downBlock = 1, size = file.getCountBlock();
                for (ulong i = 0; i < size; i++)
                    if (file.flagBlock[i] != 0) downBlock++;

                View.updateFileMessage(file.getPanel(), (100.0 / size) * downBlock);
            }
        }

        private void OnMessage(object mess)
        {
            this.OnMessage(mess, false);
        }

        private void OnMessage(object mess, bool isMine)
        {
            try
            {
                if (mess.GetType() == typeof(MESSAGE) || mess.GetType() == typeof(AddFile))
                {
                    Panel fullMessage;
                    User recipient;

                    if (mess.GetType() == typeof(MESSAGE))
                    {
                        var message = (MESSAGE)mess;
                        fullMessage = View.createMessage(message, panel1.Width, isMine);
                        if (!message.isPrivate)
                        {
                            //this.OnMessage("NewPublicMessage");
                            fullMessage.Parent = panel1;
                            fullMessage.Top += this.lastY + panel1.AutoScrollPosition.Y;
                            this.lastY = fullMessage.Height + fullMessage.Top - panel1.AutoScrollPosition.Y;
                            panel1.AutoScrollPosition = new Point(0, this.lastY);

                            if (this.privateUser != null)
                            {
                                View.newMessage(button1);
                            }
                        }
                        else
                        {
                            if (message.Creator == Server.me)
                                recipient = message.whose;
                            else
                                recipient = message.Creator;
                            //this.OnMessage("NewPrivateMessage: " + recipient.Name);

                            this.getPersonalChat(recipient);

                            fullMessage.Parent = recipient.message;
                            fullMessage.Top += recipient.lastMessageY + recipient.message.AutoScrollPosition.Y;
                            recipient.lastMessageY = fullMessage.Height + fullMessage.Top - recipient.message.AutoScrollPosition.Y;
                            recipient.message.AutoScrollPosition = new Point(0, recipient.lastMessageY);

                            if (this.privateUser != recipient)
                            {
                                View.newMessage(recipient.contact);
                            }
                        }
                    }
                    else
                    {
                        var message = (AddFile)mess;
                        fullMessage = View.createFileMessage(message, panel1.Width,this.downloadClick, isMine);

                        if (!message.isPrivate)
                        {
                            //this.OnMessage("NewPublicMessage");
                            fullMessage.Parent = panel1;
                            fullMessage.Top += this.lastY + panel1.AutoScrollPosition.Y;
                            this.lastY = fullMessage.Height + fullMessage.Top - panel1.AutoScrollPosition.Y;
                            panel1.AutoScrollPosition = new Point(0, this.lastY);

                            if (this.privateUser != null)
                            {
                                View.newMessage(button1);
                            }
                        }
                        else
                        {
                            if (message.getCreator() == Server.me)
                                recipient = message.getAddressee();
                            else
                                recipient = message.getCreator();
                            //this.OnMessage("NewPrivateMessage: " + recipient.Name);

                            this.getPersonalChat(recipient);

                            fullMessage.Parent = recipient.message;
                            fullMessage.Top += recipient.lastMessageY + recipient.message.AutoScrollPosition.Y;
                            recipient.lastMessageY = fullMessage.Height + fullMessage.Top - recipient.message.AutoScrollPosition.Y;
                            recipient.message.AutoScrollPosition = new Point(0, recipient.lastMessageY);

                            if (this.privateUser != recipient)
                            {
                                View.newMessage(recipient.contact);
                            }
                        }


                    }    
                }
                else
                {
                    if (this.Consol) 
                        this.textBox3.Lines = (this.textBox3.Text + "\n" + (string)mess).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries); 
                }
            }    
            catch (Exception e)
            {
                if(this.Consol)
                    this.textBox3.Lines = (this.textBox3.Text + "\n" + e.Message + "\n" + e.StackTrace).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);  
            }
        } 

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private User getUser(string name)
        {
            foreach (User i in this.contacts)
                if (i.Name + "#" + i.HostName == name) return i;
            if (Server.me.Name + "#" + Server.me.HostName == name) return Server.me;
            return null;
        }

        private AddFile getFile(Label infa){
            string[] data = infa.Text.Split(new string[]{ "\n" }, StringSplitOptions.None);
            if (data.Length == 3)
            {
                string name = data[0], newName;
                ulong size;
                try
                {
                    string sizeS = "";
                    for(int i=0; i<data[1].Length; i++)
                        if(data[1][i] >= '0' && data[1][i] <= '9')
                            sizeS += data[1][i];
                        else if(sizeS != "")
                            break;
                    size = Convert.ToUInt64(sizeS);
                }
                catch
                {
                    return null;
                }
                if (data[2].IndexOf(name) != -1)
                    newName = data[2];
                else
                    return null;

                User creator = null;
                foreach (object i in infa.Parent.Controls)
                    if (i.GetType() == typeof(Label))
                    {
                        creator = this.getUser(((Label)i).Text);
                        if (creator != null) break;
                    }
                if (creator != null)
                {
                    var file = new AddFile(creator, name, size, Storage.getBlockSize(), newName);
                    file.setPanel((Panel)infa.Parent);
                    return file;
                }
            }
            return null;
        }

        private void downloadClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                string[] data = ((Label)sender).Text.Split(new string[]{ "\n" }, StringSplitOptions.None);
                if (data.Length >= 2 && data.Length <= 3)
                {
                    string name = data[0], newName;
                    ulong size;
                    try
                    {
                        string sizeS = "";
                        for(int i=0; i<data[1].Length; i++)
                            if(data[1][i] >= '0' && data[1][i] <= '9')
                                sizeS += data[1][i];
                            else if(sizeS != "")
                                break;
                        size = Convert.ToUInt64(sizeS);
                    }
                    catch
                    {
                        return;
                    }
                    if ( data.Length == 3) 
                        newName = data[2];
                    else
                        newName = name;
                    
                    User creator = null;
                    foreach(object i in ((Label)sender).Parent.Controls)
                        if (i.GetType() == typeof(Label))
                        {
                            creator = this.getUser(((Label)i).Text);
                            if (creator != null) break;
                        }

                    if (creator != null )
                    {
                        var file = new AddFile(creator, name, size, Storage.getBlockSize(), newName);
                        file.setPanel((Panel)((Label)sender).Parent);

                        if (!creator.Equals(Server.me) && Server.isDownloadable(creator.client, file))
                        {
                            return;
                        }
                        else if (Storage.isHave(file))
                        {
                            try
                            {
                                Storage.open(file);
                            }
                            catch
                            {
                                if (!creator.Equals(Server.me))
                                {
                                    View.updateFileMessage(file.getPanel());
                                    Server.startDownload(creator.client, file);
                                }
                            }
                        }
                        else if (!creator.Equals(Server.me))
                        {
                            View.updateFileMessage(file.getPanel());
                            Server.startDownload(creator.client, file);
                        }
                        else
                        {
                            View.updateFileMessage(file.getPanel(), "Error Download! Это ваш файл и Вы удалили его");//eror on panel
                        }                        
                    }
                }
            }
        }

        private void Chat_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // вызываем главную форму, которая открыла текущую, главная форма всегда = 0 - [0]
                Form ifrm = Application.OpenForms[0];
                ifrm.StartPosition = FormStartPosition.Manual; // меняем параметр StartPosition у Form1, иначе она будет использовать тот, который у неё прописан в настройках и всегда будет открываться по центру экрана
                // ifrm.Left = this.Left; // задаём открываемой форме позицию слева равную позиции текущей формы
                // ifrm.Top = this.Top; // задаём открываемой форме позицию сверху равную позиции текущей формы
                ifrm.Close(); // отображаем Form1
                Application.Exit();
            }
            catch( Exception ){
                Application.Exit();
            }
        }

        public void OnConnect(object user)
        {
            syncContext.Post(NewUser, user);
        }

        public void OnDisconnect(object user)
        {
            syncContext.Post(DisUser, user);
        }

        public void downloadError(AddFile file)
        {
            syncContext.Post(errorDonwload, file);
        }

        private void NewUser(object u)
        {
            this.OnMessage("NewUser");
            User user = (User)u;
            
            foreach (var i in this.contacts)
                if (i.Name.Equals(user.Name) && i.HostName.ToLower().Equals(user.HostName.ToLower()))
                {     
                    if (i.client != user.client)
                    {
                        this.OnMessage(i.ToString());
                        if (i.client != null && i.client.isGood())
                            i.client.Close();
                        i.update(user);
                        i.client.setUser(i);
                        this.OnMessage("swap");
                        this.OnMessage(i.ToString());
                    }
                    View.updateContact(i.contact, user);
                    i.contact.ForeColor = Color.Black;
                    return;
                }
            // else
            Panel contact = View.createContact(user, panel2.Width, this.UserClick);
            
            user.contact = contact;
            this.contacts.Add(user);

            panel2.Controls.Add(contact);
        }

        private void UserClick(object sender, EventArgs e)
        {
            this.OnMessage("UserClick");
            if (sender.GetType() == typeof(Panel))
            {
                for (int i = 0; i < this.contacts.Count; i++)
                {
                    if (this.contacts[i].contact == (Panel)sender)
                    {
                        this.getPersonalChat(this.contacts[i]).Visible = true;
                        this.panel1.Visible = false;
                        this.privateUser = this.contacts[i];
                        this.button1.BackColor = Color.White;
                        this.contacts[i].contact.BackColor = Color.LightGray;
                        View.newMessageRead(this.contacts[i].contact);
                    }
                    else
                    {
                        if (this.contacts[i].contact != null)
                        {
                            this.contacts[i].contact.BackColor = Color.White;
                            this.getPersonalChat(this.contacts[i]).Visible = false;
                        }
                    }
                }
            }
            else if (sender.GetType() == typeof(Label)) 
            {
                this.UserClick( ((Label)sender).Parent, e);
            }
            else if (sender.GetType() == typeof(Button))
            {
                this.UserClick(((Button)sender).Parent, e);
            }
        }

        private void DisUser(object u)
        {
            User user = (User)u;
            foreach(var i in this.contacts)
                if (i.Name.Equals(user.Name) && i.HostName.Equals(user.HostName))
                {
                    i.contact.ForeColor = Color.Gray;
                    return;
                }
        }

        private void errorDonwload(object file){
            if (file.GetType() == typeof(AddFile))
            {
                var File = (AddFile)file;
                if (File.getCreator() != null && File.getCreator().message != null)
                {
                    foreach(var i in File.getCreator().message.Controls)
                        if (i.GetType() == typeof(Panel))
                        {
                            AddFile addFile = this.getFile(View.getInfa((Panel)i) );
                            if (addFile == null)
                                continue;
                            if(addFile.getCreator().Equals(File.getCreator()) && addFile.getName() == File.getName() && addFile.getNewName() == File.getNewName()){
                                View.updateFileMessage(addFile.getPanel(), File.getError());
                                return;
                            }
                        }
                }
                if (File.getCreator() != null)
                {
                    foreach (var i in this.panel1.Controls)
                        if (i.GetType() == typeof(Panel))
                        {
                            AddFile addFile = this.getFile(View.getInfa((Panel)i));
                            if (addFile == null)
                                continue;
                            if (addFile.getCreator().Equals(File.getCreator()) && addFile.getName() == File.getName() && addFile.getNewName() == File.getNewName())
                            {
                                View.updateFileMessage(addFile.getPanel(), File.getError());
                                return;
                            }
                        }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            textBox1.Text = "";
            
            while(text.Length > 0 && text[0] < 33)
                text = text.Substring(1);
            if (text.Length > 0)
            {
                MESSAGE mess;

                if (this.privateUser == null)
                {
                    mess = new MESSAGE(Server.me, this.privateUser, text);
                    Server.SendToAll(mess);
                }
                else
                {
                    mess = new MESSAGE(Server.me, this.privateUser, text, true);
                    Server.Send(this.privateUser.client, mess);
                }
                this.OnMessage(mess, true);
            }

            if (this.LoadedAddFile.Count > 0)
            {
                foreach(AddFile file in this.LoadedAddFile){
                    file.setAddressee(this.privateUser);
                    if (this.privateUser == null)              
                        Server.SendToAll(file);
                    else            
                        Server.Send(this.privateUser.client, file);
                    this.OnMessage(file, true);
                }
                this.LoadedAddFile.Clear();
                this.countAddFileLabel.Text = "";
            }
        }

        private Panel getPersonalChat(User i)
        {
            if (i.client != null && !i.client.isCrypted())
                i.client.openSecretLine();
            if (i.message == null) i.message = View.createPersonalChat(this.panel1, i.Name);          
            
            return i.message;
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( (e.KeyChar == (char)Keys.Enter) && this.textBox1.Text.Length > 0)
                button3_Click(sender, new EventArgs());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var i in this.contacts)
            {
                if (i.message != null) i.message.Visible = false;
                i.contact.BackColor = Color.White;
            }
            this.panel1.Visible = true;
            this.privateUser = null;
            this.button1.BackColor = Color.LightGray;
            View.newMessageRead(this.button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                this.LoadAddFileAnimation(true);

                if (!this.QueueAddFile.Exists(x => x.Equals(FileName)) == true)
                    this.QueueAddFile.Add(FileName);

                if (this.LoadAddFileThread == null || !this.LoadAddFileThread.IsAlive)
                {
                    this.LoadAddFileThread = new Thread(new ThreadStart(loadAddFile));
                    this.LoadAddFileThread.Start();
                }
            }
        }

        private void displayCountAddFile(object count)
        {
            syncContext.Post(displayCountAddFileSync, count);
        }

        private void displayCountAddFileSync(object count)
        {
            this.countAddFileLabel.Text = Convert.ToString((int)count);
        }

        private void loadAddFile()
        {
            while (this.QueueAddFile.Count > 0)
            {
                var f = this.QueueAddFile[0];
                this.QueueAddFile.RemoveAt(0);
                
                AddFile file = Storage.getAddFile(f);
                this.LoadedAddFile.Add(file);

                this.displayCountAddFile(this.LoadedAddFile.Count);
            }
            this.LoadAddFileAnimation(false);
        }

        private void LoadAddFileAnimation(object isStart)
        {
            syncContext.Post(LoadAddFileAnimationSync, isStart);
        }

        private void LoadAddFileAnimationSync(object isStart)
        {
            this.loadBox1.Visible = (bool)isStart;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void loadBox1_Click(object sender, EventArgs e)
        {
            this.button2_Click(sender, e);
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Environment.CurrentDirectory;
            System.Diagnostics.Process.Start( "\"" + s + "\\Manual of used LocalChat.docx\"");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CrypterAES.setKeySize(128);
            this.toolStripMenuItem3.Checked = false;
            this.toolStripMenuItem4.Checked = false;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CrypterAES.setKeySize(192);
            this.toolStripMenuItem2.Checked = false;
            this.toolStripMenuItem4.Checked = false;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CrypterAES.setKeySize(256);
            this.toolStripMenuItem3.Checked = false;
            this.toolStripMenuItem2.Checked = false;
        }

        private void Mode1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setMode(System.Security.Cryptography.CipherMode.CBC);
            this.Mode2ToolStripMenuItem.Checked = false;
            this.Mode3ToolStripMenuItem.Checked = false;
            this.Mode4ToolStripMenuItem.Checked = false;
            this.Mode5ToolStripMenuItem.Checked = false;
        }

        private void Mode2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setMode(System.Security.Cryptography.CipherMode.CFB);
            this.Mode1ToolStripMenuItem.Checked = false;
            this.Mode3ToolStripMenuItem.Checked = false;
            this.Mode4ToolStripMenuItem.Checked = false;
            this.Mode5ToolStripMenuItem.Checked = false;
        }

        private void Mode3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setMode(System.Security.Cryptography.CipherMode.CTS);
            this.Mode1ToolStripMenuItem.Checked = false;
            this.Mode2ToolStripMenuItem.Checked = false;
            this.Mode4ToolStripMenuItem.Checked = false;
            this.Mode5ToolStripMenuItem.Checked = false;
        }

        private void Mode4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setMode(System.Security.Cryptography.CipherMode.ECB);
            this.Mode1ToolStripMenuItem.Checked = false;
            this.Mode2ToolStripMenuItem.Checked = false;
            this.Mode3ToolStripMenuItem.Checked = false;
            this.Mode5ToolStripMenuItem.Checked = false;
        }

        private void Mode5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setMode(System.Security.Cryptography.CipherMode.OFB);
            this.Mode1ToolStripMenuItem.Checked = false;
            this.Mode2ToolStripMenuItem.Checked = false;
            this.Mode3ToolStripMenuItem.Checked = false;
            this.Mode4ToolStripMenuItem.Checked = false;
        }

        private void padding1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setPadding(System.Security.Cryptography.PaddingMode.ANSIX923);
            this.padding2ToolStripMenuItem.Checked = false;
            this.padding3ToolStripMenuItem.Checked = false;
            this.padding4ToolStripMenuItem.Checked = false;
        }

        private void padding2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setPadding(System.Security.Cryptography.PaddingMode.ISO10126);
            this.padding1ToolStripMenuItem.Checked = false;
            this.padding3ToolStripMenuItem.Checked = false;
            this.padding4ToolStripMenuItem.Checked = false;
        }

        private void padding3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setPadding(System.Security.Cryptography.PaddingMode.PKCS7);
            this.padding1ToolStripMenuItem.Checked = false;
            this.padding2ToolStripMenuItem.Checked = false;
            this.padding4ToolStripMenuItem.Checked = false;
        }

        private void padding4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrypterAES.setPadding(System.Security.Cryptography.PaddingMode.Zeros);
            this.padding1ToolStripMenuItem.Checked = false;
            this.padding2ToolStripMenuItem.Checked = false;
            this.padding3ToolStripMenuItem.Checked = false;
        }

        private void открытьПапкуЗагрузокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Storage.openDownload();
        }

        private void удалитьВсеЗагрузкиИВыгрузкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Storage.Clean();
        }

        private void удалитьВсеСообщенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.privateUser != null && this.privateUser.message != null)
            {
                this.privateUser.message.Controls.Clear();
                this.privateUser.lastMessageY = 0;
            }
            else
            {
                this.panel1.Controls.Clear();
                this.lastY = 0;
            }
        }

        private void consolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Consol)
            {
                this.Width = 730;
                this.Consol = false;
            }
            else
            {
                this.Width = 1130;
                this.Consol = true;
            }
        }

        private void информацияОПоследнейЗагрузкеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long time, count, size;
            long[] deltaTime, downLoad;
            Storage.getStatistic(out time, out count, out size, out deltaTime, out downLoad);

            ComServer.ComServer output = new ComServer.ComServer();
            output.openInExel(time, count, size, deltaTime, downLoad);      
        }


     

    }

    public struct MESSAGE {
        public User Creator;
        public User whose;
        public String text;
        public bool isPrivate;
        public DateTime time;

        public MESSAGE(User user, User whose, String text, bool isPrivate = false)
        {
            this.Creator = user;
            this.whose = whose;
            this.text = text;
            this.isPrivate = isPrivate;
            this.time = DateTime.Now;
        }

        public MESSAGE(User user, User whose, String text, DateTime time, bool isPrivate = false)
        {
            this.Creator = user;
            this.whose = whose;
            this.text = text;
            this.isPrivate = isPrivate;
            this.time = time;
        }
    }

}
