using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery
{
    public partial class mainFrm : Form
    {
        const int SIZE = 6;
        const int MEGABUCKS = 48;
        int [] lotto_ticket = new int[SIZE];
        ListViewItem itm;

        public mainFrm()
        {
            InitializeComponent();
        }

        
        private void generateNumberButton_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            try
            {
                var num_boards = int.Parse(textBox1.Text);
                for (int index = 0; index < num_boards; index++)
                {
                    for (int i = 0; i < SIZE; i++)
                    {
                        int randomNumber = rand.Next(MEGABUCKS + 1);
                        while(randomNumber == 0)
                        {
                            randomNumber = rand.Next(MEGABUCKS + 1);
                        }
                        lotto_ticket[i] = randomNumber;
                        var tmp_num = lotto_ticket[i];
                        var flag = Search(lotto_ticket, tmp_num, i);
                        while (flag)
                        {
                            tmp_num = lotto_ticket[i];
                            flag = Search(lotto_ticket, tmp_num, i);
                            if(flag)
                            {
                                randomNumber = rand.Next(MEGABUCKS + 1);
                                while (randomNumber == 0)
                                {
                                    randomNumber = rand.Next(MEGABUCKS + 1);
                                }
                                lotto_ticket[i] = randomNumber;

                            }
                            
                        }
                    }
                    Array.Sort(lotto_ticket);
                    string[] result = lotto_ticket.Select(x => x.ToString()).ToArray();
                    itm = new ListViewItem(result);
                    listView1.Items.Add(itm);
                }
                       
            }
            catch
            {
                MessageBox.Show("Input must be a number", "Error!");
            }
            
        }
        bool Search(int[] lotto_ticket, int num, int size)
        {
            int count = 0;
            while (count < size)
            {
                if (num == lotto_ticket[count])
                {
                    return (true);
                }                    
                count++;
            }
            return (false);
        }

        private void clearButton_Click(object sender, EventArgs e) => listView1.Items.Clear();

        private void printNumbersButton_Click(object sender, EventArgs e)
        {
            if (!listView1.Items.Count.Equals(0))
            {
                //call method to prompt user for printing.
                DialogResult d = MessageBox.Show("Please ready your printer", "Printer Settings",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                if (d == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    CaptureScreen();
                }
            }
            else
            {
                MessageBox.Show("Nothing to print!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
        }

        private void CaptureScreen()
        {
            //create a dialog object to select printer 
            //and print the document to a file or a printer.
            try
            {
                PrintDialog printDialog1 = new PrintDialog();
                printDialog1.Document = printDocument1;
                DialogResult result = printDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //print the current page.  Only good for the current page. If listview 
            //contains more records than shown on the page, you dont see those records.
            //So method is just good for doing a screen capture of the control as it looks
            //at time of print.
            Bitmap bitmap = new Bitmap(listView1.Width, listView1.Height);
            listView1.DrawToBitmap(bitmap, listView1.ClientRectangle);
            e.Graphics.DrawImage(bitmap, new Point(175, 150));
        }
    }

}
