using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Silk.NET.Maths;

namespace AudioPluginGL.UI;

public static class Input
{
    private static List<Key> _pressedKeys = new List<Key>();
    private static List<Key> _pressedKeysPrev = new List<Key>();
    private static List<Key> _pressedKeysBuff = new List<Key>();
    
    private static List<MouseButton> _pressedMouseButtons = new List<MouseButton>();
    private static List<MouseButton> _pressedMouseButtonsPrev = new List<MouseButton>();
    private static List<MouseButton> _pressedMouseButtonsBuff = new List<MouseButton>();

    private static Vector2D<int> _mousePos;
    private static Vector2D<int> _mouseScroll;

    public static Vector2D<int> MousePos => _mousePos;
    public static Vector2D<int> MouseScroll => _mouseScroll;

    public static Action<Key, uint, bool> KeyEvent;
    
    #region INPUT
    public static void SetKeyState(Key eKey, bool b)
    {
        if (b)
            _pressedKeysBuff.Add(eKey);
        else
            _pressedKeysBuff.Remove(eKey);
        
        KeyEvent?.Invoke(eKey, KeyHelper.GetScanCode(eKey), b);
    }

    public static void SetMousePosition(Point point)
    {
        _mousePos = new Vector2D<int>((int)point.X, (int)point.Y);
    }

    public static void SetMouseButton(MouseButton eChangedButton, MouseButtonState eButtonState)
    {
        if (eButtonState == MouseButtonState.Pressed)
            _pressedMouseButtonsBuff.Add(eChangedButton);
        else
            _pressedMouseButtonsBuff.Remove(eChangedButton);
    }

    public static void SetMouseScroll(int eDelta)
    {
        _mouseScroll = new Vector2D<int>(0, eDelta);
    }
    
    #endregion

    #region Logic

    public static bool IsKeyDown(Key key)
    {
        return _pressedKeys.Contains(key);
    }

    public static bool IsKeyReleased(Key key)
    {
        if (_pressedKeysPrev.Contains(key) && !_pressedKeys.Contains(key))
        {
            return true;
        }

        return false;
    }

    public static bool IsKeyPressed(Key key)
    {
        if (!_pressedKeysPrev.Contains(key) && _pressedKeys.Contains(key))
        {
            return true;
        }

        return false;
    }

    public static bool IsKeyUp(Key key)
    {
        return !IsKeyDown(key);
    }
    
    // Now for mouse logic

    public static bool IsMouseButtonDown(MouseButton button)
    {
        return _pressedMouseButtons.Contains(button);
    }

    public static bool IsMouseButtonReleased(MouseButton button)
    {
        if (!_pressedMouseButtons.Contains(button) && _pressedMouseButtonsPrev.Contains(button))
        {
            return true;
        }

        return false;
    }
    public static bool IsMouseButtonPressed(MouseButton button)
    {
        if (_pressedMouseButtons.Contains(button) && !_pressedMouseButtonsPrev.Contains(button))
        {
            return true;
        }

        return false;
    }

    public static void Update()
    {
        _pressedKeysPrev = _pressedKeys;
        _pressedMouseButtonsPrev = _pressedMouseButtons;

        _pressedKeys = _pressedKeysBuff;
        _pressedMouseButtons = _pressedMouseButtonsBuff;

        _pressedKeysBuff = new List<Key>();
        _pressedMouseButtonsBuff = new List<MouseButton>();
    }
    
    #endregion

    public static bool IsShiftDown()
    {
        return _pressedKeys.Contains(Key.LeftShift);
    }
}

public static class NativeMethods
{
    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, uint uMapType);

    public const uint MAPVK_VK_TO_VSC = 0x00;
}

public static class KeyHelper
{
    public static uint GetScanCode(Key key)
    {
        // Convert the Key enum to a virtual key code
        int virtualKey = KeyInterop.VirtualKeyFromKey(key);

        // Get the scan code from the virtual key code
        uint scanCode = NativeMethods.MapVirtualKey((uint)virtualKey, NativeMethods.MAPVK_VK_TO_VSC);

        return scanCode;
    }

