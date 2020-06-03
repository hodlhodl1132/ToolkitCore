using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Communication.Events;
using Verse;

namespace ToolkitCore.Models.Mixer
{
    public class MixerWebSocketClient
    {
        public string Url { get; set; } = string.Empty;
        private string AuthKey { get; set; }

        private ClientWebSocket _ws;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Queue<string> _sendQueue = new Queue<string>();
        public bool IsConnected => _ws?.State == WebSocketState.Open;

        #region Events
        public event EventHandler<OnMessageEventArgs> OnMessage;
        #endregion

        private void InitializeClient()
        {
            _ws = new ClientWebSocket();

            _ws.Options.SetRequestHeader("authorization", AuthKey);
            _ws.Options.SetRequestHeader("x-is-bot", "true");
        }

        public void SetAuthKey(string authKey)
        {
            this.AuthKey = authKey;
        }

        public bool StartMixerServer()
        {
            if (Open())
            {
                return true;
            }
            else
            {
                Log.Error("Failed to open Mixer Websocket");
                return false;
            }    
        }

        private bool Open()
        {
            try
            {
                if (IsConnected) return true;

                InitializeClient();
                _ws.ConnectAsync(new Uri(Url), _tokenSource.Token).Wait(15000);
                if (!IsConnected) return Open();

                StartListener();
                StartSenderTask();

                Task.Run(() =>
                {
                    while (_ws.State != WebSocketState.Open)
                    {

                    }
                }).Wait(15000);

                return _ws.State == WebSocketState.Open;
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                InitializeClient();
                throw;
            }
        }

        public void Close(bool callDisconnect = true)
        {
            _ws?.Abort();
            CleanupServices();
            InitializeClient();
        }

        private Task StartListener()
        {
           return Task.Run(async () =>
           {
               var message = "";

               while  (IsConnected)
               {
                   WebSocketReceiveResult result;
                   var buffer = new byte[1024];

                   try
                   {
                       result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _tokenSource.Token);
                   }
                   catch (Exception e)
                   {
                       InitializeClient();
                       break;
                   }

                   if (result == null) continue;

                   switch (result.MessageType)
                   {
                       case WebSocketMessageType.Close:
                           Close();
                           break;
                       case WebSocketMessageType.Text when !result.EndOfMessage:
                           message += Encoding.UTF8.GetString(buffer);
                           continue;
                       case WebSocketMessageType.Text:
                           message += Encoding.UTF8.GetString(buffer);
                           OnMessage?.Invoke(this, new OnMessageEventArgs() { Message = message });
                           break;
                       case WebSocketMessageType.Binary:
                           break;
                       default:
                           throw new ArgumentOutOfRangeException();
                   }

                   message = "";
               }
           });
        }

        public bool Send(string message)
        {
            try
            {
                if (!IsConnected) return false;

                Log.Message($"Sending: {message}");

                _sendQueue.Enqueue(message);

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

                return false;
            }
        }

        public Task StartSenderTask()
        {
           return Task.Run(async () =>
           {
               while (IsConnected)
               {
                   await Task.Delay(1000);

                   try
                   {
                       if (_sendQueue.TryDequeue(out string result))
                       {
                           await SendAsync(Encoding.UTF8.GetBytes(result));
                       }
                   }
                   catch (Exception e)
                   {
                       Log.Error(e.Message);
                   }
               }
           });
        }

        public Task SendAsync(byte[] message)
        {
            return _ws.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, _tokenSource.Token);
        }

        private void CleanupServices()
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
        }
    }
}
