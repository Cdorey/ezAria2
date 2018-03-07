using ezAria2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControllerTests
{
    public partial class Form1 : Form
    {
        private RpcControler Ws=new RpcControler();
        public Form1()
        {
            InitializeComponent();
        }

        private void AddUriBtm_Click(object sender, EventArgs e)
        {
            this.Requests.Text = Ws.tellStatus(Ws.addUri(this.Uri.Text));
        }

        private void StatuBtm_Click(object sender, EventArgs e)
        {
            //ezAria2.Task NewTell = Ws.tellStatus(this.Gid.Text);
            //this.Requests.Text = JsonConvert.SerializeObject(NewTell);
        }
    }
}
