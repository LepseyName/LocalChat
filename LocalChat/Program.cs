using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace LocalChat
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Chat CHAT =  new Chat();

            Thread thread = new Thread(new ParameterizedThreadStart(StartServer));
            thread.Start(CHAT);

            
             Application.Run(new Form1(CHAT));    // Normal
            //Application.Run( CHAT );                // Debug

            Server.Stop();
            thread.Abort();
            Application.Exit();
        }

        static void StartServer(object info)
        {
            Server.Start(31649, (Chat)info);
        }

        public static int FirstIndexOf(byte[] array, byte[] subArray)
        {
            if (array == null || subArray == null)
                throw new ArgumentNullException("Null");
            if (array.Length == 0 || subArray.Length == 0)
                return -1;
            if (array.Length < subArray.Length)
                return -1;
            bool isFinde = true;
            for (int i = 0; i < array.Length - subArray.Length + 1; i++)
            {
                isFinde = true;
                for (int j = 0; j < subArray.Length; j++)
                    if (array[i + j] != subArray[j])
                    {
                        isFinde = false;
                        break;
                    }
                if (isFinde) return i;
            }
            return -1;
        }

        public static byte[] SubBytes(byte[] array, int first = 0, int count = -1)
        {
            if (array == null)
                throw new ArgumentNullException("Null");
            if (array.Length == 0 || first < 0)
                throw new ArgumentException("Empity");
            if (count < 0)
                count = array.Length - first;

            byte[] subArray = new byte[count];
            for (int i = 0; i < count; i++)
                subArray[i] = array[first + i];

            return subArray;
        }

        public static byte[][] Split(byte[] array, byte[] separator, bool notEmpity = false)
        {
            if (array == null || separator == null)
                throw new ArgumentNullException("Null");
            if (array.Length == 0 || separator.Length == 0)
                return new byte[][] { };
            if (array.Length < separator.Length)
                return new byte[][]{};

            List<byte[]> list = new List<byte[]>();

            while (array.Length > 0)
            {
                int index = FirstIndexOf(array, separator);
                if (index < 0)
                {
                    if (notEmpity)
                    {
                        if (array.Length > 0) list.Add(array);
                    }
                    else
                        list.Add(array);
                    break;
                }
                else
                {
                    if (notEmpity)
                    {
                        byte[] b = SubBytes(array, 0, index);
                        if (b.Length > 0) list.Add(b);
                    }
                    else
                        list.Add(SubBytes(array, 0, index));

                    array = SubBytes(array, index + separator.Length);
                }
            }

            return list.ToArray();
        }

        public static byte[] ConcatByte(byte[] array1, byte[] array2, int count = -1){

            if (count < 0) count = array2.Length;
            if (count > array2.Length) count = array2.Length;
            byte[]  newArray = new byte[array1.Length + count];
            for (int i = 0; i < array1.Length; i++)
                newArray[i] = array1[i];

            for (int i = 0; i < count; i++)
                newArray[array1.Length + i] = array2[i];
            return newArray;
        }

        public static int HechBytes(byte[] array)
        {
            int hech = 14 * array.Length;

            for (int i = 0; i < array.Length; i++)
                hech += (i+1) * array[i]; 
            
            return hech;
        }
    }
}
