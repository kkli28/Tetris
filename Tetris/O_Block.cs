using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    //O型，或正方形
    public class O_Block : Block {
        Point core;  //核心点
        Point point1;
        Point point2;
        Point point3;
        int color;  //此方块的颜色，详见BlockInfo.txt
        int blockType;  //方块类型，详见BlockInfo.txt
        int state;  //方块状态

        //O型方块没有状态可言，怎么变换都一个样

        //构造函数
        public O_Block() {
            //O型方块初始化在界面[0,1]行[4,5]列
            color = new Random().Next(1, 6);
            core = new Point(0, 4, color);
            point1 = new Point(1, 4, color);
            point2 = new Point(1, 5, color);
            point3 = new Point(0, 5, color);
            blockType = 0;
            state = 0;
        }

        //获取Block信息
        public override Point getCore() { return core; }
        public override Point getPoint1() { return point1; }
        public override Point getPoint2() { return point2; }
        public override Point getPoint3() { return point3; }
        public override int getType() { return blockType; }
        public override int getState() { return state; }
        public override int getColor() { return color; }

        //设置颜色
        public override void setColor(int cl) {
            if (cl < 0 || cl > 5) throw new Exception("颜色值设置错误");
            color = cl;

            //四个点的颜色值也需要更换
            core.Color = cl;
            point1.Color = cl;
            point2.Color = cl;
            point3.Color = cl;
        }

        //能否旋转
        public override bool canRotate(Graph graph) {
            return false;  //O型方块不旋转
        }

        //能否左移
        public override bool canMoveLeft(Graph graph) {
            Point p1 = new Point(core.X, core.Y - 1, color);
            Point p2 = new Point(point1.X, point1.Y - 1, color);
            if (!p1.Valid || !p2.Valid) return false;  //越界
            if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;  //有障碍
            return true;
        }

        //能否右移
        public override bool canMoveRight(Graph graph) {
            Point p1 = new Point(point2.X, point2.Y + 1, color);
            Point p2 = new Point(point3.X, point3.Y + 1, color);
            if (!p1.Valid || !p2.Valid) return false;  //越界
            if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;  //有障碍
            return true;
        }

        //能否下移
        public override bool canMoveDown(Graph graph) {
            Point p1 = new Point(point1.X + 1, point1.Y, color);
            Point p2 = new Point(point2.X + 1, point2.Y, color);
            if (!p1.Valid || !p2.Valid) return false;  //超出边界
            if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;  //有障碍
            return true;
        }

        //旋转后更新点和状态
        public override void udPoints_RTT() {
            return;  //O型方块不做任何操作
        }
        //左移后更新点
        public override void udPoints_ML() {
            core.Y--;
            point1.Y--;
            point2.Y--;
            point3.Y--;
        }

        //右移后更新点
        public override void udPoints_MR() {
            core.Y++;
            point1.Y++;
            point2.Y++;
            point3.Y++;
        }

        //下移后更新点
        public override void udPoints_MD() {
            core.X++;
            point1.X++;
            point2.X++;
            point3.X++;
        }

        //复制方块信息
        public override void copyFrom(Block b) {
            core.copyFrom(b.getCore());
            point1.copyFrom(b.getPoint1());
            point2.copyFrom(b.getPoint2());
            point3.copyFrom(b.getPoint3());
            blockType = b.getType();
            state = b.getState();
            color = b.getColor();
        }
    }
}
