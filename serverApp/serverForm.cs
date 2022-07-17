using SuperSimpleTcp;
using System.Text;

namespace serverApp
{
    public partial class serverForm : Form
    {
        private static string IP="127.0.0.1";
        private static int PORT=28141;
        private static string IPORT=$"{IP}:{PORT}";
        public serverForm()
        {
            InitializeComponent();
        }
        SimpleTcpServer server;
        private void btn_connect_Click(object sender, EventArgs e)
        {
            server.Start();
            txt_chatText.Text += $"Starting...{Environment.NewLine}";
            btn_connect.Enabled = false;
            btn_send.Enabled = true;
            
        }

        private void serverForm_Load(object sender, EventArgs e)
        { 
            txt_ip.Text = IPORT;
            btn_send.Enabled = false;
            server = new SimpleTcpServer(txt_ip.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txt_chatText.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txt_chatText.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
            lst_clients.Items.Remove(e.IpPort);
        });
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txt_chatText.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                lst_clients.Items.Add(e.IpPort);
            });
            
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (!string.IsNullOrEmpty(txt_message.Text)&& lst_clients.SelectedItems!=null)
                {
                    server.Send(lst_clients.SelectedItem.ToString(), txt_message.Text);
                    txt_chatText.Text += $"Server: {txt_message.Text}{Environment.NewLine}";
                    txt_message.Text = String.Empty;
                }
            }
        }
    }
}