using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public class Point {
        int x;  //横坐标
        int y;  //纵坐标
        int color;  //颜色，用于设置PictureBox的背景色
        bool valid;  //判断点是否属于图内，graph[10,12]
        
        //用valid标志点是否有效，而不是直接在x、y设置时把关，是因为
        //初始化的点不能为图中任一点，即初始化为(-1,-1,-1,false)。
        //若初始化为图中一点如(0,0,0,true)，则若没有更新点，可能导致
        //意想不到的结果

        //无参构造函数
        public Point() {
            //初始化的点不属于图内，这样可保证使用它时，必然会出错（Exception）
            //从而避免忽略掉它的存在
            x = -1;
            y = -1;
            color = -1;
            valid = false;
        }

        //重载1：(x,y,c)构造函数
        public Point(int posX, int posY,int cl) {
            x = posX;
            y = posY;
            color = cl;
            //图有12行（0~15），10列（0~9）
            if (x < 0 || x > 15 || y < 0 || y > 9) valid = false;
            else valid = true;
        }

        //重载2：复制构造函数
        public Point(Point p) {
            x = p.X;
            y = p.Y;
            color = p.Color;
            valid = p.Valid;
        }

        //x的get和set
        public int X {
            get { return x; }
            set {
                x = value;
                if (x < 0 || x > 15) valid = false;  //每次更改x或y时，不要忘记更新valid的值
            }
        }

        //y的get和set
        public int Y {
            get { return y; }
            set {
                y = value;
                if (y < 0 || y > 9) valid = false;
            }
        }

        //color的get和set
        public int Color {
            get { return color; }
            set {
                if (value < 0 || value > 5) throw new Exception("颜色错误");
                color = value;
            }
        }

        //valid的get，无set
        public bool Valid {
            get { return valid; }
            //valid为只读属性，更新操作只能通过更新x和y进行，避免手动操作而出错
        }

        //通过索引（位置）设置点
        public void setPoint(int index) {
            if (index < 0 || index > 159) valid = false;
            x = index / 10;
            y = index % 10;
        }

        //获取点的信息-----Test
        public string getPointInfo() {
            return "x:" + x + "\ty:" + y + "\tvalid:" + valid.ToString();
        }

        //复制点信息
        public void copyFrom(Point p) {
            x = p.X;
            y = p.Y;
            valid = p.Valid;
            color = p.Color;
        }
    }
}
