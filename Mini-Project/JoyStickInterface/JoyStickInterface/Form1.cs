using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Net;
using System.Net.Sockets; //library to use any socket in laptop ...

namespace JoyStickInterface
{
    public partial class Form1 : Form
    {
        public const int MID_POINT = 1500;
        public const int MAX_POINT = 1800;
        public const int MIN_POINT = 1200;

        public const int START_DOWN_ZONE = 40000;
        public const int END_DOWN_ZONE = 65535;

        public const int START_UP_ZONE = 24000;
        public const int END_UP_ZONE = 0;


        int z;
        JoyStick joyStick;
        int flag = 0; //Prevent pilot making error before starting communication....
        Socket SCK;
        EndPoint Local_ip, Remote_ip;

        public Form1()
        {
            InitializeComponent();
            joyStick = new JoyStick(this.Handle);
            joyStick.fireEvent += new JoyStick.StateEventHandler(JoyStickStateChangedHandler);
            SCK = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            SCK.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public void JoyStickStateChangedHandler(object sender, JoystickStateEventArgs e)
        {
            // Motors Array
            int[] esMotor = new int[6] { 1500, 1500, 1500, 1500, 1500, 1500 };
            // Status
            int solenStatus = 0, bluetoothStatus = 0, dir1 = 0, dir2 = 0, pwm = 0;
            // DC Motor
            string DC = " ";

            /* Parameters 
             joyAxis -> Axis A / Axis C / Axis D / Axis E
             joyReads -> values between END_UP_ZONE to END_DOWN_ZONE
             mapPoint -> values between MIN_POINT to MAX_POINT
             */

            int map(int joyAxis, int joyRead1, int joyRead2, int mapPoint1, int mapPoint2)
            {
                return (mapPoint1 + ((joyAxis - joyRead1) * (mapPoint2 - mapPoint1) / (joyRead2 - joyRead1)));
            }

            void changeValues()
            {
                textBox9.Text = Convert.ToString(esMotor[0]);
                textBox10.Text = Convert.ToString(esMotor[1]);
                textBox11.Text = Convert.ToString(esMotor[2]);
                textBox12.Text = Convert.ToString(esMotor[3]);
                textBox13.Text = Convert.ToString(esMotor[4]);
                textBox14.Text = Convert.ToString(esMotor[5]);
                textBox16.Text = Convert.ToString(dir1);
                textBox18.Text = Convert.ToString(dir2);
                textBox19.Text = Convert.ToString(pwm);

                textBox5.Text =
                      "A" + Convert.ToString(esMotor[0])
                    + "B" + Convert.ToString(esMotor[1])
                    + "C" + Convert.ToString(esMotor[2])
                    + "D" + Convert.ToString(esMotor[3])
                    + "E" + Convert.ToString(esMotor[4])
                    + "F" + Convert.ToString(esMotor[5])
                    + "G" + Convert.ToString(solenStatus)
                    + "H" + DC
                    + "I" + Convert.ToString(bluetoothStatus)
                    + "J" + Convert.ToString(dir1)
                    + "K" + Convert.ToString(dir2)
                    + "L" + Convert.ToString(pwm) + "M";

                btnValue1.Checked = e.buttons[0];
                btnValue2.Checked = e.buttons[1];
                btnValue3.Checked = e.buttons[2];
                btnValue4.Checked = e.buttons[3];
                btnValue5.Checked = e.buttons[4];
                btnValue6.Checked = e.buttons[6];
                btnValue7.Checked = e.buttons[7];
                btnValue8.Checked = e.buttons[8];
                btnValue9.Checked = e.buttons[9];
                btnValue10.Checked = e.buttons[10];
                btnValue11.Checked = e.buttons[11];
                btnValue12.Checked = e.buttons[12];
                btnValue13.Checked = e.buttons[13];
                btnValue14.Checked = e.buttons[14];
                btnValue15.Checked = e.buttons[15];
                btnValue16.Checked = e.buttons[16];
                btnValue17.Checked = e.buttons[17];
                povXtxt.Text = Convert.ToString(e.pov[0]);
            }

            if (InvokeRequired)
            {
                this.Invoke(new Action<object, JoystickStateEventArgs>(JoyStickStateChangedHandler), sender, e);
                return;
            }
            Console.WriteLine(z.ToString());
            z++;


            //Forward : Mapping from START_UP_ZONE to END_UP_ZONE >>>> MID_POINT to MAX_POINT.
            if (e.axisD < START_UP_ZONE && e.axisD < e.axisC && e.axisD < END_DOWN_ZONE - e.axisC)
            {
                esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] =
                map(e.axisD, START_UP_ZONE, END_UP_ZONE, MID_POINT, MAX_POINT);
                changeValues();
            }

            //Backward : Mapping from START_DOWN_ZONE to END_DOWN_ZONE >>>> MID_POINT to MIN_POINT.
            else if (e.axisD > START_DOWN_ZONE && e.axisD > e.axisC && e.axisD > END_DOWN_ZONE - e.axisC)
            {
                esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] =
                    map(e.axisD, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MIN_POINT);
            }

            //Left Direction
            else if (e.axisC < START_UP_ZONE && e.axisC < e.axisD && e.axisC < END_DOWN_ZONE - e.axisD)
            {
                esMotor[0] = esMotor[2] =
                    map(e.axisC, START_UP_ZONE, END_UP_ZONE, MID_POINT, MAX_POINT);
                esMotor[1] = esMotor[3] =
                    map(e.axisC, START_UP_ZONE, END_UP_ZONE, MID_POINT, MIN_POINT);
            }

