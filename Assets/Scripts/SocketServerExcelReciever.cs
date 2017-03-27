using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketServerExcelReciever : MonoBehaviour {
    private const string nextRow = "NextRow";
    private const string @break = "DoBreak";
    private static string data = null;
    private static IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
    private static IPAddress ipAddress = ipHostInfo.AddressList[0];
    private static IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
    private static byte[] bytes;
    private static List<List<string>> genericsRow;
    private static List<string> genericsInRow;
    private static List<string> categories;
    private static DataHandler dat;
    private static int[] expenseAr = new int[14];
    private static int[] incomeAr = new int[13];
    private static CategoryDatabase cbd;

    private static string genericPath;
    private static string budgetPath;
    public static IEnumerator StartListening()
    {
        Socket listener = new Socket(ipAddress.AddressFamily,
        SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        listener.Listen(10);
        #region Wait for generics
        while (true)
        {
            Socket handler = null;
            try
            {
                handler = listener.Accept();
                data = null;
            }catch { }
            while (data == null)
            {
                try
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                }catch { }
                yield return null;
            }
            try
            {
                if (data == nextRow)
                {
                    genericsRow.Add(genericsInRow);
                }
                else if (data == @break)
                {
                    byte[] b = Encoding.ASCII.GetBytes(data);
                    handler.Send(b);
                    break;
                }
                else
                {
                    genericsInRow.Add(data);
                }
                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
            }
            catch { }
        }
        yield return null;
        #endregion

        //#region Wait for Categories
        //while (true)
        //{
        //    Socket handler = listener.Accept();
        //    data = null;
        //    while (data == null)
        //    {
        //        bytes = new byte[1024];
        //        int bytesRec = handler.Receive(bytes);
        //        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        //        yield return null;
        //    }
        //    if (data == @break)
        //    {
        //        byte[] b = Encoding.ASCII.GetBytes(data);
        //        handler.Send(b);
        //        break;
        //    }
        //    else
        //    {
        //        dat.AddCategory(data);
        //    }
        //    byte[] msg = Encoding.ASCII.GetBytes(data);
        //    handler.Send(msg);
        //    yield return null;
        //}
        //#endregion

        //#region Wait for ExpenseData
        //int k = 0;
        //while (true)
        //{
        //    Socket handler = listener.Accept();
        //    data = null;
        //    while (data == null)
        //    {
        //        bytes = new byte[1024];
        //        int bytesRec = handler.Receive(bytes);
        //        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        //        yield return null;
        //    }
        //    byte[] msg = Encoding.ASCII.GetBytes(data);
        //    if (data == nextRow)
        //    {
        //        dat.setExpenseData(expenseAr);
        //    }
        //    else if (data == @break)
        //    {
        //        byte[] b = Encoding.ASCII.GetBytes(data);
        //        handler.Send(b);
        //        break;
        //    }
        //    else
        //    {
        //        if (k == 0)
        //            expenseAr[k] = cbd.doGenericLookup(data);
        //        else
        //            expenseAr[k] = int.Parse(data);
        //    }
        //    ++k;
        //    handler.Send(msg);
        //    yield return null;
        //}
        //#endregion

        //#region Wait for IncomeData
        //k = 0;
        //while (true)
        //{
        //    Socket handler = listener.Accept();
        //    data = null;
        //    while (data == null)
        //    {
        //        bytes = new byte[1024];
        //        int bytesRec = handler.Receive(bytes);
        //        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        //        yield return null;
        //    }
        //    byte[] msg = Encoding.ASCII.GetBytes(data);
        //    if (data == nextRow)
        //    {
        //        dat.setIncomeData(incomeAr);
        //    }
        //    else if (data == @break)
        //    {
        //        byte[] b = Encoding.ASCII.GetBytes(data);
        //        handler.Send(b);
        //        break;
        //    }
        //    else
        //    {
        //            incomeAr[k] = int.Parse(data);
        //    }
        //    ++k;
        //    handler.Send(msg);
        //    yield return null;
        //}
        //#endregion
        print("k");
    }
    private void Start()
    {
        StartCoroutine(StartListening());

    }
}
