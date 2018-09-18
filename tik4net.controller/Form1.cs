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

namespace tik4net.controller
{
    public partial class Form1 : Form
    {
        private ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
        private List<string> commandRows = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        //
        private void btnSubmit_MouseClick(object sender, MouseEventArgs e)
        {
<<<<<<< HEAD
            string command = txtCommand.Text;
            ExecuteCommand(command);
=======
            string command = this.txtCommand.Text;
            this.ExecuteCommand(command);
>>>>>>> 30c02376b6756d6da1961224b946617c0026eb37
        }

        //
        private void ExecuteCommand(string commandStr)
        {
<<<<<<< HEAD
            List<string> commandRows = new List<string>();
            if (!string.IsNullOrWhiteSpace(commandStr))
                commandRows.Add(commandStr);
            else
            {
=======
                if (!string.IsNullOrWhiteSpace(commandStr))
                    commandRows.Add(commandStr);

>>>>>>> 30c02376b6756d6da1961224b946617c0026eb37
                if (commandRows.Any())
                {
                    List<string> rows = new List<string>();
                    foreach (string row in commandRows)
                    {
                        rows.AddRange(row.Split('|').Where(r => !string.IsNullOrEmpty(r)));
                    }
                    var result = connection.CallCommandSync(rows.ToArray());
                    foreach (var resultItem in result)
                        foreach (var word in resultItem.Words)
                            rtxDisplay.Text += word;

                    commandRows.Clear();
                txtCommand.Text = "";

                }

        }

        //
        private void btnConnect_MouseClick(object sender, MouseEventArgs e)
        {
            string host = txtHost.Text;
            string user = txtUser.Text;
            string password = txtPassword.Text;

            connection.OnReadRow += Connection_OnReadRow;
            connection.OnWriteRow += Connection_OnWriteRow;
            connection.Open(host, user, password);
            lblStatus.Text = "Connected";
<<<<<<< HEAD
            lblStatus.ForeColor = Color.Green;
=======
            lblStatus.ForeColor = System.Drawing.Color.Green;


>>>>>>> 30c02376b6756d6da1961224b946617c0026eb37
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
    }
}
