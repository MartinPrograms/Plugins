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
	public enum ImFontAtlasFlags : int
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		None = unchecked(0),

		/// <summary>
		/// Don't round the height to next power of two<br/>
		/// </summary>
		NoPowerOfTwoHeight = unchecked(1),

		/// <summary>
		/// Don't build software mouse cursors into the atlas (save a little texture memory)<br/>
		/// </summary>
		NoMouseCursors = unchecked(2),

		/// <summary>
		/// Don't build thick line textures into the atlas (save a little texture memory, allow support for pointnearest filtering). The AntiAliasedLinesUseTex features uses them, otherwise they will be rendered using polygons (more expensive for CPUGPU).<br/>
		/// </summary>
		NoBakedLines = unchecked(4),
	}
}
