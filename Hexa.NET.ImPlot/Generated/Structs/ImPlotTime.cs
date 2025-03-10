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
using Hexa.NET.ImGui;

namespace Hexa.NET.ImPlot
{
	/// <summary>
	/// To be documented.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct ImPlotTime
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public long S;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int Us;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotTime(long s = default, int us = default)
		{
			S = s;
			Us = us;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImPlotTimePtr : IEquatable<ImPlotTimePtr>
	{
		public ImPlotTimePtr(ImPlotTime* handle) { Handle = handle; }

		public ImPlotTime* Handle;

		public bool IsNull => Handle == null;

		public static ImPlotTimePtr Null => new ImPlotTimePtr(null);

		public ImPlotTime this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImPlotTimePtr(ImPlotTime* handle) => new ImPlotTimePtr(handle);

		public static implicit operator ImPlotTime*(ImPlotTimePtr handle) => handle.Handle;

		public static bool operator ==(ImPlotTimePtr left, ImPlotTimePtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImPlotTimePtr left, ImPlotTimePtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImPlotTimePtr left, ImPlotTime* right) => left.Handle == right;

		public static bool operator !=(ImPlotTimePtr left, ImPlotTime* right) => left.Handle != right;

		public bool Equals(ImPlotTimePtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImPlotTimePtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImPlotTimePtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref long S => ref Unsafe.AsRef<long>(&Handle->S);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Us => ref Unsafe.AsRef<int>(&Handle->Us);
	}

}
