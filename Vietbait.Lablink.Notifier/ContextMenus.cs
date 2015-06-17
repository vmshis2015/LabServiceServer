using System;
using System.Diagnostics;
using System.Windows.Forms;
using Vietbait.Lablink.Notifier.Properties;

namespace Vietbait.Lablink.Notifier
{
    /// <summary>
    /// </summary>
    internal class ContextMenus
    {
        /// <summary>
        ///     Is the About box displayed?
        /// </summary>
        private bool _isAboutLoaded;
        private bool _isConfigLoaded;

        /// <summary>
        ///     Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            var menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            // Windows Explorer.
            item = new ToolStripMenuItem();
            item.Text = @"Config";
            item.Click += Config_Click;
            item.Image = Resources.Explorer;
            menu.Items.Add(item);

            // About.
            item = new ToolStripMenuItem();
            item.Text = @"About";
            item.Click += About_Click;
            item.Image = Resources.About;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = @"Exit";
            item.Click += Exit_Click;
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        /// <summary>
        ///     Handles the Click event of the Explorer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void Config_Click(object sender, EventArgs e)
        {
            if (_isConfigLoaded) return;

            _isConfigLoaded = true;
            new FrmConfig().ShowDialog();
            _isConfigLoaded = false;
        }

        /// <summary>
        ///     Handles the Click event of the About control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void About_Click(object sender, EventArgs e)
        {
            if (_isAboutLoaded) return;

            _isAboutLoaded = true;
            new AboutBox().ShowDialog();
            _isAboutLoaded = false;
        }

        /// <summary>
        ///     Processes a menu item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void Exit_Click(object sender, EventArgs e)
        {
            // Quit without further ado.
            Application.Exit();
        }
    }
}