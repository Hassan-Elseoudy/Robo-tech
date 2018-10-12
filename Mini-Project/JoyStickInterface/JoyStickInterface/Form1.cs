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

            //Functions for Mapping ....functions take integer from joystick like (e.axisE,e.axisD.....or axisA)

            int toMAX_Point(int input) =>
                (MID_POINT + ((Math.Abs(input - START_UP_ZONE) / START_UP_ZONE) * (MAX_POINT - MID_POINT)));

            int UndoMAX_Point(int input) =>
                 (MID_POINT - ((Math.Abs(input - START_UP_ZONE) / START_UP_ZONE) * (MID_POINT - MIN_POINT)));

            int toMIN_Point(int input) =>
                (MID_POINT - (((input - START_DOWN_ZONE) / (END_DOWN_ZONE - START_DOWN_ZONE)) * (MID_POINT - MIN_POINT)));

            int UndoMIN_Point(int input) =>
                (MID_POINT + (((input - START_DOWN_ZONE) / (END_DOWN_ZONE - START_DOWN_ZONE)) * (MAX_POINT - MID_POINT)));

            void changeValues()
            {
                textBox9.Text = Convert.ToString(esMotor[0]);
                textBox10.Text = Convert.ToString(esMotor[1]);
                textBox11.Text = Convert.ToString(esMotor[2]);
                textBox12.Text = Convert.ToString(esMotor[3]);
                textBox13.Text = Convert.ToString(esMotor[4]);
                textBox14.Text = Convert.ToString(esMotor[5]);

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
                    + "L" + Convert.ToString(pwm) + "M\r\n";
            }

            if (InvokeRequired)
            {
                this.Invoke(new Action<object, JoystickStateEventArgs>(JoyStickStateChangedHandler), sender, e);
                return;
            }
            Console.WriteLine(z.ToString());
            z++;
            axisAtxt.Text = Convert.ToString(e.axisA);
            axisCtxt.Text = Convert.ToString(e.axisC);
            axisDtxt.Text = Convert.ToString(e.axisD);
            axisEtxt.Text = Convert.ToString(e.axisE);
            axisFtxt.Text = Convert.ToString(e.axisF);

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

            //Forward : Mapping from START_UP_ZONE to END_UP_ZONE >>>> MID_POINT to MAX_POINT.
            if (e.axisD < START_UP_ZONE && e.axisD < e.axisC && e.axisD < END_DOWN_ZONE - e.axisC)
            {
                for (int i = 0; i < esMotor.Length - 2; i++) /* First 4 Motors */
                    esMotor[i] = toMAX_Point(e.axisD);
                changeValues();
            }

            //Backward : Mapping from START_DOWN_ZONE to END_DOWN_ZONE >>>> MID_POINT to MIN_POINT.
            else if (e.axisD > 40000 && e.axisD > e.axisC && e.axisD > END_DOWN_ZONE - e.axisC)
            {
                for (int i = 0; i < esMotor.Length - 2; i++) /* First 4 Motors */
                    esMotor[i] = toMIN_Point(e.axisD);
            }

            //Left Direction
            else if (e.axisC < START_UP_ZONE && e.axisC < e.axisD && e.axisC < END_DOWN_ZONE - e.axisD)
            {
                esMotor[0] = toMAX_Point(e.axisC);
                esMotor[1] = UndoMAX_Point(e.axisC);
                esMotor[2] = toMAX_Point(e.axisC);
                esMotor[3] = UndoMAX_Point(e.axisC);
            }

            //Right Direction
            else if (e.axisC > 40000 && e.axisC > e.axisD && e.axisC > END_DOWN_ZONE - e.axisD)
            {
                esMotor[0] = toMIN_Point(e.axisC);
                esMotor[1] = UndoMIN_Point(e.axisC);
                esMotor[2] = toMIN_Point(e.axisC);
                esMotor[3] = UndoMIN_Point(e.axisC);
            }

            //Up Direction
            if (e.axisE < START_UP_ZONE && e.axisE < e.axisA && e.axisE < END_DOWN_ZONE - e.axisA)
            {
                esMotor[4] = toMAX_Point(e.axisA);
                esMotor[5] = toMAX_Point(e.axisA);
            }

            //Down Direction
            else if (e.axisE > START_DOWN_ZONE && e.axisE > e.axisA && e.axisE > END_DOWN_ZONE - e.axisA)
            {
                esMotor[4] = toMIN_Point(e.axisA);
                esMotor[5] = toMIN_Point(e.axisA);
            }

            //Rotate Left
            if (e.axisA < START_UP_ZONE && e.axisE > e.axisA && e.axisE < END_DOWN_ZONE - e.axisA)
            {
                esMotor[0] = UndoMAX_Point(e.axisE);
                esMotor[1] = toMAX_Point(e.axisE);
                esMotor[2] = toMAX_Point(e.axisE);
                esMotor[3] = UndoMAX_Point(e.axisE);
            }

            //Rotate Right
            if (e.axisA > START_DOWN_ZONE && e.axisE < e.axisA && e.axisE > END_DOWN_ZONE - e.axisA)
            {
                esMotor[0] = UndoMIN_Point(e.axisE);
                esMotor[1] = toMIN_Point(e.axisE);
                esMotor[2] = toMIN_Point(e.axisE);
                esMotor[3] = UndoMIN_Point(e.axisE);
            }

            //Solenoid is closed.
            if (e.buttons[5])
                textBox15.Text = Convert.ToString(solenStatus = 0);

            //Solenoid is open.
            if (e.buttons[6])
                textBox15.Text = Convert.ToString(solenStatus = 1);

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
                    textBox16.Text = Convert.ToString(dir1);
                    textBox18.Text = Convert.ToString(dir2);
                    textBox19.Text = Convert.ToString(pwm);
                }

                //Left DC
                else if (e.axisC < START_UP_ZONE && e.axisC < e.axisD && e.axisC < END_DOWN_ZONE - e.axisD)
                {
                    dir1 = 0; dir2 = 1; DC = "Left";
                    pwm = (Math.Abs(e.axisC - START_UP_ZONE) / START_UP_ZONE) * 255;
                    textBox16.Text = Convert.ToString(dir1);
                    textBox18.Text = Convert.ToString(dir2);
                    textBox19.Text = Convert.ToString(pwm);
                }

            }

            changeValues();

            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] msg = new byte[1500];
            msg = enc.GetBytes(textBox5.Text);

            SCK.Send(msg);
            textBox6.AppendText("ME : " + textBox5.Text + "\r\n");
            textBox5.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Prevent user from using it again 
            flag = 1; // Pilot can use joystick without any error ...
            button1.Enabled = false; 
            button2.Enabled = false;
            button3.Enabled = false;
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

        //Default data. 
        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = "192.168.0.201";
            textBox2.Text = "192.168.0.7";
            textBox3.Text = "8234";
            textBox4.Text = "8002";
        }

        //Default data. 
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "192.168.0.1";
            textBox2.Text = "192.168.0.1";
            textBox3.Text = "2";
            textBox4.Text = "1";

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