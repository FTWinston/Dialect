using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dialect;
using Dialect.Sources;

namespace DialectBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            pronounciation = PronounciationManager.CreateDefault();
            pronounciation.LookupComplete += (o, e) => textBox2.Text = e.Pronounciation ?? "<not found>";
        }

        PronounciationManager pronounciation;
        private void button1_Click(object sender, EventArgs e)
        {
            pronounciation.Lookup(textBox1.Text);
        }
    }
}
