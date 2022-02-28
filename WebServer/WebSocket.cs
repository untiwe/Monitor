using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Monitor.WebServer
{
    class WebSocket
    {
        public string ip;
        public int port;
        public TcpClient client;
        public TcpListener server;
        private string swkaSha1Base64 = string.Empty;
        private byte[] response = new byte[0];
        public NetworkStream stream;

        public WebSocket(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            if (server == null)
            {
                server = new TcpListener(System.Net.IPAddress.Parse(ip), port);
                server.Start();
            }
            Task.Run(() =>  Start() );
        }

        public WebSocket(string ip, int port, TcpListener server)
        {
            this.ip = ip;
            this.port = port;
            this.server = server;
            Task.Run(() => Start());
        }

        public async void Start()
        {

           

            client = await server.AcceptTcpClientAsync();
            //Console.WriteLine("A client connected.");

            stream = client.GetStream();
            // enter to an infinite cycle to be able to handle every change in stream




        }

        public void ReceiveMessage()
        {

            if (client == null || !client.Connected || stream == null)
            {
                return;
            }

           

            if (!stream.DataAvailable && client.Available < 3)
                return;

            byte[] bytes = new byte[client.Available];
            stream.Read(bytes, 0, client.Available);
            string s = Encoding.UTF8.GetString(bytes);

            if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase))
            {
                //Console.WriteLine("=====Handshaking from client=====\n{0}", s);

                // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
                // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
                // 3. Compute SHA-1 and Base64 hash of the new value
                // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
                string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
                swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

                // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
                response = Encoding.UTF8.GetBytes(
                    "HTTP/1.1 101 Switching Protocols\r\n" +
                    "Connection: Upgrade\r\n" +
                    "Upgrade: websocket\r\n" +
                    "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");
                stream.Write(response, 0, response.Length);
            }

            else
            {
                bool fin = (bytes[0] & 0b10000000) != 0,
                    mask = (bytes[1] & 0b10000000) != 0; // must be true, "All messages from the client to the server have this bit set"

                if (bytes[0] == 0x88)
                {
                    //Console.WriteLine("соединение закрыто");

                }

                int opcode = bytes[0] & 0b00001111, // expecting 1 - text message
                    msglen = bytes[1] - 128, // & 0111 1111
                    offset = 2;

                if (msglen == 126)
                {
                    // was ToUInt16(bytes, offset) but the result is incorrect
                    msglen = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
                    offset = 4;
                }
                else if (msglen == 127)
                {
                    //Console.WriteLine("TODO: msglen == 127, needs qword to store msglen");
                    // i don't really know the byte order, please edit this
                    // msglen = BitConverter.ToUInt64(new byte[] { bytes[5], bytes[4], bytes[3], bytes[2], bytes[9], bytes[8], bytes[7], bytes[6] }, 0);
                    // offset = 10;
                }

                if (msglen == 0)
                {
                    //Console.WriteLine("msglen == 0");
                }
                else if (mask)
                {
                    byte[] decoded = new byte[msglen];
                    byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
                    offset += 4;


                    for (int i = 0; i < msglen; ++i)
                        decoded[i] = (byte)(bytes[offset + i] ^ masks[i % 4]);

                    string text = Encoding.UTF8.GetString(decoded);


                    //ToSend(text); //TestEcho
                    NewComandEvent?.Invoke(text);
                    //Console.WriteLine("{0}", text);
                }
                //else
                    //Console.WriteLine("mask bit not set");

                    //Console.WriteLine();
            }

        }

        public delegate void NewComand(string message);
        public event NewComand NewComandEvent;

        public void ToSend(string inputText)
        {
            //если нет соединения, ничего не отправляем
            if (stream == null)
                return;

            byte[] sendBytes = Encoding.UTF8.GetBytes(inputText);
            byte lengthHeader = 0;
            byte[] lengthCount = new byte[] { };

            if (sendBytes.Length <= 125)
                lengthHeader = (byte)sendBytes.Length;

            if (125 < sendBytes.Length && sendBytes.Length < 65535) //System.UInt16
            {
                lengthHeader = 126;

                lengthCount = new byte[] {
                    (byte)(sendBytes.Length >> 8),
                    (byte)(sendBytes.Length)
                };
            }

            if (sendBytes.Length > 65535)//max 2_147_483_647 but .Length -> System.Int32
            {
                lengthHeader = 127;
                lengthCount = new byte[] {
                    (byte)(sendBytes.Length >> 56),
                    (byte)(sendBytes.Length >> 48),
                    (byte)(sendBytes.Length >> 40),
                    (byte)(sendBytes.Length >> 32),
                    (byte)(sendBytes.Length >> 24),
                    (byte)(sendBytes.Length >> 16),
                    (byte)(sendBytes.Length >> 8),
                    (byte)sendBytes.Length,
                };
            }

            List<byte> responseArray = new List<byte>() { 0b10000001 };

            responseArray.Add(lengthHeader);
            responseArray.AddRange(lengthCount);
            responseArray.AddRange(sendBytes);
            try
            {
                stream.Write(responseArray.ToArray(), 0, responseArray.Count);
            }
            catch (System.IO.IOException) { }//ловим исклоючение, когда соединение разрывается в момент передачи
            }
    }
}
