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
	public partial struct ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public uint Storage_0;
		public uint Storage_1;
		public uint Storage_2;
		public uint Storage_3;
		public uint Storage_4;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN(uint* storage = default)
		{
			if (storage != default(uint*))
			{
				Storage_0 = storage[0];
				Storage_1 = storage[1];
				Storage_2 = storage[2];
				Storage_3 = storage[3];
				Storage_4 = storage[4];
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImBitArrayImGuiKeyNamedKeyCOUNTLessImGuiKeyNamedKeyBEGIN(Span<uint> storage = default)
		{
			if (storage != default(Span<uint>))
			{
				Storage_0 = storage[0];
				Storage_1 = storage[1];
				Storage_2 = storage[2];
				Storage_3 = storage[3];
				Storage_4 = storage[4];
			}
		}


	}

}
