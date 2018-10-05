using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tik4net.controller
{
    public partial class UserMangementForm : Form
    {
        private ITikConnection connection;
        public delegate void getConnection(ITikConnection conn);
        List<string> commandRows = new List<string>();
        public getConnection getter;
        public UserMangementForm()
        {
            InitializeComponent();
            getter = new getConnection(getConn);
        }

        private void getConn(ITikConnection conn)
        {
            connection = conn;
        }
        private class User
        {
            public string name { get; set; }
            public string group { get; set; }
            public string address { get; set; }
            public string lastLoggedIn { get; set; }
        }
        //
        // Execute Command
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
        // Execute Command With Parameter
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
        //
        //
        private List<User> getUsers(List<string> command)
        {
            List<User> users = new List<User>();
            if (command.Any())
            {
                List<string> rows = new List<string>();
                foreach (string row in commandRows)
                {
                    rows.AddRange(row.Split('|').Where(r => !string.IsNullOrEmpty(r)));
                }
                var result = connection.CallCommandSync(rows.ToArray());
                foreach (var resultItem in result)
                {
                    User u = new User();
                    foreach (var word in resultItem.Words)
                    {
                        if (word.Key.Equals("name")){
                            u.name = word.Value;
                        }
                        else if(word.Key.Equals("group"))
                        {
                            u.group = word.Value;
                        } else if (word.Key.Equals("last-logged-in"))
                        {
                            u.lastLoggedIn = word.Value;
                        } else if (word.Key.Equals("disabled"))
                        {
                            
                        }
                    }
                }
            }
            return users;
        }
        //
        // Remove User Button
        //
        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            string name = UserGridView.CurrentRow.Cells["Name"].FormattedValue.ToString();
            commandRows.Add("/user/remove");
            commandRows.Add("=numbers=" + name);
            ExecuteParameterCommand(commandRows);
        }
        //
        // Enable User Button
        //
        private void btnEnabledUser_Click(object sender, EventArgs e)
        {
            string name = UserGridView.CurrentRow.Cells["Name"].FormattedValue.ToString();
            commandRows.Add("/user/enable");
            commandRows.Add("=numbers=" + name);
            ExecuteParameterCommand(commandRows);
        }
        //
        // Disable User Button
        //
        private void btnDisabledUser_Click(object sender, EventArgs e)
        {
            string name = UserGridView.CurrentRow.Cells["Name"].FormattedValue.ToString();
            commandRows.Add("/user/disable");
            commandRows.Add("=numbers=" + name);
            ExecuteParameterCommand(commandRows);
        }

        private void UserMangementForm_Load(object sender, EventArgs e)
        {
            commandRows.Add("/user/print");
            if (commandRows.Any())
            {
                getUsers(commandRows);
            }
        }
    }
}
