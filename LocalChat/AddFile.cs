using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LocalChat
{
    public class AddFile
    {
        private string Name;
        private string NewName;
        private string Error;
        private int BlockSize;
        private ulong CountBlock;
        private ulong Size;
        private DateTime Time;
        private User Creator;
        private User Addressee;
        public bool isPrivate;
        public bool isSave;
        public byte[] flagBlock;
        private Panel panel; 

        public AddFile(User creator, string name, ulong size, int blockSize, DateTime time, string newName = "")
        {
            this.Creator = creator;
            this.Addressee = null;
            this.isPrivate = false;
            this.isSave = false;
            this.Name = name;
            if (newName == "" || newName == null)
                this.NewName = name;
            else
                this.NewName = newName;
            this.BlockSize = blockSize;
            this.Size = size;
            
            if(time == null)
                this.Time = DateTime.Now;
            else
                this.Time = time;

            if (this.BlockSize != 0)
                this.CountBlock = this.Size / (ulong)this.BlockSize;
            else
                this.CountBlock = 0;
            if (this.CountBlock * (ulong)this.BlockSize < this.Size) 
                this.CountBlock++;

            this.flagBlock = new byte[this.CountBlock];
            for (ulong i = 0; i < this.CountBlock; i++)
                this.flagBlock[i] = 0;

            this.panel = null;
        }

        public AddFile(User creator, string name, ulong size, int blockSize, string newName = "")
            : this(creator, name, size, blockSize, DateTime.Now, newName)
        {
            
        }

        public string getName(){
            return this.Name;
        }

        public string getNewName()
        {
            return this.NewName;
        }

        public User getCreator()
        {
            return this.Creator;
        }

        public int getBlockSize()
        {
            return this.BlockSize;
        }

        public ulong getSize()
        {
            return this.Size;
        }

        public ulong getCountBlock()
        {
            return this.CountBlock;
        }

        public DateTime getTime()
        {
            return this.Time;
        }
        public Panel getPanel()
        {
            return this.panel;
        }

        public User getAddressee()
        {
            return this.Addressee;
        }

        public string getError()
        {
            return this.Error;
        }

        public void setAddressee(User addressee)
        {
            this.Addressee = addressee;
            if (addressee != null)
                this.isPrivate = true;
        }

        public void setPanel(Panel panel)
        {
            this.panel = panel;
        }

        public void setError(string error)
        {
            this.Error = error;
        }

        public bool Equals(AddFile i)
        {
            if (this.Name != i.Name) return false;
            if (this.Size != i.Size) return false;
            if (this.CountBlock != i.CountBlock) return false;

            return true;
        }

    }
}
