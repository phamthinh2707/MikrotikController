using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tik4net.Api;
using tik4net;
using Newtonsoft.Json;
using System.IO;

namespace tik4net.controller
{
    public partial class Form1 : Form
    {
        private ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
        private List<string> commandRows = new List<string>();
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        public delegate ITikConnection sendConnection(ITikConnection con);

        public Form1()
        {
            InitializeComponent();
            if (lblStatus.Text.Equals("Disconnect") || lblStatus.Text.Equals("Not Connected!"))
            {
                btnSubmit.Enabled = false;
                tableOption.Visible = false;
                txtCommand.ReadOnly = true;
            }
        }

        //
        private void btnSubmit_MouseClick(object sender, MouseEventArgs e)
        {
            string command = txtCommand.Text;
            ExecuteCommand(command);
        }

        //
        private void ExecuteCommand(string commandStr)
        {

            if (!string.IsNullOrWhiteSpace(commandStr))
                commandRows.Add(commandStr);
            if (commandRows.Any())
            {
                List<string> rows = new List<string>();
                foreach (string row in commandRows)
                {
                    rows.AddRange(row.Split('|').Where(r => !string.IsNullOrEmpty(r)));
                }
                var result = connection.CallCommandSync(rows.ToArray());
                commandRows.Clear();
            }

        }

        //
        private void ExecuteParameterCommand(List<string> commandRows)
        {
            if (commandRows.Any())
            {
                List<string> rows = new List<string>();
                foreach (string row in commandRows)
                {
                    rows.AddRange(row.Split('|').Where(r => !string.IsNullOrEmpty(r)));
                }
                var result = connection.CallCommandSync(rows.ToArray());
                commandRows.Clear();
            }
        }

        //
        // Connect Button
        //
        private void btnConnect_MouseClick(object sender, MouseEventArgs e)
        {
            string action = btnConnect.Text;
            if (action == "Connect")
            {
                string host = txtHost.Text;
                string user = txtUser.Text;
                string password = txtPassword.Text;
                try
                {
                    connection.OnReadRow += Connection_OnReadRow;
                    connection.OnWriteRow += Connection_OnWriteRow;
                    connection.Open(host, user, password);
                    lblStatus.Text = "Connected";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    tableOption.Visible = true;
                    btnSubmit.Enabled = true;
                    txtCommand.ReadOnly = false;
                    btnConnect.Text = "Disconnect";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (action == "Disconnect")
            {
                connection.Close();
                btnConnect.Text = "Connect";
                lblStatus.Text = "Disconnect";
                lblStatus.ForeColor = Color.Crimson;
            }
        }

        //
        private void Connection_OnWriteRow(object sender, TikConnectionCommCallbackEventArgs args)
        {
            rtxDisplay.Text += (args.Word + "\n");
        }

        //
        private void Connection_OnReadRow(object sender, TikConnectionCommCallbackEventArgs args)
        {
            rtxDisplay.Text += (args.Word + "\n");
        }

        //
        // Add command by Enter key
        //
        private void txtCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (txtCommand.Text.Equals("clear"))
                {
                    rtxDisplay.Text = "";
                }
                else
                {
                    commandRows.Add(txtCommand.Text);
                    List<string> rows = new List<string>();
                    foreach (string row in commandRows)
                    {
                        rtxDisplay.Text = txtCommand.Text + "\n";
                    }
                    txtCommand.Text = "";
                }
            }
        }

        //
        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                var router = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                txtHost.Text = router.host;
                txtUser.Text = router.user;
                txtPassword.Text = router.password;
            }
        }

        //
        private void btnReboot_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to reboot Router?", "Reboot Router", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ExecuteCommand("/system/reboot");
            }
            else
            {
                MessageBox.Show("Action Cancelled!");
            }
        }

        //
        // Show Reset Form and Tranfer Connection
        //
        private void btnResetConfiguration_Click(object sender, EventArgs e)
        {
            Reset reset = new Reset();
            reset.getter(connection);
            reset.ShowDialog();
        }
    }
}
