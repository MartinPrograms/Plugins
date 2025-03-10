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
	public partial struct ImGuiTableSettings
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ID;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiTableFlags SaveFlags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float RefScale;

		/// <summary>
		/// To be documented.
		/// </summary>
		public sbyte ColumnsCount;

		/// <summary>
		/// To be documented.
		/// </summary>
		public sbyte ColumnsCountMax;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte WantApply;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiTableSettings(uint id = default, ImGuiTableFlags saveFlags = default, float refScale = default, sbyte columnsCount = default, sbyte columnsCountMax = default, bool wantApply = default)
		{
			ID = id;
			SaveFlags = saveFlags;
			RefScale = refScale;
			ColumnsCount = columnsCount;
			ColumnsCountMax = columnsCountMax;
			WantApply = wantApply ? (byte)1 : (byte)0;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiTableSettingsPtr : IEquatable<ImGuiTableSettingsPtr>
	{
		public ImGuiTableSettingsPtr(ImGuiTableSettings* handle) { Handle = handle; }

		public ImGuiTableSettings* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiTableSettingsPtr Null => new ImGuiTableSettingsPtr(null);

		public ImGuiTableSettings this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiTableSettingsPtr(ImGuiTableSettings* handle) => new ImGuiTableSettingsPtr(handle);

		public static implicit operator ImGuiTableSettings*(ImGuiTableSettingsPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiTableSettingsPtr left, ImGuiTableSettingsPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiTableSettingsPtr left, ImGuiTableSettingsPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiTableSettingsPtr left, ImGuiTableSettings* right) => left.Handle == right;

		public static bool operator !=(ImGuiTableSettingsPtr left, ImGuiTableSettings* right) => left.Handle != right;

		public bool Equals(ImGuiTableSettingsPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiTableSettingsPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiTableSettingsPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ID => ref Unsafe.AsRef<uint>(&Handle->ID);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiTableFlags SaveFlags => ref Unsafe.AsRef<ImGuiTableFlags>(&Handle->SaveFlags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float RefScale => ref Unsafe.AsRef<float>(&Handle->RefScale);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref sbyte ColumnsCount => ref Unsafe.AsRef<sbyte>(&Handle->ColumnsCount);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref sbyte ColumnsCountMax => ref Unsafe.AsRef<sbyte>(&Handle->ColumnsCountMax);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool WantApply => ref Unsafe.AsRef<bool>(&Handle->WantApply);
	}

}
