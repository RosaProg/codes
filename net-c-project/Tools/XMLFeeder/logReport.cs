using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProXmlFeeder
{
    public class logReport
    {
        public static string LogDir;

        public string logTxt;
        public static void loadDirectory(string lDirectory){
            LogDir = lDirectory;
        }

        public static void returnError(string error)
        {
            try
            {
                using (StreamWriter w = File.AppendText(LogDir))
                {
                    Log(error, w);
                }

                using (StreamReader r = File.OpenText(LogDir))
                {
                    DumpLog(r);
                }
            }catch(Exception exc){
                Form1.Print("Log.txt is not possible to create");
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\n Log Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
