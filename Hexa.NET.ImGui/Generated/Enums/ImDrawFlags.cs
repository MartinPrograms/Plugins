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
	public enum ImDrawFlags : int
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		None = unchecked(0),

		/// <summary>
		/// PathStroke(), AddPolyline(): specify that shape should be closed (Important: this is always == 1 for legacy reason)<br/>
		/// </summary>
		Closed = unchecked(1),

		/// <summary>
		/// AddRect(), AddRectFilled(), PathRect(): enable rounding top-left corner only (when rounding &gt; 0.0f, we default to all corners). Was 0x01.<br/>
		/// </summary>
		RoundCornersTopLeft = unchecked(16),

		/// <summary>
		/// AddRect(), AddRectFilled(), PathRect(): enable rounding top-right corner only (when rounding &gt; 0.0f, we default to all corners). Was 0x02.<br/>
		/// </summary>
		RoundCornersTopRight = unchecked(32),

		/// <summary>
		/// AddRect(), AddRectFilled(), PathRect(): enable rounding bottom-left corner only (when rounding &gt; 0.0f, we default to all corners). Was 0x04.<br/>
		/// </summary>
		RoundCornersBottomLeft = unchecked(64),

		/// <summary>
		/// AddRect(), AddRectFilled(), PathRect(): enable rounding bottom-right corner only (when rounding &gt; 0.0f, we default to all corners). Wax 0x08.<br/>
		/// </summary>
		RoundCornersBottomRight = unchecked(128),

		/// <summary>
		/// AddRect(), AddRectFilled(), PathRect(): disable rounding on all corners (when rounding &gt; 0.0f). This is NOT zero, NOT an implicit flag!<br/>
		/// </summary>
		RoundCornersNone = unchecked(256),

		/// <summary>
		/// To be documented.
		/// </summary>
		RoundCornersTop = unchecked(48),

		/// <summary>
		/// To be documented.
		/// </summary>
		RoundCornersBottom = unchecked(192),

		/// <summary>
		/// To be documented.
		/// </summary>
		RoundCornersLeft = unchecked(80),

		/// <summary>
		/// To be documented.
		/// </summary>
		RoundCornersRight = unchecked(160),

		/// <summary>
		/// To be documented.
		/// </summary>
		RoundCornersAll = unchecked(240),

		/// <summary>
		/// Default to ALL corners if none of the _RoundCornersXX flags are specified.<br/>
		/// </summary>
		RoundCornersDefault = RoundCornersAll,

		/// <summary>
		/// To be documented.
		/// </summary>
		RoundCornersMask = unchecked(496),
	}
}
