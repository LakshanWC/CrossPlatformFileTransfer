using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSoftware.Service_Layer
{
    public class HttpListenerService
    {
        private static HttpListener listener;
        private static bool isRunning;

        public static void StartHttpListener(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            listener = new HttpListener();
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }

            listener.Start();
            isRunning = true;
            Console.WriteLine("Listening...");

            // Start a new thread to handle requests continuously
            Task.Run(() => HandleRequests());
        }

        private static void HandleRequests()
        {
            try
            {
                while (isRunning)
                {
                    // GetContext blocks while waiting for a request
                    var context = listener.GetContext();

                    // Handle each request in a separate task to avoid blocking
                    Task.Run(() => ProcessRequest(context));
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (like if listener is stopped)
                Console.WriteLine($"Listener error: {ex.Message}");
            }
        }

        private static void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                if(context.Request.HttpMethod == "POST")
                {
                    string fileName = context.Request.Headers["X-Filename"];
                    if (string.IsNullOrEmpty(fileName))
                        fileName = "received_file_" + DateTime.Now.Ticks;

                    string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                    using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        context.Request.InputStream.CopyTo(fs);
                    }

                    string responseString = $"<html><body>File received: {fileName}</body></html>";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.ContentType = "text/html";

                    using (var output = context.Response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                    }

                    Console.WriteLine($"✅ File saved to: {savePath}");
                }
                else
                {
                    string responseString = "<html><body>Only POST supported</body></html>";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.ContentType = "text/html";

                    using (var output = context.Response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }

        public static void StopHttpListener()
        {
            isRunning = false;
            listener?.Stop();
            listener?.Close();
        }
    }
}
