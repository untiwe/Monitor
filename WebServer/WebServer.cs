
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor.WebServer
{
    class WebServer
    {

        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;
        //private const string _filesFolder = "C:\\Users\\Михаил\\source\\repos\\Monitor\\WebServer\\";
        private static List<WebSocket> _listSockets = new List<WebSocket>();
        private const int _maxSockets = 5;
        private static int _startSocketsWith;
        private static string _IPAddressSockets;
        private static int _lastGetSocket = 0;
        private static string _PortServer;

        public WebServer(IReadOnlyCollection<string> prefixes, Func<HttpListenerRequest, string> method)
        {
            _PortServer = ConfigurationManager.AppSettings.Get("PortServer");
            _startSocketsWith = Convert.ToInt32(ConfigurationManager.AppSettings.Get("PortSockets"));
            _IPAddressSockets = ConfigurationManager.AppSettings.Get("IpServer");

            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            }

            foreach (var s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            _responderMethod = method;
            _listener.Start();

            foreach (int i in Enumerable.Range(0, _maxSockets))
            {
                _listSockets.Add(new WebSocket(_IPAddressSockets, _startSocketsWith + i));
                _listSockets[i].NewComandEvent += NewCmd;

            }

            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        foreach (WebSocket i in _listSockets)
                            i.ReceiveMessage();
                    }
                    catch (InvalidOperationException)
                    {//делаем перезапуск цикла, если мы поменяли состав коллекции
                        continue;
                    }
                }
            });
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
           : this(prefixes, method)
        {
        }

        public static void NewCmd(string s)
        {
            NewComandEvent?.Invoke(s);
        }

        public delegate void NewComand(string message);
        public static event NewComand NewComandEvent;

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                //Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if (ctx == null)
                                {
                                    return;
                                }

                                var rstr = _responderMethod(ctx.Request);
                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch
                            {
                                // ignored
                            }
                            finally
                            {
                                //always close the stream
                                if (ctx != null)
                                {
                                    ctx.Response.OutputStream.Close();
                                }
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
       
        public static string SendResponse(HttpListenerRequest request)
        {
            if (request.RawUrl == "/getsocket/")
            {
                WebSocket webSockerResponse = GetSocket();
                return $"{webSockerResponse.ip}:{webSockerResponse.port}";
            }

            if (request.RawUrl.Contains("/admin"))
                return File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "WebServer", "admin.html")).Replace("<<ServerIP>>", _IPAddressSockets + ":" + _PortServer);
            
            if (request.RawUrl.Contains("/jadge"))
            {
                if (request.QueryString[0] == "судья арены")
                    return "true";

                return "false";
            }

            if (request.RawUrl.Contains("/mainuser"))
            {
                if (request.QueryString[0] == "я тут главный")
                    return "true";

                return "false";
            }

            return File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "WebServer", "admin.html")).Replace("<<ServerIP>>", _IPAddressSockets + ":" + _PortServer);
        }

        public static WebSocket GetSocket()
        {


            //ищем свободные сокеты
            foreach (int i in Enumerable.Range(0, _listSockets.Count()))
            {
                if (_listSockets[i].stream == null || _listSockets[i].client == null || _listSockets[i].client.Connected == false)
                {
                    _listSockets[i].NewComandEvent -= NewCmd;//отписываемся от событияя старого объйкта
                    _listSockets[i] = new WebSocket(_IPAddressSockets, _startSocketsWith + i, _listSockets[i].server);
                    _listSockets[i].NewComandEvent += NewCmd;//подписываемся на событие нового
                    return _listSockets[i];
                }
            }

            //если не находим, берем из спика используемых
            if (_lastGetSocket == _maxSockets - 1)
                _lastGetSocket = 0;

            _listSockets[_lastGetSocket].NewComandEvent -= NewCmd;//отписываемся от событияя старого объйкта
            _listSockets[_lastGetSocket] = new WebSocket(_IPAddressSockets, _startSocketsWith + _lastGetSocket, _listSockets[_lastGetSocket].server);
            _listSockets[_lastGetSocket].NewComandEvent += NewCmd;//подписываемся на событие нового

            _lastGetSocket++;

            return _listSockets[_lastGetSocket];
        }

        public static void SendMessAllSockets(string message)
        {
            try
            {
                _listSockets.ForEach(s => s.ToSend(message));
            }
            catch (System.InvalidOperationException)
            {
                //если во время отправки сообщения, список сокетов обновился, просто игнорируем
            }
        }
    }
}
