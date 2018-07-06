using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookDemo
{


    public enum HookType
    {
        All = 0,
        Mouse = 1,
        Keyboard = 2,
    }

    public class Hocy_Hook
    {

        #region win32api
         
        /// <summary> 安装钩子
        /// </summary> 
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(WH_Codes idHook, HookProc lpfn, IntPtr pInstance, int threadId);

        /// <summary> 卸载钩子
        /// </summary> 
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);

        /// <summary>  传递钩子
        /// </summary> 
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(IntPtr pHookHandle, int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>  转换当前按键信息
        /// </summary>
 
        [DllImport("user32.dll")]
        public static extern int ToAscii(UInt32 uVirtKey, UInt32 uScanCode,  byte[] lpbKeyState, byte[] lpwTransKey, UInt32 fuState);

        /// <summary> 获取按键状态,返回非0表示成功
        /// </summary> 
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        
        [DllImport("user32.dll")]
        public static extern short GetKeyStates(int vKey);
         
        #endregion


        #region  委托 枚举 结构

        /// <summary> 钩子委托声明
        /// </summary> 
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        /// <summary> 鼠标更新事件委托声明
        /// </summary> 
        public delegate void MouseUpdateEventHandler(int x, int y);

     
        public enum WH_Codes : int
        {
            /// <summary> 底层键盘钩子
            /// </summary>
            WH_KEYBOARD_LL = 13,

            /// <summary> 底层鼠标钩子
            /// </summary>
            WH_MOUSE_LL = 14
        }

        public enum WM_MOUSE : int
        {
            /// <summary> 鼠标开始
            /// </summary>
            WM_MOUSEFIRST = 0x200,

            /// <summary>  鼠标移动
            /// </summary>
            WM_MOUSEMOVE = 0x200,

            /// <summary> 左键按下
            /// </summary>
            WM_LBUTTONDOWN = 0x201,

            /// <summary>  左键释放
            /// </summary>
            WM_LBUTTONUP = 0x202,

            /// <summary> 左键双击
            /// </summary>
            WM_LBUTTONDBLCLK = 0x203,

            /// <summary> 右键按下
            /// </summary>
            WM_RBUTTONDOWN = 0x204,

            /// <summary>  右键释放
            /// </summary>
            WM_RBUTTONUP = 0x205,

            /// <summary> 右键双击
            /// </summary>
            WM_RBUTTONDBLCLK = 0x206,

            /// <summary  中键按下
            /// </summary>
            WM_MBUTTONDOWN = 0x207,

            /// <summary>  中键释放
            /// </summary>
            WM_MBUTTONUP = 0x208,

            /// <summary> 中键双击
            /// </summary>
            WM_MBUTTONDBLCLK = 0x209,

            /// <summary> 滚轮滚动
            /// </summary> 
            WM_MOUSEWHEEL = 0x020A
        }

        public enum WM_KEYBOARD : int
        {
            /// <summary>  非系统按键按下
            /// </summary>
            WM_KEYDOWN = 0x100,

            /// <summary> 非系统按键释放
            /// </summary>
            WM_KEYUP = 0x101,

            /// <summary> 系统按键按下
            /// </summary>
            WM_SYSKEYDOWN = 0x104,

            /// <summary> 系统按键释放
            /// </summary>
            WM_SYSKEYUP = 0x105
        }

       
    

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        /// <summary> 鼠标钩子事件结构定义
        /// </summary> 
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        { 
            public POINT Point;

            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public UInt32 ExtraInfo;
        }

        /// <summary> 键盘钩子事件结构定义
        /// </summary> 
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
            /// </summary>
            public UInt32 VKCode;

            /// <summary>
            /// Specifies a hardware scan code for the key.
            /// </summary>
            public UInt32 ScanCode;

            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, 
            /// and transition-state flag. This member is specified as follows. 
            /// An application can use the following values to test the keystroke flags. 
            /// </summary>
            public UInt32 Flags;

            /// <summary>
            /// Specifies the time stamp for this message. 
            /// </summary>
            public UInt32 Time;

            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public UInt32 ExtraInfo;
        }

        #endregion
         
        #region 静态变量 常量  

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;

        #endregion

        #region 事件定义
         
        /// <summary>鼠标事件
        /// </summary>
        public event MouseEventHandler OnMouseActivity;
         
        /// <summary>  按键按下事件
        /// </summary>
        public event KeyEventHandler OnKeyDown;

        /// <summary> 按键按下并释放事件
        /// </summary>
        public event KeyPressEventHandler OnKeyPress;

        /// <summary> 按键释放事件
        /// </summary>
        public event KeyEventHandler OnKeyUp;

        #endregion 事件定义

        #region 私有变量

        /// <summary> 按键状态数组
        /// </summary>
        private byte[] m_KeyState = new byte[256];

        /// <summary> flag=0 正常  flag=1 监控状态  flag=2 屏蔽键盘
        /// </summary>
        private HookType flags;

        /// <summary> 鼠标钩子句柄
        /// </summary>
        private IntPtr m_pMouseHook = IntPtr.Zero;

        /// <summary> 键盘钩子句柄
        /// </summary>
        private IntPtr m_pKeyboardHook = IntPtr.Zero;

        /// <summary>  鼠标钩子委托实例
        /// </summary> 
        private HookProc m_MouseHookProcedure;

        /// <summary> 键盘钩子委托实例
        /// </summary> 
        private HookProc m_KeyboardHookProcedure;


        #endregion 私有变量

        #region 构造函数

        /// <summary> 钩子类,实现 键盘和鼠标的钩子
        /// </summary> 
        public Hocy_Hook()
        { 
            Hocy_Hook.GetKeyboardState(this.m_KeyState);
        }

        #endregion 构造函数
         
        #region 公共方法

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <returns></returns>
        public bool InstallHook(HookType flagsinfo)
        {
            this.flags = flagsinfo;
            IntPtr pInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule);
         
            // 假如没有安装鼠标钩子
            if (this.m_pMouseHook == IntPtr.Zero)
            {
                this.m_MouseHookProcedure = new HookProc(this.MouseHookProc);
                //注册鼠标钩子
                this.m_pMouseHook = Hocy_Hook.SetWindowsHookEx(WH_Codes.WH_MOUSE_LL, this.m_MouseHookProcedure, pInstance, 0);
                if (this.m_pMouseHook == IntPtr.Zero)
                {
                    this.UnInstallHook();
                    return false;
                }
            }
            // 假如没有安装键盘钩子
            if (this.m_pKeyboardHook == IntPtr.Zero)
            {
                this.m_KeyboardHookProcedure = new HookProc(this.KeyboardHookProc);
                //注册键盘钩子
                this.m_pKeyboardHook = Hocy_Hook.SetWindowsHookEx(WH_Codes.WH_KEYBOARD_LL, this.m_KeyboardHookProcedure, pInstance, 0);
                if (this.m_pKeyboardHook == IntPtr.Zero)
                {
                    this.UnInstallHook();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <returns></returns>
        public bool UnInstallHook()
        {
            bool result = true;
            if (this.m_pMouseHook != IntPtr.Zero)
            {
                result = (Hocy_Hook.UnhookWindowsHookEx(this.m_pMouseHook) && result);
                this.m_pMouseHook = IntPtr.Zero;
            }
            if (this.m_pKeyboardHook != IntPtr.Zero)
            {
                result = (Hocy_Hook.UnhookWindowsHookEx(this.m_pKeyboardHook) && result);
                this.m_pKeyboardHook = IntPtr.Zero;
            }

            return result;
        }

        #endregion 公共方法
         
        #region 私有方法

        /// <summary> 鼠标钩子处理函数
        /// </summary> 
        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (flags == HookType.Mouse || flags == HookType.All)
            {

            }
            else
            {
                return Hocy_Hook.CallNextHookEx(this.m_pMouseHook, nCode, wParam, lParam);
            }

            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                //Marshall the data from callback.
                MouseHookStruct mouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

                //进行数据转换
                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                switch (wParam)
                {
                    case (int)WM_MOUSE.WM_LBUTTONDOWN:
                        //case WM_LBUTTONUP: 
                        //case WM_LBUTTONDBLCLK: 
                        button = MouseButtons.Left;
                        break;
                    case (int)WM_MOUSE.WM_RBUTTONDOWN:
                        //case WM_RBUTTONUP: 
                        //case WM_RBUTTONDBLCLK: 
                        button = MouseButtons.Right;
                        break;
                    case (int)WM_MOUSE.WM_MOUSEWHEEL:
                        //If the message is WM_MOUSEWHEEL, the high-order word of mouseData member is the wheel delta. 
                        //One wheel click is defined as WHEEL_DELTA, which is 120. 
                        //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        //TODO: X BUTTONS (I havent them so was unable to test)
                        //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                        //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                        //and the low-order word is reserved. This value can be one or more of the following values. 
                        //Otherwise, mouseData is not used. 
                        break;
                }


                //double clicks
                int clickCount = 0;
                if (button != MouseButtons.None)
                {
                    if (wParam == (int)WM_MOUSE.WM_LBUTTONDBLCLK || wParam == (int)WM_MOUSE.WM_RBUTTONDBLCLK)
                    {
                        clickCount = 2;
                    }
                    else
                    {
                        clickCount = 1;
                    }
                }


                //触发事件
                //generate event 
                MouseEventArgs e = new MouseEventArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta);
                //raise it
                OnMouseActivity(this, e);
            }

            //*

            return Hocy_Hook.CallNextHookEx(this.m_pMouseHook, nCode, wParam, lParam);
        }

        /// <summary>  键盘钩子处理函数
        /// </summary> 
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //为 2 ，直接返回，不响应键盘事件

            if(flags == HookType.Keyboard || flags == HookType.All)
            {

            }
            else
            {
                return Hocy_Hook.CallNextHookEx(this.m_pKeyboardHook, nCode, wParam, lParam);
            }
    


            bool handled = false;
            //it was ok and someone listens to events
            if ((nCode >= 0) && (this.OnKeyDown != null || this.OnKeyUp != null || this.OnKeyPress != null))
            {
                //read structure KeyboardHookStruct at lParam
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //raise KeyDown
                if (this.OnKeyDown != null && (wParam == (int)WM_KEYBOARD.WM_KEYDOWN || wParam == (int)WM_KEYBOARD.WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VKCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    this.OnKeyDown(this, e);
                    handled = handled || e.Handled;
                }

                // raise KeyPress
                if (this.OnKeyPress != null && wParam == (int)WM_KEYBOARD.WM_KEYDOWN)
                {
                    bool isDownShift, isDownCapslock;
                    try
                    {
                        isDownShift = ((Hocy_Hook.GetKeyStates(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                        isDownCapslock = (Hocy_Hook.GetKeyStates(VK_CAPITAL) != 0 ? true : false);
                    }
                    catch
                    {
                        isDownCapslock = false;
                        isDownShift = false;
                    }

                    byte[] keyState = new byte[256];
                    Hocy_Hook.GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (Hocy_Hook.ToAscii(MyKeyboardHookStruct.VKCode,
                              MyKeyboardHookStruct.ScanCode,
                              keyState,
                              inBuffer,
                              MyKeyboardHookStruct.Flags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        KeyPressEventArgs e = new KeyPressEventArgs(key);
                        this.OnKeyPress(this, e);
                        handled = handled || e.Handled;
                    }
                }
                // raise KeyUp
                if (this.OnKeyUp != null && (wParam == (int)WM_KEYBOARD.WM_KEYUP || wParam == (int)WM_KEYBOARD.WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VKCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    this.OnKeyUp(this, e);
                    handled = handled || e.Handled;
                }

            }
            //handled 为true, 则其他程序无法接受到键盘事件 
            if (handled)
                return 1;
            else
                return Hocy_Hook.CallNextHookEx(this.m_pKeyboardHook, nCode, wParam, lParam);
        }



        #endregion 私有方法
         
    }
}
