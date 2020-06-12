using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LocalChat
{
    public partial class Form1 : Form
    {
        int Avatar = -1;
        Chat NextSite;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1( Chat NextSite):this()
        {
            this.NextSite = NextSite;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath Button_Path = new System.Drawing.Drawing2D.GraphicsPath();
            Button_Path.AddEllipse(5, 5, this.button1.Width - 10, this.button1.Height - 10);
            Region Button_Region = new Region(Button_Path);

            foreach ( var i in ImageBox.Controls ){
                if (i.GetType() == typeof(Button)) ((Button)i).Region = Button_Region;
                if (i.GetType() == typeof(Label)) ((Label)i).Region = Button_Region;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            User me;
            string name = textBox1.Text;
            if (name.IndexOf('#') >= 0)
            {
                label2.Text = "# - not";
                return;
            }

            if (name.Length < 4)
            {
                label2.Text = "Name it small";
                return;
            }

            if (Storage.Initialize(name))
            {
                label2.Text = "1";
                me = Storage.getMe();
            }
            else
            {
                if (Avatar == -1)
                {
                    label2.Text = "Avatar not selected";
                    return;
                }

                me = new User(name, System.Net.Dns.GetHostName(), Avatar, null);
                me.crypt = new CrypterAES();
                Storage.SaveMe(me);
            }
            
            
            Server.me = me;
            NetworkTools.FindOtherServer();
            
            this.NextSite.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Avatar = 0;
            foreach (var i in ImageBox.Controls)
            {
                if (i.GetType() == typeof(Label))
                {
                    Label x = (Label) i;
                    if (x.Left == button1.Left) x.BackColor = Color.Green;
                    else x.BackColor = ImageBox.BackColor;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Avatar = 1;
            foreach (var i in ImageBox.Controls)
            {
                if (i.GetType() == typeof(Label))
                {
                    Label x = (Label)i;
                    if (x.Left == button5.Left) x.BackColor = Color.Green;
                    else x.BackColor = ImageBox.BackColor;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Avatar = 2;
            foreach (var i in ImageBox.Controls)
            {
                if (i.GetType() == typeof(Label))
                {
                    Label x = (Label)i;
                    if (x.Left == button2.Left) x.BackColor = Color.Green;
                    else x.BackColor = ImageBox.BackColor;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Avatar = 3;
            foreach (var i in ImageBox.Controls)
            {
                if (i.GetType() == typeof(Label))
                {
                    Label x = (Label)i;
                    if (x.Left == button3.Left) x.BackColor = Color.Green;
                    else x.BackColor = ImageBox.BackColor;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Avatar = 4;
            foreach (var i in ImageBox.Controls)
            {
                if (i.GetType() == typeof(Label))
                {
                    Label x = (Label)i;
                    if (x.Left == button4.Left) x.BackColor = Color.Green;
                    else x.BackColor = ImageBox.BackColor;
                }
            }
        }
    }
}
