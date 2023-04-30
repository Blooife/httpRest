using System;
using System.IO;
using System.Net;

using System.Net.Http;



        string serverURL = "http://localhost:"; 
        StreamWriter outStream;
        Console.WriteLine("Enter port:");
        string port = Console.ReadLine();
        serverURL += port + "/";
        var client = new HttpClient();
        
        while (true)
        {
            try
            {
                Console.WriteLine("\nВыберите запрос:");
                Console.WriteLine("1.Get 2.Put 3.Post 4.Delete 5.Copy 6.Move 0.Exit");

                switch (int.Parse(Console.ReadLine()))
                    {
                        case 1:
                        {
                            SendGet();
                            break;
                        }
                        case 2:
                        {
                            SendPut();
                            break;
                        }
                        case 3:
                        {
                            SendPost();
                            break;
                        }
                        case 4:
                        {
                            SendDelete();
                            break;
                        }
                        case 5:
                        {
                            SendCopy();
                            break;
                        }
                        case 6:
                        {
                            SendMove();
                            break;
                        }
                        case 0:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    }
            }
            catch (Exception e) 
            {
                    Console.WriteLine(e.Message);
            }
        }

        async void sendGetAsync()
        {
            Console.WriteLine("Enter filename");
            string filename = Console.ReadLine();
            string url = serverURL + filename;
            HttpMethod method = new HttpMethod("GET");
            var msg = new HttpRequestMessage(method, url);
            StringContent str = new StringContent(filename);
            HttpResponseMessage response =  await client.SendAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                var s = await response.Content.ReadAsStringAsync();
                Console.WriteLine(s);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
            
        }

        void SendGet()
        {
            Console.WriteLine("Enter filename to read");
            string filename = Console.ReadLine();
            string url = serverURL + filename;
            HttpMethod method = new HttpMethod("GET");
            var msg = new HttpRequestMessage(method, url);
            HttpResponseMessage response =  client.Send(msg);
            if (response.IsSuccessStatusCode)
            {
                StreamReader streamReader = new StreamReader(response.Content.ReadAsStream());
                Console.WriteLine(streamReader.ReadToEnd());
                streamReader.Close();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        void SendDelete()
        {
            Console.WriteLine("Enter filename to delete:");
            string filename = Console.ReadLine();
            string url = serverURL + filename;
            HttpMethod method = new HttpMethod("DELETE");
            var msg = new HttpRequestMessage(method, url);
            HttpResponseMessage response =  client.Send(msg);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        void SendPut()
        {
            Console.WriteLine("Enter filename to rewrite:");
            string filename = Console.ReadLine();
            string url = serverURL + filename;
            HttpMethod method = new HttpMethod("PUT");
            var msg = new HttpRequestMessage(method, url);
            Console.WriteLine("Enter new content of the file");
            string data = Console.ReadLine();
            msg.Content = new StringContent(data);
            HttpResponseMessage response =  client.Send(msg);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
        
        void SendPost()
        {
            Console.WriteLine("Enter filename to add:");
            string filename = Console.ReadLine();
            string url = serverURL + filename;
            HttpMethod method = new HttpMethod("POST");
            var msg = new HttpRequestMessage(method, url);
            Console.WriteLine("Enter content to add");
            Stream? inputStream = null;
            string data = Console.ReadLine();
            msg.Content = new StringContent(data);
            HttpResponseMessage response =  client.Send(msg);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        void SendCopy()
        {
            Console.WriteLine("Enter filename to copy:");
            string filename = Console.ReadLine();
            string url = serverURL + filename;
            HttpMethod method = new HttpMethod("COPY");
            var msg = new HttpRequestMessage(method, url);
            Console.WriteLine("Enter ditectory to copy");
            string data = Console.ReadLine();
            msg.Content = new StringContent(data);
            HttpResponseMessage response =  client.Send(msg);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
                StreamReader streamReader = new StreamReader(response.Content.ReadAsStream());
                Console.WriteLine(streamReader.ReadToEnd());
                streamReader.Close();
            }
            else
            {
                StreamReader streamReader = new StreamReader(response.Content.ReadAsStream());
                Console.WriteLine(streamReader.ReadToEnd());
                streamReader.Close();
            }

        }

        void SendMove()
        {
            Console.WriteLine("Enter filename to move:");
            string filename = Console.ReadLine();
            string url = serverURL + filename;
            HttpMethod method = new HttpMethod("MOVE");
            var msg = new HttpRequestMessage(method, url);
            Console.WriteLine("Enter ditectory to move");
            string data = Console.ReadLine();
            msg.Content = new StringContent(data);
            HttpResponseMessage response =  client.Send(msg);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
                StreamReader streamReader = new StreamReader(response.Content.ReadAsStream());
                Console.WriteLine(streamReader.ReadToEnd());
                streamReader.Close();
            }
            else
            {
                StreamReader streamReader = new StreamReader(response.Content.ReadAsStream());
                Console.WriteLine(streamReader.ReadToEnd());
                streamReader.Close();
            }

        }

    
