using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using FlitBit.Core;
using FlitBit.Core.Collections;

namespace FlitBit.Dto.Tests.Model
{
	[Serializable]
	public sealed class SamplingOfTypesDTO : ISamplingOfTypes, IEquatable<SamplingOfTypesDTO>, IEquatable<ISamplingOfTypes>,
																					INotifyPropertyChanged
	{
		static readonly int CHashCodeSeed = typeof(SamplingOfTypesDTO).AssemblyQualifiedName.GetHashCode();

		static readonly string[] __fieldMap = new[]
		{
			"Boolean", "Byte", "Char", "DateTime", "DateTimeOffeset", "Decimal", "Double", "Float", "Guid", "Int16", "Int32",
			"Int32Arr", "Int64", "MyByteEnum", "MyInt16Enum", "MyInt32Enum", "NullableBoolean", "NullableByte", "NullableChar",
			"NullableDateTime", "NullableDateTimeOffeset", "NullableDecimal", "NullableDouble", "NullableFloat", "NullableGuid",
			"NullableInt16", "NullableInt32", "NullableInt64", "NullableMyByteEnum", "NullableMyInt16Enum", "NullableMyInt32Enum"
			, "NullableSByte", "NullableUInt16", "NullableUInt32", "NullableUInt64", "SByte", "String", "UInt16", "UInt32",
			"UInt64"
		};

		// Fields
		BitVector DirtyFlags;
		bool ISamplingOfTypes_Boolean_field;
		byte ISamplingOfTypes_Byte_field;
		char ISamplingOfTypes_Char_field;
		DateTimeOffset ISamplingOfTypes_DateTimeOffeset_field;
		DateTime ISamplingOfTypes_DateTime_field;
		decimal ISamplingOfTypes_Decimal_field;
		double ISamplingOfTypes_Double_field;
		float ISamplingOfTypes_Float_field;
		Guid ISamplingOfTypes_Guid_field;
		short ISamplingOfTypes_Int16_field;
		int[] ISamplingOfTypes_Int32Arr_field;
		int ISamplingOfTypes_Int32_field;
		long ISamplingOfTypes_Int64_field;
		MyByteEnum ISamplingOfTypes_MyByteEnum_field;
		MyInt16Enum ISamplingOfTypes_MyInt16Enum_field;
		MyInt32Enum ISamplingOfTypes_MyInt32Enum_field;
		bool? ISamplingOfTypes_NullableBoolean_field;
		byte? ISamplingOfTypes_NullableByte_field;
		char? ISamplingOfTypes_NullableChar_field;
		DateTimeOffset? ISamplingOfTypes_NullableDateTimeOffeset_field;
		DateTime? ISamplingOfTypes_NullableDateTime_field;
		decimal? ISamplingOfTypes_NullableDecimal_field;
		double? ISamplingOfTypes_NullableDouble_field;
		float? ISamplingOfTypes_NullableFloat_field;
		Guid? ISamplingOfTypes_NullableGuid_field;
		short? ISamplingOfTypes_NullableInt16_field;
		int? ISamplingOfTypes_NullableInt32_field;
		long? ISamplingOfTypes_NullableInt64_field;
		MyByteEnum? ISamplingOfTypes_NullableMyByteEnum_field;
		MyInt16Enum? ISamplingOfTypes_NullableMyInt16Enum_field;
		MyInt32Enum? ISamplingOfTypes_NullableMyInt32Enum_field;
		sbyte? ISamplingOfTypes_NullableSByte_field;
		ushort? ISamplingOfTypes_NullableUInt16_field;
		uint? ISamplingOfTypes_NullableUInt32_field;
		ulong? ISamplingOfTypes_NullableUInt64_field;
		sbyte ISamplingOfTypes_SByte_field;
		ObservableCollection<string> ISamplingOfTypes_StringCollection_field;
		ObservableCollection<string> ISamplingOfTypes_StringList_field;
		string ISamplingOfTypes_String_field;
		ushort ISamplingOfTypes_UInt16_field;
		uint ISamplingOfTypes_UInt32_field;
		ulong ISamplingOfTypes_UInt64_field;

		public SamplingOfTypesDTO()
		{
			DirtyFlags = new BitVector(40);
			StringCollection = null;
		}

		public override bool Equals(object obj)
		{
			return ((obj is SamplingOfTypesDTO) && this.Equals((SamplingOfTypesDTO) obj));
		}

		public override int GetHashCode()
		{
			var num = 0xf3e9b;
			var seed = CHashCodeSeed * num;
			seed ^= num * this.DirtyFlags.GetHashCode();
			seed ^= num * Convert.ToInt32(this.ISamplingOfTypes_Boolean_field);
			seed ^= num * this.ISamplingOfTypes_Byte_field;
			seed ^= num * this.ISamplingOfTypes_Char_field;
			seed ^= num * this.ISamplingOfTypes_DateTime_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_DateTimeOffeset_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_Decimal_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_Double_field.GetHashCode();
			seed ^= (int) (num * this.ISamplingOfTypes_Float_field);
			seed ^= num * this.ISamplingOfTypes_Guid_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_Int16_field;
			seed ^= num * this.ISamplingOfTypes_Int32_field;
			seed ^= num * this.ISamplingOfTypes_Int32Arr_field.CalculateCombinedHashcode<int>(seed);
			seed ^= num * this.ISamplingOfTypes_Int64_field.GetHashCode();
			seed ^= num * ((int) this.ISamplingOfTypes_MyByteEnum_field);
			seed ^= num * ((int) this.ISamplingOfTypes_MyInt16Enum_field);
			seed ^= num * ((int) this.ISamplingOfTypes_MyInt32Enum_field);
			seed ^= num * this.ISamplingOfTypes_NullableBoolean_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableByte_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableChar_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableDateTime_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableDateTimeOffeset_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableDecimal_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableDouble_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableFloat_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableGuid_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableInt16_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableInt32_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableInt64_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableMyByteEnum_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableMyInt16Enum_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableMyInt32Enum_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableSByte_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableUInt16_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableUInt32_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_NullableUInt64_field.GetHashCode();
			seed ^= num * this.ISamplingOfTypes_SByte_field;
			if (this.ISamplingOfTypes_String_field != null)
			{
				seed ^= num * this.ISamplingOfTypes_String_field.GetHashCode();
			}
			seed ^= num * this.ISamplingOfTypes_UInt16_field;
			seed ^= (int) (num * this.ISamplingOfTypes_UInt32_field);
			return (seed ^ (num * this.ISamplingOfTypes_UInt64_field.GetHashCode()));
		}

		void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		void ISamplingOfTypes_StringCollection_field_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			DirtyFlags[41] = true;
			OnPropertyChanged("StringCollection");
		}

