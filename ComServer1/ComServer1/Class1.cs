using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ComServer
{
    //[ComVisible(true)] // Это обязательно.
    [Guid("4945B34B-1B63-4a58-B5FE-9627FEFAEA9D")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ICom
    {
        void openInExel(long time, long count, long size, long[] deltaTime, long[] downLoad);
    }

    //[ComVisible(true)] // Это обязательно.
    [Guid("36E6BC94-308C-4952-84E6-109041990EF7")]
    //[ProgId ( "ComServer.ComServer")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ComServer : ICom
    {
        public ComServer()
        {
        }

        ~ComServer()
        {
        }

        public void openInExel(long time, long count, long size, long[] deltaTime, long[]downLoad)
        {
            
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Excel//Excel



            //Количество листов в рабочей книге
            xlApp.SheetsInNewWorkbook = 2;
            //Добавить рабочую книгу
            Microsoft.Office.Interop.Excel.Workbook xlWB = xlApp.Workbooks.Add(Type.Missing);
            //Отключить отображение окон с сообщениями
            xlApp.DisplayAlerts = false;
            //Получаем первый лист документа (счет начинается с 1)
            Microsoft.Office.Interop.Excel.Worksheet xlSht = (Microsoft.Office.Interop.Excel.Worksheet)xlApp.Worksheets.get_Item(1);



            //xlWB = xlApp.Workbooks.Open("download.xlsx"); //название файла Excel  
            //xlSht = xlWB.ActiveSheet; //или xlWB.Worksheets[1]; // или xlSht = xlWB.Worksheets["Лист1"];

            xlSht.Cells[1, 1] = "countBlock";
            xlSht.Cells[1, 2] = count;

            xlSht.Cells[2, 1] = "blockSize";
            xlSht.Cells[2, 2] = size;

            xlSht.Cells[3, 1] = "time"; xlSht.Cells[3, 2] = "%"; xlSht.Cells[3, 3] = "countBit"; xlSht.Cells[3, 4] = "instantSpeed"; xlSht.Cells[3, 5] = "averageSpeed";
            for (int i = 0; i < deltaTime.Length; i++)
            {
                double delta = (deltaTime[i] - time) / (double)TimeSpan.TicksPerSecond;
                double downloadBit = (count * size * downLoad[i] / 100.0),
                        instantSpeed = i > 0 ? ((count * size / 100.0) / ((deltaTime[i] - deltaTime[i - 1]) / (double)TimeSpan.TicksPerSecond)) : downloadBit / delta;
                xlSht.Cells[4 + i, 1] = delta; xlSht.Cells[4 + i, 2] = downLoad[i]; xlSht.Cells[4 + i, 3] = downloadBit; xlSht.Cells[4 + i, 4] = instantSpeed; xlSht.Cells[4 + i, 5] = downloadBit / delta;
            }

            xlApp.Visible = true;
            //xlApp.Application.ActiveWorkbook.SaveAs("doc.xlsx", Type.Missing,
            //  Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
            //  Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
    }
}
