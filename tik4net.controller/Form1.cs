﻿using System;
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
            string command = txtCommand.Text;
            this.ExecuteCommand(command);
        }
        
        //
        private void ExecuteCommand(string commandStr)
        {
            if (!string.IsNullOrWhiteSpace(commandStr))
                commandRows.Add(commandStr);
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
                            rtxDisplay.Text += word;

                    commandRows.Clear();
                }
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
            lblStatus.ForeColor = System.Drawing.Color.Green;
        }
        
        //
        private void Connection_OnWriteRow(object sender, TikConnectionCommCallbackEventArgs args)
        {
            rtxDisplay.Text += args.Word;
        }
        
        //
        private void Connection_OnReadRow(object sender, TikConnectionCommCallbackEventArgs args)
        {
            rtxDisplay.Text += (args.Word + "\n");
        }
    }
}
