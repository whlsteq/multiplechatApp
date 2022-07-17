namespace chatApp
{
    public partial class loginForm : Form
    {
        private static string nickname;
        public loginForm()
        {
            InitializeComponent();
        }

        public static string Nickname { get => nickname; set => nickname = value; }

        private void btn_login_Click(object sender, EventArgs e)
        {
            Nickname=txt_nickname.Text;
            clienForm clienForm = new clienForm();
            clienForm.Show();
                        
        }
    }
}