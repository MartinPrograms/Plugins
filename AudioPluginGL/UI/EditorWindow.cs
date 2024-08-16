using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AudioPlugSharp;
using Silk.NET.Maths;
using Rectangle = System.Drawing.Rectangle;

namespace AudioPluginGL.UI;

public class EditorWindow : Window
{
    public static EditorWindow Instance;
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

    public IAudioPluginEditor Editor { get; private set; }
    public GLView EditorView { get; private set; }

    public Rectangle DisplayRectangle
    {
        get
        {
            Rectangle displayRectangle = new Rectangle();

            GetWindowRect(parentWindow, ref displayRectangle);

            return displayRectangle;
        }
    }

    public Action<float> Render;
    
    IntPtr parentWindow;
    public EditorWindow(IAudioPluginEditor editor, GLView editorView)
    {
        this.Editor = editor;
        this.EditorView = editorView;

        DataContext = Editor.Processor;
        StartRenderTimer();
        Instance = this;
    }

    public void SetSize(uint width, uint height)
    {
        Width = width;
        Height = height;
    }

    private float lastTime;

    public void Show(IntPtr parentWindow)
    {
        this.parentWindow = parentWindow;

        Content = EditorView;

        Top = 0;
        Left = 0;
        ShowInTaskbar = false;
        WindowStyle = System.Windows.WindowStyle.None;
        ResizeMode = System.Windows.ResizeMode.NoResize;
        Show();

        var windowHwnd = new System.Windows.Interop.WindowInteropHelper(this);
        IntPtr hWnd = windowHwnd.Handle;
        SetParent(hWnd, parentWindow);

        Instance = this;

        this.PreviewMouseMove += Host_OnMouseMove;
        this.PreviewKeyDown += Host_OnKeyDown;
        this.PreviewKeyUp += Host_OnKeyUp;
        this.PreviewMouseDown += Host_OnMouseDown;
        this.PreviewMouseUp += Host_OnMouseUp;
        this.PreviewMouseWheel += Host_OnMouseWheel;
        this.Closing += OnClosing;
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        EditorView.Stop();
    }

    protected override void OnRender(DrawingContext context)
    {
        Render?.Invoke(1.0f / FPS);
    }

    private float _fps = 144;
    public float FPS
    {
        get
        {
            return _fps;
        }
        set
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1.0 / value);
            _fps = value;
        }
    }
    private  void Host_OnKeyDown(object sender, KeyEventArgs e)
    {
        Input.SetKeyState(e.Key, true);
    }

    private void Host_OnKeyUp(object sender, KeyEventArgs e)
    {
        Input.SetKeyState(e.Key, false);
    }

    private void Host_OnMouseMove(object sender, MouseEventArgs e)
    {
        
    }

    private void Host_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Input.SetMouseButton(e.ChangedButton, e.ButtonState);
    }

    private void Host_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        Input.SetMouseButton(e.ChangedButton, e.ButtonState);
    }

    private void Host_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        Input.SetMouseScroll(e.Delta);
    }

    
    private void StartRenderTimer()
    {
        // Set up a timer to force re-rendering every second
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(1.0 / FPS);
        _timer.Tick += (s, e) =>
        {
            InvalidateVisual();
            if(this.IsActive)
                 Input.SetMousePosition(this.PointFromScreen(new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y)));
            Input.SetMouseButton(MouseButton.Left, Mouse.LeftButton);
            Input.SetMouseButton(MouseButton.Right, Mouse.RightButton);
            Input.SetMouseButton(MouseButton.Middle, Mouse.MiddleButton);
        };
        
        Mouse.AddPreviewMouseWheelHandler(this, (sender, args) =>
        {
            Input.SetMouseScroll(args.Delta);
            Console.Beep();
        });
        
        _timer.Start();
    }
    private DispatcherTimer _timer;
    public Action<Vector2D<int>> Resize;
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        Resize?.Invoke(sizeInfo.NewSize.ToV2());
        base.OnRenderSizeChanged(sizeInfo);
    }
}