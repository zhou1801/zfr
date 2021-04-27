# zfr
#include<stdio.h>
#include<stdlib.h>
#include<stdio.h>     // 随机函数的头文件
#include<iostream>  //输入输出流吧      c也可以 cout  换成printf 就好了
#include<conio.h>
#include<ctime>
using namespace std;
char s[1000][1000];
int N = 28, M = 80, direction = 75, k = 77, grade = 0;      //n, m是地图的大小 ，dir和是方向  ， grade是成绩     
int head = 4, tail = 1, leng, x, y, z = 1000, game = 1;
//head ,tail 是蛇头蛇尾的位置
struct note
{
int x, y;
}snake[1000000];     //蛇的结构体

void random()            //地图上随机出现蛇的食物
{
srand((unsigned int)time(NULL));

x = rand() % N;
y = rand() % M;

while (x == snake[head].x && y == snake[head].y) //判断食物是否会被随机到蛇身上   如果是重新随机
{
x = rand() % N;
y = rand() % M;
}
s[x][y] = &apos;*&apos;;
}
void start()  //初始化函数
{
for (int i = 0; i <= N; i++)
{
for (int j = 0; j <= M; j++)
{
s[i][j] = &apos; &apos;;
if (j == 0 || j == M) s[i][j] = &apos;|&apos;;

if (i == 0 || i == N) s[i][j] = &apos;-&apos;;
}
}
s[1][1] = s[1][2] = s[1][3] = &apos;*&apos;;
s[1][4] = &apos;#&apos;;

snake[1].x = 1, snake[1].y = 1;
snake[2].x = 1, snake[2].y = 2;
snake[3].x = 1, snake[3].y = 3;
snake[4].x = 1, snake[4].y = 4;
}

int gameover()          //游戏结束函数   但是蛇可以自己吃自己bug没解决
{
if (snake[head].x <= 0 || snake[head].x >= N || snake[head].y <= 0 || snake[head].y >= M)     return 0; //超过边界  就结束
return 1;
}
void display()   //输出函数
{
if (x == snake[head].x && y == snake[head].y)//吃到食物就
{
tail--;    grade += 20;
s[snake[tail].x][snake[tail].y] = &apos;*&apos;;
random();
}

system("cls"); // 清屏 
z = 1e4;
for (int i = 0; i <= N; i++)
{
puts(s[i]);
}
while (z--);
}

void  f()  //方向函数吧      不知道起名啥了 
{
direction = k;
head++;      //在控制台中向上是72  向下是80  等等  
if (direction == 72)     snake[head].x = snake[head - 1].x - 1, snake[head].y = snake[head - 1].y;                    //向上
if (direction == 80)    snake[head].x = snake[head - 1].x + 1, snake[head].y = snake[head - 1].y;                        //向下
if (direction == 75)    snake[head].x = snake[head - 1].x, snake[head].y = snake[head - 1].y - 1;                           //向左
if (direction == 77)    snake[head].x = snake[head - 1].x, snake[head].y = snake[head - 1].y + 1;                    //向右
s[snake[tail].x][snake[tail].y] = &apos; &apos;;
tail++;
s[snake[head].x][snake[head].y] = &apos;#&apos;;
s[snake[head - 1].x][snake[head - 1].y] = &apos;*&apos;;
if (!gameover())
{
game = 0;
system("cls"); // 清屏 
cout << endl << endl;;
cout << "\t\t\t\t\t" << "你的得分是：" << grade;
cout << "\n\n\n\n\n\n\n\n\n\n";
}
else display();
}

int main()
{
cout << "\n\n\n\n\n\t\t\t 欢迎进入贪吃蛇游戏!" << endl;//欢迎界面;  
cout << "\n\n\n\t\t\t 按任意键马上开始。。。" << endl;//准备开始;;  
_getch();     //和getchar类似   得到键盘的一个字符  不会的百度哦

system("cls"); // 清屏 
start();
display();
random();
while (1)
{
if (!game) break;
//无按键   继续运动 
if (_kbhit())                       // _kbhit()判断是否有输入
{
k = _getch();
}
else f();                  //  有按键   转向运动
}
}

