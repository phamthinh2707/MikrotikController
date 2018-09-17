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
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_MouseClick(object sender, MouseEventArgs e){
            string host = this.txtHost.Text;
            string user = this.txtUser.Text;
            string password = this.txtPassword.Text;
            this.rtxDisplay.Text = this.txtCommand.Text + host + user + password;


            //connection.onreadrow += connection_onreadrow;
            //connection.onwriterow += connection_onwriterow;
            //connection.open(configurationmanager.appsettings[host], configurationmanager.appsettings[user], configurationmanager.appsettings[password]);

            //List<string> commandRows = new List<string>();
            //string command;
            //Console.WriteLine("MIKROTIK CONTROLLER");
            //Console.WriteLine("Write command and press [ENTER] on empty line");
            //Console.WriteLine("Empty command + [ENTER] stops console.");
            //do
            //{
            //    command = Console.ReadLine();
            //    if (!string.IsNullOrWhiteSpace(command))
            //        commandRows.Add(command);
            //    else
            //    {
            //        if (commandRows.Any())
            //        {
            //            List<string> rows = new List<string>();
            //            foreach (string row in commandRows)
            //            {
            //                rows.AddRange(row.Split('|').Where(r => !string.IsNullOrEmpty(r)));
            //            }
            //            var result = connection.CallCommandSync(rows.ToArray());
            //            foreach (var resultItem in result)
            //                foreach (var word in resultItem.Words)
            //                    Console.WriteLine(word);

            //            commandRows.Clear();
            //        }
            //        else
            //        {
            //            break; //empty row and empty command -> end
            //        }
            //    }
            //}
            //while (true);
        }

        //private static void Connection_OnWriteRow(object sender, TikConnectionCommCallbackEventArgs args)
        //{
        //    Console.BackgroundColor = ConsoleColor.Magenta;
        //    Console.WriteLine(">" + args.Word);
        //    Console.BackgroundColor = ConsoleColor.Black;
        //}

        //private static void Connection_OnReadRow(object sender, TikConnectionCommCallbackEventArgs args)
        //{
        //    Console.BackgroundColor = ConsoleColor.Green;
        //    Console.WriteLine("<" + args.Word);
        //    Console.BackgroundColor = ConsoleColor.Black;
        //}
    }
}