    public static Silk.NET.Input.Key ToSilk(this Key key)
    {
        switch (key)
        {
            case Key.A:
                return Silk.NET.Input.Key.A;
            case Key.B:
                return Silk.NET.Input.Key.B;
            case Key.C:
                return Silk.NET.Input.Key.C;
            case Key.D:
                return Silk.NET.Input.Key.D;
            case Key.E:
                return Silk.NET.Input.Key.E;
            case Key.F:
                return Silk.NET.Input.Key.F;
            case Key.G:
                return Silk.NET.Input.Key.G;
            case Key.H:
                return Silk.NET.Input.Key.H;
            case Key.I:
                return Silk.NET.Input.Key.I;
            case Key.J:
                return Silk.NET.Input.Key.J;
            case Key.K:
                return Silk.NET.Input.Key.K;
            case Key.L:
                return Silk.NET.Input.Key.L;
            case Key.M:
                return Silk.NET.Input.Key.M;
            case Key.N:
                return Silk.NET.Input.Key.N;
            case Key.O:
                return Silk.NET.Input.Key.O;
            case Key.P:
                return Silk.NET.Input.Key.P;
            case Key.Q:
                return Silk.NET.Input.Key.Q;
            case Key.R:
                return Silk.NET.Input.Key.R;
            case Key.S:
                return Silk.NET.Input.Key.S;
            case Key.T:
                return Silk.NET.Input.Key.T;
            case Key.U:
                return Silk.NET.Input.Key.U;
            case Key.V:
                return Silk.NET.Input.Key.V;
            case Key.W:
                return Silk.NET.Input.Key.W;
            case Key.X:
                return Silk.NET.Input.Key.X;
            case Key.Y:
                return Silk.NET.Input.Key.Y;
            case Key.Z:
                return Silk.NET.Input.Key.Z;
            case Key.D0:
                return Silk.NET.Input.Key.Number0;
            case Key.D1:
                return Silk.NET.Input.Key.Number1;
            case Key.D2:
                return Silk.NET.Input.Key.Number2;
            case Key.D3:
                return Silk.NET.Input.Key.Number3;
            case Key.D4:
                return Silk.NET.Input.Key.Number4;
            case Key.D5:
                return Silk.NET.Input.Key.Number5;
            case Key.D6:
                return Silk.NET.Input.Key.Number6;
            case Key.D7:
                return Silk.NET.Input.Key.Number7;
            case Key.D8:
                return Silk.NET.Input.Key.Number8;
            case Key.D9:
                return Silk.NET.Input.Key.Number9;
            case Key.LeftShift:
                return Silk.NET.Input.Key.ShiftLeft;
            case Key.RightShift:
                return Silk.NET.Input.Key.ShiftRight;
            case Key.LeftCtrl:
                return Silk.NET.Input.Key.ControlLeft;
            case Key.RightCtrl:
                return Silk.NET.Input.Key.ControlRight;
            case Key.LeftAlt:
                return Silk.NET.Input.Key.AltLeft;
            case Key.RightAlt:
                return Silk.NET.Input.Key.AltRight;
            case Key.Left:
                return Silk.NET.Input.Key.Left;
            case Key.Right:
                return Silk.NET.Input.Key.Right;
            case Key.Up:
                return Silk.NET.Input.Key.Up;
            case Key.Down:
                return Silk.NET.Input.Key.Down;
            case Key.Enter:
                return Silk.NET.Input.Key.Enter;
            case Key.Space:
                return Silk.NET.Input.Key.Space;
            case Key.Back:
                return Silk.NET.Input.Key.Backspace;
            case Key.Tab:
                return Silk.NET.Input.Key.Tab;
            case Key.Escape:
                return Silk.NET.Input.Key.Escape;
            case Key.F1:
                return Silk.NET.Input.Key.F1;
            case Key.F2:
                return Silk.NET.Input.Key.F2;
            case Key.F3:
                return Silk.NET.Input.Key.F3;
            case Key.F4:
                return Silk.NET.Input.Key.F4;
            case Key.F5:
                return Silk.NET.Input.Key.F5;
            case Key.F6:
                return Silk.NET.Input.Key.F6;
            case Key.F7:
                return Silk.NET.Input.Key.F7;
            case Key.F8:
                return Silk.NET.Input.Key.F8;
            case Key.F9:
                return Silk.NET.Input.Key.F9;
            case Key.F10:
                return Silk.NET.Input.Key.F10;
            case Key.F11:
                return Silk.NET.Input.Key.F11;
            case Key.F12:
                return Silk.NET.Input.Key.F12;
            case Key.Home:
                return Silk.NET.Input.Key.Home;
            case Key.End:
                return Silk.NET.Input.Key.End;
            case Key.PageUp:
                return Silk.NET.Input.Key.PageUp;
            case Key.PageDown:
                return Silk.NET.Input.Key.PageDown;
            case Key.Insert:
                return Silk.NET.Input.Key.Insert;
            case Key.Delete:
                return Silk.NET.Input.Key.Delete;
            case Key.CapsLock:
                return Silk.NET.Input.Key.CapsLock;
            case Key.NumLock:
                return Silk.NET.Input.Key.NumLock;
            case Key.Scroll:
                return Silk.NET.Input.Key.ScrollLock;
            case Key.PrintScreen:
                return Silk.NET.Input.Key.PrintScreen;
            default:
                return Silk.NET.Input.Key.SuperRight;
        }
    }