		#region IEquatable<ISamplingOfTypes> Members

		public bool Equals(ISamplingOfTypes other)
		{
			return ((other is SamplingOfTypesDTO) && this.Equals((SamplingOfTypesDTO) other));
		}

		#endregion

		#region IEquatable<SamplingOfTypesDTO> Members

		public bool Equals(SamplingOfTypesDTO other)
		{
			return this.DirtyFlags == other.DirtyFlags
				&& this.ISamplingOfTypes_Boolean_field == other.ISamplingOfTypes_Boolean_field
				&& this.ISamplingOfTypes_Byte_field == other.ISamplingOfTypes_Byte_field
				&& this.ISamplingOfTypes_Char_field == other.ISamplingOfTypes_Char_field
				&& this.ISamplingOfTypes_DateTime_field == other.ISamplingOfTypes_DateTime_field
				&& this.ISamplingOfTypes_DateTimeOffeset_field == other.ISamplingOfTypes_DateTimeOffeset_field
				&& this.ISamplingOfTypes_Decimal_field == other.ISamplingOfTypes_Decimal_field
				&& this.ISamplingOfTypes_Double_field == other.ISamplingOfTypes_Double_field
				&& this.ISamplingOfTypes_Float_field == other.ISamplingOfTypes_Float_field
				&& this.ISamplingOfTypes_Guid_field == other.ISamplingOfTypes_Guid_field
				&& this.ISamplingOfTypes_Int16_field == other.ISamplingOfTypes_Int16_field
				&& this.ISamplingOfTypes_Int32_field == other.ISamplingOfTypes_Int32_field
				&& this.ISamplingOfTypes_Int32Arr_field.EqualsOrItemsEqual<int>(other.ISamplingOfTypes_Int32Arr_field)
				&& this.ISamplingOfTypes_Int64_field == other.ISamplingOfTypes_Int64_field
				&& this.ISamplingOfTypes_MyByteEnum_field == other.ISamplingOfTypes_MyByteEnum_field
				&& this.ISamplingOfTypes_MyInt16Enum_field == other.ISamplingOfTypes_MyInt16Enum_field
				&& this.ISamplingOfTypes_MyInt32Enum_field == other.ISamplingOfTypes_MyInt32Enum_field
				&& Nullable.Equals<bool>(this.ISamplingOfTypes_NullableBoolean_field, other.ISamplingOfTypes_NullableBoolean_field)
				&& Nullable.Equals<byte>(this.ISamplingOfTypes_NullableByte_field, other.ISamplingOfTypes_NullableByte_field)
				&& Nullable.Equals<char>(this.ISamplingOfTypes_NullableChar_field, other.ISamplingOfTypes_NullableChar_field)
				&&
				Nullable.Equals<DateTime>(this.ISamplingOfTypes_NullableDateTime_field,
																	other.ISamplingOfTypes_NullableDateTime_field)
				&&
				Nullable.Equals<DateTimeOffset>(this.ISamplingOfTypes_NullableDateTimeOffeset_field,
																				other.ISamplingOfTypes_NullableDateTimeOffeset_field)
				&&
				Nullable.Equals<decimal>(this.ISamplingOfTypes_NullableDecimal_field, other.ISamplingOfTypes_NullableDecimal_field)
				&& Nullable.Equals<double>(this.ISamplingOfTypes_NullableDouble_field, other.ISamplingOfTypes_NullableDouble_field)
				&& Nullable.Equals<float>(this.ISamplingOfTypes_NullableFloat_field, other.ISamplingOfTypes_NullableFloat_field)
				&& Nullable.Equals<Guid>(this.ISamplingOfTypes_NullableGuid_field, other.ISamplingOfTypes_NullableGuid_field)
				&& Nullable.Equals<short>(this.ISamplingOfTypes_NullableInt16_field, other.ISamplingOfTypes_NullableInt16_field)
				&& Nullable.Equals<int>(this.ISamplingOfTypes_NullableInt32_field, other.ISamplingOfTypes_NullableInt32_field)
				&& Nullable.Equals<long>(this.ISamplingOfTypes_NullableInt64_field, other.ISamplingOfTypes_NullableInt64_field)
				&&
				Nullable.Equals<MyByteEnum>(this.ISamplingOfTypes_NullableMyByteEnum_field,
																		other.ISamplingOfTypes_NullableMyByteEnum_field)
				&&
				Nullable.Equals<MyInt16Enum>(this.ISamplingOfTypes_NullableMyInt16Enum_field,
																		other.ISamplingOfTypes_NullableMyInt16Enum_field)
				&&
				Nullable.Equals<MyInt32Enum>(this.ISamplingOfTypes_NullableMyInt32Enum_field,
																		other.ISamplingOfTypes_NullableMyInt32Enum_field)
				&& Nullable.Equals<sbyte>(this.ISamplingOfTypes_NullableSByte_field, other.ISamplingOfTypes_NullableSByte_field)
				&& Nullable.Equals<ushort>(this.ISamplingOfTypes_NullableUInt16_field, other.ISamplingOfTypes_NullableUInt16_field)
				&& Nullable.Equals<uint>(this.ISamplingOfTypes_NullableUInt32_field, other.ISamplingOfTypes_NullableUInt32_field)
				&& Nullable.Equals<ulong>(this.ISamplingOfTypes_NullableUInt64_field, other.ISamplingOfTypes_NullableUInt64_field)
				&& ISamplingOfTypes_StringCollection_field.SequenceEqual(other.ISamplingOfTypes_StringCollection_field)
				&& ISamplingOfTypes_StringList_field.SequenceEqual(other.ISamplingOfTypes_StringList_field)
				&& this.ISamplingOfTypes_SByte_field == other.ISamplingOfTypes_SByte_field
				&& this.ISamplingOfTypes_String_field == other.ISamplingOfTypes_String_field
				&& this.ISamplingOfTypes_UInt16_field == other.ISamplingOfTypes_UInt16_field
				&& this.ISamplingOfTypes_UInt32_field == other.ISamplingOfTypes_UInt32_field
				&& this.ISamplingOfTypes_UInt64_field == other.ISamplingOfTypes_UInt64_field;
		}

