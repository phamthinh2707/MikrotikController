using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using tik4net;

namespace tik4net.controller
{
    public partial class Reset : Form
    {
        private ITikConnection connection;
        public delegate void getConnection(ITikConnection conn);
        public getConnection getter;

        public Reset()
        {
            InitializeComponent();
            getter = new getConnection(getConn);
        }

        private void getConn(ITikConnection conn)
        {
            connection = conn;
        }

        public class Script
        {
            public int ScriptIndex { get; set; }
            public List<string> ScriptValue { get; set; }
        }

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

        private void btnResetConfiguration_Click(object sender, EventArgs e)
        {
            if (radioResetNoDefault.Checked == true)
            {
                using (StreamReader stream = new System.IO.StreamReader(@"C:\Users\firel\Desktop\Script.json"))
                {
                    var str = stream.ReadToEnd();
                    var script = JsonConvert.DeserializeObject<List<Script>>(str);
                    ExecuteParameterCommand(script[0].ScriptValue);
                    Close();
                }
            }

            else if (radioResetKeepUserConfiguration.Checked == true)
            {
                using (StreamReader stream = new System.IO.StreamReader(@"C:\Users\firel\Desktop\Script.json"))
                {
                    var str = stream.ReadToEnd();
                    var script = JsonConvert.DeserializeObject<List<Script>>(str);
                    ExecuteParameterCommand(script[1].ScriptValue);
                    Close();
                }
            }

            else if (radioResetCAPSMode.Checked == true)
            {
                using (StreamReader stream = new System.IO.StreamReader(@"C:\Users\firel\Desktop\Script.json"))
                {
                    List<string> commandRows = new List<string>();
                    var str = stream.ReadToEnd();
                    var script = JsonConvert.DeserializeObject<List<Script>>(str);
                    ExecuteParameterCommand(script[3].ScriptValue);
                    Close();
                }
            }

            else if (radioResetNotBackup.Checked == true)
            {
                using (StreamReader stream = new System.IO.StreamReader(@"C:\Users\firel\Desktop\Script.json"))
                {
                    var str = stream.ReadToEnd();
                    var script = JsonConvert.DeserializeObject<List<Script>>(str);
                    ExecuteParameterCommand(script[2].ScriptValue);
                    Close();
                }
            }

            else
            {
                MessageBox.Show("Please Choose An Option To Perform!");
            }
        }

        private void Reset_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
