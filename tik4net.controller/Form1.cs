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
        public Form1()
        {
            InitializeComponent();
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
        private void btnConnect_MouseClick(object sender, MouseEventArgs e)
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
            }
            catch (System.Net.Sockets.SocketException)
            {
                lblStatus.Text = "Failed to connect, please check the connection";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        //
        private void Connection_OnWriteRow(object sender, TikConnectionCommCallbackEventArgs args)
        {            
            rtxDisplay.Text += (args.Word + "\n");
            rtxDisplay.ForeColor = System.Drawing.Color.Magenta;
        }

        //
        private void Connection_OnReadRow(object sender, TikConnectionCommCallbackEventArgs args)
        {
            rtxDisplay.Text += (args.Word + "\n");
            rtxDisplay.ForeColor = System.Drawing.Color.Green;
        }

        private void txtCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
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
    }
}