		#endregion

		// Properties

		#region ISamplingOfTypes Members

		public bool Boolean
		{
			get { return this.ISamplingOfTypes_Boolean_field; }
			set
			{
				if (this.ISamplingOfTypes_Boolean_field != value)
				{
					this.ISamplingOfTypes_Boolean_field = value;
					DirtyFlags[0] = true;
				}
			}
		}

		public byte Byte { get { return this.ISamplingOfTypes_Byte_field; } set { this.ISamplingOfTypes_Byte_field = value; } }

		public char Char { get { return this.ISamplingOfTypes_Char_field; } set { this.ISamplingOfTypes_Char_field = value; } }

		public DateTime DateTime { get { return this.ISamplingOfTypes_DateTime_field; } set { this.ISamplingOfTypes_DateTime_field = value; } }

		public DateTimeOffset DateTimeOffset { get { return this.ISamplingOfTypes_DateTimeOffeset_field; } set { this.ISamplingOfTypes_DateTimeOffeset_field = value; } }

		public decimal Decimal { get { return this.ISamplingOfTypes_Decimal_field; } set { this.ISamplingOfTypes_Decimal_field = value; } }

		public double Double { get { return this.ISamplingOfTypes_Double_field; } set { this.ISamplingOfTypes_Double_field = value; } }

