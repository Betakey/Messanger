﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiDLL
{
    public partial class ChatBox : UserControl
    {
        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public Color FriendMessageColor { get; set; }

        public Color YourMessageColor { get; set; }

        public Color DateBoxColor { get; set; }

        private int y;

        public string YourName { get; set; }

        private Dictionary<DateTime, List<ChatBoxEntry>> history;

        public ChatBox()
        {
            InitializeComponent();
            BackColor = BackgroundColor;
            ForeColor = ForegroundColor;
            history = new Dictionary<DateTime, List<ChatBoxEntry>>();
        }

        public void Clear()
        {
            y = 0;
            history.Clear();
            Controls.Clear();
            Invalidate();
        }

        public void AddChatMessage(Dictionary<DateTime, List<ChatBoxEntry>> contentNotOrdered, string yourName)
        {
            Dictionary<DateTime, List<ChatBoxEntry>> content = new Dictionary<DateTime, List<ChatBoxEntry>>();
            foreach (DateTime time in contentNotOrdered.Keys.OrderByDescending(d => d))
            {
                List<ChatBoxEntry> entries = contentNotOrdered[time];
                entries.Reverse();
                content.Add(time, entries);
            }
            foreach (DateTime time in content.Keys)
            {
                foreach (ChatBoxEntry entry in content[time])
                {
                    if (history.ContainsKey(time))
                    {
                        List<ChatBoxEntry> list = history[time];
                        list.Add(entry);
                        history[time] = list;
                        MessageBubble msgBubble = new MessageBubble(entry.Sender.ToLower().Equals(yourName.ToLower()) ? Color.Chartreuse : Color.CornflowerBlue, Color.Black, Font, entry.Message, entry.Time, (Width / 3) * 2);
                        msgBubble.Location = new Point(entry.Sender.ToLower().Equals(yourName.ToLower()) ? 10 : Width - 20 - msgBubble.Width, y);
                        msgBubble.Visible = true;
                        Controls.Add(msgBubble);
                        y += 10 + msgBubble.Height;
                    }
                    else
                    {
                        List<ChatBoxEntry> list = new List<ChatBoxEntry>();
                        list.Add(entry);
                        history.Add(time, list);
                        MessageBubble timeBubble = new MessageBubble(Color.Cyan, Color.DimGray, Font, time.ToString("dd.MM.yy"), time, Width / 3, false);
                        timeBubble.Location = new Point(Width / 2 - timeBubble.Width / 2, y);
                        timeBubble.Visible = true;
                        Controls.Add(timeBubble);
                        y += 10 + timeBubble.Height;
                        MessageBubble msgBubble = new MessageBubble(entry.Sender.ToLower().Equals(yourName.ToLower()) ? Color.Chartreuse : Color.CornflowerBlue, Color.Black, Font, entry.Message, entry.Time, (Width / 3) * 2);
                        msgBubble.Location = new Point(entry.Sender.ToLower().Equals(yourName.ToLower()) ? 10 : Width - 20 - msgBubble.Width, y);
                        msgBubble.Visible = true;
                        Controls.Add(msgBubble);
                        y += 10 + msgBubble.Height;
                    }
                }
            }
            
        }
    }
}
