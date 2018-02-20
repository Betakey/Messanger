﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using NetDLL;
using GuiDLL;
using NetDLL.Data;

namespace ChatClient
{
    public partial class ChatClientForm : Form
    {
        private Client client;
        private string name; 

        public ChatClientForm(string name, string friends)
        {
            this.name = name;
            TcpClient Tclient = new TcpClient();
            try
            {
                Tclient.Connect(Program.Config.AsString(IO.ConfigKey.ServerIP), 34563);
            }
            catch
            {
               MessageBox.Show("Connection to server failed!");
               Close();
            }
            client = new Client(Tclient, this, name);
            InitializeComponent();
            sendButton.TabStop = false;
            sendButton.FlatStyle = FlatStyle.Flat;
            sendButton.FlatAppearance.BorderSize = 0;
            List<FriendEntry> entries = new List<FriendEntry>();
            foreach (string friend in friends.Split(';'))
            {
                entries.Add(new FriendEntry(friend));
            }
            friendsList.Update(entries);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            client.Write(new PacketSendText(inputRichTextbox.Text, "Receiver1"));
            inputRichTextbox.Clear();
        }

        /// <summary>
        /// Checks the type of a Packet.
        /// </summary>
        /// <param name="packet"></param>
        public void PacketHandler(Packet packet)
        {
            if (packet is PacketSendID)
            {
                client.ID = (packet as PacketSendID).ID;
                client.Write(new PacketSendName(client.Name));
            }
            else if (packet is PacketSendHistory)
            {
                Dictionary<DateTime, List<MessageData>> dict = ((PacketSendHistory)packet).History;
                Dictionary<DateTime, List<ChatBoxEntry>> convertedDict = new Dictionary<DateTime, List<ChatBoxEntry>>();
                foreach (DateTime time in dict.Keys)
                {
                    List<ChatBoxEntry> entries = new List<ChatBoxEntry>();
                    foreach (MessageData data in dict[time])
                    {
                        entries.Add(new ChatBoxEntry(data.FriendName, data.Message, data.Time));
                    }
                    convertedDict.Add(time, entries);
                }
                if (chatBox.InvokeRequired)
                {
                    MethodInvoker invoker = delegate
                    {
                        chatBox.AddChatMessage(convertedDict, name);
                    };
                    chatBox.Invoke(invoker);
                } 
            }
        }

        private void ChatClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void friendsList_BubbleClick(object sender, MouseEventArgs e, FriendEntry entry)
        {
            // TODO Search for open Message Server
        }
    }
}
