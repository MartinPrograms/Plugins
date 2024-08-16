using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Silk.NET.Core.Contexts;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using StupidSimpleLogger;

namespace AudioPluginGL.UI;

public class RenderHost : HwndHost, INativeContext
  {
      private HwndSource? _hwndSource;
      private GL? _gl;
      private readonly nint _hLib;
      private nint _glCtx;
      private nint _hdc;

      #region NATIVE

      [DllImport("Opengl32.dll")]
      static extern IntPtr wglCreateContext(IntPtr hdc);

      [DllImport("Opengl32.dll")]
      static extern IntPtr wglDeleteContext(IntPtr hglrc);

      [DllImport("Opengl32.dll", SetLastError = true)]
      static extern IntPtr wglChoosePixelFormatARB(IntPtr hdc);

      [DllImport("Opengl32.dll", SetLastError = true)]
      static extern IntPtr wglGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string unnamedParam1);

      [DllImport("Opengl32.dll", SetLastError = true)]
      static extern bool wglMakeCurrent(IntPtr hdc, IntPtr hglrc);

      [DllImport("gdi32.dll", SetLastError = true)]
      static extern int ChoosePixelFormat(IntPtr hDC,
      [In, MarshalAs(UnmanagedType.LPStruct)] PIXELFORMATDESCRIPTOR ppfd);

      [DllImport("gdi32.dll", SetLastError = true)]
      static extern bool SetPixelFormat(IntPtr hDC, int iPixelFormat,
          [In, MarshalAs(UnmanagedType.LPStruct)] PIXELFORMATDESCRIPTOR ppfd);

      [DllImport("gdi32.dll")]
      static extern int SwapBuffers(IntPtr hDC);

      [DllImport("User32.dll")]
      static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);


      [DllImport("User32.dll")]
      static extern IntPtr GetDC(IntPtr hWnd);


      [DllImport("kernel32.dll")]
      static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);

      [DllImport("kernel32.dll")]
      static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr)] string lpLibFileName);

      [StructLayout(LayoutKind.Explicit)]
      class PIXELFORMATDESCRIPTOR
      {
          [FieldOffset(0)]
          public UInt16 nSize;
          [FieldOffset(2)]
          public UInt16 nVersion;
          [FieldOffset(4)]
          public UInt32 dwFlags;
          [FieldOffset(8)]
          public Byte iPixelType;
          [FieldOffset(9)]
          public Byte cColorBits;
          [FieldOffset(10)]
          public Byte cRedBits;
          [FieldOffset(11)]
          public Byte cRedShift;
          [FieldOffset(12)]
          public Byte cGreenBits;
          [FieldOffset(13)]
          public Byte cGreenShift;
          [FieldOffset(14)]
          public Byte cBlueBits;
          [FieldOffset(15)]
          public Byte cBlueShift;
          [FieldOffset(16)]
          public Byte cAlphaBits;
          [FieldOffset(17)]
          public Byte cAlphaShift;
          [FieldOffset(18)]
          public Byte cAccumBits;
          [FieldOffset(19)]
          public Byte cAccumRedBits;
          [FieldOffset(20)]
          public Byte cAccumGreenBits;
          [FieldOffset(21)]
          public Byte cAccumBlueBits;
          [FieldOffset(22)]
          public Byte cAccumAlphaBits;
          [FieldOffset(23)]
          public Byte cDepthBits;
          [FieldOffset(24)]
          public Byte cStencilBits;
          [FieldOffset(25)]
          public Byte cAuxBuffers;
          [FieldOffset(26)]
          public SByte iLayerType;
          [FieldOffset(27)]
          public Byte bReserved;
          [FieldOffset(28)]
          public UInt32 dwLayerMask;
          [FieldOffset(32)]
          public UInt32 dwVisibleMask;
          [FieldOffset(36)]
          public UInt32 dwDamageMask;
      }

      const byte PFD_TYPE_RGBA = 0;
      const byte PFD_TYPE_COLORINDEX = 1;

      const uint PFD_DOUBLEBUFFER = 1;
      const uint PFD_STEREO = 2;
      const uint PFD_DRAW_TO_WINDOW = 4;
      const uint PFD_DRAW_TO_BITMAP = 8;
      const uint PFD_SUPPORT_GDI = 16;
      const uint PFD_SUPPORT_OPENGL = 32;
      const uint PFD_GENERIC_FORMAT = 64;
      const uint PFD_NEED_PALETTE = 128;
      const uint PFD_NEED_SYSTEM_PALETTE = 256;
      const uint PFD_SWAP_EXCHANGE = 512;
      const uint PFD_SWAP_COPY = 1024;
      const uint PFD_SWAP_LAYER_BUFFERS = 2048;
      const uint PFD_GENERIC_ACCELERATED = 4096;
      const uint PFD_SUPPORT_DIRECTDRAW = 8192;
      const uint PFD_DIRECT3D_ACCELERATED = 0x00004000;
      const uint PFD_SUPPORT_COMPOSITION = 0x00008000;

      const sbyte PFD_MAIN_PLANE = 0;
      const sbyte PFD_OVERLAY_PLANE = 1;
      const sbyte PFD_UNDERLAY_PLANE = -1;

      const uint WS_CHILD = 0x40000000;

      #endregion

      public RenderHost()
      {
          try
          {
              _hLib = LoadLibraryW("opengl32.dll");
          }
          catch (Exception ex)
          {
              Logger.Fatal(ex.Message, ex.StackTrace);
              Logger.DumpLogs();

          }
      }

      protected unsafe override HandleRef BuildWindowCore(HandleRef hwndParent)
      {
          try
          {
              if (DesignerProperties.GetIsInDesignMode(this))
                  return new HandleRef(null, IntPtr.Zero);

              _hwndSource = new HwndSource(0, (int)WS_CHILD, 0, 0, 0, "RenderView", hwndParent.Handle);

              var handle = _hwndSource.CreateHandleRef();

              var pfd = new PIXELFORMATDESCRIPTOR
              {
                  nSize = (ushort)Marshal.SizeOf<PIXELFORMATDESCRIPTOR>(),
                  nVersion = 1,
                  iPixelType = PFD_TYPE_RGBA,
                  dwFlags = PFD_SUPPORT_OPENGL | PFD_SUPPORT_COMPOSITION | PFD_DIRECT3D_ACCELERATED |
                            PFD_DRAW_TO_WINDOW | PFD_DOUBLEBUFFER,
                  iLayerType = PFD_MAIN_PLANE,
                  cColorBits = 24,
                  cDepthBits = 32
              };

              _hdc = GetDC(handle.Handle);

              var pfIndex = ChoosePixelFormat(_hdc, pfd);
              if (pfIndex <= 0)
                  throw new Win32Exception();

              if (!SetPixelFormat(_hdc, pfIndex, pfd))
                  throw new Win32Exception();

              _glCtx = wglCreateContext(_hdc);

              if (_glCtx == IntPtr.Zero)
                  throw new Win32Exception();

              TakeContext();
              
              _gl = GL.GetApi(this);
              
              return handle;
          }
          catch (Exception ex)
          {
              Logger.Fatal(ex.Message, ex.StackTrace);
              Logger.DumpLogs();

          }

          return new HandleRef(null, IntPtr.Zero);
      }

      public void SwapBuffers()
      {
          try
          {
              _ = SwapBuffers(_hdc);
          }
          catch (Exception ex)
          {
              Logger.Fatal(ex.Message, ex.StackTrace);
              Logger.DumpLogs();

          }
      }

      protected override void DestroyWindowCore(HandleRef hwnd)
        {
            try
            {
                if (_glCtx != 0)
                {
                    wglDeleteContext(_glCtx);
                    _glCtx = 0;
                }

                if (_hwndSource != null)
                {
                    _ = ReleaseDC(_hwndSource.Handle, _hdc);
                    _hwndSource.Dispose();
                    _hwndSource = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message, ex.StackTrace);
                Logger.DumpLogs();
            }
        }

        public nint GetProcAddress(string proc, int? slot = null)
        {
            var addr = GetProcAddress(_hLib, proc);

            if (addr == IntPtr.Zero)
            {
                addr = wglGetProcAddress(proc);
                if (addr == IntPtr.Zero)
                    throw new NotSupportedException(proc);
            }

            return addr;
        }

        public void ReleaseContext()
        {
            if (!wglMakeCurrent(_hdc, IntPtr.Zero))
                throw new Win32Exception();
        }

        public void TakeContext()
        {
            if (!wglMakeCurrent(_hdc, _glCtx))
                throw new Win32Exception();
        }

        public bool TryGetProcAddress(string proc, out nint addr, int? slot = null)
        {
            addr = wglGetProcAddress(proc);
            return addr != 0;
        }

        public GL? Gl => _gl;

        public IntPtr Hdc => _hdc;

        public IntPtr GlCtx => _glCtx;
        public RenderHostView View;
  }

