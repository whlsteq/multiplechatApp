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
        TcpClient _client;
        byte[] _buffer=new byte[4096];
        public clienForm()
        {
            InitializeComponent();
            _client = new TcpClient();

        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _client.Connect("127.0.0.1", 28141);

            _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Server_MessageReceived, null);

        }
        private void Server_MessageReceived(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                //TODO: RECEIVE MESSAGE
                var bytesIn=_client.GetStream().EndRead(ar);
                if (bytesIn>0)
                {
                    var tmp = new byte[bytesIn];
                    Array.Copy(_buffer,0,tmp,0,bytesIn);
                    var str=Encoding.ASCII.GetString(tmp);
                    //TODO:MAKE THIS AN ACTUAL DELEGATE READ DOCS
                    BeginInvoke((Action)(()=>{
                        lstb_chatText.Items.Add(str);
                        lstb_chatText.SelectedIndex = lstb_chatText.Items.Count - 1;
                    }));
                }
                Array.Clear(_buffer, 0, _buffer.Length);
                _client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Server_MessageReceived, null);
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            var msg=Encoding.ASCII.GetBytes(txt_message.Text);
            _client.GetStream().Write(msg,0,msg.Length);

            txt_message.Text = "";
            txt_message.Focus();
        }
    }
}
