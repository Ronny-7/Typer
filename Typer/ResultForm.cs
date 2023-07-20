using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Typer
{
    public partial class ResultForm : Form
    {
        private Typer typer;

        public ResultForm(Typer frm)
        {
            InitializeComponent();
            typer = frm;
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            TimeSpan timeSpan = typer.End - typer.Start;
            typer.Start = DateTime.Now;
            lblNumber.Text = (typer.GetCorrectNumber() + typer.GetErrorNumber()).ToString(); //总字数
            lblCorrect.Text = typer.GetCorrectNumber().ToString(); //正确的字数
            lblError.Text = typer.GetErrorNumber().ToString(); //错误的字数
            lblAccuracy.Text = Math.Round((double)typer.GetCorrectNumber() / (typer.GetCorrectNumber() + typer.GetErrorNumber()) * 100, 2).ToString(); //正确率
            lblCostTime.Text = ((int)(timeSpan.TotalSeconds)).ToString(); //打字耗时
            lblSpeed.Text = Math.Round((double)(typer.GetCorrectNumber()) / (int)(timeSpan.TotalSeconds), 2).ToString(); //打字速率
        }

    }
}
