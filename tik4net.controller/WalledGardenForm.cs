using System;
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

        private class walledGardenItem
        {
            public string id { get; set; }
            public string Action { get; set; }
            public string dstHost { get; set; }
            public string hits { get; set; }
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
            commandRows.Add("/ip/hotspot/walled-garden/print");
            getWalledGarden(commandRows);
        }
        //
        // Remove Dst_Host
        //
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int number = dataGridView.CurrentCell.RowIndex;
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
            commandRows.Add("/ip/hotspot/walled-garden/print");
            getWalledGarden(commandRows);
        }
        //
        // Get Walled Garden 
        //
        private List<walledGardenItem> getWalledGarden(List<string> command)
        {
            List<walledGardenItem> walledGarden = new List<walledGardenItem>();
            if (commandRows.Any())
            {
                List<string> rows = new List<string>();
                foreach (string row in commandRows)
                {
                    rows.AddRange(row.Split('|').Where(r => !string.IsNullOrEmpty(r)));
                }
                var result = connection.CallCommandSync(rows.ToArray());
                foreach (var resultItem in result)
                {
                    walledGardenItem wl = new walledGardenItem();
                    foreach (var word in resultItem.Words)
                    {
                        if (word.Key.Equals("action"))
                        {
                            wl.Action = word.Value;
                        }
                        else if (word.Key.Equals("dst-host"))
                        {
                            wl.dstHost = word.Value;
                        }
                        else if (word.Key.Equals("hits"))
                        {
                            wl.hits = word.Value;
                        }
                        else if (word.Key.Equals(".id"))
                        {
                            wl.id = word.Value;
                        }
                    }
                    walledGarden.Add(wl);
                }
                commandRows.Clear();
            }
            return walledGarden;
        }
        //
        // On load get Walled Garden to Display
        //
        private void WalledGardenForm_Load(object sender, EventArgs e)
        {
            int STT = 0;
            
            txtAddHost.Text = "Enter Your Destination Host Here ...";
            commandRows.Add("/ip/hotspot/walled-garden/print");
            if (commandRows.Any())
            {
                List<walledGardenItem> walledGardenLists = getWalledGarden(commandRows);
                foreach (var wl in walledGardenLists)
                {
                    object[] Rows = new object[]
                    {
                        STT.ToString(),
                        wl.Action,
                        wl.dstHost,
                        wl.hits
                    };
                    dataGridView.Rows.Add(Rows);
                    STT++;
                }
            }
        }

        private void WalledGardenForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    {
                        Close();
                        break;
                    }
            }
        }

        private void txtAddHost_Enter(object sender, EventArgs e)
        {
            if (txtAddHost.Text.Equals("Enter Your Destination Host Here ..."))
            {
                txtAddHost.Text = "";
            }
        }

        private void txtAddHost_Leave(object sender, EventArgs e)
        {
            if (txtAddHost.Text.Equals(""))
            {
                txtAddHost.Text = "Enter Your Destination Host Here ...";
                txtAddHost.ForeColor = Color.Gray;
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
