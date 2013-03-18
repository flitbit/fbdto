using System;
using System.Collections.Generic;
using FlitBit.Dto.SPI;

namespace FlitBit.Dto.Tests.Model
{
	[DTO]
	public interface ISamplingOfTypes : IDataTransferObject
	{
		bool Boolean { get; set; }
		byte Byte { get; set; }
		char Char { get; set; }
		DateTime DateTime { get; set; }
		DateTimeOffset DateTimeOffset { get; set; }
		decimal Decimal { get; set; }
		double Double { get; set; }
		float Float { get; set; }
		Guid Guid { get; set; }
		short Int16 { get; set; }
		int Int32 { get; set; }
		int[] Int32Arr { get; set; }
		long Int64 { get; set; }
		MyByteEnum MyByteEnum { get; set; }
		MyInt16Enum MyInt16Enum { get; set; }
		MyInt32Enum MyInt32Enum { get; set; }

		bool? NullableBoolean { get; set; }
		byte? NullableByte { get; set; }
		char? NullableChar { get; set; }
		DateTime? NullableDateTime { get; set; }
		DateTimeOffset? NullableDateTimeOffset { get; set; }
		decimal? NullableDecimal { get; set; }
		double? NullableDouble { get; set; }
		float? NullableFloat { get; set; }
		Guid? NullableGuid { get; set; }
		short? NullableInt16 { get; set; }
		int? NullableInt32 { get; set; }
		long? NullableInt64 { get; set; }
		MyByteEnum? NullableMyByteEnum { get; set; }
		MyInt16Enum? NullableMyInt16Enum { get; set; }
		MyInt32Enum? NullableMyInt32Enum { get; set; }
		sbyte? NullableSByte { get; set; }
		ushort? NullableUInt16 { get; set; }
		uint? NullableUInt32 { get; set; }
		ulong? NullableUInt64 { get; set; }
		sbyte SByte { get; set; }
		string String { get; set; }
		ICollection<string> StringCollection { get; }
		IList<string> StringList { get; }
		ushort UInt16 { get; set; }
		uint UInt32 { get; set; }
		ulong UInt64 { get; set; }
	}
}