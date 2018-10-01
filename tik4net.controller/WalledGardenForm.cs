﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tik4net;

namespace tik4net.controller
{
    public partial class WalledGardenForm : Form
    {
        List<string> commandRows = new List<string>();
        private ITikConnection connection;
        public delegate void getConnection(ITikConnection conn);
        public getConnection getter;

        public WalledGardenForm()
        {
            InitializeComponent();
            getter = new getConnection(getConn);
        }

        private void getConn(ITikConnection conn)
        {
            connection = conn;
        }
        //
        // Execute Command
        //
        private List<string> ExecuteCommand(List<string> command)
        {
            List<string> walledGarden = null;
            if (commandRows.Any())
            {
                List<string> rows = new List<string>();
                foreach (string row in commandRows)
                {
                    rows.AddRange(row.Split('|').Where(r => !string.IsNullOrEmpty(r)));
                }
                var result = connection.CallCommandSync(rows.ToArray());
                walledGarden = (List<string>)result;
                commandRows.Clear();
            }
            return walledGarden;
        }
        //
        // Add Dst_Host
        //
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAddHost.Text.IsNullOrWhiteSpace())
            {
                MessageBox.Show("Please Input Something Before Add.");
            }
            else
            {
                commandRows.Add("/ip/hotspot/walled-garden/add");
                commandRows.Add("=dst-host=" + txtAddHost.Text);
                ExecuteCommand(commandRows);
            }
        }
        //
        // Get Walled Garden 
        //
        private List<string> getWalledGarden(List<string> command)
        {
            List<string> walledGarden = null;
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
            return walledGarden;
        }
        //
        //
        //
        private void WalledGardenForm_Load(object sender, EventArgs e)
        {
            commandRows.Add("/ip/hotspot/walled-garden/print");
            if (commandRows.Any())
            {
                rtxDisplay.Text += getWalledGarden(commandRows);
            }
        }
    }
}
