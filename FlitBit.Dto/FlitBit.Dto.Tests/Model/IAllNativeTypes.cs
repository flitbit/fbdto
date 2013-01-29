using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlitBit.Dto.Tests.Model
{
	public enum MyEnum
	{
		One,
		Two
	}

	[DTO]
	public interface IAllNativeTypes
	{
		byte Byte { get; set; }
		bool Boolean { get; set; }
		char Char { get; set; }
		double Double { get; set; }
		decimal Decimal { get; set; }
		float Float { get; set; }
		MyEnum MyEnum { get; set; }
		Guid Guid { get; set; }
		string String { get; set; }
		sbyte SByte { get; set; }
		short Int16 { get; set; }
		int Int32 { get; set; }
		long Int64 { get; set; }
		ushort UInt16 { get; set; }
		uint UInt32 { get; set; }
		ulong UInt64 { get; set; }
		int? NullableInt32 { get; set; }
		int[] Int32Arr { get; set; }
	}
}
