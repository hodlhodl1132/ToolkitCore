using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using UnityEngine;
using Verse;

namespace rim_twitch
{
    public class TwitchWrapper : MonoBehaviour
    {
        public static TwitchClient Client { get; private set; }

        public void Initialize(ConnectionCredentials credentials)
        {
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);

            if (Client == null)
            {
                Client = new TwitchClient(customClient);
            }

            // Initialize the client with the credentials instance, and setting a default channel to connect to.
            Client.Initialize(credentials, RimTwitchSettings.channel_username);

            // Bind callbacks to events
            Client.OnConnected += OnConnected;
            Client.OnJoinedChannel += OnJoinedChannel;
            Client.OnMessageReceived += OnMessageReceived;

            Client.Connect();

            AutoResetEvent eventHandler = new AutoResetEvent(false);

            wait: eventHandler.WaitOne(250);

            if (ThreadWorker.stayConnected == false && Client.IsConnected)
            {
                Client.Disconnect();
            }
            else if (ThreadWorker.stayConnected == true && !Client.IsConnected)
            {
                Client.Connect();
            }

            if (ThreadWorker.sendDebugMSG)
            {
                ThreadWorker.sendDebugMSG = false;
                Client.SendMessage(Client.JoinedChannels.First(), "This is a test message");
            }

            if (MessageQueue.messageQueue.Count > 0)
            {
                MessageQueue.messageQueue.TryDequeue(out string messageToSend);
                Client.SendMessage(Client.GetJoinedChannel("hodlhodl"), messageToSend);
            }

            if (ThreadWorker.runThread)
            goto wait;

            Log.Message("Thread is ending");
        }

        private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            Log.Message($"The bot {e.BotUsername} succesfully connected to Twitch.");

            if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
                Log.Message($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
        }

        private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            Log.Message($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
            Client.SendMessage(e.Channel, "RimTwitch Client Connected");
        }

        private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            List<TwitchInterfaceBase> receivers = Current.Game.components.OfType<TwitchInterfaceBase>().ToList();

            foreach (TwitchInterfaceBase receiver in receivers)
            {
                receiver.ParseCommand(e.ChatMessage);
            }
        }
    }
}

