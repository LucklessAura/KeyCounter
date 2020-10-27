using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyCounter
{
    /// <summary>
    /// Structure returned by a LowLevelKeyboardEvent
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public struct LowLevelKeyboardStructure
    {
        public uint VirtualKeyCode;

        public uint HardwareScanCodeKey;

        public uint Flags;

        public uint TimeStamp;

        public UIntPtr ExtraInformation;

    }

    /// <summary>
    /// Structure returned by a LowLevelMouseEvent
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public struct LowLevelMouseStructure
    {
        public Point Point;

        public int MouseData;

        public int Flags;

        public uint TimeStamp;

        public UIntPtr ExtraInformation;

    }

    /// <summary>
    /// Class providing mouse hooks and related operations
    /// </summary>
    public class MouseHookClass
    {
        private const int WH_MOUSE_LL = 14;

        private static IntPtr _mouseHookId = IntPtr.Zero;

        public delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static readonly MouseProc _procMouse = HandleMouseEvent;

        private static Point _mousePos = new Point(0, 0);

        private static Int32 _lastMouseUpdate = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;


        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int hookId, MouseProc lpfn, IntPtr hmod, uint dwThreadId);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool UnhookWindowsHookEx(IntPtr hookId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetModuleHandle(string name);

        /// <summary>
        /// attaches a low level mouse hook and set the handler for the events
        /// </summary>
        /// <param name="proc">the handler for the mouse events </param>
        /// <returns>pointer to the attached hook</returns>
        private IntPtr AttachMouseHook(MouseProc proc)
        {
            using (Process process = Process.GetCurrentProcess())
            {
                using (ProcessModule processModule = process.MainModule)
                {
                    IntPtr hook = SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(processModule.ModuleName), 0);
                    return hook;
                }
            }
        }

        /// <summary>
        /// remove the attached hook
        /// </summary>
        public void DeleteMouseHook()
        {
            UnhookWindowsHookEx(_mouseHookId);
        }

        /// <summary>
        /// Adds to the CurrentProfile mouse dictionary the direction of the mouse movement
        /// </summary>
        /// <param name="oldCoordinates">the last coordinates of the mouse</param>
        /// <param name="newCoordinates">the new coordinates of the mouse</param>
        private static void  MovementDirection(Point oldCoordinates, Point newCoordinates)
        {
            if(!oldCoordinates.Equals(newCoordinates))
            {
                if (newCoordinates.Y > oldCoordinates.Y)
                {
                    // moved downwards
                    if (newCoordinates.X == oldCoordinates.X)
                    {
                        ProfileManager.AddToDictionary("mouse moved down", "mouse");

                    }
                    else
                    {
                        //moved diagonally
                        if (newCoordinates.X < oldCoordinates.X)
                        {
                            ProfileManager.AddToDictionary("mouse moved down left", "mouse");
                        }
                        else
                        {
                            ProfileManager.AddToDictionary("mouse moved down right", "mouse");
                        }
                    }
                }
                else if (newCoordinates.Y < oldCoordinates.Y)
                {
                    // moved upwards
                    if (newCoordinates.X == oldCoordinates.X)
                    {
                        ProfileManager.AddToDictionary("mouse moved up", "mouse");
                    }
                    else
                    {
                        //moved diagonally
                        if (newCoordinates.X < oldCoordinates.X)
                        {
                            ProfileManager.AddToDictionary("mouse moved up left", "mouse");
                        }
                        else
                        {
                            ProfileManager.AddToDictionary("mouse moved up right", "mouse");
                        }
                    }

                }
                else
                {
                    //moved sideways 
                    if (newCoordinates.X < oldCoordinates.X)
                    {
                        ProfileManager.AddToDictionary("mouse moved left", "mouse");
                    }
                    else
                    {
                        ProfileManager.AddToDictionary("mouse moved right", "mouse");
                    }
                }
            }
        }

        /// <summary>
        /// handle the mouse event
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam">parameter describing the type of event</param>
        /// <param name="lParam">pointer to a structure in memory containing the event parameters</param>
        /// <returns></returns>
        static IntPtr HandleMouseEvent(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //get the parameters to which the pointer point to
            LowLevelMouseStructure mouseStruct = (LowLevelMouseStructure)Marshal.PtrToStructure(lParam, typeof(LowLevelMouseStructure));

           // add to the CurrentProfile mouse dictionary the corresponding key
            switch ((int)wParam)
            {
                case 514:
                    {
                        ProfileManager.AddToDictionary("Mouse button 1", "mouse");
                        break;
                    }
                case 517:
                    {
                        ProfileManager.AddToDictionary("Mouse button 2", "mouse");
                        break;
                    }
                case 520:
                    {
                        ProfileManager.AddToDictionary("Middle mouse button", "mouse");
                        break;
                    }
                case 524:
                    {
                        int button = 3 + Int16.Parse(mouseStruct.MouseData.ToString("X")) / 10000;
                        ProfileManager.AddToDictionary("Mouse button " + button.ToString(), "mouse");
                        break;
                    }
                case 522:
                    {
                        if (mouseStruct.MouseData > 0)
                        {
                            ProfileManager.AddToDictionary("Mouse wheel forward", "mouse");
                        }
                        else
                        {
                            ProfileManager.AddToDictionary("Mouse wheel backwards", "mouse");
                        }
                        break;
                    }
                default:
                    {
                        if (mouseStruct.TimeStamp - _lastMouseUpdate > 15)
                        {
                            _lastMouseUpdate = (Int32)mouseStruct.TimeStamp;
                            MovementDirection(_mousePos, mouseStruct.Point);
                            _mousePos = mouseStruct.Point;
                        }
                        break;
                    }
                
            }

            return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);

        }

        // start the attachment function 
        public void Initialize()
        {
            _mouseHookId = AttachMouseHook(_procMouse);
        }

    }

    /// <summary>
    /// Class providing keyboard hooks and related operations
    /// </summary>
    public class KeyboardHookClass
    {
        private const int WH_KEYBOARD_LL = 13;

        private static IntPtr _keyboardHookId = IntPtr.Zero;

        public delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static readonly KeyboardProc _procKeyboard = HandleKeyboardEvent;

        // used to ignore consecutive key pressed events that happen when a key is pressed without releasing the physical key
        private static readonly List<Keys> _currentlyPressed = new List<Keys>();

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int hookId, KeyboardProc lpfn, IntPtr hmod, uint dwThreadId);

        [DllImport("USER32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool UnhookWindowsHookEx(IntPtr hookId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetModuleHandle(string name);


        /// <summary>
        /// attaches a low level keyboard hook and set the handler for the events
        /// </summary>
        /// <param name="proc">the handler for the keyboard events </param>
        /// <returns>pointer to the attached hook</returns>
        private IntPtr AttachKeyboardHook(KeyboardProc proc)
        {
            using (Process process = Process.GetCurrentProcess())
            {
                using (ProcessModule processModule = process.MainModule)
                {
                    IntPtr hook = SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(processModule.ModuleName), 0);
                    return hook;
                }
            }
        }


        /// <summary>
        /// remove the attached hook
        /// </summary>
        public void DeleteKeyboardHook()
        {
            UnhookWindowsHookEx(_keyboardHookId);
        }


        /// <summary>
        /// handle the keyboard event
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam">parameter describing the type of event</param>
        /// <param name="lParam">pointer to a structure in memory containing the event parameters</param>
        /// <returns></returns>
        static IntPtr HandleKeyboardEvent(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //get the parameters to which the pointer point to
            LowLevelKeyboardStructure keyboardStructure = (LowLevelKeyboardStructure)Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardStructure));

            // if the event is of type key down or alt key down add them to a the corresponding
            // dictionary of the CurrentProfile and to a dictionary containing all the pressed keys 
            if ((int) wParam == 256 || (int)wParam == 260)
            {
                if (!_currentlyPressed.Contains((Keys)keyboardStructure.VirtualKeyCode))
                {
                    ProfileManager.AddToDictionary(((Keys)keyboardStructure.VirtualKeyCode).ToString(), "keyboard");
                    _currentlyPressed.Add((Keys)keyboardStructure.VirtualKeyCode);
                }
            }
            // if the event is key up remove the key from the dictionary
            else if ((int)wParam == 257)
            { 
                _currentlyPressed.Remove((Keys)keyboardStructure.VirtualKeyCode);
            }

            return CallNextHookEx(_keyboardHookId, nCode, wParam, lParam);
        }

        // start the attachment function
        public void Initialize()
        {
            _keyboardHookId = AttachKeyboardHook(_procKeyboard);
        }

    }
}
