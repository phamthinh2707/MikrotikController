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
    public partial class AddUserForm : Form
    {
        private ITikConnection connection;
        public delegate void getConnection(ITikConnection conn);
        //public delegate void getGroup(List<AddUserForm.Group> g);
        public getConnection getter;
        public AddUserForm()
        {   
            InitializeComponent();
            getter = new getConnection(getConn);
        }

        private void getConn(ITikConnection conn)
        {
            connection = conn;
        }

        private class Group
        {
            public string name { get; set; }
            public string policies { get; set; }
            public string skin { get; set; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }
    }
}
