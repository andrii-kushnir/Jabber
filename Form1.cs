using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using agsXMPP;
using agsXMPP.protocol.client;
using Message = agsXMPP.protocol.client.Message;
using agsXMPP.Collections;
using agsXMPP.protocol.iq.roster;
using System.Threading;
using agsXMPP.Xml.Dom;

namespace Jabber
{
    public partial class Form1 : Form
    {
        private static bool _wait;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string JID_Sender = "1443@jabber.ars.ua";
            string Password = "123456";
            Jid jidSender = new Jid(JID_Sender);
            XmppClientConnection xmpp1 = new XmppClientConnection(jidSender.Server)
            {
                UseStartTLS = false,
                UseSSL = true,
                ConnectServer = "jabber.ars.ua",
                Server = "jabber.ars.ua",
                UseCompression = true
            };

            xmpp1.Open(jidSender.User, Password);
            xmpp1.OnLogin += new ObjectHandler(xmpp_OnLogin);
            xmpp1.OnAuthError += delegate (object sender, Element e)
            {
                MessageBox.Show("Помилка аунтифікації!!!!!!!!!!");
            };

            int i = 0;
            _wait = true;
            do
            {
                MessageBox.Show(xmpp1.Authenticated.ToString());
                i++;
                if (i == 10)
                    _wait = false;
                Thread.Sleep(500);
            } while (_wait);

            return;
            SendJabberAlert("Тест з С#", "1002@jabber.ars.ua");
            return;

            agsXMPP.XmppClientConnection xmpp = new XmppClientConnection();
            xmpp.Status = "available";
            xmpp.Show = ShowType.chat;
            xmpp.Priority = 1;
            xmpp.SendMyPresence();
            xmpp.Port = 5222;
            xmpp.Server = "jabber.ars.ua"; //Сервер, н-р jabber.ru
            xmpp.Username = "1443"; //Логин
            xmpp.Password = "123456"; //Пароль
            xmpp.Resource = "ZennoBot";

            xmpp.UseSSL = false;
            xmpp.UseStartTLS = false;
            xmpp.UseCompression = false;

            xmpp.Open();
            agsXMPP.Jid JID = new Jid("1002@jabber.ars.ua"); //Кому отправляем, можно самому себе
            agsXMPP.protocol.client.Message msg = new agsXMPP.protocol.client.Message();
            msg.Type = agsXMPP.protocol.client.MessageType.chat;
            msg.To = JID;
            msg.Body = "Уведомление из ZennoPoster!"; //Текст уведомления
            xmpp.OnLogin += delegate (object o) 
            {
                MessageBox.Show("Є підключення!!!!!!!!!!");
                xmpp.Send(msg); 
            };
            agsXMPP.protocol.server.Presence presencia2 = new agsXMPP.protocol.server.Presence();
            presencia2.Type = new PresenceType();
        }

        static void xmpp_OnLogin(object sender)
        {
            _wait = false;
            MessageBox.Show("Є підключення!!!!!!!!!!");
        }

        public static async void SendJabberAlert(string msg, string receivers = null)
        {
            int waitMs = 3000; //Пауза после отправки, чтобы успело дойти сообщение
            int maxWaitMinutes = 2; // Максимальное время ожидания отправки в минутах

            //Данные отправителя 
            const string JID_SENDER = "1443@jabber.ars.ua";
            const string PASSWORD = "123456";

            //Получатели по умолчанию 
            if (receivers == null) receivers = "coder@jabber.ru, client@jabber.ru";
            string[] arrReceivers = receivers.Split(',').Select(a => a.Trim()).ToArray();

            // Чтобы не ждать пока отправка произойдет дергаем ее в отдельном таске
            await Task.Run(() =>
            {
                bool sended = false; // Отправили или нет
                DateTime endTime = DateTime.Now.AddMinutes(maxWaitMinutes); // Время когда прекращаем ожидать отправки, даже если не отправили сообщение

                Jid jidSender = new Jid(JID_SENDER);
                XmppClientConnection xmpp = new XmppClientConnection(jidSender.Server);
                xmpp.Open(jidSender.User, PASSWORD);
                xmpp.OnLogin += delegate (object o)
                {
                    MessageBox.Show("Є підключення!!!!!!!!!!");
                    foreach (var receiver in arrReceivers)
                    {
                        MessageBox.Show("Є підключення!!!!!!!!!!");
                        xmpp.Send(new Message(new Jid(receiver), MessageType.chat, msg));
                    }
                    sended = true;
                };

                do
                    new System.Threading.ManualResetEvent(false).WaitOne(waitMs);
                while (!sended && DateTime.Compare(DateTime.Now, endTime) < 0);
                xmpp.Close();
            });
        }
    }
}
