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
	public partial struct ImPlotPlot
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ID;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotFlags Flags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotFlags PreviousFlags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotLocation MouseTextLocation;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotMouseTextFlags MouseTextFlags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotAxis Axes_0;
		public ImPlotAxis Axes_1;
		public ImPlotAxis Axes_2;
		public ImPlotAxis Axes_3;
		public ImPlotAxis Axes_4;
		public ImPlotAxis Axes_5;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiTextBuffer TextBuffer;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotItemGroup Items;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImAxis CurrentX;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImAxis CurrentY;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect FrameRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect CanvasRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect PlotRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect AxesRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect SelectRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 SelectStart;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int TitleOffset;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte JustCreated;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Initialized;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte SetupLocked;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte FitThisFrame;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Hovered;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Held;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Selecting;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Selected;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte ContextLocked;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotPlot(uint id = default, ImPlotFlags flags = default, ImPlotFlags previousFlags = default, ImPlotLocation mouseTextLocation = default, ImPlotMouseTextFlags mouseTextFlags = default, ImPlotAxis* axes = default, ImGuiTextBuffer textBuffer = default, ImPlotItemGroup items = default, ImAxis currentX = default, ImAxis currentY = default, ImRect frameRect = default, ImRect canvasRect = default, ImRect plotRect = default, ImRect axesRect = default, ImRect selectRect = default, Vector2 selectStart = default, int titleOffset = default, bool justCreated = default, bool initialized = default, bool setupLocked = default, bool fitThisFrame = default, bool hovered = default, bool held = default, bool selecting = default, bool selected = default, bool contextLocked = default)
		{
			ID = id;
			Flags = flags;
			PreviousFlags = previousFlags;
			MouseTextLocation = mouseTextLocation;
			MouseTextFlags = mouseTextFlags;
			if (axes != default(ImPlotAxis*))
			{
				Axes_0 = axes[0];
				Axes_1 = axes[1];
				Axes_2 = axes[2];
				Axes_3 = axes[3];
				Axes_4 = axes[4];
				Axes_5 = axes[5];
			}
			TextBuffer = textBuffer;
			Items = items;
			CurrentX = currentX;
			CurrentY = currentY;
			FrameRect = frameRect;
			CanvasRect = canvasRect;
			PlotRect = plotRect;
			AxesRect = axesRect;
			SelectRect = selectRect;
			SelectStart = selectStart;
			TitleOffset = titleOffset;
			JustCreated = justCreated ? (byte)1 : (byte)0;
			Initialized = initialized ? (byte)1 : (byte)0;
			SetupLocked = setupLocked ? (byte)1 : (byte)0;
			FitThisFrame = fitThisFrame ? (byte)1 : (byte)0;
			Hovered = hovered ? (byte)1 : (byte)0;
			Held = held ? (byte)1 : (byte)0;
			Selecting = selecting ? (byte)1 : (byte)0;
			Selected = selected ? (byte)1 : (byte)0;
			ContextLocked = contextLocked ? (byte)1 : (byte)0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotPlot(uint id = default, ImPlotFlags flags = default, ImPlotFlags previousFlags = default, ImPlotLocation mouseTextLocation = default, ImPlotMouseTextFlags mouseTextFlags = default, Span<ImPlotAxis> axes = default, ImGuiTextBuffer textBuffer = default, ImPlotItemGroup items = default, ImAxis currentX = default, ImAxis currentY = default, ImRect frameRect = default, ImRect canvasRect = default, ImRect plotRect = default, ImRect axesRect = default, ImRect selectRect = default, Vector2 selectStart = default, int titleOffset = default, bool justCreated = default, bool initialized = default, bool setupLocked = default, bool fitThisFrame = default, bool hovered = default, bool held = default, bool selecting = default, bool selected = default, bool contextLocked = default)
		{
			ID = id;
			Flags = flags;
			PreviousFlags = previousFlags;
			MouseTextLocation = mouseTextLocation;
			MouseTextFlags = mouseTextFlags;
			if (axes != default(Span<ImPlotAxis>))
			{
				Axes_0 = axes[0];
				Axes_1 = axes[1];
				Axes_2 = axes[2];
				Axes_3 = axes[3];
				Axes_4 = axes[4];
				Axes_5 = axes[5];
			}
			TextBuffer = textBuffer;
			Items = items;
			CurrentX = currentX;
			CurrentY = currentY;
			FrameRect = frameRect;
			CanvasRect = canvasRect;
			PlotRect = plotRect;
			AxesRect = axesRect;
			SelectRect = selectRect;
			SelectStart = selectStart;
			TitleOffset = titleOffset;
			JustCreated = justCreated ? (byte)1 : (byte)0;
			Initialized = initialized ? (byte)1 : (byte)0;
			SetupLocked = setupLocked ? (byte)1 : (byte)0;
			FitThisFrame = fitThisFrame ? (byte)1 : (byte)0;
			Hovered = hovered ? (byte)1 : (byte)0;
			Held = held ? (byte)1 : (byte)0;
			Selecting = selecting ? (byte)1 : (byte)0;
			Selected = selected ? (byte)1 : (byte)0;
			ContextLocked = contextLocked ? (byte)1 : (byte)0;
		}


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe Span<ImPlotAxis> Axes
		
		{
			get
			{
				fixed (ImPlotAxis* p = &this.Axes_0)
				{
					return new Span<ImPlotAxis>(p, 6);
				}
			}
		}
	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImPlotPlotPtr : IEquatable<ImPlotPlotPtr>
	{
		public ImPlotPlotPtr(ImPlotPlot* handle) { Handle = handle; }

		public ImPlotPlot* Handle;

		public bool IsNull => Handle == null;

		public static ImPlotPlotPtr Null => new ImPlotPlotPtr(null);

		public ImPlotPlot this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImPlotPlotPtr(ImPlotPlot* handle) => new ImPlotPlotPtr(handle);

		public static implicit operator ImPlotPlot*(ImPlotPlotPtr handle) => handle.Handle;

		public static bool operator ==(ImPlotPlotPtr left, ImPlotPlotPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImPlotPlotPtr left, ImPlotPlotPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImPlotPlotPtr left, ImPlotPlot* right) => left.Handle == right;

		public static bool operator !=(ImPlotPlotPtr left, ImPlotPlot* right) => left.Handle != right;

		public bool Equals(ImPlotPlotPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImPlotPlotPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImPlotPlotPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ID => ref Unsafe.AsRef<uint>(&Handle->ID);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotFlags Flags => ref Unsafe.AsRef<ImPlotFlags>(&Handle->Flags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotFlags PreviousFlags => ref Unsafe.AsRef<ImPlotFlags>(&Handle->PreviousFlags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotLocation MouseTextLocation => ref Unsafe.AsRef<ImPlotLocation>(&Handle->MouseTextLocation);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotMouseTextFlags MouseTextFlags => ref Unsafe.AsRef<ImPlotMouseTextFlags>(&Handle->MouseTextFlags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe Span<ImPlotAxis> Axes
		
		{
			get
			{
				return new Span<ImPlotAxis>(&Handle->Axes_0, 6);
			}
		}
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiTextBuffer TextBuffer => ref Unsafe.AsRef<ImGuiTextBuffer>(&Handle->TextBuffer);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotItemGroup Items => ref Unsafe.AsRef<ImPlotItemGroup>(&Handle->Items);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImAxis CurrentX => ref Unsafe.AsRef<ImAxis>(&Handle->CurrentX);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImAxis CurrentY => ref Unsafe.AsRef<ImAxis>(&Handle->CurrentY);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect FrameRect => ref Unsafe.AsRef<ImRect>(&Handle->FrameRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect CanvasRect => ref Unsafe.AsRef<ImRect>(&Handle->CanvasRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect PlotRect => ref Unsafe.AsRef<ImRect>(&Handle->PlotRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect AxesRect => ref Unsafe.AsRef<ImRect>(&Handle->AxesRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect SelectRect => ref Unsafe.AsRef<ImRect>(&Handle->SelectRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 SelectStart => ref Unsafe.AsRef<Vector2>(&Handle->SelectStart);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int TitleOffset => ref Unsafe.AsRef<int>(&Handle->TitleOffset);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool JustCreated => ref Unsafe.AsRef<bool>(&Handle->JustCreated);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Initialized => ref Unsafe.AsRef<bool>(&Handle->Initialized);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool SetupLocked => ref Unsafe.AsRef<bool>(&Handle->SetupLocked);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool FitThisFrame => ref Unsafe.AsRef<bool>(&Handle->FitThisFrame);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Hovered => ref Unsafe.AsRef<bool>(&Handle->Hovered);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Held => ref Unsafe.AsRef<bool>(&Handle->Held);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Selecting => ref Unsafe.AsRef<bool>(&Handle->Selecting);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Selected => ref Unsafe.AsRef<bool>(&Handle->Selected);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool ContextLocked => ref Unsafe.AsRef<bool>(&Handle->ContextLocked);
	}

}
