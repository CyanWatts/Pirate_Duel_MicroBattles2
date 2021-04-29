using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_Key   // 速度类
{
    class Velocity  // 速度类          // x, y类内使用，外部用X，Y
    {
        private int x, y;
        public static int init_v_y = 2;
        public static Velocity gravity = new Velocity(0, init_v_y);    // 重力每次对空中单位y方向速度增加恒定值


        public Velocity()
        {

        }

        public Velocity(int setX, int setY)
        {
            x = setX;
            y = setY;
        }

        public static Velocity Gravity(Velocity v)    // !!!!!!!!!!!!!!!!!!!!
        {
            //v = v + gravity;
            return v + gravity;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        //运算符+的重载
        public static Velocity operator +(Velocity v1, Velocity v2)
        {
            return new Velocity(v1.X + v2.X, v1.Y + v2.Y);
        }

        //运算符-的重载
        public static Velocity operator -(Velocity v1, Velocity v2)
        {
            return new Velocity(v1.X - v2.X, v1.Y - v2.Y);
        }

    }
}
