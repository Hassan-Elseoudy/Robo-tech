using System;
using System.Net;
using System.Net.Sockets; //Library to use any socket in laptop ...
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace JoyStickInterface
{
    public partial class Form1 : Form
    {
        #region Variables & Objects
        private const int MID_POINT = 1500;
        private int MAX_POINT = 1800;
        private int MIN_POINT = 1200;
        private int UD_MAX_POINT = 1800;
        private int UD_MIN_POINT = 1200;

        private const int START_DOWN_ZONE = 40000;
        private const int END_DOWN_ZONE = 65535;

        private const int START_UP_ZONE = 24000;
        private const int END_UP_ZONE = 0;

        //Constants
        private double water_denisty = 1000;
        private double gravity = 9.81;

        //Drwaing Points
        private Point p1;
        private Point p2;
        private int stateCounter = 0; // 0 --> Get P1, 1 --> Get P2, 2 Get Answer


        //Reference Length
        private double refVirtualLength = 1.0;
        private double refActualLength = 1.0;

        //Parameters
        private double D1 = 1.0;
        private double D2 = 1.0;
        private double D3 = 1.0;
        private double Length = 1.0;
        private double specific_gravity = 1.0;
        private double lift_capability = 0;
        private double Given_Force = 0;

        //Results (Experimental)
        private double expr_Volume = 0;
        private double expr_Force = 0;
        private double expr_mass = 0;

        //Results (Real)
        private double real_Volume = 0;
        private double real_Force = 0;
        private double real_mass = 0;
        private double difference = 0;

        private int[] esMotor = new int[6] { 1500, 1500, 1500, 1500, 1500, 1500 };

        // Status
        private int micro = 0;
        private int solenStatus = 0, dir1 = 0, dir2 = 0, pwm1 = 0, pwm2 = 0, pneu_flag = 0, light_flag = 0, light = 0;
        public string Data = "";
        public int temp_count = 0; // --> Getting avg temperatue
        public double temp_avg = 0.0;

        //Micro-ROV
        private int dir1_micro = 0, dir2_micro = 0, pwm1_micro = 0, pwm2_micro = 0;

        //Joystick & Communications
        JoyStick joyStick;
        Socket SCK = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
        EndPoint Local_ip, Remote_ip;

        private void btn2_CheckedChanged(object sender, EventArgs e)
        {
            micro++;
            micro %= 2;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                pictureBox1.Image = Image.FromFile(ofd.FileName);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (stateCounter == 2)
            {
                this.refVirtualLength = (Math.Sqrt(Math.Pow(this.p2.X - this.p1.X, 2) + Math.Pow(this.p2.Y - this.p1.Y, 2)));
                this.stateCounter++;
                this.stateCounter %= 3;
                label35.Text = "Select 2 Points"; //For The Next Operation
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (stateCounter == 2)
            {
                D1 = (Math.Sqrt(Math.Pow(this.p2.X - this.p1.X, 2) + Math.Pow(this.p2.Y - this.p1.Y, 2))) * this.refActualLength / this.refVirtualLength;
                label28.Text = D1.ToString("0.#") + " cm";
                this.stateCounter++;
                this.stateCounter %= 3;
                label35.Text = "Select 2 Points"; //For The Next Operation
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (stateCounter == 2)
            {
                D2 = (Math.Sqrt(Math.Pow(this.p2.X - this.p1.X, 2) + Math.Pow(this.p2.Y - this.p1.Y, 2))) * this.refActualLength / this.refVirtualLength;
                label29.Text = D2.ToString("0.#") + " cm";
                this.stateCounter++;
                this.stateCounter %= 3;
                label35.Text = "Select 2 Points"; //For The Next Operation
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (stateCounter == 2)
            {
                D3 = (Math.Sqrt(Math.Pow(this.p2.X - this.p1.X, 2) + Math.Pow(this.p2.Y - this.p1.Y, 2))) * this.refActualLength / this.refVirtualLength;
                label30.Text = D3.ToString("0.#") + " cm";
                this.stateCounter++;
                this.stateCounter %= 3;
                label35.Text = "Select 2 Points"; //For The Next Operation
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (stateCounter == 2)
            {
                Length = (Math.Sqrt(Math.Pow(this.p2.X - this.p1.X, 2) + Math.Pow(this.p2.Y - this.p1.Y, 2))) * this.refActualLength / this.refVirtualLength;
                label31.Text = Length.ToString("0.#") + " cm";
                this.stateCounter++;
                this.stateCounter %= 3;
                label35.Text = "Select 2 Points"; //For The Next Operation
            }
        }

        private void button12_Click(object sender, EventArgs e) =>
            this.refActualLength = Convert.ToDouble(textBox24.Text);

        private void button11_Click(object sender, EventArgs e)
        {

            //Reference Length
            this.refVirtualLength = 1.0;
            this.refActualLength = 1.0;

            //Parameters
            this.D1 = 0.0;
            this.D2 = 0.0;
            this.D3 = 0.0;
            this.Length = 0.0;
            this.specific_gravity = 1.0;

            //Results (Experimental)
            this.expr_Volume = 0;
            this.expr_Force = 0;
            this.expr_mass = 0;

            //Results (Real)
            this.real_Volume = 0;
            this.real_Force = 0;
            this.real_mass = 0;
            this.difference = 0;

            //Labels
            label35.Text = "Select 2 Points!";
            label25.Text = "0";
            label26.Text = "0";
            label27.Text = "0";
            label28.Text = "0";
            label29.Text = "0";
            label41.Text = "0";
            label42.Text = "0";
            label43.Text = "0";
            label44.Text = "0";
            label30.Text = "0";
            label31.Text = "0";
            label32.Text = "0";
            label33.Text = "0";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (stateCounter == 0)
            {
                this.p1 = ((MouseEventArgs)e).Location;
                this.stateCounter++;
                this.stateCounter %= 3;
                label35.Text = "1 Point Left!"; //For The Next Operation
            }

            else if (stateCounter == 1)
            {
                this.p2 = ((MouseEventArgs)e).Location;
                this.stateCounter++;
                this.stateCounter %= 3;
                label35.Text = "0 Points Left!"; //For The Next Operation
            }
        }

        private void button13_Click(object sender, EventArgs e) => //Click On S.G Button    
            specific_gravity = Convert.ToDouble(textBox25.Text);

        private void button22_Click_1(object sender, EventArgs e) =>
            lift_capability = Convert.ToDouble(textBox41.Text);

        private void button24_Click(object sender, EventArgs e) =>
            Given_Force = Convert.ToDouble(textBox42.Text);

        private void button7_Click(object sender, EventArgs e) //Getting Expr_Volume
        {
            expr_Volume = (1.0 / 3.0) * Math.PI * Length * (Math.Pow((D3 / 2.0), 2) + (D3 / 2.0) * (D1 / 2) + Math.Pow((D1 / 2.0), 2)) - (Length * Math.PI * Math.Pow((D2 / 2.0), 2));
            label32.Text = expr_Volume.ToString(("0.#") + " cm^3");
        }

        private void button16_Click(object sender, EventArgs e) //Getting Expr_Weight
        {
            expr_mass = ((specific_gravity * water_denisty) - water_denisty) * expr_Volume * 1e-6;
            label43.Text = expr_mass.ToString(("0.#") + "Kg");
        }

        private void button8_Click(object sender, EventArgs e) //Getting Expr_Force
        {
            expr_Force = ((specific_gravity * water_denisty) - water_denisty) * expr_Volume * 1e-6 * gravity;
            label33.Text = expr_Force.ToString(("0.#") + "N");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            /*  Getting Real Values */
            double D1, D2, D3, Length;
            D1 = Convert.ToDouble(textBox26.Text) * 2;
            D2 = Convert.ToDouble(textBox27.Text) * 2;
            D3 = Convert.ToDouble(textBox28.Text) * 2;
            Length = Convert.ToDouble(textBox29.Text);
            real_Volume = Math.Abs((1.0 / 3.0) * Math.PI * Length * (Math.Pow((D3 / 2.0), 2) + (D3 / 2.0) * (D1 / 2.0) + Math.Pow((D1 / 2.0), 2)) - (Length * Math.PI * Math.Pow((D2 / 2.0), 2)));
            label41.Text = "Real Volume: " + real_Volume.ToString(("0.#") + " cm^3");
            real_mass = ((specific_gravity * water_denisty) - water_denisty) * real_Volume * 1e-6;
            label42.Text = "Real mass: " + real_mass.ToString(("0.#") + " Kg");
            label64.Text = "S.G " + specific_gravity.ToString(("0.#"));

            real_Force = (real_mass * gravity);
            label44.Text = "Real Force: " + real_Force.ToString(("0.#") + " N");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            difference = Given_Force - lift_capability;
            label62.Text = difference.ToString(("0.#") + " N");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox32.Text = "http://192.168.0.88";
            button15.Enabled = false;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri(textBox32.Text);
        }

        private void button18_Click(object sender, EventArgs e) =>
            PythonController.controlShapes();

        private void button19_Click(object sender, EventArgs e) =>
            PythonController.control();

        private void toggle(object sender, EventArgs e)
        {
            if (radioButton13.Checked == true)
            {
                radioButton13.Checked = false;
                radioButton13.Text = "";
                this.UD_MAX_POINT = Convert.ToInt32(textBox39.Text);
                this.UD_MIN_POINT = Convert.ToInt32(textBox40.Text);
                this.MAX_POINT = Convert.ToInt32(textBox35.Text);
                this.MIN_POINT = Convert.ToInt32(textBox36.Text);
                radioButton14.Checked = true;
                radioButton14.Text = "High";
            }
            else
            {
                radioButton14.Checked = false;
                radioButton14.Text = "";
                this.UD_MAX_POINT = Convert.ToInt32(textBox37.Text);
                this.UD_MIN_POINT = Convert.ToInt32(textBox38.Text);
                this.MAX_POINT = Convert.ToInt32(textBox34.Text);
                this.MIN_POINT = Convert.ToInt32(textBox33.Text);
                radioButton13.Checked = true;
                radioButton13.Text = "Low";
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {

            /*  H   I   G   H       M   O   D   E   */
            (textBox39.Text) = "1800"; //Up Down MAX_POINT
            (textBox40.Text) = "1250"; //Up Down MIN_Point
            (textBox35.Text) = "1750"; //Horizontal MAX_Point
            (textBox36.Text) = "1250"; //Horizontal MIN_Point

            /*  -   -   -   -   -   -   -   -   -   -   -   -   -   -   -   -*/


            /*  L   O   W       M   O   D   E   */
            (textBox37.Text) = "1650"; //Up Down MAX_POINT
            (textBox38.Text) = "1350"; //Up Down MIN_Point
            (textBox34.Text) = "1600"; //Horizontal MAX_Point
            (textBox33.Text) = "1400"; //Horizontal MIN_Point
        }

        private void tabControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals(' '))
                toggle(null, null);
            else if (e.KeyChar.Equals('-'))
            {
                if (label65.Text == "Connected")
                {
                    label65.Text = "Not Connected";
                    SCK.Shutdown(SocketShutdown.Both);
                    SCK.Close();
                }
                else
                {
                    button2_Click(null, null);
                    label65.Text = "Connected";         
                }
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            specific_gravity = 7.87;
            label64.Text = "S.G " + specific_gravity.ToString(("0.#"));
        }

        private void button25_Click(object sender, EventArgs e)
        {
            specific_gravity = 7.87;
            label64.Text = "S.G " + specific_gravity.ToString(("0.#"));
        }

        private void button27_Click(object sender, EventArgs e)
        {
            specific_gravity = 8.03;
            label64.Text = "S.G " + specific_gravity.ToString(("0.#"));
        }

        private void button26_Click(object sender, EventArgs e)
        {
            specific_gravity = 7.87;
            label64.Text = "S.G " + specific_gravity.ToString(("0.#"));
        }

        #endregion

        #region Form
        public Form1()
        {
            InitializeComponent();
            joyStick = new JoyStick(this.Handle);
            joyStick.fireEvent += new JoyStick.StateEventHandler(JoyStickStateChangedHandler);
            SCK = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            SCK.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }
        #endregion

        #region Joystick Event

        #region mapping function 
        int map(int joyAxis, int joyRead1, int joyRead2, int mapPoint1, int mapPoint2)
        {
            return (mapPoint1 + ((joyAxis - joyRead1) * (mapPoint2 - mapPoint1) / (joyRead2 - joyRead1)));
        }

        public void JoyStickStateChangedHandler(object sender, JoystickStateEventArgs e)
        {
            #region joystick data 
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, JoystickStateEventArgs>(JoyStickStateChangedHandler), sender, e);
                return;
            }


            if (e.buttons[9])
                toggle(null, null);
            // if(e.buttons[0])

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

            #endregion


            #endregion

            #region joystick regions and buttons 


            //Forward : Mapping from START_UP_ZONE to END_UP_ZONE >>>> MID_POINT to MAX_POINT.
            if (e.axisD < START_UP_ZONE && e.axisD < e.axisC && e.axisD < END_DOWN_ZONE - e.axisC)
            {
                if (micro % 2 == 1)
                {
                    pwm1_micro = map(e.axisD, START_UP_ZONE, END_UP_ZONE, 0, 255);
                    pwm1_micro = pwm1_micro <= 20 ? 0 : pwm1_micro;
                    dir1_micro = 1;
                    esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] = esMotor[4] = esMotor[5] = 1500;
                }
                else
                {
                    pwm1_micro = pwm2_micro = dir1_micro = dir2_micro = 0;
                    esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] =
                map(e.axisD, START_UP_ZONE, END_UP_ZONE, MID_POINT, MAX_POINT);
                }
            }

            //Backward : Mapping from START_DOWN_ZONE to END_DOWN_ZONE >>>> MID_POINT to MIN_POINT.
            else if (e.axisD > START_DOWN_ZONE && e.axisD > e.axisC && e.axisD > END_DOWN_ZONE - e.axisC)
            {
                if (micro % 2 == 1)
                {
                    pwm1_micro = map(e.axisD, START_DOWN_ZONE, END_DOWN_ZONE, 0, 255);
                    pwm1_micro = pwm1_micro <= 20 ? 0 : pwm1_micro;
                    dir1_micro = 0;
                    esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] = esMotor[4] = esMotor[5] = 1500;
                }
                else
                {
                    pwm1_micro = pwm2_micro = dir1_micro = dir2_micro = 0;
                    esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] =
                    map(e.axisD, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MIN_POINT);
                }
            }

            //Left Direction
            else if (e.axisC < START_UP_ZONE && e.axisC < e.axisD && e.axisC < END_DOWN_ZONE - e.axisD)
            {
                if (micro % 2 == 1)
                {
                    pwm2_micro = map(e.axisC, START_UP_ZONE, END_UP_ZONE, 0, 255);
                    pwm2_micro = pwm2_micro <= 20 ? 0 : pwm2_micro;
                    dir2_micro = 0;
                    esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] = esMotor[4] = esMotor[5] = 1500;
                }
                else
                {
                    pwm1_micro = pwm2_micro = dir1_micro = dir2_micro = 0;
                    esMotor[0] = esMotor[2] =
                    map(e.axisC, START_UP_ZONE, END_UP_ZONE, MID_POINT + 30, MAX_POINT - 30);
                    esMotor[1] = esMotor[3] =
                        map(e.axisC, START_UP_ZONE, END_UP_ZONE, MID_POINT + 30, MIN_POINT - 30);
                }
            }

            //Right Direction
            else if (e.axisC > START_DOWN_ZONE && e.axisC > e.axisD && e.axisC > END_DOWN_ZONE - e.axisD)
            {
                if (micro % 2 == 1)
                {
                    pwm2_micro = map(e.axisC, START_DOWN_ZONE, END_DOWN_ZONE, 0, 255);
                    pwm2_micro = pwm2_micro <= 20 ? 0 : pwm2_micro;
                    dir2_micro = 1;
                    esMotor[0] = esMotor[1] = esMotor[2] = esMotor[3] = esMotor[4] = esMotor[5] = 1500;
                }
                else
                {
                    esMotor[0] = esMotor[2] =
                    map(e.axisC, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT + 30, MIN_POINT - 30);
                    esMotor[1] = esMotor[3] =
                        map(e.axisC, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT + 30, MAX_POINT - 30);
                }
            }

            //Up Direction
            if (e.axisF < START_UP_ZONE)
            {

                esMotor[4] = esMotor[5] =
                map(e.axisF, START_UP_ZONE, END_UP_ZONE, MID_POINT, UD_MAX_POINT);
            }

            //Down Direction
            else if (e.axisF > START_DOWN_ZONE)
            {
                esMotor[4] = esMotor[5] =
map(e.axisF, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, UD_MIN_POINT);
            }

            //Rotate Left
            if (e.axisA < START_UP_ZONE)
            {
                esMotor[3] = esMotor[2] =
                map(e.axisA, START_UP_ZONE, END_UP_ZONE, MID_POINT, MAX_POINT);
                esMotor[0] = esMotor[1] =
                    map(e.axisA, START_UP_ZONE, END_UP_ZONE, MID_POINT, MIN_POINT);
            }

            //Rotate Right
            else if (e.axisA > START_DOWN_ZONE)
            {
                esMotor[3] = esMotor[2] =
                map(e.axisA, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MIN_POINT);
                esMotor[0] = esMotor[1] =
                    map(e.axisA, START_DOWN_ZONE, END_DOWN_ZONE, MID_POINT, MAX_POINT);
            }

            //Pneu Arm    

            if (e.buttons[2] && pneu_flag == 0 || e.buttons[0])
            {
                pneu_flag = 1;
                solenStatus = (solenStatus == 0) ? 1 : 0;

            }

            else if (!e.buttons[2])
                pneu_flag = 0;

            //Pneu Arm rotating
            if (e.pov[0] == 9000)
            {
                dir1 = 0;
                pwm1 = 255;
                // "Rotate Clockwise"
            }
            else if (e.pov[0] == 27000)
            {
                dir1 = 1;
                pwm1 = 255;
                // "Rotate Anticlockwise"
            }
            else
            {
                pwm1 = 0;

            }

            //DC Arm open and close
            if (e.pov[0] == 0)
            {
                dir2 = 0;
                pwm2 = 255;
                // "Open"
            }
            else if (e.pov[0] == 18000)
            {
                dir2 = 1;
                pwm2 = 255;
                // "Close"
            }
            else
            {
                pwm2 = 0;

            }

            //light
            if (e.buttons[7] && (light_flag == 0))
            {
                light_flag = 1;

                if (light == 0)
                {
                    light = 1;
                }

                else if (light == 1)
                {
                    light = 0;
                }
            }
            else if (e.buttons[7] == false)
            {
                light_flag = 0;
            }

            #endregion

            #region concatenation 
            Data = "A" + Convert.ToString(micro)
                + "B" + Convert.ToString(esMotor[0])
                + "C" + Convert.ToString(esMotor[1])
                + "D" + Convert.ToString(esMotor[2])
                + "E" + Convert.ToString(esMotor[3])
                + "F" + Convert.ToString(esMotor[4])
                + "G" + Convert.ToString(esMotor[5])
                + "H" + Convert.ToString(dir1)
                + "I" + Convert.ToString(pwm1)
                + "J" + Convert.ToString(dir2)
                + "K" + Convert.ToString(pwm2)
                + "L" + Convert.ToString(solenStatus)
                + "M" + Convert.ToString(light)
                + "N" + Convert.ToString(dir1_micro)
                + "O" + Convert.ToString(dir2_micro)
                + "P" + Convert.ToString(pwm1_micro)
                + "Q" + Convert.ToString(pwm2_micro)
                + "Z";
            #endregion

            #region Sending 
            if (label65.Text == "Connected")
            {
                try
                {
                    ASCIIEncoding enc = new ASCIIEncoding();
                    byte[] msg = new byte[15000];
                    msg = enc.GetBytes(Data);
                    textBox5.AppendText("ME : " + Data);
                    textBox5.AppendText("\r\n");
                    SCK.Send(msg);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
            #endregion

            #region Showing Data 
            textBox9.Text = Convert.ToString(esMotor[0]);
            textBox10.Text = Convert.ToString(esMotor[1]);
            textBox11.Text = Convert.ToString(esMotor[2]);
            textBox12.Text = Convert.ToString(esMotor[3]);
            textBox13.Text = Convert.ToString(esMotor[4]);
            textBox14.Text = Convert.ToString(esMotor[5]);
            textBox16.Text = Convert.ToString(dir1);
            textBox18.Text = Convert.ToString(dir2);
            textBox19.Text = Convert.ToString(pwm1);
            textBox20.Text = Convert.ToString(pwm2);
            textBox21.Text = Convert.ToString(light);
            textBox15.Text = Convert.ToString(solenStatus);
            textBox22.Text = Convert.ToString(pwm1_micro);
            textBox23.Text = Convert.ToString(dir1_micro);
            textBox31.Text = Convert.ToString(pwm2_micro);
            textBox17.Text = Convert.ToString(dir2_micro);
            #endregion

        }

        #endregion

        #region Start Comm 
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
 
                    button1.Enabled = false;
                  //  button2.Enabled = false;
                    Local_ip = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox3.Text));//son (IPEndpoint)...take ip and port && IPAddress.parse convert any input to ip address of laptop
                    SCK.Bind(Local_ip);// make lap to take ip address and port ...

                    Remote_ip = new IPEndPoint(IPAddress.Parse(textBox2.Text), Convert.ToInt32(textBox4.Text));//son inherit form Endpoint ...
                    SCK.Connect(Remote_ip);
                    //start communication after setup 
                    byte[] buffer = new byte[20000];//store the sending or receiving data...
                    AsyncCallback processing = new AsyncCallback(process);//processing is a delegate...process is a function
                    SCK.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref Remote_ip, processing, buffer);//BeginReceive is the fn will be called in library
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Default data
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "192.168.0.201";
            textBox2.Text = "192.168.0.7";
            textBox3.Text = "8234";
            textBox4.Text = "8002";
        }
        #endregion

        #region Receiving
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
                        temp_count++;
                        //Place of receiving.
                        byte[] ReceivedData = (byte[])state.AsyncState;
                        //Convert byte to string 
                        ASCIIEncoding convert = new ASCIIEncoding();
                        string final_data = convert.GetString(ReceivedData);

                        //Convert byte to string and store in final_data
                        textBox6.AppendText("Friend : " + final_data);
                        textBox6.AppendText("\r\n");
                        // any analysis needed to make processing on data received ....
                        int findpH = final_data.IndexOf('A');
                        int findTemp = final_data.IndexOf('B');
                        int findPressure = final_data.IndexOf('C');
                        textBox7.Text = final_data.Substring(findPressure + 1, ((final_data.Length - 1) - findPressure)); //Pressure
                        if (temp_count == 5)
                        {
                            temp_count = 0;
                            textBox8.Text = Convert.ToString(temp_avg); //Temperature
                            temp_avg = 0.0;
                        }
                        textBox30.Text = final_data.Substring(findpH + 1, (findTemp - findpH)); //pH
                        temp_avg += Convert.ToDouble(final_data.Substring(findTemp + 1, (findPressure - findTemp)));
                    }
                }

                byte[] buffer = new byte[15000];// to receive data again .....
                AsyncCallback processing = new AsyncCallback(process);
                SCK.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref Remote_ip, processing, buffer);
            }
            catch (Exception exp)
            {
               MessageBox.Show(exp.ToString());

            }
        }
        #endregion

    }
}