using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;
using System.Net;
using System.Net.Sockets;

namespace JoyStickInterface
{
    public partial class Form1 : Form
    {
        int z;
        JoyStick joyStick;
        int flag;
        Socket SCK;
        EndPoint Local_ip, Remote_ip;

        public Form1()
        {
            InitializeComponent();
            joyStick = new JoyStick(this.Handle);
            joyStick.fireEvent += new JoyStick.StateEventHandler(JoyStickStateChangedHandler);
            SCK = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            SCK.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            button1.Enabled = false;

        }


        
        private void button4_Click(object sender, EventArgs e)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] msg = new byte[1500];
            msg = enc.GetBytes(txt6ip.Text);

            SCK.Send(msg);

            txt5ip.AppendText("ME : " + txt6ip.Text + "\r\n");
      
            txt6ip.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txt1ip.Text = "192.168.0.201";
            txt3ip.Text = "8234";
            txt4ip.Text = "192.168.0.7";
            txt2ip.Text = "8002";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txt4ip.Text = "192.168.0.1";
            txt2ip.Text = "1";
            txt1ip.Text = "192.168.0.1";
            txt3ip.Text = "2";

        }

        private void button4_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            txt6ip.Text = "";
        }


        public void JoyStickStateChangedHandler(object sender, JoystickStateEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, JoystickStateEventArgs>(JoyStickStateChangedHandler), sender, e);
                return;
            }
            Console.WriteLine(z.ToString());
            z++;

            axisAtxt.Text = Convert.ToString(e.axisACDF[0]);
            axisCtxt.Text = Convert.ToString(e.axisACDF[1]);
            axisDtxt.Text = Convert.ToString(e.axisACDF[2]);
            axisFtxt.Text = Convert.ToString(e.axisACDF[3]);
            axisEtxt.Text = Convert.ToString(e.axisE);
            btn1.Checked = e.buttons[0];
            btn2.Checked = e.buttons[1];
            btn3.Checked = e.buttons[2];
            btn4.Checked = e.buttons[3];
            btn5.Checked = e.buttons[4];
            btn6.Checked = e.buttons[5];
            btn7.Checked = e.buttons[6];
            btn8.Checked = e.buttons[7];
            btn9.Checked = e.buttons[8];
            btn10.Checked = e.buttons[9];
            btn11.Checked = e.buttons[10];
            btn12.Checked = e.buttons[11];
            btn13.Checked = e.buttons[12];
            btn14.Checked = e.buttons[13];
            btn15.Checked = e.buttons[14];
            btn16.Checked = e.buttons[15];
            btn17.Checked = e.buttons[16];
            btn18.Checked = e.buttons[17];
            String temp = "";
            for (int i = 0; i < e.axisACDF.Length; i++)
                temp += Map(e.axisACDF[i]);
            stringValues.Text = temp;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            flag = 1;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true; // Send button
            button1.Text = "Connected"; //Text1

            Local_ip = new IPEndPoint(IPAddress.Parse(txt1ip.Text), Convert.ToInt32(txt3ip.Text));  //Instead of End Point
            SCK.Bind(Local_ip); //Local ip {Start}

            Remote_ip = new IPEndPoint(IPAddress.Parse(txt4ip.Text), Convert.ToInt32(txt2ip.Text));
            SCK.Connect(Remote_ip); // {Connect with}

            byte[] buffer = new byte[1500];
            AsyncCallback processing = new AsyncCallback(process);  // delegate
            SCK.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref Remote_ip, processing, buffer);


        }

        void process(IAsyncResult state)
        {
            try
            {
                if (txt5ip.InvokeRequired)
                {
                    txt5ip.Invoke((MethodInvoker)delegate { process(state); });
                }

                else
                {

                    int size = SCK.EndReceiveFrom(state, ref Remote_ip);

                    if (size > 0)
                    {
                        byte[] ReceivedData = (byte[])state.AsyncState;
                        ASCIIEncoding convert = new ASCIIEncoding();
                        string final_data = convert.GetString(ReceivedData);

                        txt5ip.AppendText("Friend : " + final_data);
                        txt5ip.AppendText("\r\n");



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

        private string Map(int value)
        {
            return Convert.ToString(value * (2000 / 65535));
        }
    }
}
