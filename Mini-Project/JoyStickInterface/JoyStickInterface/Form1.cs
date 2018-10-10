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
        public const int END = 1800; // End PWM
        public const int START = 1200; // START PWM
        public const int MAX = 65535; // MAX VALUE OF THE MAPPING FUNCTION (RIGHT, BELOW)
        public const int MIN = 24000; // MIN VALUE OF THE MAPPING FUNCTION (LEFT,UP)

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

        public void JoyStickStateChangedHandler(object sender, JoystickStateEventArgs e)
        {
            int es1 = 1500, es2 = 1500, es3 = 1500, es4 = 1500, es5 = 1500, es6 = 1500, p, motorValue;
            int [] esMotor = new int[6]{1500,1500,1500,1500,1500,1500};
            string Data, e1, e2, e3, e4, e5, e6;
            Data = " ";

            int mapTheMotor(int input)
            {
                //maximum value (RIGHT / UP)
                if (input == 0) 
                    return START;
            else
                return input = (input > 40000 && input < MAX) ? (MAX * 1500 / END) : (MAX * START / 1500);

            }

            /* int increasePWM24000()
            {
                p = 1500 + ((Math.Abs(e.axisD - 24000) / 24000) * 300); return p;

            }
            int increasePWM40000()
            {
                p = 1500 + (((e.axisC - 40000) / (65535 - 40000)) * 300); return p;

            }
            int decrease40000()
            {
                p = 1500 - (((e.axisD - 40000) / (65535 - 40000)) * 300); return p;

            }
            int decrease24000()
            {
                p = 1500 - ((Math.Abs(e.axisC - 24000) / 24000) * 300); return p;
            } */
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
            axisEtxt.Text = Convert.ToString(e.axisE);
            axisFtxt.Text = Convert.ToString(e.axisACDF[3]);



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
            povXtxt.Text = Convert.ToString(e.pov[0]);



            //axis ACDF [] --> A[0] C[1] D[2] F[3]

            if (e.axisACDF[2] < 24000 && e.axisACDF[2] < e.axisACDF[1] && e.axisACDF[2] < 65535 - e.axisACDF[1])
            {    //forward .....
                //mapping equation from 0 to 24000 >>>>> 1500 to 1800 ....

                motorValue = mapTheMotor((int)e.axisACDF[2]); //Arguments(Axis D)
                
                /*First 4 Motors*/
                for (int i = 0; i < esMotor.Length - 2; i++)
                    esMotor[i] = motorValue;

                for (int i = 0; i < e.axisACDF.Length; i++) {
                    txt5ip.AppendText(Convert.ToString('A'+i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");
            }

            else if (e.axisACDF[2] > 40000 && e.axisACDF[2] > e.axisACDF[1] && e.axisACDF[2] > 65535 - e.axisACDF[1])
            { //backward....
              //mapping from 40000 to 65535 >>>> 1500 to 1200.... 
                motorValue = mapTheMotor((int)e.axisACDF[2]); //Arguments(Axis D)

                /*First 4 Motors*/
                for (int i = 0; i < esMotor.Length - 2; i++)
                    esMotor[i] = motorValue;

                for (int i = 0; i < e.axisACDF.Length; i++)
                {
                    txt5ip.AppendText(Convert.ToString('A' + i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");
            }

            else if (e.axisACDF[1] < 24000 && e.axisACDF[1] < e.axisACDF[2] && e.axisACDF[1] < 65535 - e.axisACDF[2])
            { //left....

                esMotor[1] = mapTheMotor(e.axisACDF[1]);
                esMotor[3] = mapTheMotor(e.axisACDF[1]);

                for (int i = 0; i < e.axisACDF.Length; i++)
                {
                    txt5ip.AppendText(Convert.ToString('A' + i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");

            }

            else if (e.axisACDF[2] > 40000 && e.axisACDF[1] > e.axisACDF[2] && e.axisACDF[1] > 65535 - e.axisACDF[2])
            { //right....

                esMotor[1] = mapTheMotor(e.axisACDF[1]);
                esMotor[3] = mapTheMotor(e.axisACDF[1]);

                for (int i = 0; i < e.axisACDF.Length; i++)
                {
                    txt5ip.AppendText(Convert.ToString('A' + i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");

            }
            if (e.axisE < 24000 && e.axisE < e.axisACDF[0] && e.axisE < 65535 - e.axisACDF[0])
            {   //up....
               
                for (int i = 0; i < e.axisACDF.Length; i++)
                {
                    txt5ip.AppendText(Convert.ToString('A' + i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");
            }
            else if (e.axisE > 40000 && e.axisE > e.axisACDF[0] && e.axisE > 65535 - e.axisACDF[0])
            {   //down....
                for (int i = 0; i < e.axisACDF.Length; i++)
                {
                    txt5ip.AppendText(Convert.ToString('A' + i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");

            }
            if (e.axisACDF[0] < 24000 && e.axisE > e.axisACDF[0] && e.axisE < 65535 - e.axisACDF[0])
            {
                //rotate left .....
                for (int i = 0; i < e.axisACDF.Length; i++)
                {
                    txt5ip.AppendText(Convert.ToString('A' + i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");

            }
            if (e.axisACDF[0] > 40000 && e.axisE < e.axisACDF[0] && e.axisE > 65535 - e.axisACDF[0])
            {
                //rotate right .....
                
                for (int i = 0; i < e.axisACDF.Length; i++)
                {
                    txt5ip.AppendText(Convert.ToString('A' + i));
                    txt5ip.AppendText(Convert.ToString(e.axisACDF[i]));
                }
                txt5ip.AppendText("G\r\n");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            flag = 1;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
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

        private void arm1_Click(object sender, EventArgs e)
        {

        }

        private string Map(int value)
        {
            return Convert.ToString(value * (2000 / 65535));
        }
    }
}
