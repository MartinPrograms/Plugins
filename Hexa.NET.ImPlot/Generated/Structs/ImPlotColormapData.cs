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
	public partial struct ImPlotColormapData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<uint> Keys;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> KeyCounts;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> KeyOffsets;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<uint> Tables;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> TableSizes;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> TableOffsets;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiTextBuffer Text;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> TextOffsets;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> Quals;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiStorage Map;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int Count;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormapData(ImVector<uint> keys = default, ImVector<int> keyCounts = default, ImVector<int> keyOffsets = default, ImVector<uint> tables = default, ImVector<int> tableSizes = default, ImVector<int> tableOffsets = default, ImGuiTextBuffer text = default, ImVector<int> textOffsets = default, ImVector<int> quals = default, ImGuiStorage map = default, int count = default)
		{
			Keys = keys;
			KeyCounts = keyCounts;
			KeyOffsets = keyOffsets;
			Tables = tables;
			TableSizes = tableSizes;
			TableOffsets = tableOffsets;
			Text = text;
			TextOffsets = textOffsets;
			Quals = quals;
			Map = map;
			Count = count;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImPlotColormapDataPtr : IEquatable<ImPlotColormapDataPtr>
	{
		public ImPlotColormapDataPtr(ImPlotColormapData* handle) { Handle = handle; }

		public ImPlotColormapData* Handle;

		public bool IsNull => Handle == null;

		public static ImPlotColormapDataPtr Null => new ImPlotColormapDataPtr(null);

		public ImPlotColormapData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImPlotColormapDataPtr(ImPlotColormapData* handle) => new ImPlotColormapDataPtr(handle);

		public static implicit operator ImPlotColormapData*(ImPlotColormapDataPtr handle) => handle.Handle;

		public static bool operator ==(ImPlotColormapDataPtr left, ImPlotColormapDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImPlotColormapDataPtr left, ImPlotColormapDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImPlotColormapDataPtr left, ImPlotColormapData* right) => left.Handle == right;

		public static bool operator !=(ImPlotColormapDataPtr left, ImPlotColormapData* right) => left.Handle != right;

		public bool Equals(ImPlotColormapDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImPlotColormapDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImPlotColormapDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<uint> Keys => ref Unsafe.AsRef<ImVector<uint>>(&Handle->Keys);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> KeyCounts => ref Unsafe.AsRef<ImVector<int>>(&Handle->KeyCounts);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> KeyOffsets => ref Unsafe.AsRef<ImVector<int>>(&Handle->KeyOffsets);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<uint> Tables => ref Unsafe.AsRef<ImVector<uint>>(&Handle->Tables);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> TableSizes => ref Unsafe.AsRef<ImVector<int>>(&Handle->TableSizes);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> TableOffsets => ref Unsafe.AsRef<ImVector<int>>(&Handle->TableOffsets);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiTextBuffer Text => ref Unsafe.AsRef<ImGuiTextBuffer>(&Handle->Text);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> TextOffsets => ref Unsafe.AsRef<ImVector<int>>(&Handle->TextOffsets);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> Quals => ref Unsafe.AsRef<ImVector<int>>(&Handle->Quals);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiStorage Map => ref Unsafe.AsRef<ImGuiStorage>(&Handle->Map);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Count => ref Unsafe.AsRef<int>(&Handle->Count);
	}

}
