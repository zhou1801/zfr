using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace Four_operation_generators
{
    public partial class Form1 : Form
    {
        private int IExercisesNum = 100;                            //100道题也不少了，照顾点孩子吧！！！
        private const int MaxNumOfOperands = 4;                    // 最大操作数
        private bool IsContainDecimal = true;                       // 是否包含小数
        string strQuestionPath = @"C:\";
        string strAnswerPath;
        private bool IsRight;                                       //类型转换结果标志量（用于string转int、double）    
        private bool[] bFourOperator = { true, true, true, true };  //四个操作符选择
        private int ISelection = 0;                                 //难度选择
        private double DMin = -100;                                 //操作数最小值
        private double DMax = 100;                                  //操作数最大值
        public Form1() 
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            IsRight = Int32.TryParse(textBox1.Text, out IExercisesNum); 
            if (IsRight || String.IsNullOrEmpty(textBox1.Text))
            {
                ;
            }
            else
            {
                MessageBox.Show("能不能整点阳间能看懂的数啊！秋梨膏！","错误提示");
            }
        }

        private void Form1_Load(object sender, EventArgs e)             //窗体加载执行初始参数设置
        {
            textBox1.Text = IExercisesNum.ToString();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            comboBox1.SelectedIndex = 0;
            minText.Text = DMin.ToString();
            maxText.Text = DMax.ToString();
        }

        private void txt_GotFocus(object sender, EventArgs e)           //一旦题目量输入口得到焦点，清除显示值
        {
            textBox1.Text = "";
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                bFourOperator[i] = checkedListBox1.GetItemChecked(i);
            }
            if(!bFourOperator[0] && !bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3])
            {
                MessageBox.Show("至少要选择一个算符吧？？？", "错误提示");;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ISelection = comboBox1.Items.IndexOf(comboBox1.Text);
        }

        private void MIN_MAX_LeftFocus(object sender, EventArgs e)
        {
            
            IsRight = Double.TryParse(minText.Text, out DMin);
            if (IsRight || String.IsNullOrEmpty(minText.Text))
            {
                ;
            }
            else
            {
                MessageBox.Show("能不能整点阳间能看懂的数啊！秋梨膏！", "错误提示");
            }
            IsRight = Double.TryParse(maxText.Text, out DMax);
            if (IsRight || String.IsNullOrEmpty(maxText.Text))
            {
                ;
            }
            else
            {
                MessageBox.Show("能不能整点阳间能看懂的数啊！秋梨膏！", "错误提示");
            }
            if(DMin>=DMax)
            {
                MessageBox.Show("喂喂喂！最大值和最小值填反了吧，搞事情？", "错误提示");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                IsContainDecimal = true;
            }
            else
            {
                IsContainDecimal = false;
            }
        }

        private void produce_Click(object sender, EventArgs e)
        {
            if(!CheckProjectList())
            {
                ;
            }
            else
            {
                SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                dialog.Filter = "文件(*.txt)|*.txt";                            //设置对话框保存的文件类型
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//将ok返回默认用户公共对话框
                {
                    strQuestionPath = dialog.FileName;                          //获取文件路径和文件名
                    strAnswerPath = strQuestionPath.Substring(0, strQuestionPath.LastIndexOf(".")) + "_Answer.txt";
                    CreateTxt();
                    textBox2.Text = strQuestionPath;
                    textBox3.Text = strAnswerPath;
                    MessageBox.Show("创建成功！");
                } 
            }
        }

        private bool CheckProjectList()                         //检查项目设置，没有错误生成项目清单，有错返回false
        {
            if (String.IsNullOrEmpty(textBox1.Text)|| !Int32.TryParse(textBox1.Text, out IExercisesNum)
               || IExercisesNum<=0)
            {
                MessageBox.Show("请输入合法题目数量值，生成失败！", "错误提示");
                return false;
            }
            else if(!bFourOperator[0] && !bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3])
            {
                MessageBox.Show("请至少选择一个算符，生成失败！", "错误提示");
                return false;
            }
            else if((!Double.TryParse(maxText.Text, out DMax) || String.IsNullOrEmpty(minText.Text))
                || !Double.TryParse(maxText.Text, out DMax) || String.IsNullOrEmpty(maxText.Text)
                || DMin>=DMax)
            {
                MessageBox.Show("请输入合法的操作数范围区间，生成失败！", "错误提示");
                return false;
            }
            else if(!IsContainDecimal&&(DMax - DMin) <= 1.0)
            {
                MessageBox.Show("此区间无整数,生成失败！", "错误提示");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CreateTxt()                                //生成题目和答案
        {
            FileStream fsQuestion = File.Create(strQuestionPath);
            FileStream fsAnswer = File.Create(strAnswerPath);
            fsQuestion.Close();
            fsAnswer.Close();
            bool brackets = false;              //括号开关
            if(!IsContainDecimal)
            {
                NotContainDecimal();
            }
            else
            {
                ContainDecimal();
            }
        }
        private void ContainDecimal()
        {
            StreamWriter swQuestion = new StreamWriter(strQuestionPath);
            StreamWriter swAnswer = new StreamWriter(strAnswerPath);
            for (int x = 0; x < IExercisesNum; x++)
            {
                ArrayList list = new ArrayList();
                bool Istongji; 
                string opt = "";
                string tmp = "";
                //Random rd = new Random();           //创立随机数种子
                int GetRandomSeed()                   //提高随机数不重复概率的种子生成方法
                {
                    byte[] bytes = new byte[4];
                    System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
                    rng.GetBytes(bytes);
                    return BitConverter.ToInt32(bytes, 0);
                }
                Random rd = new Random(GetRandomSeed());
                int left = rd.Next() % (MaxNumOfOperands - 1);   // 括号左位置
                int right = rd.Next() % (MaxNumOfOperands - left - 1) + left + 1; //括号右位置
                while (left == 0 && right == MaxNumOfOperands - 1)
                {
                    left = rd.Next() % (MaxNumOfOperands - 1);   // 括号左位置
                    right = rd.Next() % (MaxNumOfOperands - left - 1) + left + 1; //括号右位置
                }

                if ((bFourOperator[0] && bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && !bFourOperator[1] && bFourOperator[2] && bFourOperator[3]) || (bFourOperator[0] && !bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && !bFourOperator[1] && bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && !bFourOperator[1] && !bFourOperator[2] && bFourOperator[3]))
                {
                     Istongji = true;
                }
                else
                {
                    Istongji = false;
                }
                while (left == 0 && Istongji)
                {
                    left = rd.Next() % (MaxNumOfOperands - 1);   // 括号左位置
                    right = rd.Next() % (MaxNumOfOperands - left - 1) + left + 1; //括号右位置
                }

                if (bFourOperator[0])
                {
                    opt = opt.Insert(opt.Length, "+");
                }
                if(bFourOperator[1])
                {
                    opt = opt.Insert(opt.Length, "-");
                }
                if(bFourOperator[2])
                {
                    opt = opt.Insert(opt.Length, "*");
                }
                if(bFourOperator[3])
                {
                    opt = opt.Insert(opt.Length, "÷");
                }

                for (int i = 0; i < MaxNumOfOperands; i++)
                {
                    string temp;
                    double a = rd.NextDouble() * (DMax - DMin) + DMin;
                    double b = Math.Round(a, 2);
                    bool Isnegative = false;
                    if ( b < 0 )
                    {
                        Isnegative = true;
                    }
                    temp = Convert.ToString(b);
                    if(Isnegative)
                    {
                        temp = temp.Insert(0, "(");
                        temp = temp.Insert(temp.Length, ")");
                    }
                    if(ISelection == 1)
                    {
                        if (i == left)
                        {
                            temp = temp.Insert(0, "(");
                            temp = temp.Insert(temp.Length, " ");
                        }
                        else if (i == right)
                        {
                            temp = temp.Insert(temp.Length, ")");
                        }
                        else
                        {
                            temp = temp.Insert(temp.Length, " ");
                        }
                    }
                    else
                    {
                        temp = temp.Insert(temp.Length, " ");
                    }
                    if(temp.Length == 2)
                    {
                        temp = temp.Insert(0, " ");
                    }
                    else if(temp.Length == 3)
                    {
                        temp = temp.Insert(0, " ");
                    }
                    tmp = tmp.Insert(tmp.Length, temp);

                    if (i < MaxNumOfOperands - 1)
                    {
                        string option = opt.Substring(rd.Next() % opt.Length, 1);
                        tmp = tmp.Insert(tmp.Length, option);
                        tmp = tmp.Insert(tmp.Length, " ");
                    }
                    else
                        tmp = tmp.Insert(tmp.Length, " = ");
                }
                list.Add(tmp);
                for (int i = 0; i < list.Count; i++)
                {
                    swQuestion.WriteLine(list[i].ToString() + " ");
                }
            }                
            swQuestion.Close();
            swAnswer.Close();
        }

        private void NotContainDecimal()
        {
            StreamWriter swQuestion = new StreamWriter(strQuestionPath);
            StreamWriter swAnswer = new StreamWriter(strAnswerPath);
            for (int x = 0; x < IExercisesNum; x++)
            {
                ArrayList list = new ArrayList();
                bool Istongji;
                string opt = "";
                string tmp = "";
                //Random rd = new Random();           //创立随机数种子
                int GetRandomSeed()                   //提高随机数不重复概率的种子生成方法
                {
                    byte[] bytes = new byte[4];
                    System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
                    rng.GetBytes(bytes);
                    return BitConverter.ToInt32(bytes, 0);
                }
                Random rd = new Random(GetRandomSeed());
                int left = rd.Next() % (MaxNumOfOperands - 1);   // 括号左位置
                int right = rd.Next() % (MaxNumOfOperands - left - 1) + left + 1; //括号右位置
                while (left == 0 && right == MaxNumOfOperands - 1)
                {
                    left = rd.Next() % (MaxNumOfOperands - 1);   // 括号左位置
                    right = rd.Next() % (MaxNumOfOperands - left - 1) + left + 1; //括号右位置
                }

                if ((bFourOperator[0] && bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && !bFourOperator[1] && bFourOperator[2] && bFourOperator[3]) || (bFourOperator[0] && !bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && bFourOperator[1] && !bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && !bFourOperator[1] && bFourOperator[2] && !bFourOperator[3]) || (!bFourOperator[0] && !bFourOperator[1] && !bFourOperator[2] && bFourOperator[3]))
                {
                    Istongji = true;
                }
                else
                {
                    Istongji = false;
                }
                while (left == 0 && Istongji)
                {
                    left = rd.Next() % (MaxNumOfOperands - 1);   // 括号左位置
                    right = rd.Next() % (MaxNumOfOperands - left - 1) + left + 1; //括号右位置
                }

                if (bFourOperator[0])
                {
                    opt = opt.Insert(opt.Length, "+");
                }
                if(bFourOperator[1])
                {
                    opt = opt.Insert(opt.Length, "-");
                }
                if(bFourOperator[2])
                {
                    opt = opt.Insert(opt.Length, "*");
                }
                if(bFourOperator[3])
                {
                    opt = opt.Insert(opt.Length, "÷");
                }

                for (int i = 0; i < MaxNumOfOperands; i++)
                {
                    string temp;
                    bool Isnegative = false;
                    int max = Convert.ToInt32(DMax);
                    int min = Convert.ToInt32(DMin);
                    int a = rd.Next(0, 2);                    
                    int b;
                    if(min < 0 && max < 0)
                    {
                        b = rd.Next(-max, -min + 1) * (-1);
                    }
                    else if(min >= 0 && max >0)
                    {
                        b = rd.Next(min, max + 1);
                    }
                    else
                    {
                        if(a == 0)
                        {
                            b = rd.Next(0, max + 1);
                        }
                        else
                        {
                            b = rd.Next(0 , -min + 1) * (-1);
                        }
                    }
                    if ( b < 0 )
                    {
                        Isnegative = true;
                    }
                    temp = Convert.ToString(b);
                    if(Isnegative)
                    {
                        temp = temp.Insert(0, "(");
                        temp = temp.Insert(temp.Length, ")");
                    }
                    if(ISelection == 1)
                    {
                        if (i == left)
                        {
                            temp = temp.Insert(0, "(");
                            temp = temp.Insert(temp.Length, " ");
                        }
                        else if (i == right)
                        {
                            temp = temp.Insert(temp.Length, ")");
                        }
                        else
                        {
                            temp = temp.Insert(temp.Length, " ");
                        }
                    }
                    else
                    {
                        temp = temp.Insert(temp.Length, " ");
                    }
                    if(temp.Length == 2)
                    {
                        temp = temp.Insert(0, " ");
                    }
                    else if(temp.Length == 3)
                    {
                        temp = temp.Insert(0, " ");
                    }
                    tmp = tmp.Insert(tmp.Length, temp);

                    if (i < MaxNumOfOperands - 1)
                    {
                        string option = opt.Substring(rd.Next() % opt.Length, 1);
                        tmp = tmp.Insert(tmp.Length, option);
                        tmp = tmp.Insert(tmp.Length, " ");
                    }
                    else
                        tmp = tmp.Insert(tmp.Length, " = ");
                }
                list.Add(tmp);
                for (int i = 0; i < list.Count; i++)
                {
                    swQuestion.WriteLine(list[i].ToString() + " ");
                }
            }                
            swQuestion.Close();
            swAnswer.Close();
        }

        private void 版本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("四则运算生成器1.1.1.2020042517_beta\ndesigned by ZhangHao and Yangyingying", "版本");
        }



    }
    
}
