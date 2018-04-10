using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwinCAT.Ads;
using System.IO;
namespace ADS
{
    class Program
    {
        static void Main(string[] args)
        {

            TcAdsClient tcClient = new TcAdsClient();
            AdsStream dataStream = new AdsStream(4);
            AdsBinaryReader binReader = new AdsBinaryReader(dataStream);

            int handle = 0;
            int iValue = 0;
            int exValue = 0;
            string variable, adres;

            Console.WriteLine("Podaj adres serwera ADS: ");
            adres = Console.ReadLine();
            Console.WriteLine("Podaj  nazwe zmienna do zapysywania w bazie danych: ");
            variable = Console.ReadLine();
            FileStream File = new FileStream("data.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(File);
            writer.WriteLine(adres + "  801  " + variable);

            writer.Close();
            try
            {

                tcClient.Connect(adres, 801);

                handle = tcClient.CreateVariableHandle(variable);

                Console.WriteLine("Czekam na znak");

                do
                {
                    tcClient.Read(handle, dataStream);
                    iValue = binReader.ReadInt32();
                    dataStream.Position = 0;
                    if (iValue != exValue)
                        writer.WriteLine(iValue);

                    Console.WriteLine("Aktualna wartosc wynosi: " + iValue);

                    exValue = iValue;
                } while (Console.ReadKey().Key.Equals(ConsoleKey.Enter));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " xdddd");

            }
            finally
            {
             
               tcClient.DeleteVariableHandle(handle);
                 tcClient.Dispose();
            }
            Console.ReadKey();
        }
    }
}