    public static Key ToWindows(this Silk.NET.Input.Key key)
    {
        switch (key)
        {
            case Silk.NET.Input.Key.A:
                return Key.A;
            case Silk.NET.Input.Key.B:
                return Key.B;
            case Silk.NET.Input.Key.C:
                return Key.C;
            case Silk.NET.Input.Key.D:
                return Key.D;
            case Silk.NET.Input.Key.E:
                return Key.E;
            case Silk.NET.Input.Key.F:
                return Key.F;
            case Silk.NET.Input.Key.G:
                return Key.G;
            case Silk.NET.Input.Key.H:
                return Key.H;
            case Silk.NET.Input.Key.I:
                return Key.I;
            case Silk.NET.Input.Key.J:
                return Key.J;
            case Silk.NET.Input.Key.K:
                return Key.K;
            case Silk.NET.Input.Key.L:
                return Key.L;
            case Silk.NET.Input.Key.M:
                return Key.M;
            case Silk.NET.Input.Key.N:
                return Key.N;
            case Silk.NET.Input.Key.O:
                return Key.O;
            case Silk.NET.Input.Key.P:
                return Key.P;
            case Silk.NET.Input.Key.Q:
                return Key.Q;
            case Silk.NET.Input.Key.R:
                return Key.R;
            case Silk.NET.Input.Key.S:
                return Key.S;
            case Silk.NET.Input.Key.T:
                return Key.T;
            case Silk.NET.Input.Key.U:
                return Key.U;
            case Silk.NET.Input.Key.V:
                return Key.V;
            case Silk.NET.Input.Key.W:
                return Key.W;
            case Silk.NET.Input.Key.X:
                return Key.X;
            case Silk.NET.Input.Key.Y:
                return Key.Y;
            case Silk.NET.Input.Key.Z:
                return Key.Z;
            case Silk.NET.Input.Key.Number0:
                return Key.D0;
            case Silk.NET.Input.Key.Number1:
                return Key.D1;
            case Silk.NET.Input.Key.Number2:
                return Key.D2;
            case Silk.NET.Input.Key.Number3:
                return Key.D3;
            case Silk.NET.Input.Key.Number4:
                return Key.D4;
            case Silk.NET.Input.Key.Number5:
                return Key.D5;
            case Silk.NET.Input.Key.Number6:
                return Key.D6;
            case Silk.NET.Input.Key.Number7:
                return Key.D7;
            case Silk.NET.Input.Key.Number8:
                return Key.D8;
            case Silk.NET.Input.Key.Number9:
                return Key.D9;
            case Silk.NET.Input.Key.ShiftLeft:
                return Key.LeftShift;
            case Silk.NET.Input.Key.ShiftRight:
                return Key.RightShift;
            case Silk.NET.Input.Key.ControlLeft:
                return Key.LeftCtrl;
            case Silk.NET.Input.Key.ControlRight:
                return Key.RightCtrl;
            case Silk.NET.Input.Key.AltLeft:
                return Key.LeftAlt;
            case Silk.NET.Input.Key.AltRight:
                return Key.RightAlt;
            case Silk.NET.Input.Key.Left:
                return Key.Left;
            case Silk.NET.Input.Key.Right:
                return Key.Right;
            case Silk.NET.Input.Key.Up:
                return Key.Up;
            case Silk.NET.Input.Key.Down:
                return Key.Down;
            case Silk.NET.Input.Key.Enter:
                return Key.Enter;
            case Silk.NET.Input.Key.Space:
                return Key.Space;
            case Silk.NET.Input.Key.Backspace:
                return Key.Back;
            case Silk.NET.Input.Key.Tab:
                return Key.Tab;
            case Silk.NET.Input.Key.Escape:
                return Key.Escape;
            case Silk.NET.Input.Key.F1:
                return Key.F1;
            case Silk.NET.Input.Key.F2:
                return Key.F2;
            case Silk.NET.Input.Key.F3:
                return Key.F3;
            case Silk.NET.Input.Key.F4:
                return Key.F4;
            case Silk.NET.Input.Key.F5:
                return Key.F5;
            case Silk.NET.Input.Key.F6:
                return Key.F6;
            case Silk.NET.Input.Key.F7:
                return Key.F7;
            case Silk.NET.Input.Key.F8:
                return Key.F8;
            case Silk.NET.Input.Key.F9:
                return Key.F9;
            case Silk.NET.Input.Key.F10:
                return Key.F10;
            case Silk.NET.Input.Key.F11:
                return Key.F11;
            case Silk.NET.Input.Key.F12:
                return Key.F12;
            case Silk.NET.Input.Key.Home:
                return Key.Home;
            case Silk.NET.Input.Key.End:
                return Key.End;
            case Silk.NET.Input.Key.PageUp:
                return Key.PageUp;
            case Silk.NET.Input.Key.PageDown:
                return Key.PageDown;
            case Silk.NET.Input.Key.Insert:
                return Key.Insert;
            case Silk.NET.Input.Key.Delete:
                return Key.Delete;
            case Silk.NET.Input.Key.CapsLock:
                return Key.CapsLock;
            case Silk.NET.Input.Key.NumLock:
                return Key.NumLock;
            case Silk.NET.Input.Key.ScrollLock:
                return Key.Scroll;
            case Silk.NET.Input.Key.PrintScreen:
                return Key.PrintScreen;
            default:
                return Key.Oem1;
        }
    }

    public static char? ToCharacter(this Key key, bool upperCase)
    {        
        char returnChar = 'a';

        if (Char.TryParse(upperCase ? key.ToString().ToLower() : key.ToString().ToUpper(), out returnChar))
            return returnChar;
        
        // Convert alphabetic keys to their character representations.
        switch (key)
        {

            // Convert numeric keys to their character representations.
            case Key.D0: return '0';
            case Key.D1: return '1';
            case Key.D2: return '2';
            case Key.D3: return '3';
            case Key.D4: return '4';
            case Key.D5: return '5';
            case Key.D6: return '6';
            case Key.D7: return '7';
            case Key.D8: return '8';
            case Key.D9: return '9';

            // Handle common punctuation keys.
            case Key.Space: return ' ';
            case Key.Enter: return '\n';
            case Key.Tab: return '\t';
            
            default:
                return null;
        }
    }
}