		public float Float { get { return this.ISamplingOfTypes_Float_field; } set { this.ISamplingOfTypes_Float_field = value; } }

		public Guid Guid { get { return this.ISamplingOfTypes_Guid_field; } set { this.ISamplingOfTypes_Guid_field = value; } }

		public short Int16 { get { return this.ISamplingOfTypes_Int16_field; } set { this.ISamplingOfTypes_Int16_field = value; } }

		public int Int32 { get { return this.ISamplingOfTypes_Int32_field; } set { this.ISamplingOfTypes_Int32_field = value; } }

		public int[] Int32Arr { get { return this.ISamplingOfTypes_Int32Arr_field; } set { this.ISamplingOfTypes_Int32Arr_field = value; } }

		public long Int64 { get { return this.ISamplingOfTypes_Int64_field; } set { this.ISamplingOfTypes_Int64_field = value; } }

		public MyByteEnum MyByteEnum { get { return this.ISamplingOfTypes_MyByteEnum_field; } set { this.ISamplingOfTypes_MyByteEnum_field = value; } }

		public MyInt16Enum MyInt16Enum { get { return this.ISamplingOfTypes_MyInt16Enum_field; } set { this.ISamplingOfTypes_MyInt16Enum_field = value; } }

		public MyInt32Enum MyInt32Enum { get { return this.ISamplingOfTypes_MyInt32Enum_field; } set { this.ISamplingOfTypes_MyInt32Enum_field = value; } }

		public bool? NullableBoolean { get { return this.ISamplingOfTypes_NullableBoolean_field; } set { this.ISamplingOfTypes_NullableBoolean_field = value; } }

		public byte? NullableByte { get { return this.ISamplingOfTypes_NullableByte_field; } set { this.ISamplingOfTypes_NullableByte_field = value; } }

		public char? NullableChar { get { return this.ISamplingOfTypes_NullableChar_field; } set { this.ISamplingOfTypes_NullableChar_field = value; } }

		public DateTime? NullableDateTime { get { return this.ISamplingOfTypes_NullableDateTime_field; } set { this.ISamplingOfTypes_NullableDateTime_field = value; } }

		public DateTimeOffset? NullableDateTimeOffset { get { return this.ISamplingOfTypes_NullableDateTimeOffeset_field; } set { this.ISamplingOfTypes_NullableDateTimeOffeset_field = value; } }

		public decimal? NullableDecimal { get { return this.ISamplingOfTypes_NullableDecimal_field; } set { this.ISamplingOfTypes_NullableDecimal_field = value; } }

		public double? NullableDouble { get { return this.ISamplingOfTypes_NullableDouble_field; } set { this.ISamplingOfTypes_NullableDouble_field = value; } }

		public float? NullableFloat { get { return this.ISamplingOfTypes_NullableFloat_field; } set { this.ISamplingOfTypes_NullableFloat_field = value; } }

		public Guid? NullableGuid { get { return this.ISamplingOfTypes_NullableGuid_field; } set { this.ISamplingOfTypes_NullableGuid_field = value; } }

		public short? NullableInt16 { get { return this.ISamplingOfTypes_NullableInt16_field; } set { this.ISamplingOfTypes_NullableInt16_field = value; } }

