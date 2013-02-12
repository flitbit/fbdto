using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlitBit.Core;
using FlitBit.Core.Collections;
using System.Runtime.InteropServices;

namespace FlitBit.Dto.Tests.Model
{
	[Serializable]
	public sealed class IntAndStringDTO : DataTransferObject<IIntAndString>, IIntAndString
	{
		// Fields
		private IntAndStringDTOData _data = IntAndStringDTOData.Create();
		private static readonly int CHashCodeSeed = typeof(IntAndStringDTO).AssemblyQualifiedName.GetHashCode();

		public IntAndStringDTO() : base(true)
		{																				
		}

		public override int GetHashCode()
		{
			int prime = 0xf3e9b;
			int res = CHashCodeSeed * prime;
			res ^= base.GetHashCode() * prime;
			res ^= _data.GetHashCode() * prime;
			return res;
		}

		protected override void PerformCopySource(IIntAndString other)
		{
			base.CheckWriteOnce();

		}

		protected override void PerformCopyState(DataTransferObject<IIntAndString> other)
		{
			base.CheckWriteOnce();
			IntAndStringDTO o = other as IntAndStringDTO;
			if (o != null)
			{
				this._data = o._data.Copy();
			}
		}
				
		public int IntProp
		{
			get
			{
				return this._data.IIntAndString_IntProp;
			}
			set
			{
				base.CheckWriteOnce();
				_data.WriteIIntAndString_IntProp(value);
			}
		}

		public string StringProp
		{
			get
			{
				return this._data.IIntAndString_StringProp;
			}
			set
			{
				base.CheckWriteOnce();
				_data.WriteIIntAndString_StringProp(value);
			}
		}

		protected override bool PerformEqual(IIntAndString other)
		{
			var o = other as IntAndStringDTO;
			return other != null && this._data.Equals(o._data);
		}
	}

	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct IntAndStringDTOData : IEquatable<IntAndStringDTOData>
	{
		static readonly int CHashCodeSeed;
		public BitVector _DirtyFlags;
		public int IIntAndString_IntProp;
		public string IIntAndString_StringProp;

		public override bool Equals(object obj)
		{
			return (typeof(IntAndStringDTOData).IsInstanceOfType(obj) 
				&& this.Equals((IntAndStringDTOData)obj));
		}

		public override int GetHashCode()
		{
			int num = 0xf3e9b;
			int num2 = CHashCodeSeed * num;
			num2 ^= num * this._DirtyFlags.GetHashCode();
			num2 ^= num * this.IIntAndString_IntProp;
			if (this.IIntAndString_StringProp != null)
			{
				num2 ^= num * this.IIntAndString_StringProp.GetHashCode();
			}
			return num2;
		}

		static IntAndStringDTOData()
		{
			CHashCodeSeed = typeof(IntAndStringDTOData).AssemblyQualifiedName.GetHashCode();
		}

		internal bool WriteIIntAndString_IntProp(int value)
		{
			if (this.IIntAndString_IntProp != value)
			{
				this.IIntAndString_IntProp = value;
				this._DirtyFlags[0] = true;
				return true;
			}
			return false;
		}

		internal bool WriteIIntAndString_StringProp(string value)
		{
			if (!(this.IIntAndString_StringProp == value))
			{
				this.IIntAndString_StringProp = value;
				this._DirtyFlags[1] = true;
				return true;
			}
			return false;
		}

		public bool Equals(IntAndStringDTOData other)
		{
			return (((this._DirtyFlags == other._DirtyFlags) 
				&& (this.IIntAndString_IntProp == other.IIntAndString_IntProp)) 
				&& (this.IIntAndString_StringProp == other.IIntAndString_StringProp));
		}

		public static bool operator ==(IntAndStringDTOData rhs, IntAndStringDTOData data1)
		{
			return rhs.Equals(data1);
		}

		public static bool operator !=(IntAndStringDTOData rhs, IntAndStringDTOData data1)
		{
			return !rhs.Equals(data1);
		}

		internal static IntAndStringDTOData Create()
		{
			IntAndStringDTOData res = new IntAndStringDTOData();
			res._DirtyFlags = new BitVector(2);
			return res;
		}

		internal IntAndStringDTOData Copy()
		{
			IntAndStringDTOData res = this;
			res._DirtyFlags = this._DirtyFlags.Copy();
			return res;
		}
	}
}
