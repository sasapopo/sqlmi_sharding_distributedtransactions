using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.Json;
using System.Windows.Forms;

namespace ShardManager
{
    public partial class ShardMapManagerForm : Form
    {
        ShardManager shardManager = new ShardManager();

        public ShardMapManagerForm()
        {
            InitializeComponent();
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            if (shardManager.Login == null)
            {
                string mask = "";
                string login = "";
                SecureString password = new SecureString();

                login = loginText.Text;
                passwordText.Text.ToCharArray().ToList().ForEach(c => { password.AppendChar(c); mask += '.'; });
                passwordText.Text = mask;

                shardManager.Login = login;
                shardManager.Password = password;
            }

            JsonDocument shardMapDefinition = null;
            using (StreamReader sr = new StreamReader("ShardMapDefinition.json"))
            {
                string s = sr.ReadToEnd();
                shardMapDefinition = JsonDocument.Parse(s);
            }

            outputText.Text = shardManager.Setup(shardMapDefinition);
        }

        private void ShardMapManagerForm_Load(object sender, EventArgs e)
        {
            TryToLoadCreds();
        }

        private void TryToLoadCreds()
        {
            try
            {
                string line1;
                string line2;
                string login = "";
                SecureString password = new SecureString();

                using (StreamReader sr = new StreamReader("creds.txt"))
                {
                    line1 = sr.ReadLine();
                    line2 = sr.ReadLine();
                }

                login = line1;
                line2.ToCharArray().ToList().ForEach(c => { password.AppendChar(c); });

                shardManager.Login = login;
                shardManager.Password = password;
            }
            finally
            {

            }
        }
    }
}