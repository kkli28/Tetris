using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public class Graph {
        //graph各个元素的值value代表颜色，详见BlockInfo.txt
        int[,] graph;
        int minX;

        //构造函数，初始化graph
        public Graph() {
            graph = new int[16, 10];  //听说？默认值为0，因此不再进行for循环赋值
            minX = 15;
        }

        //重载1：通过坐标点(i,j)和值value)设置图对应位置的值
        public void setValue(int i, int j, int value) {
            if (i < 0 || i > 15) throw new Exception("横坐标越界");
            if (j < 0 || j > 9) throw new Exception("纵坐标越界");
            if (value < 0 || value > 5) throw new Exception("坐标点的值无效");
            if (i < minX) minX = i;
            graph[i, j] = value;  //只有保证数据安全，才能将数据写入图中
        }

        //重载2：通过点p及值value设置图对应位置的值
        public void setValue(Point p) {
            int value = p.Color;
            if (!p.Valid) throw new Exception("坐标越界");  //可通过查看点的valid属性直接判定坐标是否越界
            if (value < 0 || value > 5) throw new Exception("值无效");
            if (p.X < minX) minX = p.X;
            graph[p.X, p.Y] = value;
        }

        //重载1：获取点在图中位置的值
        public int getValue(Point p) {
            if (!p.Valid) throw new Exception("坐标越界");
            return graph[p.X, p.Y];  //只有当点有效，才能安全地访问graph，否则会导致下标越界
        }

        //重载2：获取索引在图中位置的值
        public int getValue(int index) {
            if (index < 0 || index > 159) throw new Exception("坐标越界");
            return graph[index / 10, index % 10];
        }

        //重载3：获取坐标在途中位置的值
        public int getValue(int x, int y) {
            if (x < 0 || x > 15 || y < 0 || y > 9) throw new Exception("坐标越界");
            return graph[x, y];
        }

        //获取图信息
        public string getInfo() {
            string s = "";
            for (int i = 0; i < 16; i++) {
                for (int j = 0; j < 10; j++) {
                    s += graph[i, j] + " ";
                }
                s += "\n";
            }
            return s;
        }

        //消除值全不为0的一行，并将上面所有行下移
        public bool eliminate(out int count) {
            bool needReDraw = false;  //是否产生了行消除
            count = 0;
            int flag;  //是否有消除行
            for (int i = minX; i < 16; i++) {
                flag = 1;
                for (int j = 0; j < 10; j++) {
                    if (graph[i, j] == 0)  flag = 0;  //此行有0，即此行没有满，不用消除
                }
                if (flag == 1) {
                    count++;
                    needReDraw = true;
                    moveLines(i);  //覆盖i行（i上所有行下移）
                }
            }
            return needReDraw;
        }

        //获取minX
        public int getMinX() { return minX; }

        //将参数index行的上面所有行下移，覆盖index行
        public void moveLines(int index) {
            if (index < 0 || index > 15) throw new Exception("行索引错误");
            for (int i = index; i >minX; i--) {
                for (int j = 0; j < 10; j++) {
                    graph[i, j] = graph[i - 1, j];
                }
            }
            for (int i = 0; i < 10; i++)
                graph[minX, i] = 0;  //minX行所有元素清空
            minX++;  //消除行后，minX增加
        }
    }
}
