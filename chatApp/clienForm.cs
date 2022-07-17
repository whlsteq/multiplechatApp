using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatApp
{
    public partial class clienForm : Form
    {
        private static string IP = "127.0.0.1";
        private static int PORT = 28141;
        private static string IPORT = $"{IP}:{PORT}";
        public clienForm()
        {
            InitializeComponent();

        }
        SimpleTcpClient client;

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                if (!string.IsNullOrEmpty(txt_message.Text))
                {
                    client.Send(txt_message.Text);
                    txt_chatText.Text = $"Me :{txt_message.Text}{Environment.NewLine}";
                    txt_message.Text = string.Empty;
                }
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
                btn_send.Enabled= true;
                btn_connect.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void clienForm_Load(object sender, EventArgs e)
        {
            txt_ip.Text = IPORT;
            client = new(txt_ip.Text);
            client.Events.Connected += Events_Connected;
            client.Events.DataReceived += Events_DataReceived;
            client.Events.Disconnected += Events_Disconnected;
            btn_send.Enabled = false;
        }

        private void Events_Disconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txt_chatText.Text += $"Server disconnected.{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
                this.Invoke((MethodInvoker)delegate
                {
                    txt_chatText.Text += $"Server: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
                });
            }

        private void Events_Connected(object? sender, ConnectionEventArgs e)
        {
                    this.Invoke((MethodInvoker)delegate
                    {
                        txt_chatText.Text += $"Server connected.{Environment.NewLine}";
                    });
        }
    }
}
