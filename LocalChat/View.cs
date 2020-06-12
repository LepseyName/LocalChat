using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LocalChat
{
    static class View
    {
        public delegate void downloadClick(object sender, EventArgs e);
        public delegate void contactClick(object sender, EventArgs e);
        private static int lastHeightContacts = 0;
        
        private class ContactsView
        {
            public int ImageLeft = 10;
            public int ImageTop = 10;
            public int ImageWidth = 40;
            public int ImageHeight = 40;

            public int LabelTop = 20;
            public int LabelHeight = 20;
            public Region ImageRegion;

            public ContactsView(int PanelWidth)
            {
                System.Drawing.Drawing2D.GraphicsPath Button_Path = new System.Drawing.Drawing2D.GraphicsPath();
                Button_Path.AddEllipse(3, 3, this.ImageWidth - 6, this.ImageHeight - 6);
                this.ImageRegion = new Region(Button_Path);
            }
        }

        private class MessageView
        {
            public Region MessageRegion;
            public int MessageTop = 20;
            public int MessageLeft= 20;

            public int ImageLeft = 10;
            public int ImageTop = 10;
            public int ImageWidth;
            public int ImageHeight;
            public Region ImageRegion;

            public int TextLeft = 10;
            public int TextTop = 20;
            public int TextHeight = 20;
            public int TextWidth;

            public int NameLeft = 10;
            public int NameTop = 10;
            public int NameHeight = 20;
            public int NameWidth;

            public int DateLeft = 10;
            public int DateTop = 10;
            public int DateHeight = 20;
            public int DateWidth;

            public MessageView(int PanelWidth)
            {
                MessageRegion = null;


                ImageWidth = 15 * PanelWidth / 100;
                ImageHeight = ImageWidth;
                
                System.Drawing.Drawing2D.GraphicsPath Button_Path = new System.Drawing.Drawing2D.GraphicsPath();
                Button_Path.AddEllipse(5, 5, ImageWidth - 10, ImageHeight - 10);
                ImageRegion = new Region(Button_Path);


                TextWidth = 60 * PanelWidth / 100;
                NameWidth = TextWidth / 2;
                DateWidth = NameWidth;
            }
        }
        
        
        public static Panel createMessage(MESSAGE message, int PanelWidth, bool isMine)
        {
            var MesView = new MessageView(PanelWidth * 9 / 10);

            var image = new Button();
            image.Text = "";
            image.Width = MesView.ImageWidth;
            image.Height = MesView.ImageHeight;
            image.Top = MesView.ImageTop;
            image.Region = MesView.ImageRegion;
            image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            image.BackgroundImage = View.getAvatar(message.Creator.avatarNumber);
            
            
            if (!isMine) image.Left = MesView.ImageLeft;

            var text = new Label();
            text.Text = message.text;
            text.Font = new Font(text.Font.Name, 8.0f, text.Font.Style);
            text.AutoSize = true;
            text.MaximumSize = new Size(PanelWidth * 7 / 10, 0);
            text.MinimumSize = new Size(MesView.TextWidth, MesView.TextHeight);
            text.Height = text.PreferredHeight;
            text.Width = text.PreferredWidth;
            text.Top = MesView.TextTop;

            if (!isMine) 
                text.Left = MesView.TextLeft + 2 * MesView.ImageLeft + MesView.ImageWidth;
            else
            {
                text.Left = MesView.TextLeft;
                image.Left = MesView.ImageLeft + 2 * MesView.TextLeft + text.Width;
            }


            var name = new Label();
            name.Text = message.Creator.Name + "#" + message.Creator.HostName;
            name.Width = MesView.NameWidth;
            name.Left = text.Left;
            name.Top = text.Height + MesView.TextTop + MesView.NameTop;
            name.Height = MesView.NameHeight;

            var date = new Label();
            date.Text = message.time.ToString();
            date.Width = MesView.DateWidth;
            date.Left = name.Left + name.Width + MesView.DateLeft;
            date.Top = name.Top;
            date.Height = name.Height;


            var fullMessage = new Panel();
            
            fullMessage.Width = image.Width + text.Width + 2 * MesView.ImageLeft + 2 * MesView.TextLeft;
            fullMessage.Text = "";
            fullMessage.Height = Math.Max(MesView.ImageHeight + 2 * MesView.ImageTop, text.Height + MesView.TextTop + MesView.NameTop + name.Height);
            fullMessage.BorderStyle = BorderStyle.None;
            fullMessage.BackColor = Color.LightGray;

            fullMessage.Controls.Add(image);
            fullMessage.Controls.Add(text);
            fullMessage.Controls.Add(name);
            fullMessage.Controls.Add(date);

            if (!isMine) fullMessage.Left = MesView.MessageLeft;
            else fullMessage.Left = PanelWidth - MesView.MessageLeft - fullMessage.Width;

            System.Drawing.Drawing2D.GraphicsPath Mess_Path = new System.Drawing.Drawing2D.GraphicsPath();
            int r = 30;
            Mess_Path.AddPie(0, 0, 2 * r, 2 * r, 180, 90);
            Mess_Path.AddPie(0, fullMessage.Height - 2 * r, 2 * r, 2 * r, 90, 90);
            Mess_Path.AddPie(fullMessage.Width - 2 * r, 0, 2 * r, 2 * r, 270, 90);
            Mess_Path.AddPie(fullMessage.Width - 2 * r, fullMessage.Height - 2 * r, 2 * r, 2 * r, 0, 90);

            Mess_Path.AddRectangle(new Rectangle(r, 0, fullMessage.Width - 2 * r, fullMessage.Height));
            Mess_Path.AddRectangle(new Rectangle(0, r, r, fullMessage.Height - 2 * r));
            Mess_Path.AddRectangle(new Rectangle(fullMessage.Width - r, r, r, fullMessage.Height - 2 * r));

            fullMessage.Region = new Region(Mess_Path);
            fullMessage.Top = MesView.MessageTop;

            return fullMessage;
        }

        public static Panel createFileMessage(AddFile message, int PanelWidth, downloadClick click, bool isMine)
        {
            var MesView = new MessageView(PanelWidth * 9 / 10);

            var image = new Button();
            image.Text = "";
            image.Width = MesView.ImageWidth;
            image.Height = MesView.ImageHeight;
            image.Top = MesView.ImageTop;
            image.Region = MesView.ImageRegion;
            image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            image.BackgroundImage = View.getAvatar(message.getCreator().avatarNumber);


            if (!isMine) image.Left = MesView.ImageLeft;

            var fileName = new Label();
            fileName.Text = message.getName() + "\nsize: " + Convert.ToString(message.getSize()) + "\n" + message.getNewName();
            fileName.Font = new Font(fileName.Font.Name, 8.0f, FontStyle.Bold | FontStyle.Underline);
            fileName.ForeColor = Color.White;
            fileName.AutoSize = true;
            fileName.MaximumSize = new Size(PanelWidth * 7 / 10, 0);
            fileName.MinimumSize = new Size(MesView.TextWidth, MesView.TextHeight);
            fileName.Height = fileName.PreferredHeight;
            fileName.Width = fileName.PreferredWidth;
            fileName.Top = MesView.TextTop;
            fileName.Click += new EventHandler(click);
            fileName.Cursor = Cursors.Hand;

            if (!isMine)
                fileName.Left = MesView.TextLeft + 2 * MesView.ImageLeft + MesView.ImageWidth;
            else
            {
                fileName.Left = MesView.TextLeft;
                image.Left = MesView.ImageLeft + 2 * MesView.TextLeft + fileName.Width;
            }


            var name = new Label();
            name.Text = message.getCreator().Name + "#" + message.getCreator().HostName;
            name.Width = MesView.NameWidth;
            name.Left = fileName.Left;
            name.Top = fileName.Height + MesView.TextTop + MesView.NameTop;
            name.Height = MesView.NameHeight;

            var date = new Label();
            date.Text = message.getTime().ToString();
            date.Width = MesView.DateWidth;
            date.Left = name.Left + name.Width + MesView.DateLeft;
            date.Top = name.Top;
            date.Height = name.Height;


            var fullMessage = new Panel();

            fullMessage.Width = image.Width + fileName.Width + 2 * MesView.ImageLeft + 2 * MesView.TextLeft;
            fullMessage.Text = "";
            fullMessage.Height = Math.Max(MesView.ImageHeight + 2 * MesView.ImageTop, fileName.Height + MesView.TextTop + MesView.NameTop + name.Height);
            fullMessage.BorderStyle = BorderStyle.None;
            fullMessage.BackColor = Color.LightBlue;

            fullMessage.Controls.Add(image);
            fullMessage.Controls.Add(fileName);
            fullMessage.Controls.Add(name);
            fullMessage.Controls.Add(date);

            if (!isMine) fullMessage.Left = MesView.MessageLeft;
            else fullMessage.Left = PanelWidth - MesView.MessageLeft - fullMessage.Width;

            System.Drawing.Drawing2D.GraphicsPath Mess_Path = new System.Drawing.Drawing2D.GraphicsPath();
            int r = 30;
            Mess_Path.AddPie(0, 0, 2 * r, 2 * r, 180, 90);
            Mess_Path.AddPie(0, fullMessage.Height - 2 * r, 2 * r, 2 * r, 90, 90);
            Mess_Path.AddPie(fullMessage.Width - 2 * r, 0, 2 * r, 2 * r, 270, 90);
            Mess_Path.AddPie(fullMessage.Width - 2 * r, fullMessage.Height - 2 * r, 2 * r, 2 * r, 0, 90);

            Mess_Path.AddRectangle(new Rectangle(r, 0, fullMessage.Width - 2 * r, fullMessage.Height));
            Mess_Path.AddRectangle(new Rectangle(0, r, r, fullMessage.Height - 2 * r));
            Mess_Path.AddRectangle(new Rectangle(fullMessage.Width - r, r, r, fullMessage.Height - 2 * r));

            fullMessage.Region = new Region(Mess_Path);
            fullMessage.Top = MesView.MessageTop;

            return fullMessage;
        }

        public static Panel createContact(User user, int PanelWidht, contactClick click)
        {
            ContactsView ConView = new ContactsView(PanelWidht);
            
            Button ava = new Button();
            ava.Text = "";
            ava.Width = ConView.ImageWidth;
            ava.Height = ConView.ImageHeight;
            ava.Left = ConView.ImageLeft;
            ava.Top = ConView.ImageTop;
            ava.Region = ConView.ImageRegion;
            ava.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ava.BackgroundImage = View.getAvatar(user.avatarNumber);
            ava.Click += new EventHandler(click);

            Label name = new Label();
            name.Font = new Font(name.Font.Name, 8.0f, name.Font.Style);
            name.Text = user.Name + "#" + user.HostName;
            name.Left = 2 * ConView.ImageLeft + ava.Width;
            name.Top = ConView.LabelTop;
            name.Height = ConView.LabelHeight;
            name.Click += new EventHandler(click);

            Panel con = new Panel();
            con.Top = View.lastHeightContacts;
            con.Width = (int)(0.9 * PanelWidht);
            con.Text = "";
            con.Height = Math.Max(ConView.ImageHeight + 2 * ConView.ImageTop, ConView.LabelHeight + 2 * ConView.LabelTop);
            con.Controls.Add(ava);
            con.Controls.Add(name);
            con.BorderStyle = BorderStyle.None;
            con.Click += new EventHandler(click);

            View.lastHeightContacts += con.Height;
            return con;
        }

        public static void newMessage(Panel contact) { 
            var font = contact.Font;
            font = new Font(font.Name, font.Size, FontStyle.Bold | FontStyle.Underline);
            foreach (Control i in contact.Controls)
                i.Font = font;
            contact.Focus();
        }

        public static void newMessage(Button contact)
        {
            var font = contact.Font;
            contact.Font = new Font(font.Name, font.Size, FontStyle.Bold | FontStyle.Underline);
            contact.Focus();
        }

        public static void newMessageRead(Panel contact)
        {
            var font = contact.Font;
            font = new Font(font.Name, font.Size, FontStyle.Regular);
            foreach (Control i in contact.Controls)
                i.Font = font;
        }

        public static void newMessageRead(Button contact)
        {
            var font = contact.Font;
            contact.Font = new Font(font.Name, font.Size, FontStyle.Regular);
        }

        private static Image getAvatar(int number)
        {
            switch (number)
            {
                case 0:
                    {
                        return LocalChat.Properties.Resources.car;
                    }
                case 1:
                    {
                        return LocalChat.Properties.Resources.phone;
                    }
                case 2:
                    {
                        return LocalChat.Properties.Resources.beer;
                    }
                case 3:
                    {
                        return LocalChat.Properties.Resources.drag;
                    }
                case 4:
                    {
                        return LocalChat.Properties.Resources.flower;
                    }
                default:
                    {
                        return LocalChat.Properties.Resources.car;
                    }
            }
        }

        public static Panel createPersonalChat(Panel samlpe, string name)
        {
            var panel = new Panel();

            panel.AutoScroll = samlpe.AutoScroll;
            panel.Parent = samlpe.Parent;
            panel.Width = samlpe.Width;
            panel.Height = samlpe.Height;
            panel.Top = samlpe.Top;
            panel.Left = samlpe.Left;

            return panel;
        }
        public static void updateContact(Panel contact, User user)
        {
            foreach (var i in contact.Controls)
                if (i.GetType() == typeof(Button))
                    ((Button)i).BackgroundImage = View.getAvatar(user.avatarNumber);

        }

        public static void updateFileMessage(Panel panel)
        {
            foreach (var control in panel.Controls)
                if (control.GetType() == typeof(Label))
                {
                    string[] text = ((Label)control).Text.Split(new string[] { "\n" }, StringSplitOptions.None);

                    if (text.Length != 3)
                        continue;

                    int p = text[1].IndexOf(" preparation...");
                    if (p == -1)
                        text[1] = text[1] + " preparation...";

                    ((Label)control).Text = text[0];
                    for (int i = 1; i < text.Length; i++)
                        ((Label)control).Text += "\n" + text[i];

                    return;
                }
        }

        public static void updateFileMessage(Panel panel, double countDownload)
        {
            foreach (var control in panel.Controls)
                if( control.GetType() == typeof(Label) ){
                    string[] text = ((Label)control).Text.Split(new string[]{"\n"}, StringSplitOptions.None);

                    if (text.Length != 3)
                        continue;

                    int p = text[1].IndexOf(" preparation...");
                    if (p != -1)
                        text[1] = text[1].Substring(0, p);
                    
                    p = text[1].IndexOf(" download: ");
                    if (p == -1)
                        text[1] = text[1] + " download: " + (int)countDownload + "%";
                    else
                        text[1] = text[1].Substring(0, p) + " download: " + (int)countDownload + "%";

                    ((Label)control).Text = text[0];
                    for (int i = 1; i < text.Length; i++)
                        ((Label)control).Text += "\n" + text[i];
                    
                    return;
                }
        }

        public static void updateFileMessage(Panel panel, string error)
        {
            panel.BackColor = Color.Red;
            foreach (var control in panel.Controls)
                if (control.GetType() == typeof(Label))
                {
                    string[] text = ((Label)control).Text.Split(new string[] { "\n" }, StringSplitOptions.None);

                    if (text.Length != 3)
                        continue;

                    text[text.Length - 1] = error;

                    ((Label)control).Text = text[0];
                    for (int i = 1; i < text.Length; i++)
                        ((Label)control).Text += "\n" + text[i];

                    return;
                }
        }

        public static Label getInfa(Panel panel)
        {
            foreach (var control in panel.Controls)
                if (control.GetType() == typeof(Label))
                {
                    string[] text = ((Label)control).Text.Split(new string[] { "\n" }, StringSplitOptions.None);

                    if (text.Length != 3)
                        continue;

                    if (text[1].IndexOf("size: ") == -1)
                        continue;

                    if (text[2].IndexOf(text[0]) == -1)
                        continue;

                    return (Label)control;
                }
            return null;
        }
    }
}
