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
	public partial struct ImGuiNextItemData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiNextItemDataFlags Flags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiItemFlags ItemFlags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint FocusScopeId;

		/// <summary>
		/// To be documented.
		/// </summary>
		public long SelectionUserData;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float Width;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int Shortcut;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiInputFlags ShortcutFlags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte OpenVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte OpenCond;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiDataTypeStorage RefVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint StorageId;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiNextItemData(ImGuiNextItemDataFlags flags = default, ImGuiItemFlags itemFlags = default, uint focusScopeId = default, long selectionUserData = default, float width = default, int shortcut = default, ImGuiInputFlags shortcutFlags = default, bool openVal = default, byte openCond = default, ImGuiDataTypeStorage refVal = default, uint storageId = default)
		{
			Flags = flags;
			ItemFlags = itemFlags;
			FocusScopeId = focusScopeId;
			SelectionUserData = selectionUserData;
			Width = width;
			Shortcut = shortcut;
			ShortcutFlags = shortcutFlags;
			OpenVal = openVal ? (byte)1 : (byte)0;
			OpenCond = openCond;
			RefVal = refVal;
			StorageId = storageId;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiNextItemDataPtr : IEquatable<ImGuiNextItemDataPtr>
	{
		public ImGuiNextItemDataPtr(ImGuiNextItemData* handle) { Handle = handle; }

		public ImGuiNextItemData* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiNextItemDataPtr Null => new ImGuiNextItemDataPtr(null);

		public ImGuiNextItemData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiNextItemDataPtr(ImGuiNextItemData* handle) => new ImGuiNextItemDataPtr(handle);

		public static implicit operator ImGuiNextItemData*(ImGuiNextItemDataPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiNextItemDataPtr left, ImGuiNextItemDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiNextItemDataPtr left, ImGuiNextItemDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiNextItemDataPtr left, ImGuiNextItemData* right) => left.Handle == right;

		public static bool operator !=(ImGuiNextItemDataPtr left, ImGuiNextItemData* right) => left.Handle != right;

		public bool Equals(ImGuiNextItemDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiNextItemDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiNextItemDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiNextItemDataFlags Flags => ref Unsafe.AsRef<ImGuiNextItemDataFlags>(&Handle->Flags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiItemFlags ItemFlags => ref Unsafe.AsRef<ImGuiItemFlags>(&Handle->ItemFlags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint FocusScopeId => ref Unsafe.AsRef<uint>(&Handle->FocusScopeId);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref long SelectionUserData => ref Unsafe.AsRef<long>(&Handle->SelectionUserData);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float Width => ref Unsafe.AsRef<float>(&Handle->Width);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Shortcut => ref Unsafe.AsRef<int>(&Handle->Shortcut);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiInputFlags ShortcutFlags => ref Unsafe.AsRef<ImGuiInputFlags>(&Handle->ShortcutFlags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool OpenVal => ref Unsafe.AsRef<bool>(&Handle->OpenVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref byte OpenCond => ref Unsafe.AsRef<byte>(&Handle->OpenCond);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiDataTypeStorage RefVal => ref Unsafe.AsRef<ImGuiDataTypeStorage>(&Handle->RefVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint StorageId => ref Unsafe.AsRef<uint>(&Handle->StorageId);
	}

}
