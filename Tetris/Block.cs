using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public abstract class Block {
        //Block相关信息详见BlockInfo.txt
        public abstract bool canRotate(Graph graph);  //能否旋转
        public abstract bool canMoveLeft(Graph graph);  //能否左移
        public abstract bool canMoveRight(Graph graph);  //能否右移
        public abstract bool canMoveDown(Graph graph);  //能否下移
        public abstract void udPoints_RTT();  //旋转后更新点和状态
        public abstract void udPoints_ML();  //左移后更新点
        public abstract void udPoints_MR();  //右移后更新点
        public abstract void udPoints_MD();  //下移后更新点
        public abstract Point getCore();  //core
        public abstract Point getPoint1();  //point1
        public abstract Point getPoint2();  //point2
        public abstract Point getPoint3();  //point3
        public abstract int getType();  //方块类型
        public abstract int getState();  //方块状态
        public abstract int getColor();  //方块颜色
        public abstract void copyFrom(Block b);  //复制方块信息
        public abstract void setColor(int cl);  //设置方块颜色
    }
}
