using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Tetris {
    public partial class RecordForm : Form {
        public RecordForm() {
            InitializeComponent();
            string record = File.ReadAllText("record\\record.txt");
            recordLabel.Text = record;
        }

        private void Record_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            File.WriteAllText("record\\record.txt","0");
            recordLabel.Text = "0";
        }
    }
}
