using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace KeyCounter
{
    public static class KeyboardHookClass
    {
        public static bool Initialized = false;

        private struct LowLevelKeyboardStructure
        {
            public uint VirtualKeyCode;

            public uint HardwareScanCodeKey;

            public uint Flags;

            public uint TimeStamp;

            public UIntPtr ExtraInformation;
        }

        private const int WH_KEYBOARD_LL = 13;

        private static IntPtr _keyboardHookId = IntPtr.Zero;

        public delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static readonly KeyboardProc _procKeyboard = HandleKeyboardEvent;

        //I am interested only in events when a key is physically pressed, not when it is held down
        // using this to check that
        private static readonly List<Keys> _currentlyPressed = new List<Keys>();

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int hookId, KeyboardProc lpfn, IntPtr hmod, uint dwThreadId);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hookId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);


        private static IntPtr AttachKeyboardHook(KeyboardProc proc)
        {
            using (var process = Process.GetCurrentProcess())
            {
                using (var processModule = process.MainModule)
                {
                    var hook = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(processModule.ModuleName),
                        0);

                    return hook;
                }
            }
        }


        public static void DeleteKeyboardHook()
        {
            UnhookWindowsHookEx(_keyboardHookId);
            Initialized = false;
            //Console.WriteLine("Keyboard hook destroyed");
        }


        private static IntPtr HandleKeyboardEvent(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                var keyboardStructure =
                    (LowLevelKeyboardStructure) (Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardStructure)) ?? throw new NullReferenceException("keyboard structure was null"));

                // if the event is of type key down or alt key down add them to a the corresponding
                // dictionary of the CurrentProfile and to a dictionary containing all the pressed keys
                // one of the values is for a normal key down, the other is for the alt-key down since they have different codes
                if ((int) wParam == 256 || (int) wParam == 260)
                {
                    if (!_currentlyPressed.Contains((Keys) keyboardStructure.VirtualKeyCode))
                    {
                        //for some reason gamepads report the middle button as a keypress, so check for that 
                        if (((Keys) keyboardStructure.VirtualKeyCode).ToString() != "LButton, XButton2")
                        {
                            ProfileManager.KeyboardInputUpdate(((Keys) keyboardStructure.VirtualKeyCode).ToString());
                            _currentlyPressed.Add((Keys) keyboardStructure.VirtualKeyCode);
                        }
                        else
                        {
                            ProfileManager.GamepadInputUpdate(new List<string>(){"LButton, XButton2"});
                        }
                   
                    }
                }
                // if the event is "key up" remove the key from the dictionary as it was released and now can be pressed physically again
                else if ((int) wParam == 257)
                {
                    _currentlyPressed.Remove((Keys) keyboardStructure.VirtualKeyCode);
                }

                return CallNextHookEx(_keyboardHookId, nCode, wParam, lParam);
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying reading keyboard input");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying reading keyboard input");
                    
                }
                throw exception;
            }
            
        }

        // start the attachment function
        public static void Initialize()
        {
            //Console.WriteLine("keyboard hook initialized");
            _keyboardHookId = AttachKeyboardHook(_procKeyboard);
            Initialized = true;
        }
    }


    public static class MouseHookClass
    {
        public struct LowLevelMouseStructure
        {
            public Point Point;

            public int MouseData;

            public int Flags;

            public uint TimeStamp;

            public UIntPtr ExtraInformation;
        }

        private const int WH_MOUSE_LL = 14;

        private static IntPtr _mouseHookId = IntPtr.Zero;

        public delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static readonly MouseProc _procMouse = HandleMouseEvent;

        private static Point _mousePos = new Point(0, 0);

        private static int _lastMouseUpdate =
            (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

        private static string _lastMouseMovement = "";

        private static int _lastMouseWheelMovement = 0;

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int hookId, MouseProc lpfn, IntPtr hmod, uint dwThreadId);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hookId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);

        /// <summary>
        /// attaches a low level mouse hook and set the handler for the events
        /// </summary>
        /// <param name="proc">the handler for the mouse events </param>
        /// <returns>pointer to the attached hook</returns>
        private static IntPtr AttachMouseHook(MouseProc proc)
        {
            using (var process = Process.GetCurrentProcess())
            {
                using (var processModule = process.MainModule)
                {
                    var hook = SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(processModule.ModuleName), 0);
                    return hook;
                }
            }
        }

        /// <summary>
        /// remove the attached hook
        /// </summary>
        public static void DeleteMouseHook()
        {
            UnhookWindowsHookEx(_mouseHookId);
            //Console.WriteLine("deleted mouse hook");
        }

        /// <summary>
        /// Give one of eight directions in which a point is located in relation to another point
        /// </summary>
        /// <param name="oldCoordinates"></param>
        /// <param name="newCoordinates"></param>
        /// <returns></returns>
        public static string? MovementDirection(Point newCoordinates,Point oldCoordinates)
        {
            if (oldCoordinates.Equals(newCoordinates))
                return null;

            
            newCoordinates.Y = newCoordinates.Y - oldCoordinates.Y;
            newCoordinates.X = newCoordinates.X - oldCoordinates.X;
            
            // arctan2 gives the clockwise angle between the Ox axis and a vector ((0,0),(X,Y)) in radians
            var angle = Math.Atan2(newCoordinates.Y, newCoordinates.X);

            // radians to degrees
            angle = 180 / Math.PI * angle;
            angle = angle - Math.Floor(angle / 360) * 360;

            // cut the trigonometric circle in 8 equal parts, see in which one of the eight the current point is situated based on its angle 
            switch (angle)
            {
                case { } when angle >= 67.5 && angle <= 112.5:
                {
                    return "up";
                }
                case { } when angle > 22.5 && angle < 67.5:
                {
                    return "up_right";
                }
                case { } when angle > 112.5 && angle < 157.5:
                {
                    return "up_left";
                }
                case { } when angle >= 337.5 && angle <= 360 || angle >= 0 && angle <= 22.5:
                {
                    return "right";
                }
                case { } when angle > 292.5 && angle < 337.5:
                {
                    return "down_right";
                }
                case { } when angle >= 247.5 && angle <= 292.5:
                {
                    return "down";
                }
                case { } when angle > 202.5 && angle < 247.5:
                {
                    return "down_left";
                }
                case { } when angle >= 157.5 && angle <= 202.5:
                {
                    return "left";
                }
                
            }

            return null;
        }

        /// <summary>
        /// handle the mouse event
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam">parameter describing the type of event</param>
        /// <param name="lParam">pointer to a structure in memory containing the event parameters</param>
        /// <returns></returns>
        private static IntPtr HandleMouseEvent(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                //get the parameters to which the pointer point to
            var mouseStruct = (LowLevelMouseStructure) (Marshal.PtrToStructure(lParam, typeof(LowLevelMouseStructure)) ?? throw new NullReferenceException("Mouse structure was null"));

            // add to the CurrentProfile mouse dictionary the corresponding key
            switch ((int) wParam)
            {
                case 513:
                {
                    ProfileManager.MouseInputUpdate("left_button");
                    break;
                }
                case 516:
                {
                    ProfileManager.MouseInputUpdate("right_button");
                    break;
                }
                case 519:
                {
                    ProfileManager.MouseInputUpdate("middle_button");
                    break;
                }
                case 523:
                {
                    var button = 3 + short.Parse(mouseStruct.MouseData.ToString("X")) / 10000;
                    ProfileManager.MouseInputUpdate("Button " + button);
                    break;
                }
                case 522:
                {
                    if (_lastMouseWheelMovement >= 0 && mouseStruct.MouseData < 0 ||
                        _lastMouseWheelMovement <= 0 && mouseStruct.MouseData > 0 ||
                        mouseStruct.TimeStamp - _lastMouseUpdate > 1500)
                    {
                        _lastMouseWheelMovement = mouseStruct.MouseData;
                        _lastMouseUpdate = (int) mouseStruct.TimeStamp;
                        ProfileManager.MouseInputUpdate(mouseStruct.MouseData > 0 ? "scroll_up" : "scroll_down");
                    }

                    break;
                }
                case 512:
                {
                    // detect a mouse movement only if the new coordinates are outside of a dead zone and if the time since the last update is big enough or if the direction
                    // is different.
                    // Also remember that the Y on  monitor goes different than on a normal 2D space, so the true Y is height of the monitor - the given Y coordinate
                    var direction = MovementDirection(new Point(mouseStruct.Point.X,Screen.PrimaryScreen.Bounds.Height-mouseStruct.Point.Y),_mousePos);
                    var distance = Math.Sqrt(Math.Pow((mouseStruct.Point.X - _mousePos.X ), 2) +
                                             Math.Pow((Screen.PrimaryScreen.Bounds.Height-mouseStruct.Point.Y - _mousePos.Y), 2));
                    
                    if (direction != null && (mouseStruct.TimeStamp - _lastMouseUpdate > 750 ||
                                              !direction.Equals(_lastMouseMovement) ) && distance > 30)
                    {

                        _lastMouseWheelMovement = 0;
                        _lastMouseUpdate = (int) mouseStruct.TimeStamp;
                        _mousePos = new Point(mouseStruct.Point.X,Screen.PrimaryScreen.Bounds.Height-mouseStruct.Point.Y);
                        ProfileManager.MouseInputUpdate(direction);
                        _lastMouseMovement = direction;
                    }

                    break;
                }
            }

            return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to read mouse input");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to read mouse input");
                    
                }
                throw exception;
            }
            
        }

        /// <summary>
        /// Initialize mouse hook monitoring
        /// </summary>
        public static void Initialize()
        {
            _mouseHookId = AttachMouseHook(_procMouse);
            //Console.WriteLine("initialized mouse hook");
        }

    }


    public static class GamepadHookClass
    {
        private const uint ERROR_SUCCESS = 0x0;
        private const uint ERROR_DEVICE_NOT_CONNECTED = 0x48F;

        private const uint XINPUT_GAMEPAD_DPAD_UP = 0x0001;
        private const uint XINPUT_GAMEPAD_DPAD_DOWN = 0x0002;
        private const uint XINPUT_GAMEPAD_DPAD_LEFT = 0x0004;
        private const uint XINPUT_GAMEPAD_DPAD_RIGHT = 0x0008;
        private const uint XINPUT_GAMEPAD_START = 0x0010;
        private const uint XINPUT_GAMEPAD_BACK = 0x0020;
        private const uint XINPUT_GAMEPAD_LEFT_THUMB = 0x0040;
        private const uint XINPUT_GAMEPAD_RIGHT_THUMB = 0x0080;
        private const uint XINPUT_GAMEPAD_LEFT_SHOULDER = 0x0100;
        private const uint XINPUT_GAMEPAD_RIGHT_SHOULDER = 0x0200;
        private const uint XINPUT_GAMEPAD_A = 0x1000;
        private const uint XINPUT_GAMEPAD_B = 0x2000;
        private const uint XINPUT_GAMEPAD_X = 0x4000;
        private const uint XINPUT_GAMEPAD_Y = 0x8000;

        private const uint XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE = 9000;
        private const uint XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE = 9000;
        private const uint XINPUT_GAMEPAD_TRIGGER_THRESHOLD  = 30;

        private static _XINPUT_STATE LastState = new _XINPUT_STATE();
        private static _XINPUT_STATE CurrentState;


        public struct _XINPUT_GAMEPAD {
            [MarshalAs(UnmanagedType.U2)]
            public short wButtons;
            public byte  bLeftTrigger;
            public byte  bRightTrigger;
            [MarshalAs(UnmanagedType.I2)]
            public short sThumbLX;
            [MarshalAs(UnmanagedType.I2)]
            public short sThumbLY;
            [MarshalAs(UnmanagedType.I2)]
            public short sThumbRX;
            [MarshalAs(UnmanagedType.I2)]
            public short sThumbRY;
        }


        public struct _XINPUT_STATE {
            
            public UInt32 dwPacketNumber;
            public _XINPUT_GAMEPAD Gamepad;
        }

        [DllImport("Xinput1_4", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint XInputGetState(uint dwUserIndex,out _XINPUT_STATE pState);


        private static  Timer frameTimer = new Timer {Interval = 15};
        private static string LastDirectionR { get; set; } = "";
        private static string LastDirectionL { get; set; } = "";

        // set up the function to run on tick
        public static void SetUpTimer()
        {
            frameTimer.Tick += CheckGamepad;
        }


        /// <summary>
        /// verify if a gamepad is connected, if it is then check which buttons are pressed and the directions of the thumb sticks,
        /// if no gamepad is connected change the timer to 1 every 5 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        static void CheckGamepad(object? sender, EventArgs eventArgs)
        {
            try
            {
                //Console.WriteLine("checked for gamepad input");
            switch (XInputGetState(0, out CurrentState))
            {
                case ERROR_SUCCESS:
                    {
                        // if a controller was just connected go back to checking for input ~60 times per second
                        if (frameTimer.Interval == 5000)
                        {
                            frameTimer.Interval = 15;
                        }
                        if (CurrentState.dwPacketNumber != LastState.dwPacketNumber)
                        {
                            var listOfButtonPresses = new List<string>();

                            //apply the bitmask for each button and take it as a press only if the result is not equal to 0 and if the previous state was equal to 0
                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_UP) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_UP) == 0)
                            {
                                listOfButtonPresses.Add("D_pad_up");

                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_DOWN) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_DOWN) == 0)
                            {
                                listOfButtonPresses.Add("D_pad_down");

                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_RIGHT) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_RIGHT) == 0)
                            {
                                listOfButtonPresses.Add("D_pad_right");
                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_LEFT) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_DPAD_LEFT) == 0)
                            {
                                listOfButtonPresses.Add("D_pad_left");

                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_START) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_START) == 0)
                            {
                                listOfButtonPresses.Add("start");
                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_BACK) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_BACK) == 0)
                            {
                                listOfButtonPresses.Add("back");
                            }


                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_LEFT_SHOULDER) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_LEFT_SHOULDER) == 0)
                            {
                                listOfButtonPresses.Add("left_shoulder");

                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_RIGHT_SHOULDER) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_RIGHT_SHOULDER) == 0)
                            {
                                listOfButtonPresses.Add("right_shoulder");

                            }


                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_LEFT_THUMB) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_LEFT_THUMB) == 0)
                            {
                                listOfButtonPresses.Add("left_thumb");

                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_RIGHT_THUMB) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_RIGHT_THUMB) == 0)
                            {
                                listOfButtonPresses.Add("right_thumb");
                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_A) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_A) == 0)
                            {
                                listOfButtonPresses.Add("a");
                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_B) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_B) == 0)
                            {
                                listOfButtonPresses.Add("b");
                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_X) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_X) == 0)
                            {
                                listOfButtonPresses.Add("x");
                            }

                            if ((CurrentState.Gamepad.wButtons & XINPUT_GAMEPAD_Y) != 0 &&
                                (LastState.Gamepad.wButtons & XINPUT_GAMEPAD_Y) == 0)
                            {
                                listOfButtonPresses.Add("y");
                            }

                            if (CurrentState.Gamepad.bLeftTrigger > XINPUT_GAMEPAD_TRIGGER_THRESHOLD &&
                                LastState.Gamepad.bLeftTrigger < XINPUT_GAMEPAD_TRIGGER_THRESHOLD)
                            {
                                listOfButtonPresses.Add("left_trigger");
                            }

                            if (CurrentState.Gamepad.bRightTrigger > XINPUT_GAMEPAD_TRIGGER_THRESHOLD &&
                                LastState.Gamepad.bRightTrigger < XINPUT_GAMEPAD_TRIGGER_THRESHOLD)
                            {
                                listOfButtonPresses.Add("right_trigger");
                            }

                            //take care of the cases where the absolute would underflow
                            if (CurrentState.Gamepad.sThumbLY == short.MinValue)
                            {
                                CurrentState.Gamepad.sThumbLY = short.MinValue + 2;
                            }

                            if (CurrentState.Gamepad.sThumbLX == short.MinValue)
                            {
                                CurrentState.Gamepad.sThumbLX = short.MinValue + 2;
                            }

                            var directionL = MouseHookClass.MovementDirection(
                                new Point(CurrentState.Gamepad.sThumbLX, CurrentState.Gamepad.sThumbLY),
                                new Point(0, 0));
                            // update the direction of the left thumb stick only if it is outside of the dead zone and the previous state was inside the dead zone
                            // or if the direction is different
                            if ((Math.Abs(CurrentState.Gamepad.sThumbLY) > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE ||
                                 Math.Abs(CurrentState.Gamepad.sThumbLX) > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE)
                                && !(Math.Abs(LastState.Gamepad.sThumbLY) > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE ||
                                     Math.Abs(LastState.Gamepad.sThumbLX) > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) || (directionL != null && directionL != LastDirectionL))
                            {
                                
                                if (directionL != null)
                                {
                                    listOfButtonPresses.Add("left_thumb_move_" + directionL);
                                    LastDirectionL = directionL;
                                }
                            }

                            //take care of the cases where the absolute would underflow
                            if (CurrentState.Gamepad.sThumbRY == short.MinValue)
                            {
                                CurrentState.Gamepad.sThumbRY = short.MinValue + 2;
                            }

                            if (CurrentState.Gamepad.sThumbRX == short.MinValue)
                            {
                                CurrentState.Gamepad.sThumbRX = short.MinValue + 2;
                            }

                            var directionR = MouseHookClass.MovementDirection(
                                new Point(CurrentState.Gamepad.sThumbRX, CurrentState.Gamepad.sThumbRY),
                                new Point(0, 0));
                            // update the direction of the right thumb stick only if it is outside of the dead zone and the previous state was inside the dead zone
                            // or if the direction is different
                            if ((Math.Abs(CurrentState.Gamepad.sThumbRX) > XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE ||
                                 Math.Abs(CurrentState.Gamepad.sThumbRY) > XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE)
                                && !(Math.Abs(LastState.Gamepad.sThumbRX) > XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE ||
                                     Math.Abs(LastState.Gamepad.sThumbRY) > XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE) || (directionR != null && directionR != LastDirectionR))
                            {
                                
                                if (directionR != null)
                                {
                                    listOfButtonPresses.Add("right_thumb_move_" + directionR);
                                    LastDirectionR = directionR;
                                }
                            }


                            
                            if (listOfButtonPresses.Count > 0)
                            {
                                ProfileManager.GamepadInputUpdate(listOfButtonPresses);
                            }

                            LastState = CurrentState;

                        }

                        break;
                    }
                case ERROR_DEVICE_NOT_CONNECTED:
                {
                    // if no gamepad is connected then search for input only once every 5 seconds
                    if (frameTimer.Interval != 5000)
                    {
                        frameTimer.Interval = 5000;
                    }
                    break;
                }
            }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to read gamepad input");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to read gamepad input");
                    
                }
                throw exception;
            }
            

        }

        

        /// <summary>
        /// Stop the timer
        /// </summary>
        public static void StopTimer()
        {
            frameTimer.Stop();
            //Console.WriteLine("gamepad stopped checking");
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        public static void StartTimer()
        {
            frameTimer.Start();
            //Console.WriteLine("gamepad started checking");
        }

        /// <summary>
        /// Stop and dispose of the timer
        /// </summary>
        public static void DestroyTimer()
        {
            frameTimer.Stop();
            frameTimer.Dispose();
        }


    }
}