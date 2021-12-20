using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TIP_lab4
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> dictionary;
        private SortedSet<string> ss;
        public Form1()
        {
            dictionary = new Dictionary<string, string>();
            ss = new SortedSet<string>();
            InitializeComponent();
            defaultmachineLoader();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "" && textBox1.Text !="")
            {
                try
                {
                    dictionary.Add(textBox3.Text + "," + textBox4.Text + "," + textBox5.Text, textBox6.Text + "," + textBox7.Text + "," + textBox1.Text);
                    listBox1.Items.Add(label1.Text + textBox3.Text + "," + textBox4.Text + "," + textBox5.Text + label4.Text + textBox6.Text + "," + textBox7.Text + "," + textBox1.Text + ")");
                }
                catch(ArgumentException){
                    return;
                }
            } else {
                MessageBox.Show("Обнаружено пустое поле. Проверьте ввод данных.");
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                string key = listBox1.SelectedItems[0].ToString();
                var temp = key.Split(label4.Text);
                key = temp[0].Replace(label1.Text, string.Empty);              
                dictionary.Remove(key);              
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void translateButton_Click(object sender, EventArgs e)
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
                if (!dictionary.ContainsKey(key))
                {
                    MessageBox.Show("Ошибка: неизвестный переход " + key + " !");
                    textBox8.Text += "Ошибка";
                    resultLabel.Text = "Ошибка преобразования";
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

                string value = dictionary[key];
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
                        MessageBox.Show("Ошибка: удаление из пустого стека! ");
                        textBox8.Text += "Ошибка";
                        resultLabel.Text = "Ошибка преобразования";
                        return;
                    }
                }
                else
                {
                    st = temp[1] + st;
                    st1 = temp[1] + st1;
                }
                    
                if (!temp[2].Equals("`"))
                {
                    Exitchain += temp[2];  
                    Exitchain1 += temp[2];  
                }
                if (chain.Length == 1 && st1.Length == 0)
                {
                    textBox8.Text += "(q" + currentState + "," + chain + "," + st + "," + Exitchain1 + ") -> ";
                    textBox8.Text += "Успешно";
                    resultLabel.Text = "Успешно преобразовано";
                    return;
                }
                if(Exitchain1.Length !=0)
                    textBox8.Text += "(q" + currentState + "," + chain + "," + st1 + "," + Exitchain1 + ") -> ";
                else
                    textBox8.Text += "(q" + currentState + "," + chain + "," + st1 + "," + Exitchain + ") -> ";
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            dictionary.Clear();
            listBox1.Items.Clear();
        }

        private void defaultmachineLoader()
        {
            dictionary.Clear();
            dictionary.Add("0,*,!", "0,!!*,`");
            dictionary.Add("0,`,+", "0,`,+");
            dictionary.Add("0,a,!", "0,`,a");
            dictionary.Add("0,+,!", "0,!!+,`");
            dictionary.Add("0,`,*", "0,`,*");
            listBox1.Items.Clear();
            listBox1.Items.Add("(q0,*,!)->(q0,!!*,`)");
            listBox1.Items.Add("(q0,`,+)->(q0,`,+)");
            listBox1.Items.Add("(q0,a,!)->(q0,`,a)");
            listBox1.Items.Add("(q0,+,!)->(q0,!!+,`)");
            listBox1.Items.Add("(q0,`,`)->(q0,`,*)");
        }

        private void loadDefaultAutomatButton_Click(object sender, EventArgs e)
        {
            defaultmachineLoader();
        }

        private void addFromFileButton_Click(object sender, EventArgs e)
        {
            string[] textAutomat = File.ReadAllLines(@"D:\" + filePath.Text);
            dictionary.Clear();
            listBox1.Items.Clear();
            for (int i = 0; i < textAutomat.Length; i++)
            {
                addToDictionary(textAutomat[i]);
                listBox1.Items.Add(textAutomat[i]);
            }
        }

        private void addToDictionary(string text)
        {
            string text1 = text.Substring(2, text.IndexOf(')') - 2);
            string text2buf = text.Substring(text.LastIndexOf('('));
            string text2 = text2buf.Substring(2, text2buf.IndexOf(')') - 2);
//            listBox1.Items.Add(text1);
//            listBox1.Items.Add(text2);
            dictionary.Add(text1, text2);
        }
    }
}