		public int? NullableInt32 { get { return this.ISamplingOfTypes_NullableInt32_field; } set { this.ISamplingOfTypes_NullableInt32_field = value; } }

		public long? NullableInt64 { get { return this.ISamplingOfTypes_NullableInt64_field; } set { this.ISamplingOfTypes_NullableInt64_field = value; } }

		public MyByteEnum? NullableMyByteEnum { get { return this.ISamplingOfTypes_NullableMyByteEnum_field; } set { this.ISamplingOfTypes_NullableMyByteEnum_field = value; } }

		public MyInt16Enum? NullableMyInt16Enum { get { return this.ISamplingOfTypes_NullableMyInt16Enum_field; } set { this.ISamplingOfTypes_NullableMyInt16Enum_field = value; } }

		public MyInt32Enum? NullableMyInt32Enum { get { return this.ISamplingOfTypes_NullableMyInt32Enum_field; } set { this.ISamplingOfTypes_NullableMyInt32Enum_field = value; } }

		public sbyte? NullableSByte { get { return this.ISamplingOfTypes_NullableSByte_field; } set { this.ISamplingOfTypes_NullableSByte_field = value; } }

		public ushort? NullableUInt16 { get { return this.ISamplingOfTypes_NullableUInt16_field; } set { this.ISamplingOfTypes_NullableUInt16_field = value; } }

		public uint? NullableUInt32 { get { return this.ISamplingOfTypes_NullableUInt32_field; } set { this.ISamplingOfTypes_NullableUInt32_field = value; } }

		public ulong? NullableUInt64 { get { return this.ISamplingOfTypes_NullableUInt64_field; } set { this.ISamplingOfTypes_NullableUInt64_field = value; } }

		public sbyte SByte { get { return this.ISamplingOfTypes_SByte_field; } set { this.ISamplingOfTypes_SByte_field = value; } }

		public string String { get { return this.ISamplingOfTypes_String_field; } set { this.ISamplingOfTypes_String_field = value; } }

		public ushort UInt16
		{
			get { return this.ISamplingOfTypes_UInt16_field; }
			set
			{
				if (this.ISamplingOfTypes_UInt16_field != value)
				{
					this.ISamplingOfTypes_UInt16_field = value;
					this.DirtyFlags[9] = true;
					OnPropertyChanged("Uint16");
				}
			}
		}

		public uint UInt32 { get { return this.ISamplingOfTypes_UInt32_field; } set { this.ISamplingOfTypes_UInt32_field = value; } }

		public ulong UInt64 { get { return this.ISamplingOfTypes_UInt64_field; } set { this.ISamplingOfTypes_UInt64_field = value; } }

		public BitVector GetDirtyFlags()
		{
			return (BitVector) DirtyFlags.Clone();
		}

		public bool IsDirty(string member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			var index = Array.IndexOf(__fieldMap, member);
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("member", "ISamplingOfTypes does not define property: `" + member + "`.");
			}
			return this.DirtyFlags[index];
		}

		public object Clone()
		{
			var clone = (SamplingOfTypesDTO) this.MemberwiseClone();
			clone.DirtyFlags = this.DirtyFlags.Copy();
			clone.PropertyChanged = null;

			if (this.ISamplingOfTypes_Int32Arr_field != null)
			{
				var len = this.ISamplingOfTypes_Int32Arr_field.Length;
				clone.ISamplingOfTypes_Int32Arr_field = new int[len];
				Array.Copy(ISamplingOfTypes_Int32Arr_field, clone.ISamplingOfTypes_Int32Arr_field, len);
			}
			clone.StringCollection = ISamplingOfTypes_StringCollection_field;
			return clone;
		}

		public void ResetDirtyFlags()
		{
			DirtyFlags = new BitVector(40);
		}

		public ICollection<string> StringCollection
		{
			get { return ISamplingOfTypes_StringCollection_field; }
			private set
			{
				if (value != null)
				{
					ISamplingOfTypes_StringCollection_field = new ObservableCollection<string>(value);
				}
				else
				{
					ISamplingOfTypes_StringCollection_field = new ObservableCollection<string>();
				}
				ISamplingOfTypes_StringCollection_field.CollectionChanged +=
					ISamplingOfTypes_StringCollection_field_CollectionChanged;
			}
		}

		public IList<string> StringList { get { return ISamplingOfTypes_StringList_field; } }

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}