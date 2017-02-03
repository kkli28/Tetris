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
    public partial class GameForm : Form {

        Graph graph;  //图对象
        Random random;
        bool inGame;  //游戏中

        //可设置延迟，来避免玩家频繁操作，例如用一个timer来纪录，每隔200ms可移动一次

        int time;  //游戏时间
        int score;  //游戏分数
        int record;  //游戏最高纪录
        Block block1;  //当前方块
        Block block2;  //下一个方块

        //Form构造函数
        public GameForm() {
            InitializeComponent();
            graph = new Graph();
            random = new Random();
            inGame = false;  //默认不在游戏中
            time = 0;
            score = 0;
            block1 = new O_Block();
            block2 = new O_Block();
            recordLabel.Text = File.ReadAllText("record\\record.txt");
        }
        //开始游戏
        public void start() {
            //每次开始游戏重置所有游戏状态
            graph = new Graph();  //重置图
            inGame = true;
            time = 0;
            score = 0;
            scoreLabel.Text = "0";
            record = Int32.Parse(File.ReadAllText("record\\record.txt"));
            recordLabel.Text = record.ToString();

            //重置游戏难度计时器
            timeTimer.Interval = 500;

            //重置主界面PictureBox背景色
            foreach(Control c in this.Controls) {
                if (c is PictureBox) c.BackColor = Color.Black;
            }

            //重置block1和block2的指向
            block1 = getBlock();
            block2 = getBlock();

            //每次获取的block1和block2颜色都相同，因此通过setColor函数
            //让block2的color值比block1“大1”
            int cl = (block1.getColor() + 1) % 6;
            if (cl == 0) cl++;
            block2.setColor(cl);

            showBlock(block1);
            showNextBlock(block2);
            inGame = true;
            timeTimer.Start();  //难度计时开始
            timerCountTimer.Start();  //秒计时开始
        }

        //游戏结束处理
        public void gameOver() {
            showFirstLine();  //显示第一行未显示的内容
            GameOverForm gof = new GameOverForm(score);
            gof.ShowDialog();
            resetAllPBBackColor();
        }

        //判断游戏是否结束
        public bool isGameOver(Block b) {
            Point c = b.getCore();
            Point p1 = b.getPoint1();
            Point p2 = b.getPoint2();
            Point p3 = b.getPoint3();

            //任意一个点对应的图的位置有值，表示有方块重合，即游戏结束
            if (graph.getValue(c) != 0 || graph.getValue(p1) != 0
                || graph.getValue(p2) != 0 || graph.getValue(p3) != 0) {
                inGame = false;
                return true;
            }
            return false;
        }

        //每次方块落地，更新游戏状态
        public void update() {
            showNextBlock(block2, true);  //重置block2对应PictureBox的背景色

            //更新block1所指方块类型
            switch (block2.getType()) {
                case 0: block1 = new O_Block();break;
                case 1:block1 = new l_Block(); break;
                case 2: block1 = new T_Block(); break;
                case 3: block1 = new LL_Block(); break;
                case 4: block1 = new J_Block(); break;
                case 5: block1 = new Z_Block(); break;
                case 6: block1 = new S_Block(); break;
            }
            block1.copyFrom(block2);
            if (isGameOver(block1)) {
                timeTimer.Stop();  //难度计时停止
                timerCountTimer.Stop();  //秒钟计时停止
                gameOver();
                resetAllPBBackColor();  //重置所有PictureBox
            }
            else {
                //更新block2所指方块类型
                Block block = getBlock();  //暂存获取的新方块
                switch (block.getType()) {
                    case 0: block2 = new O_Block(); break;
                    case 1: block2 = new l_Block(); break;
                    case 2: block2 = new T_Block(); break;
                    case 3: block2 = new LL_Block(); break;
                    case 4: block2 = new J_Block(); break;
                    case 5: block2 = new Z_Block(); break;
                    case 6: block2 = new S_Block(); break;
                }
                block2.copyFrom(block);  //更新block2
                showBlock(block1);  //显示block1
                showNextBlock(block2);  //显示block2
            }
        }

        //确保游戏结束后，第一行会显示方块
        public void showFirstLine() {
            //堆叠的方块已经将第一行中间位置（出方块的位置）占据
            if (graph.getValue(3) != 0 || graph.getValue(4) != 0 || graph.getValue(5) != 0) return;
            Color color = Color.Black;
            int2Color(block1.getColor(), out color);  //获取颜色
            
            switch (block1.getType()) {
                case 0: {
                        pictureBox4.BackColor = color;
                        pictureBox5.BackColor = color;
                        break;
                    }
                case 1: {
                        pictureBox3.BackColor = color;
                        pictureBox4.BackColor = color;
                        pictureBox5.BackColor = color;
                        pictureBox6.BackColor = color;
                        break;
                    }
                case 2: {
                        pictureBox3.BackColor = color;
                        pictureBox4.BackColor = color;
                        pictureBox5.BackColor = color;
                        break;
                    }
                case 3: {
                        pictureBox3.BackColor = color;
                        pictureBox4.BackColor = color;
                        pictureBox5.BackColor = color;
                        break;
                    }
                case 4: {
                        pictureBox3.BackColor = color;
                        pictureBox4.BackColor = color;
                        pictureBox5.BackColor = color;
                        break;
                    }
                case 5: {
                        pictureBox4.BackColor = color;
                        pictureBox5.BackColor = color;
                        break;
                    }
                case 6: {
                        pictureBox3.BackColor = color;
                        pictureBox4.BackColor = color;
                        break;
                    }
                default:
                    throw new Exception("方块类型错误");
            }
        }

        //获取随机方块
        public Block getBlock() {
            int rand = random.Next(7);
            switch (rand) {
                case 0: return new O_Block();
                case 1: return new l_Block();
                case 2: return new T_Block();
                case 3: return new LL_Block();
                case 4: return new J_Block();
                case 5: return new Z_Block();
                case 6: return new S_Block();
                default:
                    throw new Exception("方块类型错误");
            }
        }

        //相应PictureBox显示对应颜色来展示当前方块
        //当提供第二个参数为true时，还原点对应PictureBox背景色为黑色
        public void showBlock(Block b,bool reset=false) {
            Point core = b.getCore();
            Point p1 = b.getPoint1();
            Point p2 = b.getPoint2();
            Point p3 = b.getPoint3();
            int index = core.X * 10 + core.Y;
            int index1 = p1.X * 10 + p1.Y;
            int index2 = p2.X * 10 + p2.Y;
            int index3 = p3.X * 10 + p3.Y;
            
            Color color = Color.Black;

            //非重置型调用
            if (!reset) {
                int2Color(core.Color, out color);  //设置color
            }

            //设置四个点的颜色
            setPBBackColor(index, color);
            setPBBackColor(index1, color);
            setPBBackColor(index2, color);
            setPBBackColor(index3, color);
        }

        //显示下一个方块
        /* 下一个方块是通过右边PictureBox显示的，因此将其消除时需要特殊对待 */
        public void showNextBlock(Block b,bool reset=false) {
            int blockType = b.getType();  //方块类型
            Color color = Color.Black;  //方块颜色
            if (!reset) int2Color(b.getColor(), out color);
            switch (blockType) {
                case 0: {
                        pictureBox164.BackColor = color;
                        pictureBox165.BackColor = color;
                        pictureBox168.BackColor = color;
                        pictureBox169.BackColor = color;
                        break;
                    }
                case 1: {
                        pictureBox164.BackColor = color;
                        pictureBox165.BackColor = color;
                        pictureBox166.BackColor = color;
                        pictureBox167.BackColor = color;
                        break;
                    }
                case 2: {
                        pictureBox165.BackColor = color;
                        pictureBox168.BackColor = color;
                        pictureBox169.BackColor = color;
                        pictureBox170.BackColor = color;
                        break;
                    }
                case 3: {
                        pictureBox166.BackColor = color;
                        pictureBox168.BackColor = color;
                        pictureBox169.BackColor = color;
                        pictureBox170.BackColor = color;
                        break;
                    }
                case 4: {
                        pictureBox164.BackColor = color;
                        pictureBox168.BackColor = color;
                        pictureBox169.BackColor = color;
                        pictureBox170.BackColor = color;
                        break;
                    }
                case 5: {
                        pictureBox164.BackColor = color;
                        pictureBox165.BackColor = color;
                        pictureBox169.BackColor = color;
                        pictureBox170.BackColor = color;
                        break;
                    }
                case 6: {
                        pictureBox165.BackColor = color;
                        pictureBox166.BackColor = color;
                        pictureBox168.BackColor = color;
                        pictureBox169.BackColor = color;
                        break;
                    }
                default:
                    throw new Exception("方块类型错误");
            }
        }

        //重置显示下一个方块的PictureBox
        public void resetNextBlockPB() {
            pictureBox160.BackColor = Color.Black;
            pictureBox161.BackColor = Color.Black;
            pictureBox162.BackColor = Color.Black;
            pictureBox163.BackColor = Color.Black;
            pictureBox164.BackColor = Color.Black;
            pictureBox165.BackColor = Color.Black;
            pictureBox166.BackColor = Color.Black;
            pictureBox167.BackColor = Color.Black;
            pictureBox168.BackColor = Color.Black;
            pictureBox169.BackColor = Color.Black;
            pictureBox170.BackColor = Color.Black;
            pictureBox171.BackColor = Color.Black;
            pictureBox172.BackColor = Color.Black;
            pictureBox173.BackColor = Color.Black;
            pictureBox174.BackColor = Color.Black;
            pictureBox175.BackColor = Color.Black;
        }

        //数值与颜色的转换，详见BlockInfo.txt
        public void int2Color(int c,out Color color) {
            switch (c) {
                case 0: color = Color.Black; break;
                case 1: color = Color.Yellow; break;
                case 2: color = Color.DodgerBlue; break;
                case 3: color = Color.LightGreen; break;
                case 4: color = Color.OrangeRed; break;
                case 5: color = Color.White; break;
                default:
                    throw new Exception("颜色参数错误");
            }
        }

        //重置所有PictureBox背景色
        public void resetAllPBBackColor() {
            foreach(Control c in this.Controls) {
                if (c is PictureBox) c.BackColor = Color.Black;
            }
        }

        //设置索引index对应的PictureBox背景色为color
        public void setPBBackColor(int index, Color color) {
            foreach(Control c in this.Controls) {
                if (c.Name == "pictureBox" + index) c.BackColor = color;
            }
        }

        //更新图的值
        public void updateGraph(Block b) {
            //将方块的每个点的值（颜色）放到图中
            graph.setValue(b.getCore());
            graph.setValue(b.getPoint1());
            graph.setValue(b.getPoint2());
            graph.setValue(b.getPoint3());
            int count = 0;  //获取消除的行数
            if (graph.eliminate(out count)) {
                redraw();
            }
            score += count;  //每次消除，都将行数加到分数上
            scoreLabel.Text = score.ToString();  //显示分数
            update();
        }

        //根据图信息重新设置PictureBox的背景色
        public void redraw() {
            resetAllPBBackColor();
            Color color;
            int minX = graph.getMinX();
            for (int i=minX; i < 16; i++) {
                for(int j = 0; j < 10; j++) {
                    int2Color(graph.getValue(i,j), out color);  //获取颜色值
                    getPictureBox(i * 10 + j).BackColor = color;  //设置颜色
                }
            }
        }

        //获取索引对应的PictureBox，用foreach来遍历获取可能导致性能问题
        public PictureBox getPictureBox(int index) {
            switch (index) {
                case 0: return pictureBox0;
                case 1: return pictureBox1;
                case 2: return pictureBox2;
                case 3: return pictureBox3;
                case 4: return pictureBox4;
                case 5: return pictureBox5;
                case 6: return pictureBox6;
                case 7: return pictureBox7;
                case 8: return pictureBox8;
                case 9: return pictureBox9;
                case 10: return pictureBox10;
                case 11: return pictureBox11;
                case 12: return pictureBox12;
                case 13: return pictureBox13;
                case 14: return pictureBox14;
                case 15: return pictureBox15;
                case 16: return pictureBox16;
                case 17: return pictureBox17;
                case 18: return pictureBox18;
                case 19: return pictureBox19;
                case 20: return pictureBox20;
                case 21: return pictureBox21;
                case 22: return pictureBox22;
                case 23: return pictureBox23;
                case 24: return pictureBox24;
                case 25: return pictureBox25;
                case 26: return pictureBox26;
                case 27: return pictureBox27;
                case 28: return pictureBox28;
                case 29: return pictureBox29;
                case 30: return pictureBox30;
                case 31: return pictureBox31;
                case 32: return pictureBox32;
                case 33: return pictureBox33;
                case 34: return pictureBox34;
                case 35: return pictureBox35;
                case 36: return pictureBox36;
                case 37: return pictureBox37;
                case 38: return pictureBox38;
                case 39: return pictureBox39;
                case 40: return pictureBox40;
                case 41: return pictureBox41;
                case 42: return pictureBox42;
                case 43: return pictureBox43;
                case 44: return pictureBox44;
                case 45: return pictureBox45;
                case 46: return pictureBox46;
                case 47: return pictureBox47;
                case 48: return pictureBox48;
                case 49: return pictureBox49;
                case 50: return pictureBox50;
                case 51: return pictureBox51;
                case 52: return pictureBox52;
                case 53: return pictureBox53;
                case 54: return pictureBox54;
                case 55: return pictureBox55;
                case 56: return pictureBox56;
                case 57: return pictureBox57;
                case 58: return pictureBox58;
                case 59: return pictureBox59;
                case 60: return pictureBox60;
                case 61: return pictureBox61;
                case 62: return pictureBox62;
                case 63: return pictureBox63;
                case 64: return pictureBox64;
                case 65: return pictureBox65;
                case 66: return pictureBox66;
                case 67: return pictureBox67;
                case 68: return pictureBox68;
                case 69: return pictureBox69;
                case 70: return pictureBox70;
                case 71: return pictureBox71;
                case 72: return pictureBox72;
                case 73: return pictureBox73;
                case 74: return pictureBox74;
                case 75: return pictureBox75;
                case 76: return pictureBox76;
                case 77: return pictureBox77;
                case 78: return pictureBox78;
                case 79: return pictureBox79;
                case 80: return pictureBox80;
                case 81: return pictureBox81;
                case 82: return pictureBox82;
                case 83: return pictureBox83;
                case 84: return pictureBox84;
                case 85: return pictureBox85;
                case 86: return pictureBox86;
                case 87: return pictureBox87;
                case 88: return pictureBox88;
                case 89: return pictureBox89;
                case 90: return pictureBox90;
                case 91: return pictureBox91;
                case 92: return pictureBox92;
                case 93: return pictureBox93;
                case 94: return pictureBox94;
                case 95: return pictureBox95;
                case 96: return pictureBox96;
                case 97: return pictureBox97;
                case 98: return pictureBox98;
                case 99: return pictureBox99;
                case 100: return pictureBox100;
                case 101: return pictureBox101;
                case 102: return pictureBox102;
                case 103: return pictureBox103;
                case 104: return pictureBox104;
                case 105: return pictureBox105;
                case 106: return pictureBox106;
                case 107: return pictureBox107;
                case 108: return pictureBox108;
                case 109: return pictureBox109;
                case 110: return pictureBox110;
                case 111: return pictureBox111;
                case 112: return pictureBox112;
                case 113: return pictureBox113;
                case 114: return pictureBox114;
                case 115: return pictureBox115;
                case 116: return pictureBox116;
                case 117: return pictureBox117;
                case 118: return pictureBox118;
                case 119: return pictureBox119;
                case 120: return pictureBox120;
                case 121: return pictureBox121;
                case 122: return pictureBox122;
                case 123: return pictureBox123;
                case 124: return pictureBox124;
                case 125: return pictureBox125;
                case 126: return pictureBox126;
                case 127: return pictureBox127;
                case 128: return pictureBox128;
                case 129: return pictureBox129;
                case 130: return pictureBox130;
                case 131: return pictureBox131;
                case 132: return pictureBox132;
                case 133: return pictureBox133;
                case 134: return pictureBox134;
                case 135: return pictureBox135;
                case 136: return pictureBox136;
                case 137: return pictureBox137;
                case 138: return pictureBox138;
                case 139: return pictureBox139;
                case 140: return pictureBox140;
                case 141: return pictureBox141;
                case 142: return pictureBox142;
                case 143: return pictureBox143;
                case 144: return pictureBox144;
                case 145: return pictureBox145;
                case 146: return pictureBox146;
                case 147: return pictureBox147;
                case 148: return pictureBox148;
                case 149: return pictureBox149;
                case 150: return pictureBox150;
                case 151: return pictureBox151;
                case 152: return pictureBox152;
                case 153: return pictureBox153;
                case 154: return pictureBox154;
                case 155: return pictureBox155;
                case 156: return pictureBox156;
                case 157: return pictureBox157;
                case 158: return pictureBox158;
                case 159: return pictureBox159;
                default: throw new Exception("索引错误");
            }
        }
        
        private void Form1_Load(object sender, EventArgs e) {

        }

        private void startBtn_Click(object sender, EventArgs e) {
            if (inGame) {
                timeTimer.Stop();  //难度计时暂停
                timerCountTimer.Stop();  //秒钟计时暂停
                ConfirmNewGame cng = new ConfirmNewGame();
                if (cng.ShowDialog() == DialogResult.Cancel) {
                    timeTimer.Start();  //点击取消，难度计时继续
                    timerCountTimer.Start();  //秒钟计时继续
                    return;  //继续当前游戏
                }
            }

            //第一次点击，或点击了确定

            //可能是游戏结束后，再次点击开始按钮，此时的record可能已经更改，需要重新显示
            recordLabel.Text = File.ReadAllText("record\\record.txt");
            start();  //开始游戏
        }

        private void recordBtn_Click(object sender, EventArgs e) {
            timeTimer.Stop();
            timerCountTimer.Stop();
            bool isInGame = inGame;
            inGame = false;
            RecordForm rf = new RecordForm();
            rf.ShowDialog();
            if (isInGame) {  //假如之前是在游戏中
                inGame = true;  //切换回游戏状态
                timeTimer.Start();  //继续计时
                timerCountTimer.Start();

                //可能重置了record，因此需要重新显示recordLabel
                recordLabel.Text = File.ReadAllText("record\\record.txt");
            }
            //之前不是在游戏中，不做任何操作
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e) {
            if (inGame) {  //在游戏中才有效
                switch (e.KeyCode) {
                    case Keys.W: {  //按下W键
                            if (block1.canRotate(graph)) {  //能旋转
                                showBlock(block1, true);  //清除当前方块对应PictureBox的背景色
                                block1.udPoints_RTT();  //更新方块的点信息
                                showBlock(block1);  //重新设置方块对应PictureBox的背景色
                            }
                            break;
                        }
                    case Keys.A: {  //按下A键
                            if (block1.canMoveLeft(graph)) {
                                showBlock(block1, true);  //清除PictureBox背景色
                                block1.udPoints_ML();  //更新方块点的信息
                                showBlock(block1);  //重新设置PictureBox
                            }
                            break;
                        }
                    case Keys.S: {  //按下S键
                            if (block1.canMoveDown(graph)) {
                                showBlock(block1, true);
                                block1.udPoints_MD();
                                showBlock(block1);
                            }
                            break;
                        }
                    case Keys.D: {  //按下D键
                            if (block1.canMoveRight(graph)) {
                                showBlock(block1, true);
                                block1.udPoints_MR();
                                showBlock(block1);
                            }
                            break;
                        }
                    default: break;  //按错了键，不做任何操作
                }
            }
        }

        private void timeTimer_Tick(object sender, EventArgs e) {
            if (time >= 20) {  //10秒增加一次难度
                time = 0;
                if(timeTimer.Interval>200)  //最高难度为每秒方块下移5格
                    timeTimer.Interval -= 20;  //每次减少方块下移一次的时间20ms
            }
            if (block1.canMoveDown(graph)) {
                showBlock(block1, true);  //清除block1对应PictureBox的背景色
                block1.udPoints_MD();  //更新下移后的点
                showBlock(block1);  //重新显示block1
            }
            else {
                updateGraph(block1);
            }
        }

        private void timerCountTimer_Tick(object sender, EventArgs e) {
            time++;  //1秒计时一次。将time放在timeTimer中会导致滚雪球效应，因此用固定的计时器来计数
        }
    }
}
