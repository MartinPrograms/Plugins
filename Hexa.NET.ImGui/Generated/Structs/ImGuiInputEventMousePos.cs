// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HexaGen.Runtime;
using System.Numerics;

namespace Hexa.NET.ImGui
{
	/// <summary>
	/// To be documented.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct ImGuiInputEventMousePos
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public float PosX;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float PosY;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiMouseSource MouseSource;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiInputEventMousePos(float posX = default, float posY = default, ImGuiMouseSource mouseSource = default)
		{
			PosX = posX;
			PosY = posY;
			MouseSource = mouseSource;
		}


	}

}
