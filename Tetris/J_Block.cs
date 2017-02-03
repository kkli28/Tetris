using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public class J_Block:Block {
        Point core;
        Point point1;
        Point point2;
        Point point3;
        int color;  //方块颜色，详见BlockInfo.txt
        int blockType;  //方块类型，详见BlockInfo.txt
        int state;  //方块状态，详见BlockInfo.txt

        //构造函数
        public J_Block() {
            color = new Random().Next(1, 6);
            core = new Point(1, 4, color);
            point1 = new Point(0, 3, color);
            point2 = new Point(1, 3, color);
            point3 = new Point(1, 5, color);
            blockType = 4;
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
            Point p1 = new Point();
            Point p2 = new Point();
            Point p3 = new Point();
            Point p4 = new Point();

            if (state == 0) {
                p1 = new Point(core.X - 1, core.Y, color);
                p2 = new Point(core.X - 1, core.Y + 1, color);
                p3 = new Point(core.X + 1, core.Y - 1, color);
                p4 = new Point(core.X + 1, core.Y, color);
            }
            else if (state == 1) {
                p1 = new Point(core.X - 1, core.Y - 1, color);
                p2 = new Point(core.X, core.Y - 1, color);
                p3 = new Point(core.X, core.Y + 1, color);
                p4 = new Point(core.X + 1, core.Y + 1, color);
            }
            else if (state == 2) {
                p1 = new Point(core.X - 1, core.Y, color);
                p2 = new Point(core.X - 1, core.Y + 1, color);
                p3 = new Point(core.X + 1, core.Y - 1, color);
                p4 = new Point(core.X + 1, core.Y, color);
            }
            else if (state == 3) {
                p1 = new Point(core.X - 1, core.Y - 1, color);
                p2 = new Point(core.X, core.Y - 1, color);
                p3 = new Point(core.X, core.Y + 1, color);
                p4 = new Point(core.X + 1, core.Y + 1, color);
            }
            else {
                throw new Exception("状态错误");
            }
            if (!p1.Valid || !p2.Valid || !p3.Valid || !p4.Valid) return false;
            if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0
                || graph.getValue(p3) != 0 || graph.getValue(p4) != 0)
                return false;
            return true;
        }

        //能否左移
        public override bool canMoveLeft(Graph graph) {
            if (state == 0) {
                Point p1 = new Point(point1.X, point1.Y - 1, color);
                Point p2 = new Point(point2.X, point2.Y - 1, color);
                if (!p1.Valid || !p2.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;
            }
            else if (state == 1) {
                Point p1 = new Point(point3.X, point3.Y - 1, color);
                Point p2=new Point(core.X, core.Y - 1, color);
                Point p3 = new Point(point1.X, point1.Y - 1, color);
                if (!p1.Valid || !p2.Valid || !p3.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0 || graph.getValue(p3) != 0) return false;
            }
            else if (state == 2) {
                Point p1 = new Point(point3.X, point3.Y - 1, color);
                Point p2 = new Point(point1.X, point1.Y - 1, color);
                if (!p1.Valid || !p2.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;
            }
            else if (state == 3) {
                Point p1 = new Point(point2.X, point2.Y - 1, color);
                Point p2 = new Point(core.X, core.Y - 1, color);
                Point p3 = new Point(point3.X, point3.Y - 1, color);
                if (!p1.Valid || !p2.Valid || !p3.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0 || graph.getValue(p3) != 0) return false;
            }
            else {
                throw new Exception("状态错误");
            }
            return true;
        }

        //能否右移
        public override bool canMoveRight(Graph graph) {
            if (state == 0) {
                Point p1 = new Point(point1.X, point1.Y + 1, color);
                Point p2 = new Point(point3.X, point3.Y + 1, color);
                if (!p1.Valid || !p2.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;
            }
            else if (state == 1) {
                Point p1 = new Point(point3.X, point3.Y + 1, color);
                Point p2 = new Point(core.X, core.Y + 1, color);
                Point p3 = new Point(point2.X, point2.Y + 1, color);
                if (!p1.Valid || !p2.Valid || !p3.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0 || graph.getValue(p3) != 0) return false;
            }
            else if (state == 2) {
                Point p1 = new Point(point2.X, point2.Y + 1, color);
                Point p2 = new Point(point1.X, point1.Y + 1, color);
                if (!p1.Valid || !p2.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;
            }
            else if (state == 3) {
                Point p1 = new Point(point1.X, point1.Y + 1, color);
                Point p2 = new Point(core.X, core.Y + 1, color);
                Point p3 = new Point(point3.X, point3.Y + 1, color);
                if (!p1.Valid || !p2.Valid || !p3.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0 || graph.getValue(p3) != 0) return false;
            }
            else {
                throw new Exception("状态错误");
            }
            return true;
        }

        //能否下移
        public override bool canMoveDown(Graph graph) {
            if (state == 0) {
                Point p1 = new Point(point2.X + 1, point2.Y, color);
                Point p2 = new Point(core.X + 1, core.Y, color);
                Point p3 = new Point(point3.X + 1, point3.Y, color);
                if (!p1.Valid || !p2.Valid || !p3.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0 || graph.getValue(p3) != 0) return false;
            }
            else if (state == 1) {
                Point p1 = new Point(point1.X + 1, point1.Y, color);
                Point p2 = new Point(point2.X + 1, point2.Y, color);
                if (!p1.Valid || !p2.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;
            }
            else if (state == 2) {
                Point p1=new Point(point3.X + 1, point3.Y, color);
                Point p2 = new Point(core.X + 1, core.Y, color);
                Point p3 = new Point(point1.X + 1, point1.Y, color);
                if (!p1.Valid || !p2.Valid || !p3.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0 || graph.getValue(p3) != 0) return false;                
            }
            else if (state == 3) {
                Point p1 = new Point(point3.X + 1, point3.Y, color);
                Point p2 = new Point(point1.X + 1, point1.Y, color);
                if (!p1.Valid || !p2.Valid) return false;
                if (graph.getValue(p1) != 0 || graph.getValue(p2) != 0) return false;
            }
            else {
                throw new Exception("状态错误");
            }
            return true;
        }

        //旋转后更新点
        public override void udPoints_RTT() {
            if (state == 0) {
                point1.X += 2;
                point2.X++;
                point2.Y++;
                point3.X--;
                point3.Y--;
            }
            else if (state == 1) {
                point1.Y += 2;
                point2.X--;
                point2.Y++;
                point3.X++;
                point3.Y--;
            }
            else if (state == 2) {
                point1.X -= 2;
                point2.X--;
                point2.Y--;
                point3.X++;
                point3.Y++;
            }
            else if (state == 3) {
                point1.Y -= 2;
                point2.X++;
                point2.Y--;
                point3.X--;
                point3.Y++;
            }
            else {
                throw new Exception("状态错误");
            }
            state = (state + 1) % 4;  //更新状态
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
