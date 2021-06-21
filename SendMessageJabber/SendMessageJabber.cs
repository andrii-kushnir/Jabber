using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artalk.Xmpp.Client;
using Artalk.Xmpp.Im;

namespace SendMessageJabber
{
    public class Send
    {
        private const string _server = "jabber.ars.ua";
        private const string _port = "5222";

        private const string _botName = "openfire";
        private const string _botPassword = "V01POpnFr444";


        public static int SendMessage(string contact, string message)
        {
            var client = new ArtalkXmppClient(_server, _botName, _botPassword);
            //client = new ArtalkXmppClient("jabber.ars.ua", "openfire", "V01POpnFr444");

            //client.Message += OnNewMessage;
            //client.Connect();


            using (var cl = new ArtalkXmppClient("jabber.ars.ua", "1443", "123456"))
            {
                cl.Connect();

            }

            client.SendMessage(contact, message, null, null, MessageType.Chat, null);

            return 1;

        }

    }
}
