﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Typer
{
    public partial class Typer : Form
    {
        private char[] arr = new char[62] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
        private int number,numberOfCharacter; //显示框内字符数字个数
        DateTime timeOfStart; //记录开始时间
        DateTime timeOfEnd; //记录结束时间
        private int correctNumber=0, errorNumber=0; //打字区域内正确个数和错误个数

        public DateTime Start //属性
        {
            set
            {
                timeOfStart = value;
            }
            get
            {
                return timeOfStart;
            }
        }

        public DateTime End //属性
        {
            set
            {
                timeOfEnd = value;
            }
            get
            {
                return timeOfEnd;
            }
        }

        public int GetCorrectNumber()
        {
            return correctNumber;
        }

        public int GetErrorNumber()
        {
            return errorNumber;
        }
 
        public Typer()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        //为txtNumber输入焦点
        private void Typer_Load(object sender, EventArgs e)
        {
            txtBNumber.Focus();
        }

        //弹出随机字符
        private void txtBNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Regex regex = new Regex("^[0-9]*$");
                if (!regex.IsMatch(txtBNumber.Text))
                {
                    MessageBox.Show("输入数据不合法！");
                    txtBNumber.Focus();
                    return;
                }
                if (txtBNumber.Text == "")
                {
                    MessageBox.Show("请输入想要随机生成数字或字符的个数！");
                    txtBNumber.Focus();
                    return;
                }
                numberOfCharacter = number = Int32.Parse(txtBNumber.Text);
                Random rd = new Random();
                for (int i = 1; i <= number; i++)
                {
                    richTextBoxTop.Text += arr[rd.Next(0, 62)].ToString();
                }

                correctNumber = errorNumber = 0;
                richTextBoxBottom.Focus();
                SetTime();
                timerStartChoice(trackBar.Value);
                btnOpenFile.Enabled = false;
                btnRandomOpen.Enabled = false;
                btnSubmit.Enabled = true;
                timeOfStart = DateTime.Now;
            }
        }

        //打开电脑里的指定文件
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "(*.*)allfiles|*.*|*.cs|*.cs";
            if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBoxTop.LoadFile(of.FileName, RichTextBoxStreamType.PlainText);
                numberOfCharacter = number = richTextBoxTop.TextLength;
                correctNumber = errorNumber = 0;
                richTextBoxBottom.Focus();
                txtBNumber.Enabled = false;
                btnOpenFile.Enabled = false;
                btnSubmit.Enabled = true;
                timeOfStart = DateTime.Now;
                timerStartChoice(trackBar.Value);
            }
        }

        //随机打开项目里存好的文件
        private void btnRandomOpen_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            richTextBoxTop.LoadFile(@"txtFiles\" + rd.Next(1, 11).ToString() + ".txt", RichTextBoxStreamType.PlainText);
            numberOfCharacter = number = richTextBoxTop.TextLength;
            correctNumber = errorNumber = 0;
            richTextBoxBottom.Focus();
            txtBNumber.Enabled = false;
            btnOpenFile.Enabled = false;
            btnSubmit.Enabled = true;
            timeOfStart = DateTime.Now;
            timerStartChoice(trackBar.Value);
        }

        //显示倒计时
        private void SetTime()
        {
            int h, m, s;
            string a,b,c;
            h = number / 3600;
            m = number % 3600 / 60;
            s = number % 60;

            if(h<=9)
                a="0"+h.ToString();
            else a = h.ToString();
            if (m  <= 9)
                b = "0" + m.ToString();
            else b = m.ToString();
            if (s  <= 9)
                c = "0" + s.ToString();
            else c = s.ToString();

            lblTime.Text = a + ":" + b + ":" + c;
        }

        //计时器
        private void timer_Tick(object sender, EventArgs e)
        {
            if (number == 0)
            {
                timeOfEnd = DateTime.Now;
                timerEndChoice(trackBar.Value);
                MessageBox.Show("时间到！");
                Submit();
                lblTime.Text = "";
                richTextBoxTop.Clear();
                richTextBoxBottom.Clear();
                btnSubmit.Enabled = false;
                txtBNumber.Enabled = true;
                return;
            }
            timerStartChoice(trackBar.Value);
            number--;
            SetTime();
        }

        //将打字区域里对应错误的字段标记成红色
        private void richTextBoxBottom_TextChanged(object sender, EventArgs e)
        {
            int len = richTextBoxBottom.TextLength;
            if (len >= 1 && len <= numberOfCharacter)
                if (richTextBoxBottom.Text[len - 1] != richTextBoxTop.Text[len - 1])
                {
                    richTextBoxBottom.Select(len - 1, 1);
                    richTextBoxBottom.SelectionColor = Color.Red;
                    richTextBoxBottom.Select(len, 0);
                    errorNumber++;
                }
                else 
                {
                    richTextBoxBottom.Select(len - 1, 1);
                    richTextBoxBottom.SelectionColor = Color.Black;
                    richTextBoxBottom.Select(len, 0);
                    correctNumber++;
                }
        } 

        //点击提交按钮
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            timeOfEnd = DateTime.Now;
            timerEndChoice(trackBar.Value);
            if (MessageBox.Show("你确认交卷吗？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
            {
                Submit();
                lblTime.Text = "";
                richTextBoxTop.Clear();
                richTextBoxBottom.Clear();
                btnSubmit.Enabled = false;
                txtBNumber.Enabled = true;
                return;
            }
            timerStartChoice(trackBar.Value);
        }

        //打开结果窗口
        private void Submit()
        {
            btnOpenFile.Enabled = true;
            btnRandomOpen.Enabled = true;
            ResultForm frm = new ResultForm(this);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        //选择timer，即选择对应时间快慢速率
        private void timerStartChoice(int num)
        {
            switch (num)
            {
                case 1: 
                    timer1.Enabled = true;
                    timer2.Enabled = false;  //在打字的时候，可以随意拖动滑条调整速率，打开对应timer，关闭其它timer
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                    timer5.Enabled = false;
                    break;
                case 2:
                    timer2.Enabled = true;
                    timer1.Enabled = false;
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                    timer5.Enabled = false;
                    break;
                case 3:
                    timer3.Enabled = true;
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer4.Enabled = false;
                    timer5.Enabled = false;
                    break;
                case 4:
                    timer4.Enabled = true;
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    timer5.Enabled = false;
                    break;
                case 5:
                    timer5.Enabled = true;
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                    break;
            }
        }

        //关闭timer
        private void timerEndChoice(int num)
        {
            switch (num)
            {
                case 1:
                    timer1.Enabled = false;
                    break;
                case 2:
                    timer2.Enabled = false;
                    break;
                case 3:
                    timer3.Enabled = false;
                    break;
                case 4:
                    timer4.Enabled = false;
                    break;
                case 5:
                    timer5.Enabled = false;
                    break;
            }
        }

        //点击重置按钮
        private void btnReset_Click(object sender, EventArgs e)
        {
            richTextBoxTop.Clear();
            richTextBoxBottom.Clear();
            timerEndChoice(trackBar.Value);
            txtBNumber.Enabled = true;
            txtBNumber.Clear();
            txtBNumber.Focus();
            btnOpenFile.Enabled = true;
            btnRandomOpen.Enabled = true;
            btnSubmit.Enabled = false;
            lblTime.Text = "";
            correctNumber = errorNumber = 0;
            trackBar.Value = 2;
        }
    }
}
