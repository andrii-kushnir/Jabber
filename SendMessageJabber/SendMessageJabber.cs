using Artalk.Xmpp.Client;
using Artalk.Xmpp.Im;

namespace SendMessageJabber
{
    public class Send
    {
        private const string _server = "jabber.ars.ua";
        private const int _port = 5222;

        private const string _botName = "openfire";
        private const string _botPassword = "V01POpnFr444";


        public static int SendMessage(string contact, string message)
        {
            try
            {
                if (contact.Length < 14 || contact.Substring(contact.Length - 13) != _server)
                {
                    contact += "@" + _server;
                }
                var msg = new Message(contact, message, null, null, MessageType.Chat, null);
                using (var client = new ArtalkXmppClient(_server, _botName, _botPassword, _port, true))
                {
                    client.Connect();
                    client.SendMessage(msg);
                }
            }
            catch
            {
                return -1;
            }

            return 1;
        }
    }
}
