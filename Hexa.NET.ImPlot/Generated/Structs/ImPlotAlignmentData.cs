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
	public partial struct ImPlotAlignmentData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Vertical;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float PadA;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float PadB;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float PadAMax;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float PadBMax;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotAlignmentData(bool vertical = default, float padA = default, float padB = default, float padAMax = default, float padBMax = default)
		{
			Vertical = vertical ? (byte)1 : (byte)0;
			PadA = padA;
			PadB = padB;
			PadAMax = padAMax;
			PadBMax = padBMax;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImPlotAlignmentDataPtr : IEquatable<ImPlotAlignmentDataPtr>
	{
		public ImPlotAlignmentDataPtr(ImPlotAlignmentData* handle) { Handle = handle; }

		public ImPlotAlignmentData* Handle;

		public bool IsNull => Handle == null;

		public static ImPlotAlignmentDataPtr Null => new ImPlotAlignmentDataPtr(null);

		public ImPlotAlignmentData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImPlotAlignmentDataPtr(ImPlotAlignmentData* handle) => new ImPlotAlignmentDataPtr(handle);

		public static implicit operator ImPlotAlignmentData*(ImPlotAlignmentDataPtr handle) => handle.Handle;

		public static bool operator ==(ImPlotAlignmentDataPtr left, ImPlotAlignmentDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImPlotAlignmentDataPtr left, ImPlotAlignmentDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImPlotAlignmentDataPtr left, ImPlotAlignmentData* right) => left.Handle == right;

		public static bool operator !=(ImPlotAlignmentDataPtr left, ImPlotAlignmentData* right) => left.Handle != right;

		public bool Equals(ImPlotAlignmentDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImPlotAlignmentDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImPlotAlignmentDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Vertical => ref Unsafe.AsRef<bool>(&Handle->Vertical);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float PadA => ref Unsafe.AsRef<float>(&Handle->PadA);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float PadB => ref Unsafe.AsRef<float>(&Handle->PadB);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float PadAMax => ref Unsafe.AsRef<float>(&Handle->PadAMax);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float PadBMax => ref Unsafe.AsRef<float>(&Handle->PadBMax);
	}

}
