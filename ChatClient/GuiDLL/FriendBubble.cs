﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GuiDLL
{
    internal class FriendBubble : Panel
    {
        public FriendEntry Entry { get; private set; }

        private bool notifyEnabled;
        private Label nameLabel;

        public bool NotifyEnabled
        {
            get
            {
                return notifyEnabled;
            }
            set
            {
                notifyEnabled = value;
                Invalidate();
            }
        }

        public FriendBubble(FriendEntry entry)
        {
            Entry = entry;
            nameLabel = new Label();
            nameLabel.Text = entry.Name;
            nameLabel.AutoSize = false;
            nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            nameLabel.Location = new Point(Height - 5, 0);
            nameLabel.Size = new Size(Width - Height - 5, Height);
            BackColor = Color.White;
            nameLabel.Font = new System.Drawing.Font(nameLabel.Font.FontFamily, 22, FontStyle.Bold);
            Controls.Add(nameLabel);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.DrawImage(Entry.Image, 5, 5, Height - 10, Height - 10);
            g.DrawString(Entry.Name, Font, Brushes.Black, new Point(Width - (Height - 10) / 2 + (Height - 10) - (int)g.MeasureString(Entry.Name, Font).Width / 2, Height / 2 - (int)g.MeasureString(Entry.Name, Font).Height / 2));
            if (notifyEnabled)
            {
                g.DrawImage(Properties.Resources.exclamation_mark_PNG52, Width - 12, 2, 10, 10);
            }
        }
    }
}
