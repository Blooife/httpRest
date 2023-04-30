using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace HttpServer
{
    public class Server
    {
        private static string IP_ADDRESS = "";
        private static string dir = "C:/Users/Asus/RiderProjects/httpS/httpS/fileStorage/";
        private static int status = 200;
        private static byte[] response = new byte[0];

        public static void Main(string[] args)
        {
            bool ok = false;
            while (!ok)
            {
                try
                {
                    Console.WriteLine("Enter port number");
                    int port = Int32.Parse(Console.ReadLine());
                    HttpListener server = new HttpListener();
                    IP_ADDRESS = "http://localhost:" + port + "/";
                    server.Prefixes.Add(IP_ADDRESS);
                    ok = true;
                    server.Start();
                    Console.WriteLine("Server started");
                    while (true)
                    {
                       // var context = await server.GetContextAsync();
                        HttpListenerContext context = server.GetContext();
                        Console.WriteLine(context.Request.Url);
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;
                        
                        var hand = new MyHandler();
                        hand.Handle(context);
                    }
                    server.Close();
                    server.Stop();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Wrong input");
                }
                catch (HttpListenerException)
                {
                    Console.WriteLine("Port is already in use");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public class MyHandler
        {
            public void Handle(HttpListenerContext context)
            {
                string requestURI = context.Request.Url.ToString();
                Console.WriteLine(context.Request.HttpMethod + " " + requestURI + " " +
                                  context.Request.ProtocolVersion);
                var headers = context.Request.Headers;
                foreach (string key in headers.Keys)
                {
                    Console.WriteLine(key + " : " + headers[key]);
                }

                //string f = context.Request.
                string filename = requestURI.Substring(IP_ADDRESS.Length);
                Console.WriteLine(filename);
                try
                {
                    switch (context.Request.HttpMethod)
                    {

                        // чтение файла
                        case "GET":
                            doGet(filename);
                            break;

                        // добавление в конец файла
                        case "POST":
                            doPost(context.Request, filename);
                            break;

                        // перезапись файла
                        case "PUT":
                            doPut(context.Request, filename);
                            break;

                        // удаление файла
                        case "DELETE":
                            doDelete(filename);
                            break;

                        // перемещение файла
                        case "MOVE":
                            doMove(context, filename);
                            break;

                        // копирование файла
                        case "COPY":
                            doCopy(context, filename);
                            break;

                        default:
                            context.Response.StatusCode = 405;
                            break;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    context.Response.StatusCode = 404;
                }
                catch (IOException ex)
                {
                    context.Response.StatusCode = 400;
                }
                context.Response.ContentLength64 = response.Length;
                context.Response.OutputStream.Write(response, 0, response.Length);
                context.Response.OutputStream.Close();
                Array.Resize(ref response,0);
            }

            private static void doGet(string filename)
            {
                response = File.ReadAllBytes(dir + filename);
            }

            private static void doPut(HttpListenerRequest request, string filename)
            {
                
                Stream s = request.InputStream;
                FileStream fileStream = new FileStream(dir+filename, FileMode.Truncate, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(fileStream);
                byte[] buffer = new byte[1024];
                int bytesRead = s.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    writer.Write(buffer, 0, bytesRead);
                    bytesRead = s.Read(buffer, 0, buffer.Length);
                }
                writer.Close();
                fileStream.Close();
                s.Close();
            }
            
            private static void doPost(HttpListenerRequest request, string filename)
            {
                Stream s = request.InputStream;
                StreamReader sr = new StreamReader(s);
                string str = sr.ReadToEnd();
                if (File.Exists(dir + filename))
                {
                    File.AppendAllText(dir+filename, str);
                }
                else
                {
                   // File.Create(dir + filename);
                    File.WriteAllText(dir+filename,str);
                }
                sr.Close();
                s.Close();
            }


            private static void doMove(HttpListenerContext context, string filename)
            {
                Stream s = context.Request.InputStream;
                StreamReader sr = new StreamReader(s);
                string newPath = sr.ReadToEnd();
                newPath += filename.Substring(filename.LastIndexOf('/')+1);
                if (File.Exists(dir+newPath))
                {
                    response = Encoding.Default.GetBytes("File with such name is already exists");
                } else
                if (!File.Exists(dir + filename))
                {
                    response = Encoding.Default.GetBytes("There is no such file you want to move");
                }
                else
                {
                    File.Copy(dir+filename, dir+newPath);
                    File.Delete(dir+filename);  
                }
            }

            private static void doDelete(string filename)
            {
                try
                {
                    File.Delete(Path.Combine(dir, filename));
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex);
                }
                
            }
           
            private static void doCopy(HttpListenerContext context, string filename)
            {
                Stream s = context.Request.InputStream;
                StreamReader sr = new StreamReader(s);
                string newPath = sr.ReadToEnd();
                newPath += filename.Substring(filename.LastIndexOf('/')+1);
                if (File.Exists(dir+newPath))
                {
                    response = Encoding.Default.GetBytes("File with such name is already exists");
                } else
                if (!File.Exists(dir + filename))
                {
                    response = Encoding.Default.GetBytes("There is no such file you want to copy");
                }else
                    File.Copy(dir+filename, dir+newPath);
            }
        }
    }
}