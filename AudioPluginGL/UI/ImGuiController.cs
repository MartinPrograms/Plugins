﻿#define GL

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AudioPluginGL.UI;
using Hexa.NET.ImGui;
using Silk.NET.Input;
using Silk.NET.Input.Extensions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Key = Silk.NET.Input.Key;
using MouseButton = Silk.NET.Input.MouseButton;

namespace AudioPluginGL.UI;


    public class ImGuiController : IDisposable
    {
        private GL _gl;
        private bool _frameBegun;
        private readonly List<char> _pressedChars = new();
        private IKeyboard _keyboard;

        private int _attribLocationTex;
        private int _attribLocationProjMtx;
        private int _attribLocationVtxPos;
        private int _attribLocationVtxUV;
        private int _attribLocationVtxColor;
        private uint _vboHandle;
        private uint _elementsHandle;
        private uint _vertexArrayObject;

        private Texture _fontTexture;
        private Shader _shader;

        private int _windowWidth;
        private int _windowHeight;

        public ImGuiContextPtr Context;

        /// <summary>
        /// Constructs a new ImGuiController.
        /// </summary>
        public ImGuiController(GL gl) : this(gl, null, null)
        {
        }

        /// <summary>
        /// Constructs a new ImGuiController with font configuration.
        /// </summary>
        public ImGuiController(GL gl, ImGuiFontConfig imGuiFontConfig) : this(gl, imGuiFontConfig, null)
        {
        }

        /// <summary>
        /// Constructs a new ImGuiController with an onConfigureIO Action.
        /// </summary>
        public ImGuiController(GL gl, Action onConfigureIO) : this(gl, null, onConfigureIO)
        {
        }

        /// <summary>
        /// Constructs a new ImGuiController with font configuration and onConfigure Action.
        /// </summary>
        public ImGuiController(GL gl, ImGuiFontConfig? imGuiFontConfig = null, Action onConfigureIO = null)
        {
            Init(gl);

            var io = ImGui.GetIO();
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
            io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;
            
            onConfigureIO?.Invoke();

            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

            CreateDeviceResources();

            SetPerFrameImGuiData(1f / 60f);

            BeginFrame();
        }

        public void MakeCurrent()
        {
            ImGui.SetCurrentContext(Context);
        }

        private void Init(GL gl)
        {
            _gl = gl;
            _windowWidth = (int)EditorWindow.Instance.RenderSize.Width;
            _windowHeight = (int)EditorWindow.Instance.RenderSize.Height;
            
            // load the cimgui.dll
            Context = ImGui.CreateContext();
            
            ImGui.SetCurrentContext(Context);
            
            ImGui.StyleColorsDark();
        }

        private void BeginFrame()
        {
            ImGui.NewFrame();
            _frameBegun = true;

            EditorWindow.Instance.Resize += WindowResized;
            Input.KeyEvent += (key, u, arg3) =>
            {
                OnKeyEvent(key.ToSilk(), (int)u, arg3);
                if (arg3)
                {
                    var keychar = key.ToCharacter(Input.IsShiftDown());
                    if (keychar != null)
                    {
                        _pressedChars.Add(keychar.Value);
                    }
                }
            };
        }



        /// <summary>
        /// Delegate to receive keyboard key events.
        /// </summary>
        /// <param name="keyboard">The keyboard context generating the event.</param>
        /// <param name="keycode">The native keycode of the key generating the event.</param>
        /// <param name="scancode">The native scancode of the key generating the event.</param>
        /// <param name="down">True if the event is a key down event, otherwise False</param>
        private static void OnKeyEvent( Key keycode, int scancode, bool down)
        {
            var io = ImGui.GetIO();
            var imGuiKey = TranslateInputKeyToImGuiKey(keycode);
            io.AddKeyEvent(imGuiKey, down);
            io.SetKeyEventNativeData(imGuiKey, (int) keycode, scancode);
        }

        private void OnKeyChar(IKeyboard arg1, char arg2)
        {
            _pressedChars.Add(arg2);
        }

        private void WindowResized(Vector2D<int> size)
        {
            _windowWidth = size.X;
            _windowHeight = size.Y;
        }

        /// <summary>
        /// Renders the ImGui draw list data.
        /// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
        /// or index data has increased beyond the capacity of the existing buffers.
        /// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
        /// </summary>
        public void Render()
        {
            if (_frameBegun)
            {
                var oldCtx = ImGui.GetCurrentContext();

                if (oldCtx != Context)
                {
                    ImGui.SetCurrentContext(Context);
                }

                _frameBegun = false;
                ImGui.Render();
                RenderImDrawData(ImGui.GetDrawData());

                if (oldCtx != Context)
                {
                    ImGui.SetCurrentContext(oldCtx);
                }
            }
        }

        /// <summary>
        /// Updates ImGui input and IO configuration state.
        /// </summary>
        public void Update(float deltaSeconds)
        {
            var oldCtx = ImGui.GetCurrentContext();

            if (oldCtx != Context)
            {
                ImGui.SetCurrentContext(Context);
            }

            if (_frameBegun)
            {
                ImGui.Render();
            }

            SetPerFrameImGuiData(deltaSeconds);
            UpdateImGuiInput();

            _frameBegun = true;
            ImGui.NewFrame();

            if (oldCtx != Context)
            {
                ImGui.SetCurrentContext(oldCtx);
            }
        }

        /// <summary>
        /// Sets per-frame data based on the associated window.
        /// This is called by Update(float).
        /// </summary>
        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            var io = ImGui.GetIO();
            io.DisplaySize = new Vector2(_windowWidth, _windowHeight);

            if (_windowWidth > 0 && _windowHeight > 0)
            {
                io.DisplayFramebufferScale = new Vector2((int)EditorWindow.Instance.RenderSize.Width / _windowWidth,
                    (int)EditorWindow.Instance.RenderSize.Height / _windowHeight);
            }

            io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
        }

        private void UpdateImGuiInput()
        {
            var io = ImGui.GetIO();
            // TODO: make this

            io.MouseDown[0] = Input.IsMouseButtonDown(System.Windows.Input.MouseButton.Left);
            io.MouseDown[1] = Input.IsMouseButtonDown(System.Windows.Input.MouseButton.Right);
            io.MouseDown[2] = Input.IsMouseButtonDown(System.Windows.Input.MouseButton.Middle);

            var point = new Point((int)Input.MousePos.X, (int)Input.MousePos.Y);
            io.MousePos = new Vector2(point.X, point.Y);
            io.MouseWheel = Input.MouseScroll.Y;
            io.MouseWheelH = Input.MouseScroll.X;

            foreach (var c in _pressedChars)
            {
                io.AddInputCharacter(c);
            }
            
            _pressedChars.Clear();

            io.KeyCtrl = Input.IsKeyDown(System.Windows.Input.Key.RightCtrl) ||
                         Input.IsKeyDown(System.Windows.Input.Key.LeftCtrl);
            
            io.KeyAlt = Input.IsKeyDown(System.Windows.Input.Key.LeftAlt) ||
                        Input.IsKeyDown(System.Windows.Input.Key.RightAlt);

            io.KeyShift = Input.IsKeyDown(System.Windows.Input.Key.LeftShift) ||
                          Input.IsKeyDown(System.Windows.Input.Key.RightShift);
        }

        internal void PressChar(char keyChar)
        {
            _pressedChars.Add(keyChar);
        }

        /// <summary>
        /// Translates a Silk.NET.Input.Key to an ImGuiKey.
        /// </summary>
        /// <param name="key">The Silk.NET.Input.Key to translate.</param>
        /// <returns>The corresponding ImGuiKey.</returns>
        /// <exception cref="NotImplementedException">When the key has not been implemented yet.</exception>
        private static ImGuiKey TranslateInputKeyToImGuiKey(Key key)
        {
            return key switch
            {
                Key.Tab => ImGuiKey.Tab,
                Key.Left => ImGuiKey.LeftArrow,
                Key.Right => ImGuiKey.RightArrow,
                Key.Up => ImGuiKey.UpArrow,
                Key.Down => ImGuiKey.DownArrow,
                Key.PageUp => ImGuiKey.PageUp,
                Key.PageDown => ImGuiKey.PageDown,
                Key.Home => ImGuiKey.Home,
                Key.End => ImGuiKey.End,
                Key.Insert => ImGuiKey.Insert,
                Key.Delete => ImGuiKey.Delete,
                Key.Backspace => ImGuiKey.Backspace,
                Key.Space => ImGuiKey.Space,
                Key.Enter => ImGuiKey.Enter,
                Key.Escape => ImGuiKey.Escape,
                Key.Apostrophe => ImGuiKey.Apostrophe,
                Key.Comma => ImGuiKey.Comma,
                Key.Minus => ImGuiKey.Minus,
                Key.Period => ImGuiKey.Period,
                Key.Slash => ImGuiKey.Slash,
                Key.Semicolon => ImGuiKey.Semicolon,
                Key.Equal => ImGuiKey.Equal,
                Key.LeftBracket => ImGuiKey.LeftBracket,
                Key.BackSlash => ImGuiKey.Backslash,
                Key.RightBracket => ImGuiKey.RightBracket,
                Key.GraveAccent => ImGuiKey.GraveAccent,
                Key.CapsLock => ImGuiKey.CapsLock,
                Key.ScrollLock => ImGuiKey.ScrollLock,
                Key.NumLock => ImGuiKey.NumLock,
                Key.PrintScreen => ImGuiKey.PrintScreen,
                Key.Pause => ImGuiKey.Pause,
                Key.Keypad0 => ImGuiKey.Keypad0,
                Key.Keypad1 => ImGuiKey.Keypad1,
                Key.Keypad2 => ImGuiKey.Keypad2,
                Key.Keypad3 => ImGuiKey.Keypad3,
                Key.Keypad4 => ImGuiKey.Keypad4,
                Key.Keypad5 => ImGuiKey.Keypad5,
                Key.Keypad6 => ImGuiKey.Keypad6,
                Key.Keypad7 => ImGuiKey.Keypad7,
                Key.Keypad8 => ImGuiKey.Keypad8,
                Key.Keypad9 => ImGuiKey.Keypad9,
                Key.KeypadDecimal => ImGuiKey.KeypadDecimal,
                Key.KeypadDivide => ImGuiKey.KeypadDivide,
                Key.KeypadMultiply => ImGuiKey.KeypadMultiply,
                Key.KeypadSubtract => ImGuiKey.KeypadSubtract,
                Key.KeypadAdd => ImGuiKey.KeypadAdd,
                Key.KeypadEnter => ImGuiKey.KeypadEnter,
                Key.KeypadEqual => ImGuiKey.KeypadEqual,
                Key.ShiftLeft => ImGuiKey.LeftShift,
                Key.ControlLeft => ImGuiKey.LeftCtrl,
                Key.AltLeft => ImGuiKey.LeftAlt,
                Key.SuperLeft => ImGuiKey.LeftSuper,
                Key.ShiftRight => ImGuiKey.RightShift,
                Key.ControlRight => ImGuiKey.RightCtrl,
                Key.AltRight => ImGuiKey.RightAlt,
                Key.SuperRight => ImGuiKey.RightSuper,
                Key.Menu => ImGuiKey.Menu,
                Key.Number0 => ImGuiKey.Key0,
                Key.Number1 => ImGuiKey.Key1,
                Key.Number2 => ImGuiKey.Key2,
                Key.Number3 => ImGuiKey.Key3,
                Key.Number4 => ImGuiKey.Key4,
                Key.Number5 => ImGuiKey.Key5,
                Key.Number6 => ImGuiKey.Key6,
                Key.Number7 => ImGuiKey.Key7,
                Key.Number8 => ImGuiKey.Key8,
                Key.Number9 => ImGuiKey.Key9,
                Key.A => ImGuiKey.A,
                Key.B => ImGuiKey.B,
                Key.C => ImGuiKey.C,
                Key.D => ImGuiKey.D,
                Key.E => ImGuiKey.E,
                Key.F => ImGuiKey.F,
                Key.G => ImGuiKey.G,
                Key.H => ImGuiKey.H,
                Key.I => ImGuiKey.I,
                Key.J => ImGuiKey.J,
                Key.K => ImGuiKey.K,
                Key.L => ImGuiKey.L,
                Key.M => ImGuiKey.M,
                Key.N => ImGuiKey.N,
                Key.O => ImGuiKey.O,
                Key.P => ImGuiKey.P,
                Key.Q => ImGuiKey.Q,
                Key.R => ImGuiKey.R,
                Key.S => ImGuiKey.S,
                Key.T => ImGuiKey.T,
                Key.U => ImGuiKey.U,
                Key.V => ImGuiKey.V,
                Key.W => ImGuiKey.W,
                Key.X => ImGuiKey.X,
                Key.Y => ImGuiKey.Y,
                Key.Z => ImGuiKey.Z,
                Key.F1 => ImGuiKey.F1,
                Key.F2 => ImGuiKey.F2,
                Key.F3 => ImGuiKey.F3,
                Key.F4 => ImGuiKey.F4,
                Key.F5 => ImGuiKey.F5,
                Key.F6 => ImGuiKey.F6,
                Key.F7 => ImGuiKey.F7,
                Key.F8 => ImGuiKey.F8,
                Key.F9 => ImGuiKey.F9,
                Key.F10 => ImGuiKey.F10,
                Key.F11 => ImGuiKey.F11,
                Key.F12 => ImGuiKey.F12,
                Key.F13 => ImGuiKey.F13,
                Key.F14 => ImGuiKey.F14,
                Key.F15 => ImGuiKey.F15,
                Key.F16 => ImGuiKey.F16,
                Key.F17 => ImGuiKey.F17,
                Key.F18 => ImGuiKey.F18,
                Key.F19 => ImGuiKey.F19,
                Key.F20 => ImGuiKey.F20,
                Key.F21 => ImGuiKey.F21,
                Key.F22 => ImGuiKey.F22,
                Key.F23 => ImGuiKey.F23,
                Key.F24 => ImGuiKey.F24,
                _ => throw new NotImplementedException(),
            };
        }

        private unsafe void SetupRenderState(ImDrawDataPtr drawDataPtr, int framebufferWidth, int framebufferHeight)
        {
            // Setup render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, polygon fill
            _gl.Enable(GLEnum.Blend);
            _gl.BlendEquation(GLEnum.FuncAdd);
            _gl.BlendFuncSeparate(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha, GLEnum.One, GLEnum.OneMinusSrcAlpha);
            _gl.Disable(GLEnum.CullFace);
            _gl.Disable(GLEnum.DepthTest);
            _gl.Disable(GLEnum.StencilTest);
            _gl.Enable(GLEnum.ScissorTest);
#if !GLES && !LEGACY
            _gl.Disable(GLEnum.PrimitiveRestart);
            _gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);
