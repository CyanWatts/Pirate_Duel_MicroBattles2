using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_Key       // 随机数类
{
    class GetRandom     // 随机数类
    {
        public static Random globalRandomGenerator = GenerateNewRandomGenerator();
        public static Random GenerateNewRandomGenerator()
        {
            globalRandomGenerator = new Random((int)DateTime.Now.Ticks);
            return globalRandomGenerator;
        }
        public static int GetRandomInt()//随机产生0至9的一个整数 
        {
            return globalRandomGenerator.Next(10); 
        }
        public static int GetRandomInt(int max) //随机产生0至max 的一个整数 
        {
            return globalRandomGenerator.Next(max);
        }

    }
}
