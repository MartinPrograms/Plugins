// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using HexaGen.Runtime;
using System.Numerics;

namespace Hexa.NET.ImGui
{
	/// <summary>
	/// To be documented.
	/// </summary>
	public enum ImGuiBackendFlags : int
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		None = unchecked(0),

		/// <summary>
		/// Backend Platform supports gamepad and currently has one connected.<br/>
		/// </summary>
		HasGamepad = unchecked(1),

		/// <summary>
		/// Backend Platform supports honoring GetMouseCursor() value to change the OS cursor shape.<br/>
		/// </summary>
		HasMouseCursors = unchecked(2),

		/// <summary>
		/// Backend Platform supports io.WantSetMousePos requests to reposition the OS mouse position (only used if ImGuiConfigFlags_NavEnableSetMousePos is set).<br/>
		/// </summary>
		HasSetMousePos = unchecked(4),

		/// <summary>
		/// Backend Renderer supports ImDrawCmd::VtxOffset. This enables output of large meshes (64K+ vertices) while still using 16-bit indices.<br/>
		/// </summary>
		RendererHasVtxOffset = unchecked(8),

		/// <summary>
		/// Backend Platform supports multiple viewports.<br/>
		/// </summary>
		PlatformHasViewports = unchecked(1024),

		/// <summary>
		/// Backend Platform supports calling io.AddMouseViewportEvent() with the viewport under the mouse. IF POSSIBLE, ignore viewports with the ImGuiViewportFlags_NoInputs flag (Win32 backend, GLFW 3.30+ backend can do this, SDL backend cannot). If this cannot be done, Dear ImGui needs to use a flawed heuristic to find the viewport under.<br/>
		/// </summary>
		HasMouseHoveredViewport = unchecked(2048),

		/// <summary>
		/// Backend Renderer supports multiple viewports.<br/>
		/// </summary>
		RendererHasViewports = unchecked(4096),
	}
}
