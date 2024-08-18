using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AudioPluginGL.UI;
using Hexa.NET.ImGui;
using Hexa.NET.ImPlot;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Color = System.Drawing.Color;

namespace AudioPluginGL.UI;

public partial class GLView : UserControl
{
    public unsafe GLView()
    {
        InitializeComponent();

        Host.Loaded += (sender, args) =>
        {
            _guiController = new ImGuiController(Host.Gl, () =>
            {
                var io = ImGui.GetIO();
                if (File.Exists("C:\\Windows\\Fonts\\Bahnschrift.ttf"))
                    io.Fonts.AddFontFromFileTTF("C:\\Windows\\Fonts\\Bahnschrift.ttf", 14);
                else
                {
                    io.Fonts.AddFontDefault();
                    Console.WriteLine("Font not found, using default font.");
                }
            });
            _loaded = true;
            
            ImPlot.SetImGuiContext(_guiController.Context);
            var plotContext = ImPlot.CreateContext();
            ImPlot.SetCurrentContext(plotContext);
            ImPlot.StyleColorsDark(ImPlot.GetStyle());
        };
    }

    public void Stop()
    {
        _loaded = false;
        this.RemoveLogicalChild(Host);
        Host.Dispose();
    }

    private float _time;
    private ImGuiController _guiController;

    private bool _loaded;
    public unsafe void Render(float f)
    {
        if (!_loaded)
            return;
        ImGui.End();

        _guiController.Render();
        
        Host.SwapBuffers();
    }
    public void PreRender(float f)
    {
        _time += f;
        if (!_loaded)
        {
            return;
        }
        GL gl = Host.Gl;

        gl.Viewport(EditorWindow.Instance.RenderSize.ToV2());


        gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        gl.ClearColor(Color.CornflowerBlue);
        _guiController.Update(f);

        ImGui.Begin("Window", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove );
        ImGui.SetWindowSize(new Vector2((float)RenderSize.Width,(float) RenderSize.Height));
        ImGui.SetWindowPos(Vector2.Zero);
    }
}