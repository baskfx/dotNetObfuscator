using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dotNetObfuscator;
using CodeTools;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Obfuscator o = new Obfuscator();
            textBox2.Text = o.Obfuscate(textBox1.Text, textBox3.Text);
        }
    }
}
