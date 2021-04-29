using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_Key   // 计分板
{                          //OKOKOK // 1.
                                    // 一个问题！！！！！考虑出手后等待一段时间再判断结束，考虑一种情形：
                                    // 两人都出刀干掉对方，但时间有些许差别，期望结果是都计一分,
                                    // 即任一方出手后不管结束与否，让画面再绘制一会
                                                
       //TobeImproved      //OKOKOK // 问题：死的一方的动作要停止，死的效果，
                                    // 相关：Bool量、速度、Rectangle
                                    // 进度：死的时候闪一下红色

                                    // 已完成：动作要停止    准备用bool量在KeyDown中判断
                                            // （已解决）加bool量后Bug：同一时刻杀死，会卡住，无法操作，2sSleep、慢动作也没有                                    
                                            // 似乎和bool量无关？是因为Form1.cs约225行判断有且仅有一个干掉了另一方
                                            // 时，若同时杀死，不会进入TimerEndUpdate中
                                                    
                                            // Bug解决：给两个flag各自加一个Preflag，在进入TimerEndUpdate中新增一个
                                            // 条件：两个preflag都是true且现在flag都为false，即同时被杀死
                                                                                                        

                                    // 已完成：死亡效果
                                    //          准备：直接更改Pirate/Knife（如果身上有刀子）的属性（成员变量）



                                    // 2.
                                    // 刀子后面轨迹？

                                    // 3.
                                    // button1 click 中注释一行可改变窗体大小！

                                    // 4.
                                    // 按钮美化

                           //OKOKOK // 5.
                                    // Knife Pirate 的矩形成员变量 是否有用存疑 -> 有用

                                    // 6.
                                    // 绘制时Rectangles获得可以封装一下？

                           //OKOKOK // 7. !!!!!!!!!!!!!
                                    // 刀子根据方向调整位置

                           //OKOKOK // 8. ！！！！！！！！！
                                    // 地面碰撞之后优化一下
                                    // 给Pirate加上脚

                                    // 9.
                                    // ShowBlock也是每次绘制把不动的砖块Draw了->
                                    // 改进考虑：每个地图对应一个背景，不用在绘制block，直接用edge判断碰撞即可
                                    // 不过是否会涉及前后覆盖问题？？

                                    // 10.！！！！！！！！！！！！！
                                    // 边界：（880, 450）
                                    // 目前是从0开始！！ -> 之后可考虑优化
                                    // Game类中边界是否要加？？？


                           //OKOKOK // 11.      21.4.23 17：02
                                    // 跳跃功能 
                                    // 加脚foot！！！！！！！！！,解决头卡在block的问题


           //ToBeObserved  //OKOKOK // 12. !!!!!!!!! 
                                    // 刀子不跟人        // 解决：Jump时刀子跟着一起动 -> 后来发现：不用这两行瞬间移动就行

                           //OKOKOK // 13. 扔刀子
                                    // 遇到问题：扔完刀子 Pirate会变得和刀子一样快？
                                    // 原因：在Knife Update时候直接将Pirate v的引用给了Knife，引用共用内存，之后修改Knife的v也会修改Pirate的v
            //Caution               // 之后注意：Velocity.Gravity（）会不会有相关问题？？？？？？？？？？
                                // 教训Lesson：少用引用直接赋值！封装好

                           //OKOKOK // 14. 刀子速度计算，多方向！！！ 
                                    // Knife.calculate_Speed()
                                    // 目前出手时恒定速度
                                    // 期望改进:像原游戏一样，方法：图片切换 or 图片/像素的旋转
                                    // //海盗内在对方向有一个计时,拿到刀子时候才显示
                                        // 上一行想法不可行：否则两个海岛永远方向一样
                                        // 故改为：捡到刀子随机开始方向
                                                
                                    // 正在实现
                                    // 1.准星的旋转
                                    // 2.基于准星，函数Pirate.ThrowKnife()的更改
                                                


                                    // 15. 刀子旋转
                                    // 实现：图片切换

                           //OKOKOK // 16. 刀子碰撞

                           //OKOKOK // 17. 刀子拾取

                           //OKOKOK // 18. 死亡判断 & 重新开局
                                    // 至少有一个在空中 -> 有可能砍人 ->
                                    // 自己的刀不砍自己: Knife类加一个PrePirate引用
                                                
                           //OKOKOK // 砍死后继续绘制一段时间 && 判断被砍死，即1.所述
                                    // 似乎不用再加一个timer与endingDraw，感觉应该再加些bool和判断即可！！！！！！！！！！1


                                    // 19. Rethink Form1里的函数如何放置（尤其是Timer_Tick中的
                                            
                            //Now   // 20. /*Pirate*/  Knife Move时候加上边界判断
                                    // 为了解决刀子出界问题
                                            
                        //OKOKOK    // 21. 整合音频!!!!!!!!!!!!!!!!!!!!!
                                    // 问题：hit声音太晚
                                    // 不能同时播放
                // One_Key2.0       
                            
                              //Now // 22. 整合图片!!!!!!!!!!!!!!!!!!!!!

                              //Now // 23.随机Strength / 可设置Strength因素 
                                    // 


                              //Now // 24. RL模块
                                    
                           //OKOKOK // 25. 刀子/人落地时后位置不好：
                                    // 预计解决：碰撞后对刀子position_y进行一下修改


                                    // -1. 之后期望的机制：
                                        // (1) 随机的云朵，遮挡刀子和人
                                        // (2) 刮风天气，横向速度也会受恒定影响
                                        // (3) 每次开局随机地图、随机位置（需要更多地图）
                                        // (4) 随机力量
                                            
    class ScoreTable    // 计分板
    {
        public static int ScoreOfPlayer1; // 玩家1分数
        public static int ScoreOfPlayer2; // 玩家2分数

        public static void Score(int player)   // 计分
        {
            if (player == 1)
                ScoreOfPlayer1++;
            else if (player == 2)
                ScoreOfPlayer2++;
            else
                return;
        }

        public static void Init()
        {
            ScoreOfPlayer1 = 0;
            ScoreOfPlayer2 = 0;
        }


        public static int IsEnd()              // 结束判断
        {                               // 1:P1 win  2:P2 win    3:Tie
            if (ScoreOfPlayer1 < 5 && ScoreOfPlayer2 < 5)
                return 0;   // Continue
            else if (ScoreOfPlayer1 >= 5 && ScoreOfPlayer2 < 5)
                return 1;   // 1:P1 win
            else if (ScoreOfPlayer2 >= 5 && ScoreOfPlayer1 < 5)
                return 2;   // 2:P2 win
            else
                return 3;   // Tie
        }
    }
}
