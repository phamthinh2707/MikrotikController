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
        private string command;
        private List<string> commandRows = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_MouseClick(object sender, MouseEventArgs e)
        {
            do
            {
                if (this.txtHost.Text.IsNullOrWhiteSpace())
                {
                    this.txtCommand.Text = "Please connect to your Mikrotik Router first";
                }
                else
                {
                    command = this.txtCommand.Text;
                    if (!string.IsNullOrWhiteSpace(command))
                        commandRows.Add(command);
                    else
                    {
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
                                    this.rtxDisplay.Text += word;

                            commandRows.Clear();
                        }
                        else
                        {
                            break; //empty row and empty command -> end
                        }
                    }
                }
            } while (true);
        }

        private void btnConnect_MouseClick(object sender, MouseEventArgs e)
        {
            string host = this.txtHost.Text;
            string user = this.txtUser.Text;
            string password = this.txtPassword.Text;

            connection.OnReadRow += Connection_OnReadRow;
            connection.OnWriteRow += Connection_OnWriteRow;
            connection.Open(host, user, password);

            this.btnConnect.Name = "Connected";

            do
            {
                command = this.txtCommand.Text;
                if (!string.IsNullOrWhiteSpace(command))
                    commandRows.Add(command);
                else
                {
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
                                this.rtxDisplay.Text += word;

                        commandRows.Clear();
                    }
                    else
                    {
                        break; //empty row and empty command -> end
                    }
                }
            }
            while (true);
        }

        private void Connection_OnWriteRow(object sender, TikConnectionCommCallbackEventArgs args)
        {
            this.rtxDisplay.Text += args.Word;
        }

        private void Connection_OnReadRow(object sender, TikConnectionCommCallbackEventArgs args)
        {
            this.rtxDisplay.Text += (args.Word + "\n");
        }
    }
}
