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

            speaker = new Speaker();
            speaker.SayAutomatically();
            speaker.GenerationComplete += (o, e) => textBox2.Text = Word.WritePronounciation(e.Words);
        }

        Speaker speaker;
        private void button1_Click(object sender, EventArgs e)
        {
            speaker.GenerateIPA(textBox1.Text);
        }
    }
}
