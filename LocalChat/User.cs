using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LocalChat
{
    public class User
    {
        public string Name;
        public string HostName;
        public int avatarNumber;
        public Client client;
        public Panel contact;
        public Panel message;
        public int lastMessageY;
        public CrypterAES crypt;        

        public User(string Name, String HostName, int avatar, Client client)
        {
            this.Name = Name;
            this.HostName = HostName;
            this.avatarNumber = avatar;
            this.client = client;
            this.contact = null;
            this.message = null;
            this.lastMessageY = 0;
            this.crypt = null;
        }

        public User()
        {
            throw new Exception("empity user");
            this.Name = null;
            this.HostName = null;
            this.avatarNumber = 0;
            this.client = null;
            this.contact = null;
            this.message = null;
            this.lastMessageY = 0;
            this.crypt = null;
        }

        public void setClient(Client i){
            this.client = i;
        }

        public void update(int avatar)
        {
            this.avatarNumber = avatar;
        }

        public bool Equals(User i)
        {
            if (this.Name != i.Name) return false;
            if (this.HostName != i.HostName) return false;

            return true;
        }

        public void update(User u)
        {
            this.avatarNumber = u.avatarNumber;
            this.client = u.client;
            this.crypt = null;       
        }

        public string ToString(){
            string s = this.Name + "#" + this.HostName + "\nava: " + this.avatarNumber + "\nc: " + (this.contact == null ? "null" : this.contact.ToString()) + "\nm:";
            s += (this.message == null ? "null" : this.message.ToString());
            return s;
        }

    }
}