public class RenderHostView : IView
{
    public RenderHostView()
    {
        Instance = this;
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }

    public bool ShouldSwapAutomatically { get; set; }
    public bool IsEventDriven { get; set; }
    public bool IsContextControlDisabled { get; set; }

    public Vector2D<int> Size
    {
        get { return EditorWindow.Instance.RenderSize.ToV2(); }
    }

    public double FramesPerSecond { get; set; }
    public double UpdatesPerSecond { get; set; }
    public GraphicsAPI API { get; }
    public bool VSync { get; set; }
    public VideoMode VideoMode { get; }
    public int? PreferredDepthBufferBits { get; }
    public int? PreferredStencilBufferBits { get; }
    public Vector4D<int>? PreferredBitDepth { get; }
    public int? Samples { get; }
    public IGLContext? GLContext { get; }
    public IVkSurface? VkSurface { get; }
    public INativeWindow? Native { get; }
    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public void DoRender()
    {
        throw new NotImplementedException();
    }

    public void DoUpdate()
    {
        throw new NotImplementedException();
    }

    public void DoEvents()
    {
        throw new NotImplementedException();
    }

    public void ContinueEvents()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public Vector2D<int> PointToClient(Vector2D<int> point)
    {
        throw new NotImplementedException();
    }

    public Vector2D<int> PointToScreen(Vector2D<int> point)
    {
        throw new NotImplementedException();
    }

    public Vector2D<int> PointToFramebuffer(Vector2D<int> point)
    {
        throw new NotImplementedException();
    }

    public object Invoke(Delegate d, params object[] args)
    {
        throw new NotImplementedException();
    }

    public void Run(Action onFrame)
    {
        throw new NotImplementedException();
    }

    public nint Handle { get; }
    public bool IsClosing { get; }
    public double Time { get; }

    public Vector2D<int> FramebufferSize
    {
        get { return Size; }
    }

    public bool IsInitialized { get; }
    public event Action<Vector2D<int>>? Resize;
    public event Action<Vector2D<int>>? FramebufferResize;
    public event Action? Closing;
    public event Action<bool>? FocusChanged;
    public event Action? Load;
    public event Action<double>? Update;
    public event Action<double>? Render;

    public static RenderHostView Instance;
}

public static class Extensions{
    public static Vector2D<int> ToV2(this Size size)
    {
        return new Vector2D<int>((int)size.Width,(int) size.Height);
    }
}