#endif

            float L = drawDataPtr.DisplayPos.X;
            float R = drawDataPtr.DisplayPos.X + drawDataPtr.DisplaySize.X;
            float T = drawDataPtr.DisplayPos.Y;
            float B = drawDataPtr.DisplayPos.Y + drawDataPtr.DisplaySize.Y;

            Span<float> orthoProjection = stackalloc float[] {
                2.0f / (R - L), 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f / (T - B), 0.0f, 0.0f,
                0.0f, 0.0f, -1.0f, 0.0f,
                (R + L) / (L - R), (T + B) / (B - T), 0.0f, 1.0f,
            };

            _shader.UseShader();
            _gl.Uniform1(_attribLocationTex, 0);
            _gl.UniformMatrix4(_attribLocationProjMtx, 1, false, orthoProjection);
            _gl.CheckGlError("Projection");

            _gl.BindSampler(0, 0);

            // Setup desired GL state
            // Recreate the VAO every time (this is to easily allow multiple GL contexts to be rendered to. VAO are not shared among GL contexts)
            // The renderer would actually work without any VAO bound, but then our VertexAttrib calls would overwrite the default one currently bound.
            _vertexArrayObject = _gl.GenVertexArray();
            _gl.BindVertexArray(_vertexArrayObject);
            _gl.CheckGlError("VAO");

            // Bind vertex/index buffers and setup attributes for ImDrawVert
            _gl.BindBuffer(GLEnum.ArrayBuffer, _vboHandle);
            _gl.BindBuffer(GLEnum.ElementArrayBuffer, _elementsHandle);
            _gl.EnableVertexAttribArray((uint) _attribLocationVtxPos);
            _gl.EnableVertexAttribArray((uint) _attribLocationVtxUV);
            _gl.EnableVertexAttribArray((uint) _attribLocationVtxColor);
            _gl.VertexAttribPointer((uint) _attribLocationVtxPos, 2, GLEnum.Float, false, (uint) sizeof(ImDrawVert), (void*) 0);
            _gl.VertexAttribPointer((uint) _attribLocationVtxUV, 2, GLEnum.Float, false, (uint) sizeof(ImDrawVert), (void*) 8);
            _gl.VertexAttribPointer((uint) _attribLocationVtxColor, 4, GLEnum.UnsignedByte, true, (uint) sizeof(ImDrawVert), (void*) 16);
        }

        private unsafe void RenderImDrawData(ImDrawDataPtr drawDataPtr)
        {
            int framebufferWidth = (int) (drawDataPtr.DisplaySize.X * drawDataPtr.FramebufferScale.X);
            int framebufferHeight = (int) (drawDataPtr.DisplaySize.Y * drawDataPtr.FramebufferScale.Y);
            if (framebufferWidth <= 0 || framebufferHeight <= 0)
                return;

            // Backup GL state
            _gl.GetInteger(GLEnum.ActiveTexture, out int lastActiveTexture);
            _gl.ActiveTexture(GLEnum.Texture0);

            _gl.GetInteger(GLEnum.CurrentProgram, out int lastProgram);
            _gl.GetInteger(GLEnum.TextureBinding2D, out int lastTexture);

            _gl.GetInteger(GLEnum.SamplerBinding, out int lastSampler);

            _gl.GetInteger(GLEnum.ArrayBufferBinding, out int lastArrayBuffer);
            _gl.GetInteger(GLEnum.VertexArrayBinding, out int lastVertexArrayObject);

#if !GLES
            Span<int> lastPolygonMode = stackalloc int[2];
            _gl.GetInteger(GLEnum.PolygonMode, lastPolygonMode);
#endif

            Span<int> lastScissorBox = stackalloc int[4];
            _gl.GetInteger(GLEnum.ScissorBox, lastScissorBox);

            _gl.GetInteger(GLEnum.BlendSrcRgb, out int lastBlendSrcRgb);
            _gl.GetInteger(GLEnum.BlendDstRgb, out int lastBlendDstRgb);

            _gl.GetInteger(GLEnum.BlendSrcAlpha, out int lastBlendSrcAlpha);
            _gl.GetInteger(GLEnum.BlendDstAlpha, out int lastBlendDstAlpha);

            _gl.GetInteger(GLEnum.BlendEquationRgb, out int lastBlendEquationRgb);
            _gl.GetInteger(GLEnum.BlendEquationAlpha, out int lastBlendEquationAlpha);

            bool lastEnableBlend = _gl.IsEnabled(GLEnum.Blend);
            bool lastEnableCullFace = _gl.IsEnabled(GLEnum.CullFace);
            bool lastEnableDepthTest = _gl.IsEnabled(GLEnum.DepthTest);
            bool lastEnableStencilTest = _gl.IsEnabled(GLEnum.StencilTest);
            bool lastEnableScissorTest = _gl.IsEnabled(GLEnum.ScissorTest);

#if !GLES && !LEGACY
            bool lastEnablePrimitiveRestart = _gl.IsEnabled(GLEnum.PrimitiveRestart);
#endif

            SetupRenderState(drawDataPtr, framebufferWidth, framebufferHeight);

            // Will project scissor/clipping rectangles into framebuffer space
            Vector2 clipOff = drawDataPtr.DisplayPos;         // (0,0) unless using multi-viewports
            Vector2 clipScale = drawDataPtr.FramebufferScale; // (1,1) unless using retina display which are often (2,2)

            // Render command lists
            for (int n = 0; n < drawDataPtr.CmdListsCount; n++)
            {
                ImDrawListPtr cmdListPtr = drawDataPtr.CmdLists.Data[n];

                // Upload vertex/index buffers

                _gl.BufferData(GLEnum.ArrayBuffer, (nuint) (cmdListPtr.VtxBuffer.Size * sizeof(ImDrawVert)), (void*) cmdListPtr.VtxBuffer.Data, GLEnum.StreamDraw);
                _gl.CheckGlError($"Data Vert {n}");
                _gl.BufferData(GLEnum.ElementArrayBuffer, (nuint) (cmdListPtr.IdxBuffer.Size * sizeof(ushort)), (void*) cmdListPtr.IdxBuffer.Data, GLEnum.StreamDraw);
                _gl.CheckGlError($"Data Idx {n}");

                for (int cmd_i = 0; cmd_i < cmdListPtr.CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmd cmd = cmdListPtr.CmdBuffer.Data[cmd_i];

                    if (cmd.UserCallback != (void*)0)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        Vector4 clipRect;
                        clipRect.X = (cmd.ClipRect.X - clipOff.X) * clipScale.X;
                        clipRect.Y = (cmd.ClipRect.Y - clipOff.Y) * clipScale.Y;
                        clipRect.Z = (cmd.ClipRect.Z - clipOff.X) * clipScale.X;
                        clipRect.W = (cmd.ClipRect.W - clipOff.Y) * clipScale.Y;

                        if (clipRect.X < framebufferWidth && clipRect.Y < framebufferHeight && clipRect.Z >= 0.0f && clipRect.W >= 0.0f)
                        {
                            // Apply scissor/clipping rectangle
                            _gl.Scissor((int) clipRect.X, (int) (framebufferHeight - clipRect.W), (uint) (clipRect.Z - clipRect.X), (uint) (clipRect.W - clipRect.Y));
                            _gl.CheckGlError("Scissor");

                            // Bind texture, Draw
                            _gl.BindTexture(GLEnum.Texture2D, (uint) cmd.TextureId.Handle);
                            _gl.CheckGlError("Texture");

                            _gl.DrawElementsBaseVertex(GLEnum.Triangles, cmd.ElemCount, GLEnum.UnsignedShort, (void*) (cmd.IdxOffset * sizeof(ushort)), (int) cmd.VtxOffset);
                            _gl.CheckGlError("Draw");
                        }
                    }
                }
            }

            // Destroy the temporary VAO
            _gl.DeleteVertexArray(_vertexArrayObject);
            _vertexArrayObject = 0;

            // Restore modified GL state
            _gl.UseProgram((uint) lastProgram);
            _gl.BindTexture(GLEnum.Texture2D, (uint) lastTexture);

            _gl.BindSampler(0, (uint) lastSampler);

            _gl.ActiveTexture((GLEnum) lastActiveTexture);

            _gl.BindVertexArray((uint) lastVertexArrayObject);

            _gl.BindBuffer(GLEnum.ArrayBuffer, (uint) lastArrayBuffer);
            _gl.BlendEquationSeparate((GLEnum) lastBlendEquationRgb, (GLEnum) lastBlendEquationAlpha);
            _gl.BlendFuncSeparate((GLEnum) lastBlendSrcRgb, (GLEnum) lastBlendDstRgb, (GLEnum) lastBlendSrcAlpha, (GLEnum) lastBlendDstAlpha);

            if (lastEnableBlend)
            {
                _gl.Enable(GLEnum.Blend);
            }
            else
            {
                _gl.Disable(GLEnum.Blend);
            }

            if (lastEnableCullFace)
            {
                _gl.Enable(GLEnum.CullFace);
            }
            else
            {
                _gl.Disable(GLEnum.CullFace);
            }

            if (lastEnableDepthTest)
            {
                _gl.Enable(GLEnum.DepthTest);
            }
            else
            {
                _gl.Disable(GLEnum.DepthTest);
            }

            if (lastEnableStencilTest)
            {
                _gl.Enable(GLEnum.StencilTest);
            }
            else
            {
                _gl.Disable(GLEnum.StencilTest);
            }

            if (lastEnableScissorTest)
            {
                _gl.Enable(GLEnum.ScissorTest);
            }
            else
            {
                _gl.Disable(GLEnum.ScissorTest);
            }

