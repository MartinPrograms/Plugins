using System.Runtime.InteropServices;
using AudioPlugSharp;
using Logger = StupidSimpleLogger.Logger;

namespace AudioPluginGL.UI;

public class AudioPluginOpenGL : AudioPluginBase
{
    [DllImport("User32.dll")]
    private static extern IntPtr MonitorFromPoint([In] System.Drawing.Point pt, [In] uint dwFlags);

    [DllImport("Shcore.dll")]
    private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

    public enum DpiType
    {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }

    public EditorWindow EditorWindow { get; set; }
    public GLView EditorView { get; set; }

    public AudioPluginOpenGL()
    {
        // WPF requires a shared assembly load context: https://github.com/dotnet/wpf/issues/1700
        CacheLoadContext = true;
    }

    public override double GetDpiScale()
    {
        uint dpiX;
        uint dpiY;

        var pnt = new System.Drawing.Point(0, 0);
        var mon = MonitorFromPoint(pnt, 2);
        GetDpiForMonitor(mon, DpiType.Effective, out dpiX, out dpiY);

        return (double)dpiX / 96.0;
    }

    public override void HideEditor()
    {
        Shown = false;
        base.HideEditor();
    }

    public GLView GetEditorView()
    {
        var view = new GLView();
        
        Render += f =>
        {

            if (Shown)
            {
                view.PreRender(f);
                if (view.IsLoaded)
                    RenderUI(f);
                view.Render(f);
            }
        };     
        
        return view;
    }

    public virtual void RenderUI(float dt) 
    {
        
    }
    
    public override void ResizeEditor(uint newWidth, uint newHeight)
    {
        base.ResizeEditor(newWidth, newHeight);

        if (EditorWindow != null)
        {
            EditorWindow.SetSize(EditorWidth, EditorHeight);
        }
    }

    public Action<float> Render;
    public bool Shown;
    public override void ShowEditor(IntPtr parentWindow)
    {
        Shown = true;
        try
        {
            Logger.Info("Open editor. Thread ID is: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            if(EditorView == null)
                EditorView = GetEditorView();

            EditorWindow = new EditorWindow(this, EditorView);

            EditorWindow.SetSize(EditorWidth, EditorHeight);

            EditorWindow.Render += f => { Render?.Invoke(f); Input.Update(); };

            EditorWindow.Show(parentWindow);
        }
        catch (Exception ex)
        {
            StupidSimpleLogger.Logger.Fatal(ex.Message, ex.StackTrace);
            StupidSimpleLogger.Logger.DumpLogs();
        }
    }
}