            //Right Direction
            else if (e.axisC > 40000 && e.axisC > e.axisD && e.axisC > END_DOWN_ZONE - e.axisD)
            {
                esMotor[0] = esMotor[2] =
                    map(e.axisC, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MIN_POINT);
                esMotor[1] = esMotor[3] =
                    map(e.axisC, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MAX_POINT);
            }

            //Up Direction
            if (e.axisF < START_UP_ZONE)
            {
                esMotor[4] = esMotor[5] =
                    map(e.axisF, START_UP_ZONE, END_UP_ZONE, MID_POINT, MAX_POINT);
            }

            //Down Direction
            else if (e.axisF > START_DOWN_ZONE)
            {
                esMotor[4] = esMotor[5] =
                    map(e.axisF, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MIN_POINT);
            }

            //Rotate Left
            if (e.axisA < START_UP_ZONE)
            {
                esMotor[1] = esMotor[2] =
                    map(e.axisE, START_UP_ZONE, END_UP_ZONE, MID_POINT, MAX_POINT);
                esMotor[0] = esMotor[3] =
                    map(e.axisE, START_UP_ZONE, END_UP_ZONE, MID_POINT, MIN_POINT);
            }

            //Rotate Right
            else if (e.axisA > START_DOWN_ZONE)
            {
                esMotor[1] = esMotor[2] =
                    map(e.axisE, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MIN_POINT);
                esMotor[0] = esMotor[3] =
                    map(e.axisE, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MAX_POINT);
            }

            //Solenoid
            textBox15.Text = (e.buttons[5]) ? 
                Convert.ToString(solenStatus = 1) :
                Convert.ToString(solenStatus = 0);

            //Bluetooth
            textBox17.Text = (e.buttons[8] == true ?
                Convert.ToString(bluetoothStatus = 1) :
                Convert.ToString(bluetoothStatus = 0));

            while (e.buttons[10])
            {
                //Right DC
                if (e.axisC > START_DOWN_ZONE && e.axisC > e.axisD && e.axisC > END_DOWN_ZONE - e.axisD)
                {
                    dir1 = 1; dir2 = 0; DC = "Right";
                    pwm = (Math.Abs(e.axisC - START_DOWN_ZONE) / (END_DOWN_ZONE - START_DOWN_ZONE)) * 255;
                }

                //Left DC
                else if (e.axisC < START_UP_ZONE && e.axisC < e.axisD && e.axisC < END_DOWN_ZONE - e.axisD)
                {
                    dir1 = 0; dir2 = 1; DC = "Left";
                    pwm = (Math.Abs(e.axisC - START_UP_ZONE) / START_UP_ZONE) * 255;
                }

            }

            changeValues();

            if (flag == 1)
            {
                ASCIIEncoding enc = new ASCIIEncoding();
                byte[] msg = new byte[1500];
                msg = enc.GetBytes(textBox5.Text);

                SCK.Send(msg);
                textBox6.AppendText("ME : " + textBox5.Text + "\r\n");
                textBox5.Clear();
            }
        }

        //Default data.
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "192.168.0.201";
            textBox2.Text = "192.168.0.7";
            textBox3.Text = "8234";
            textBox4.Text = "8002";
        }

        //Default data.
        private void button2_Click(object sender, EventArgs e)
        {
            //Prevent user from using it again 
            flag = 1; // Pilot can use joystick without any error ...
            button1.Enabled = false;
            button2.Enabled = false;
            button2.Text = "Connected";

            Local_ip = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox3.Text));//son (IPEndpoint)...take ip and port && IPAddress.parse convert any input to ip address of laptop
            SCK.Bind(Local_ip);// make lap to take ip address and port ...

            Remote_ip = new IPEndPoint(IPAddress.Parse(textBox2.Text), Convert.ToInt32(textBox4.Text));//son inherit form Endpoint ...
            SCK.Connect(Remote_ip);
            //start communication after setup 
            byte[] buffer = new byte[1500];//store the sending or receiving data...
            AsyncCallback processing = new AsyncCallback(process);//processing is a delegate...process is a function
            SCK.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref Remote_ip, processing, buffer);//BeginReceive is the fn will be called in library

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        void process(IAsyncResult state)
        {
            try
            {
                if (textBox6.InvokeRequired)
                {
                    textBox6.Invoke((MethodInvoker)delegate { process(state); });
                }

                else
                {

                    int size = SCK.EndReceiveFrom(state, ref Remote_ip);//stop receving and give the number of bytes in size ...

                    if (size > 0) // if c# received data ....
                    {
                        //Place of receiving.
                        byte[] ReceivedData = (byte[])state.AsyncState;

                        //Convert byte to string 
                        ASCIIEncoding convert = new ASCIIEncoding();
                        string final_data = convert.GetString(ReceivedData);

                        //Convert byte to string and store in final_data

                        textBox7.Text = final_data.Substring(1, 2);
                        textBox8.Text = final_data.Substring(3, 4);
                        textBox6.AppendText("Friend : " + final_data);
                        textBox6.AppendText("\r\n");
                        // any analysis needed to make processing on data received ....

                    }
                }

                byte[] buffer = new byte[1500];// to receive data again .....
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