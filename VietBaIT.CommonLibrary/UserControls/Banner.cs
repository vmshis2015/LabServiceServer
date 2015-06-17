//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;

namespace VietBaIT.CommonLibrary.Utilities.UserControls
{
    public partial class Banner : UserControl
    {
        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
        public string ShortcutGuide
        {
            get { return lblShortcutGuide.Text; }
            set { lblShortcutGuide.Text = value; }
        }
        public Image PicImg
        {
            get { return picIcon.Image; }
            set { picIcon.Image = value; }
        }
        public Color BackGroundColor
        {
            set 
            { this.BackColor = value;
            lblShortcutGuide.BackColor = value;
            lblTitle.BackColor = value;
            picIcon.BackColor = value;
            }
        }
        public Font TitleFont
        {
            get { return lblTitle.Font; }
            set { lblTitle.Font = value; }
        }
        public Font ShortcutFont
        {
            get { return lblShortcutGuide.Font; }
            set { lblShortcutGuide.Font = value; }
        }
        public Banner()
        {
            InitializeComponent();
        }
    }
}
