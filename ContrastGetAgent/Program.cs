using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.IO;

namespace ContrastGetAgent
{
    class Program
    {
        public static String ProfileUID = "";
        public static String Authorization = "";
        public static String API_Key = "";

        static void Main(string[] args)
        {
            LoadConfig(args[0]);

            //Java client
            RestClient client = new RestClient(@"https://app.contrastsecurity.com/Contrast/api/ng/"+ProfileUID+@"/agents/default/java");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader(@"Accept", @"application/java-archive");
            request.AddHeader(@"API-Key", API_Key);
            request.AddHeader(@"Authorization", Authorization);
            byte[] data = client.DownloadData(request);
            ByteArrayToFile("contrast.jar", data);

            //dotnet client
            client = new RestClient(@"https://app.contrastsecurity.com/Contrast/api/ng/" + ProfileUID + @"/agents/default/dotnet");
            request = new RestRequest(Method.GET);
            request.AddHeader(@"Accept", @"application/java-archive");
            request.AddHeader(@"API-Key", API_Key);
            request.AddHeader(@"Authorization", Authorization);
            data = client.DownloadData(request);
            ByteArrayToFile("ContrastSetup_0.zip", data);

        }


        public static void LoadConfig(String fileName)
        {
            StreamReader sr = null;
            try {
                sr = new StreamReader(fileName);

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    if (line.ToLower().StartsWith("api-key"))
                    {
                        API_Key = line.Split('|')[1];
                    }
                    if (line.ToLower().StartsWith("authorization"))
                    {
                        Authorization = line.Split('|')[1];
                    }
                    if (line.ToLower().StartsWith("profile"))
                    {
                        ProfileUID = line.Split('|')[1];
                    }
                }

            }catch (Exception wtf)
            {
                Console.Out.WriteLine(wtf.Message + "\n" + wtf.StackTrace);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

        }

        public static bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }
    }
}
