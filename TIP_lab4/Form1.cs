using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TIP_lab4
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> dic;
        private SortedSet<string> ss;
        public Form1()
        {
            dic = new Dictionary<string, string>();
            ss = new SortedSet<string>();
            InitializeComponent();
            defaultmachineLoader();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && textBox1.Text !="")
            {
                try
                {                       
                    dic.Add(textBox3.Text + "," + textBox4.Text + "," + textBox5.Text, textBox6.Text + "," + textBox7.Text + "," + textBox1.Text);
                    listBox1.Items.Add(label1.Text + textBox3.Text + "," + textBox4.Text + "," + textBox5.Text + label4.Text + textBox6.Text + "," + textBox7.Text + "," + textBox1.Text + ")");
                }
                catch(ArgumentException){
                    return;
                }
                
            } else {
                MessageBox.Show("Проверьте ввод данных! Какое-то поле пустое!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                string key = listBox1.SelectedItems[0].ToString();
                var temp = key.Split(label4.Text);
                key = temp[0].Replace(label1.Text, string.Empty);              
                dic.Remove(key);              
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            string chain = textBox2.Text;
            string Exitchain = "`" ;
            string Exitchain1 = "" ;
            string st = "!";
            string st1 = "";
            string currentState = "0";
            string finalState = "f";
           
            textBox8.Text+= "(q" + currentState + "," + chain + "," + st + "," + Exitchain + ") ->";
            while (true)
            {
                string key = "";
                try
                {
                    key = currentState + "," + chain.Substring(0, 1) + "," + st.Substring(0,1);
                }
                catch(ArgumentOutOfRangeException)
                {
                    return;
                }
                if (!dic.ContainsKey(key))
                {
                    MessageBox.Show("Нет такого перехода! " + key + " !!!");
                    textBox8.Text += " X";
                    label18.Text = "Не удачно";
                    return;
                }
                try
                {
                    chain = chain.Substring(1, chain.Length - 1);
                    if(chain.Length == 0)
                        chain = "`";
                }
                catch(ArgumentOutOfRangeException)
                {
                    return;
                }

                string value = dic[key];
                var temp = value.Split(",");
                currentState = temp[0];

                if(temp[1].Equals("`"))
                {
                    try
                    {
                        st1 = st1.Substring(1, st1.Length - 1);
                        st = st.Substring(1, st.Length - 1);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Удаление из пустого стека!!! ");
                        textBox8.Text += " X";
                        label18.Text = "Не удачно";
                        return;
                    }
                }
                else
                {
                    st = temp[1] + st;
                    st1 = temp[1] + st1;
                }
                    
                if (temp[2].Equals("`") != true)
                {
                    Exitchain += temp[2];  
                    Exitchain1 += temp[2];  
                }
                if (chain.Length == 1 && st1.Length == 0)
                {
                    textBox8.Text += "(q" + currentState + "," + chain + "," + st + "," + Exitchain1 + ") -> ";
                    textBox8.Text += "Все круто!";
                    label18.Text = "Удачно";
                    return;
                }
                if(Exitchain1.Length !=0)
                    textBox8.Text += "(q" + currentState + "," + chain + "," + st1 + "," + Exitchain1 + ") -> ";
                else
                    textBox8.Text += "(q" + currentState + "," + chain + "," + st1 + "," + Exitchain + ") -> ";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dic.Clear();
            listBox1.Items.Clear();
        }

        private void defaultmachineLoader()
        {
            dic.Clear();
            dic.Add("0,*,!", "0,!!*,`");
            dic.Add("0,`,+", "0,`,+");
            dic.Add("0,a,!", "0,`,a");
            dic.Add("0,+,!", "0,!!+,`");
            dic.Add("0,`,*", "0,`,*");
            listBox1.Items.Clear();
            listBox1.Items.Add("(q0,*,!)  - > ( q0,!!*,`)");
            listBox1.Items.Add("(q0,`,+)  - > ( q0,`,+)");
            listBox1.Items.Add("(q0,a,!)  - > ( q0,`,a)");
            listBox1.Items.Add("(q0,+,!)  - > ( q0,!!+,`)");
            listBox1.Items.Add("(q0,`,`)  - > ( q0,`,*)");
        }
    }
}