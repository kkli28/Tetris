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
    public partial class GameOverForm : Form {
        public GameOverForm(int score) {
            InitializeComponent();
            scoreLabel.Text = score.ToString();  //显示本局得分
            int record = Int32.Parse(File.ReadAllText("record\\record.txt"));
            if (score > record) {  //新纪录
                record = score;  //纪录新纪录
                File.WriteAllText("record\\record.txt", record.ToString());  //将新纪录写入文件
                newRecordLabel.Visible = true;  //显示提示超越纪录的label
            }
            recordLabel.Text = record.ToString();  //显示纪录
        }

        private void GameOverForm_Load(object sender, EventArgs e) {

        }
    }
}