#if !GLES && !LEGACY
            if (lastEnablePrimitiveRestart)
            {
                _gl.Enable(GLEnum.PrimitiveRestart);
            }
            else
            {
                _gl.Disable(GLEnum.PrimitiveRestart);
            }

            _gl.PolygonMode(GLEnum.FrontAndBack, (GLEnum) lastPolygonMode[0]);
#endif

            _gl.Scissor(lastScissorBox[0], lastScissorBox[1], (uint) lastScissorBox[2], (uint) lastScissorBox[3]);
        }

        private void CreateDeviceResources()
        {
            // Backup GL state

            _gl.GetInteger(GLEnum.TextureBinding2D, out int lastTexture);
            _gl.GetInteger(GLEnum.ArrayBufferBinding, out int lastArrayBuffer);
            _gl.GetInteger(GLEnum.VertexArrayBinding, out int lastVertexArray);

            string vertexSource =
#if GLES
                @"#version 300 es
        precision highp float;
            
        layout (location = 0) in vec2 Position;
        layout (location = 1) in vec2 UV;
        layout (location = 2) in vec4 Color;
        uniform mat4 ProjMtx;
        out vec2 Frag_UV;
        out vec4 Frag_Color;
        void main()
        {
            Frag_UV = UV;
            Frag_Color = Color;
            gl_Position = ProjMtx * vec4(Position.xy,0.0,1.0);
        }";
#elif GL
                @"#version 330
        layout (location = 0) in vec2 Position;
        layout (location = 1) in vec2 UV;
        layout (location = 2) in vec4 Color;
        uniform mat4 ProjMtx;
        out vec2 Frag_UV;
        out vec4 Frag_Color;
        void main()
        {
            Frag_UV = UV;
            Frag_Color = Color;
            gl_Position = ProjMtx * vec4(Position.xy,0,1);
        }";
#elif LEGACY
                @"#version 110
        attribute vec2 Position;
        attribute vec2 UV;
        attribute vec4 Color;

        uniform mat4 ProjMtx;

        varying vec2 Frag_UV;
        varying vec4 Frag_Color;

        void main()
        {
            Frag_UV = UV;
            Frag_Color = Color;
            gl_Position = ProjMtx * vec4(Position.xy,0,1);
        }";
#endif

            string fragmentSource =
#if GLES
                @"#version 300 es
        precision highp float;
        
        in vec2 Frag_UV;
        in vec4 Frag_Color;
        uniform sampler2D Texture;
        layout (location = 0) out vec4 Out_Color;
        void main()
        {
            Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
        }";
#elif GL
                @"#version 330
        in vec2 Frag_UV;
        in vec4 Frag_Color;
        uniform sampler2D Texture;
        layout (location = 0) out vec4 Out_Color;
        void main()
        {
            Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
        }";
#elif LEGACY
                @"#version 110
        varying vec2 Frag_UV;
        varying vec4 Frag_Color;

        uniform sampler2D Texture;

        void main()
        {
            gl_FragColor = Frag_Color * texture2D(Texture, Frag_UV.st);
        }";
#endif

            _shader = new Shader(_gl, vertexSource, fragmentSource);

            _attribLocationTex = _shader.GetUniformLocation("Texture");
            _attribLocationProjMtx = _shader.GetUniformLocation("ProjMtx");
            _attribLocationVtxPos = _shader.GetAttribLocation("Position");
            _attribLocationVtxUV = _shader.GetAttribLocation("UV");
            _attribLocationVtxColor = _shader.GetAttribLocation("Color");

            _vboHandle = _gl.GenBuffer();
            _elementsHandle = _gl.GenBuffer();

            RecreateFontDeviceTexture();

            // Restore modified GL state
            _gl.BindTexture(GLEnum.Texture2D, (uint) lastTexture);
            _gl.BindBuffer(GLEnum.ArrayBuffer, (uint) lastArrayBuffer);

            _gl.BindVertexArray((uint) lastVertexArray);

            _gl.CheckGlError("End of ImGui setup");
        }

        /// <summary>
        /// Creates the texture used to render text.
        /// </summary>
        private unsafe void RecreateFontDeviceTexture()
        {
            // Build texture atlas
            var io = ImGui.GetIO();
            byte* pixels;
            int width, height, bytesPerPixel;
            
            io.Fonts.GetTexDataAsRGBA32(&pixels, &width, &height, &bytesPerPixel);
            
            // Upload texture to graphics system
            _gl.GetInteger(GLEnum.TextureBinding2D, out int lastTexture);

            _fontTexture = new Texture(_gl, width, height, new IntPtr(pixels));
            _fontTexture.Bind();
            _fontTexture.SetMagFilter(TextureMagFilter.Linear);
            _fontTexture.SetMinFilter(TextureMinFilter.Linear);

            // Store our identifier
            io.Fonts.SetTexID((IntPtr) _fontTexture.GlTexture);

            // Restore state
            _gl.BindTexture(GLEnum.Texture2D, (uint) lastTexture);
        }

        /// <summary>
        /// Frees all graphics resources used by the renderer.
        /// </summary>
        public void Dispose()
        {
            EditorWindow.Instance.Resize -= WindowResized;
            _keyboard.KeyChar -= OnKeyChar;

            _gl.DeleteBuffer(_vboHandle);
            _gl.DeleteBuffer(_elementsHandle);
            _gl.DeleteVertexArray(_vertexArrayObject);

            _fontTexture.Dispose();
            _shader.Dispose();

            ImGui.DestroyContext(Context);
        }
    }
    public readonly struct ImGuiFontConfig
    {
        public ImGuiFontConfig(string fontPath, int fontSize, Func<ImGuiIOPtr, IntPtr> getGlyphRange = null)
        {
            if (fontSize <= 0) throw new ArgumentOutOfRangeException(nameof(fontSize));
            FontPath = fontPath ?? throw new ArgumentNullException(nameof(fontPath));
            FontSize = fontSize;
            GetGlyphRange = getGlyphRange;
        }

        public string FontPath { get; }
        public int FontSize { get; }
        public Func<ImGuiIOPtr, IntPtr> GetGlyphRange { get; }
    }
    struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public UniformType Type;
    }

    class Shader
    {
        public uint Program { get; private set; }
        private readonly Dictionary<string, int> _uniformToLocation = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _attribLocation = new Dictionary<string, int>();
        private bool _initialized = false;
        private GL _gl;
        private (ShaderType Type, string Path)[] _files;

        public Shader(GL gl, string vertexShader, string fragmentShader)
        {
            _gl = gl;
            _files = new[]{
                (ShaderType.VertexShader, vertexShader),
                (ShaderType.FragmentShader, fragmentShader),
            };
            Program = CreateProgram(_files);
        }
        public void UseShader()
        {
            _gl.UseProgram(Program);
        }

        public void Dispose()
        {
            if (_initialized)
            {
                _gl.DeleteProgram(Program);
                _initialized = false;
            }
        }

        public UniformFieldInfo[] GetUniforms()
        {
            _gl.GetProgram(Program, GLEnum.ActiveUniforms, out var uniformCount);

            UniformFieldInfo[] uniforms = new UniformFieldInfo[uniformCount];

            for (int i = 0; i < uniformCount; i++)
            {
                string name = _gl.GetActiveUniform(Program, (uint) i, out int size, out UniformType type);

                UniformFieldInfo fieldInfo;
                fieldInfo.Location = GetUniformLocation(name);
                fieldInfo.Name = name;
                fieldInfo.Size = size;
                fieldInfo.Type = type;

                uniforms[i] = fieldInfo;
            }

            return uniforms;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(string uniform)
        {
            if (_uniformToLocation.TryGetValue(uniform, out int location) == false)
            {
                location = _gl.GetUniformLocation(Program, uniform);
                _uniformToLocation.Add(uniform, location);

                if (location == -1)
                {
                    Debug.Print($"The uniform '{uniform}' does not exist in the shader!");
                }
            }

            return location;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetAttribLocation(string attrib)
        {
            if (_attribLocation.TryGetValue(attrib, out int location) == false)
            {
                location = _gl.GetAttribLocation(Program, attrib);
                _attribLocation.Add(attrib, location);

                if (location == -1)
                {
                    Debug.Print($"The attrib '{attrib}' does not exist in the shader!");
                }
            }

            return location;
        }

        private uint CreateProgram(params (ShaderType Type, string source)[] shaderPaths)
        {
            var program = _gl.CreateProgram();

            Span<uint> shaders = stackalloc uint[shaderPaths.Length];
            for (int i = 0; i < shaderPaths.Length; i++)
            {
                shaders[i] = CompileShader(shaderPaths[i].Type, shaderPaths[i].source);
            }

            foreach (var shader in shaders)
                _gl.AttachShader(program, shader);

            _gl.LinkProgram(program);

            _gl.GetProgram(program, GLEnum.LinkStatus, out var success);
            if (success == 0)
            {
                string info = _gl.GetProgramInfoLog(program);
                Debug.WriteLine($"GL.LinkProgram had info log:\n{info}");
            }

            foreach (var shader in shaders)
            {
                _gl.DetachShader(program, shader);
                _gl.DeleteShader(shader);
            }

            _initialized = true;

            return program;
        }

        private uint CompileShader(ShaderType type, string source)
        {
            var shader = _gl.CreateShader(type);
            _gl.ShaderSource(shader, source);
            _gl.CompileShader(shader);

            _gl.GetShader(shader, ShaderParameterName.CompileStatus, out var success);
            if (success == 0)
            {
                string info = _gl.GetShaderInfoLog(shader);
                Debug.WriteLine($"GL.CompileShader for shader [{type}] had info log:\n{info}");
            }

            return shader;
        }
    }

    public enum TextureCoordinate
    {
        S = TextureParameterName.TextureWrapS,
        T = TextureParameterName.TextureWrapT,
        R = TextureParameterName.TextureWrapR
    }

    class Texture : IDisposable
    {
        public const SizedInternalFormat Srgb8Alpha8 = (SizedInternalFormat) GLEnum.Srgb8Alpha8;
        public const SizedInternalFormat Rgb32F = (SizedInternalFormat) GLEnum.Rgb32f;

        public const GLEnum MaxTextureMaxAnisotropy = (GLEnum) 0x84FF;

        public static float? MaxAniso;
        private readonly GL _gl;
        public readonly string Name;
        public readonly uint GlTexture;
        public readonly uint Width, Height;
        public readonly uint MipmapLevels;
        public readonly SizedInternalFormat InternalFormat;

        public unsafe Texture(GL gl, int width, int height, IntPtr data, bool generateMipmaps = false, bool srgb = false)
        {
            _gl = gl;
            MaxAniso ??= gl.GetFloat(MaxTextureMaxAnisotropy);
            Width = (uint) width;
            Height = (uint) height;
            InternalFormat = srgb ? Srgb8Alpha8 : SizedInternalFormat.Rgba8;
            MipmapLevels = (uint) (generateMipmaps == false ? 1 : (int) Math.Floor(Math.Log(Math.Max(Width, Height), 2)));

            GlTexture = _gl.GenTexture();
            Bind();

            PixelFormat pxFormat = PixelFormat.Bgra;


            _gl.TexStorage2D(GLEnum.Texture2D, MipmapLevels, InternalFormat, Width, Height);
            _gl.TexSubImage2D(GLEnum.Texture2D, 0, 0, 0, Width, Height, pxFormat, PixelType.UnsignedByte, (void*) data);

            if (generateMipmaps)

                _gl.GenerateTextureMipmap(GlTexture);
            SetWrap(TextureCoordinate.S, TextureWrapMode.Repeat);
            SetWrap(TextureCoordinate.T, TextureWrapMode.Repeat);

            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLevel, MipmapLevels - 1);
        }

        public void Bind()
        {
            _gl.BindTexture(GLEnum.Texture2D, GlTexture);
        }

        public void SetMinFilter(TextureMinFilter filter)
        {
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, (int) filter);
        }

        public void SetMagFilter(TextureMagFilter filter)
        {
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMagFilter, (int) filter);
        }

        public void SetAnisotropy(float level)
        {
            const TextureParameterName textureMaxAnisotropy = (TextureParameterName) 0x84FE;
            _gl.TexParameter(GLEnum.Texture2D, (GLEnum) textureMaxAnisotropy, Util.Clamp(level, 1, MaxAniso.GetValueOrDefault()));
        }

        public void SetLod(int @base, int min, int max)
        {
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureLodBias, @base);
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinLod, min);
            _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLod, max);
        }

        public void SetWrap(TextureCoordinate coord, TextureWrapMode mode)
        {
            _gl.TexParameterI(GLEnum.Texture2D, (TextureParameterName) coord, (int) mode);
        }

        public void Dispose()
        {
            _gl.DeleteTexture(GlTexture);
        }
    }

    static class Util
    {
        [Pure]
        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Conditional("DEBUG")]
        public static void CheckGlError(this GL gl, string title)
        {
            var error = gl.GetError();
            if (error != GLEnum.NoError)
            {
                Debug.Print($"{title}: {error}");
            }
        }
    }
