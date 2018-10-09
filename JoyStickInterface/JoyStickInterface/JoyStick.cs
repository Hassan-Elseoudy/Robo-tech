using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.DirectInput;


namespace JoyStickInterface
{
    public class JoyStick 
    {
        #region Variables
        public delegate void StateEventHandler(object sender, JoystickStateEventArgs e);
        public event StateEventHandler fireEvent;
        private Device joystickDevice;
        private IntPtr window_handle;
        private JoystickState state;

        private AutoResetEvent JOYSTICK_EVENT = new AutoResetEvent(true);

        public int axisA;
        public int axisB;
        public int axisC;
        public int axisD;
        public int axisE;
        public int axisF;
        public int[] pov;
        public bool[] buttons;
        #endregion

        #region Constructor
        public JoyStick(IntPtr window_handle)
        {
            this.window_handle = window_handle;
            while(!AcquireJoystick()){}
            Thread thread = new Thread(new ThreadStart(Start));
            thread.Start();

        }

        #endregion

        private void Start()
        {
            while (true)
            {
                JOYSTICK_EVENT.WaitOne();
                
                if (this.fireEvent == null) continue;
                UpdateVariables();
                this.fireEvent(this, new JoystickStateEventArgs(state));
            }
        }

        #region Acquiring JoyStick
        private bool AcquireJoystick()
        {
            DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);

            if (gameControllerList.Count == 0) return false;

            gameControllerList.MoveNext();
            DeviceInstance deviceInstance = (DeviceInstance)gameControllerList.Current;

            joystickDevice = new Device(deviceInstance.InstanceGuid);
            joystickDevice.SetCooperativeLevel(window_handle, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);

            

            joystickDevice.SetDataFormat(DeviceDataFormat.Joystick);
            joystickDevice.SetEventNotification(JOYSTICK_EVENT);
            joystickDevice.Acquire();

            return true;
        }
        #endregion

        #region Updating JoyStick
        public void Poll()
        {
            try
            {
                joystickDevice.Poll();
                state = joystickDevice.CurrentJoystickState;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\nNo JoyStick Found!");
                AcquireJoystick();
            }
        }
        #endregion

        #region Updating Public Variables
        public void UpdateVariables()
        {
            Poll();
        }
        #endregion
   }

    public class JoystickStateEventArgs :EventArgs
    {
        public JoystickState state{private set; get;}
        public int [] axisACDF = new int [4];
        public int axisB;
        public int axisE;
        public int[] pov { private set; get; }
        public bool[] buttons { private set; get; }
        public JoystickStateEventArgs(JoystickState state)
        {
            this.state = state;
            int[] extraAxis = state.GetSlider();

            axisACDF[0] = state.Rz;
            axisB = state.Rx;
            axisACDF[1] = state.X;
            axisACDF[2] = state.Y;
            axisE = state.Z;
            axisACDF[3] = extraAxis[0];
            // Console.WriteLine(Convert.ToString(axisA));

            byte[] jsButtons = state.GetButtons();
            int i = 0;
            buttons = new bool[jsButtons.Length];

            foreach (byte button in jsButtons)
            {
                buttons[i++] = (button == 128);
            }

            int[] jsPOV = state.GetPointOfView();
            pov = new int[jsPOV.Length];
            pov[0] = jsPOV[0];
        }
    }
}
