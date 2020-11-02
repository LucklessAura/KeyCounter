using System;
using System.Threading;
using System.Timers;
using SharpDX.XInput;


namespace KeyCounter
{
    /// <summary>
    /// Class providing gamepad connection and button press handlers
    /// </summary>
    public class GamepadClass
    {
        private static Controller _controller;

        private bool _connected = false;

        private static System.Timers.Timer _handleGamepadTimer;
        private static System.Timers.Timer _connectGamepadTimer;


        public delegate void GamepadEvent();

        public event GamepadEvent OnGamepadFoundStatus;
        public event GamepadEvent OnGamepadDisconnectStatus;
        public event GamepadEvent OnNoGamepadFoundStatus;

        private Thread thread;


        /// <summary>
        /// Start thread to connect a gamepad and handle the button presses after that 
        /// <para>If no controller is connected when this is called the <c>OnNoGamepadFoundStatus</c> event is raised</para>
        /// </summary>
        public void Start()
        {
            if ((_controller == null || _controller.IsConnected == false) && _connected == false)
            {
                OnNoGamepadFoundStatus();
                thread = new Thread(SetUpConnectionTimer);
                thread.Start();
            }
        }

        /// <summary>
        /// Stops the connection tries and the handler and sets proprieties accordingly
        /// </summary>
        public void Stop()
        {
            _handleGamepadTimer?.Stop();
            _connectGamepadTimer?.Stop();
            _connected = false;
            _controller = null;
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            thread = null;
        }

        /// <summary>
        /// UpdateTimeInvoker up and starts a Timer to try to connect with a gamepad, this happens ever 1 sec
        /// </summary>
        private void SetUpConnectionTimer()
        {
            _connectGamepadTimer = new System.Timers.Timer(1000);
            
            _connectGamepadTimer.Elapsed += ConnectGamepad;
            
            _connectGamepadTimer.Enabled = true;
        }

        /// <summary>
        /// The handler that runs every second until Stop is called or until a gamepad is connected.
        /// <para> When a connection is established the connection timer is stopped and the handler timer is started also raises
        /// the <c>OnGamepadFoundStatus</c> envent</para>
        /// </summary>
        /// <param name="state"></param>
        /// <param name="e"></param>
        private void ConnectGamepad(Object state,ElapsedEventArgs e)
        {
            if (! _connected)
            {
                _controller = new Controller(UserIndex.One);
                _connected = _controller.IsConnected;
            }
            else
            {
                _connectGamepadTimer.Stop();
                this.SetUpKeysChecking();
                OnGamepadFoundStatus();
            }
            
        }

        /// <summary>
        /// UpdateTimeInvoker up and starts a Timer to handle gamepad key presses, the handle function is called aprox. 60 times a sec
        /// </summary>
        private void SetUpKeysChecking()
        {
            _handleGamepadTimer = new System.Timers.Timer(15);

            _handleGamepadTimer.Elapsed += CheckButtonPress;

            _handleGamepadTimer.Enabled = true;
        }

        /// <summary>
        /// Check if a key was pressed on the gamepad
        /// </summary>
        /// <returns>the key press if it exists</returns>
        public Keystroke ReturnKeystroke()
        {
            Keystroke keystroke = new Keystroke();
            _controller.GetKeystroke(DeviceQueryType.Gamepad, out keystroke);
            return keystroke;
        }

        /// <summary>
        /// Adds the key pressed to the corresponding dictionary of the CurrentProfile,
        /// <para>
        /// if it detects that the Gamepad has been disconnected it raises the <c>OnGamepadDisconnectStatus</c>
        /// and stops the handler for key presses and restarts the process to connect the gamepad 
        /// </para>
        /// </summary>
        /// <param name="state"></param>
        /// <param name="e"></param>
        public void CheckButtonPress(Object state, ElapsedEventArgs e)
        {
            if (!_controller.IsConnected)
            {
                Start();
                Stop();
                OnGamepadDisconnectStatus();
            }

            Keystroke keystroke = this.ReturnKeystroke();
            if (!keystroke.VirtualKey.Equals(SharpDX.XInput.GamepadKeyCode.None) && keystroke.Flags.Equals(SharpDX.XInput.KeyStrokeFlags.KeyUp))
            {
                string key = keystroke.VirtualKey.ToString();
                // necessary to make the difference between the keys on the keyboard and
                // those on the gamepad in the dictionary that contains all of them
                if (key == "A" || key == "B" || key == "Y" || key == "X" || key == "Back")
                {
                    key = "gamepad" + key;
                }

                //DirectDX reports LeftThumbUpLeft and LeftThumbDownLeft as RightThumbUpLeft and RightThumbDownLeft
                // while reporting the actual RightThumbUpLeft and RightThumbDownLeft as RightThumbUpleft and RightThumbDownleft
                // solving this problem for the moment with this if
                if (key == "RightThumbUpLeft")
                {
                    key = "LeftThumbUpLeft";
                }
                else if(key == "RightThumbDownLeft")
                {
                    key = "LeftThumbDownLeft";
                }

                ProfileManager.AddToDictionary(key, "gamepad");
            }
        }



    }

}

