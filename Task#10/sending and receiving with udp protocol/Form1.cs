using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets; // Library for socket
/*  UDP --> User datagram Protocol
  1.  Real life applications (don't check for errors)
  2. Ethernet Socket
*/
namespace sending_and_receiving_with_udp_protocol
{
    public partial class Form1 : Form
    {

        int flag;
        Socket SCK ;  //Declaration new Socket
        EndPoint Local_ip, Remote_ip; // Local ip -> Laptop, remote SPI Module


        public Form1()
        {
            //COnstruction 1. Developped in running time 2. Has no return 3. The same name of class
            InitializeComponent();  //Initialize design (of form)
            SCK = new Socket(AddressFamily.InterNetwork,/*Type of connection*/SocketType.Dgram,/*Type of connection*/ProtocolType.Udp); //Constructor
            SCK.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            button4.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {

                flag = 1;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = true; // Send button
                button1.Text = "Connected"; //Text1



                Local_ip = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox3.Text));  //Instead of End Point
                SCK.Bind(Local_ip); //Local ip {Start}

                Remote_ip = new IPEndPoint(IPAddress.Parse(textBox4.Text), Convert.ToInt32(textBox2.Text));
                SCK.Connect(Remote_ip); // {Connect with}

                byte[] buffer = new byte[1500];
                AsyncCallback processing = new AsyncCallback(process);  // delegate
                SCK.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref Remote_ip, processing, buffer);

        }

        private void button4_Click(object sender, EventArgs e)
        {
           ASCIIEncoding enc = new ASCIIEncoding();
            byte[] msg = new byte[1500];
            msg = enc.GetBytes(textBox6.Text);

            SCK.Send(msg);

            textBox5.AppendText  ( "ME : " + textBox6.Text + "\r\n");

            textBox6.Clear();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "192.168.0.201";
            textBox3.Text = "8234";
            textBox4.Text = "192.168.0.7";
            textBox2.Text = "8002";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text = "192.168.0.1";
            textBox2.Text = "1";
            textBox1.Text = "192.168.0.1";
            textBox3.Text = "2";

        }

        private void button4_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }



        void process(IAsyncResult state)
        {
            try
            {
                if (textBox5.InvokeRequired)
                {
                    textBox5.Invoke((MethodInvoker)delegate { process(state); });
                }

                else
                {

                    int size = SCK.EndReceiveFrom(state, ref Remote_ip);

                    if (size > 0)
                    {
                        byte[] ReceivedData = (byte[])state.AsyncState;
                        ASCIIEncoding convert = new ASCIIEncoding();
                        string final_data = convert.GetString(ReceivedData);

                        textBox5.AppendText("Friend : " + final_data);
                        textBox5.AppendText("\r\n");



                    }
                }

                byte[] buffer = new byte[1500];         //Start the connection again
                AsyncCallback processing = new AsyncCallback(process);
                SCK.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref Remote_ip, processing, buffer);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());

            }
        }
    }
}
