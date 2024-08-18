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
	public partial struct ImGuiNextWindowData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiNextWindowDataFlags Flags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiCond PosCond;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiCond SizeCond;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiCond CollapsedCond;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiCond DockCond;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 PosVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 PosPivotVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 SizeVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 ContentSizeVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 ScrollVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiChildFlags ChildFlags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte PosUndock;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte CollapsedVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect SizeConstraintRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* SizeCallback;
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* SizeCallbackUserData;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float BgAlphaVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ViewportId;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint DockId;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiWindowClass WindowClass;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 MenuBarOffsetMinVal;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiWindowRefreshFlags RefreshFlagsVal;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiNextWindowData(ImGuiNextWindowDataFlags flags = default, ImGuiCond posCond = default, ImGuiCond sizeCond = default, ImGuiCond collapsedCond = default, ImGuiCond dockCond = default, Vector2 posVal = default, Vector2 posPivotVal = default, Vector2 sizeVal = default, Vector2 contentSizeVal = default, Vector2 scrollVal = default, ImGuiChildFlags childFlags = default, bool posUndock = default, bool collapsedVal = default, ImRect sizeConstraintRect = default, ImGuiSizeCallback sizeCallback = default, void* sizeCallbackUserData = default, float bgAlphaVal = default, uint viewportId = default, uint dockId = default, ImGuiWindowClass windowClass = default, Vector2 menuBarOffsetMinVal = default, ImGuiWindowRefreshFlags refreshFlagsVal = default)
		{
			Flags = flags;
			PosCond = posCond;
			SizeCond = sizeCond;
			CollapsedCond = collapsedCond;
			DockCond = dockCond;
			PosVal = posVal;
			PosPivotVal = posPivotVal;
			SizeVal = sizeVal;
			ContentSizeVal = contentSizeVal;
			ScrollVal = scrollVal;
			ChildFlags = childFlags;
			PosUndock = posUndock ? (byte)1 : (byte)0;
			CollapsedVal = collapsedVal ? (byte)1 : (byte)0;
			SizeConstraintRect = sizeConstraintRect;
			SizeCallback = (void*)Marshal.GetFunctionPointerForDelegate(sizeCallback);
			SizeCallbackUserData = sizeCallbackUserData;
			BgAlphaVal = bgAlphaVal;
			ViewportId = viewportId;
			DockId = dockId;
			WindowClass = windowClass;
			MenuBarOffsetMinVal = menuBarOffsetMinVal;
			RefreshFlagsVal = refreshFlagsVal;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiNextWindowDataPtr : IEquatable<ImGuiNextWindowDataPtr>
	{
		public ImGuiNextWindowDataPtr(ImGuiNextWindowData* handle) { Handle = handle; }

		public ImGuiNextWindowData* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiNextWindowDataPtr Null => new ImGuiNextWindowDataPtr(null);

		public ImGuiNextWindowData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiNextWindowDataPtr(ImGuiNextWindowData* handle) => new ImGuiNextWindowDataPtr(handle);

		public static implicit operator ImGuiNextWindowData*(ImGuiNextWindowDataPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiNextWindowDataPtr left, ImGuiNextWindowDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiNextWindowDataPtr left, ImGuiNextWindowDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiNextWindowDataPtr left, ImGuiNextWindowData* right) => left.Handle == right;

		public static bool operator !=(ImGuiNextWindowDataPtr left, ImGuiNextWindowData* right) => left.Handle != right;

		public bool Equals(ImGuiNextWindowDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiNextWindowDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiNextWindowDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiNextWindowDataFlags Flags => ref Unsafe.AsRef<ImGuiNextWindowDataFlags>(&Handle->Flags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiCond PosCond => ref Unsafe.AsRef<ImGuiCond>(&Handle->PosCond);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiCond SizeCond => ref Unsafe.AsRef<ImGuiCond>(&Handle->SizeCond);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiCond CollapsedCond => ref Unsafe.AsRef<ImGuiCond>(&Handle->CollapsedCond);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiCond DockCond => ref Unsafe.AsRef<ImGuiCond>(&Handle->DockCond);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 PosVal => ref Unsafe.AsRef<Vector2>(&Handle->PosVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 PosPivotVal => ref Unsafe.AsRef<Vector2>(&Handle->PosPivotVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 SizeVal => ref Unsafe.AsRef<Vector2>(&Handle->SizeVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 ContentSizeVal => ref Unsafe.AsRef<Vector2>(&Handle->ContentSizeVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 ScrollVal => ref Unsafe.AsRef<Vector2>(&Handle->ScrollVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiChildFlags ChildFlags => ref Unsafe.AsRef<ImGuiChildFlags>(&Handle->ChildFlags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool PosUndock => ref Unsafe.AsRef<bool>(&Handle->PosUndock);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool CollapsedVal => ref Unsafe.AsRef<bool>(&Handle->CollapsedVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect SizeConstraintRect => ref Unsafe.AsRef<ImRect>(&Handle->SizeConstraintRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* SizeCallback { get => Handle->SizeCallback; set => Handle->SizeCallback = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* SizeCallbackUserData { get => Handle->SizeCallbackUserData; set => Handle->SizeCallbackUserData = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float BgAlphaVal => ref Unsafe.AsRef<float>(&Handle->BgAlphaVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ViewportId => ref Unsafe.AsRef<uint>(&Handle->ViewportId);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint DockId => ref Unsafe.AsRef<uint>(&Handle->DockId);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiWindowClass WindowClass => ref Unsafe.AsRef<ImGuiWindowClass>(&Handle->WindowClass);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 MenuBarOffsetMinVal => ref Unsafe.AsRef<Vector2>(&Handle->MenuBarOffsetMinVal);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiWindowRefreshFlags RefreshFlagsVal => ref Unsafe.AsRef<ImGuiWindowRefreshFlags>(&Handle->RefreshFlagsVal);
	}

}
