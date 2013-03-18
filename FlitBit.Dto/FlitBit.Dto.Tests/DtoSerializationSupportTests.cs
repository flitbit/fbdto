using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FlitBit.Copy;
using FlitBit.Core;
using FlitBit.Dto.Tests.Model;
using FlitBit.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlitBit.Dto.Tests
{
	[TestClass]
	public class DtoSerializationSupportTests
	{
		[TestMethod]
		public void CreateAndSerializeRandomValuesOfAllNativeTypes()
		{
			// Ensure the dynamic assembly gets dumped to disk...
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;

			var test = new
			{
				ItemsToSerialize = 10000
			};
			var fieldMap = new[]
			{
				"Boolean", "Byte", "Char", "DateTime", "DateTimeOffset", "Decimal", "Double", "Float", "Guid", "Int16", "Int32",
				"Int32Arr", "Int64", "MyByteEnum", "MyInt16Enum", "MyInt32Enum",
				"NullableBoolean", "NullableByte", "NullableChar", "NullableDateTime", "NullableDateTimeOffset", "NullableDecimal",
				"NullableDouble", "NullableFloat", "NullableGuid", "NullableInt16", "NullableInt32", "NullableInt64",
				"NullableMyByteEnum", "NullableMyInt16Enum", "NullableMyInt32Enum", "NullableSByte",
				"NullableUInt16", "NullableUInt32", "NullableUInt64", "SByte", "String", "UInt16", "UInt32", "UInt64",
				"StringCollection", "StringList"
			};

			var factory = FactoryProvider.Factory;

			var gen = new DataGenerator();
			var rand = new Random(Environment.TickCount);
			var mutation = new Mutation();
			var my = factory.CreateInstance<ISamplingOfTypes>();
			Assert.IsNotNull(my);

			var time = new Stopwatch();
			time.Start();
			for (var i = 0; i < test.ItemsToSerialize; i++)
			{
				var previous = my;
				my = Copier<ISamplingOfTypes>.CopyConstruct(my);
				Assert.IsNotNull(my);
				Assert.AreEqual(my.GetDirtyFlags(), previous.GetDirtyFlags());
				Assert.AreEqual(my, previous);
				Assert.AreNotSame(my, previous);

				my.ResetDirtyFlags();

				mutation.Value = i % 42; // cycle through the mutations

				my.PropertyChanged += (sender, e) =>
				{
					var index = Array.IndexOf<string>(fieldMap, e.PropertyName);
					Assert.AreEqual(index, mutation.Value);
					Assert.IsTrue(my.IsDirty(e.PropertyName));
					mutation.Increment();
				};

				bool leaveNull;
				switch (mutation.Value)
				{
					case 0:
						Assert.AreEqual(my.Boolean, previous.Boolean);
						Assert.IsFalse(my.IsDirty("Boolean"));
						my.Boolean = !my.Boolean;
						Assert.IsTrue(my.IsDirty("Boolean"));
						break;
					case 1:
						Assert.AreEqual(my.Byte, previous.Byte);
						Assert.IsFalse(my.IsDirty("Byte"));
						Byte @Byte;
						do
						{
							@Byte = gen.GetByte();
						} while (@Byte == my.Byte);
						my.Byte = @Byte;
						Assert.IsTrue(my.IsDirty("Byte"));
						break;
					case 2:
						Assert.AreEqual(my.Char, previous.Char);
						Assert.IsFalse(my.IsDirty("Char"));
						char @char;
						do
						{
							@char = gen.GetChar();
						} while (@char == my.Char);
						my.Char = @char;
						Assert.IsTrue(my.IsDirty("Char"));
						break;
					case 3:
						Assert.AreEqual(my.DateTime, previous.DateTime);
						Assert.IsFalse(my.IsDirty("DateTime"));
						DateTime dateTime;
						do
						{
							dateTime = gen.GetDateTime();
						} while (dateTime == my.DateTime);
						my.DateTime = dateTime;
						Assert.IsTrue(my.IsDirty("DateTime"));
						break;
					case 4:
						Assert.AreEqual(my.DateTimeOffset, previous.DateTimeOffset);
						Assert.IsFalse(my.IsDirty("DateTimeOffset"));
						DateTimeOffset dateTimeOffset;
						do
						{
							dateTimeOffset = gen.GetDateTimeOffset();
						} while (dateTimeOffset == my.DateTimeOffset);
						my.DateTimeOffset = dateTimeOffset;
						Assert.IsTrue(my.IsDirty("DateTimeOffset"));
						break;
					case 5:
						Assert.AreEqual(my.Decimal, previous.Decimal);
						Assert.IsFalse(my.IsDirty("Decimal"));
						Decimal @Decimal;
						do
						{
							@Decimal = gen.GetDecimal();
						} while (@Decimal == my.Decimal);
						my.Decimal = @Decimal;
						Assert.IsTrue(my.IsDirty("Decimal"));
						break;
					case 6:
						Assert.AreEqual(my.Double, previous.Double);
						Assert.IsFalse(my.IsDirty("Double"));
						Double @Double;
						do
						{
							@Double = gen.GetDouble();
						} while (Math.Abs(@Double - my.Double) < double.Epsilon);
						my.Double = @Double;
						Assert.IsTrue(my.IsDirty("Double"));
						break;
					case 7:
						Assert.AreEqual(my.Float, previous.Float);
						Assert.IsFalse(my.IsDirty("Float"));
						Single @Float;
						do
						{
							@Float = gen.GetSingle();
						} while (Math.Abs(@Float - my.Float) < float.Epsilon);
						my.Float = @Float;
						Assert.IsTrue(my.IsDirty("Float"));
						break;
					case 8:
						Assert.AreEqual(my.Guid, previous.Guid);
						Assert.IsFalse(my.IsDirty("Guid"));
						Guid guid;
						do
						{
							guid = gen.GetGuid();
						} while (guid == my.Guid);
						my.Guid = guid;
						Assert.IsTrue(my.IsDirty("Guid"));
						break;
					case 9:
						Assert.AreEqual(my.Int16, previous.Int16);
						Assert.IsFalse(my.IsDirty("Int16"));
						Int16 int16;
						do
						{
							int16 = gen.GetInt16();
						} while (int16 == my.Int16);
						my.Int16 = int16;
						Assert.IsTrue(my.IsDirty("Int16"));
						break;
					case 10:
						Assert.AreEqual(my.Int32, previous.Int32);
						Assert.IsFalse(my.IsDirty("Int32"));
						Int32 int32;
						do
						{
							int32 = gen.GetInt32();
						} while (int32 == my.Int32);
						my.Int32 = int32;
						Assert.IsTrue(my.IsDirty("Int32"));
						break;
					case 11:
						Assert.IsTrue(my.Int32Arr.EqualsOrItemsEqual(previous.Int32Arr));
						Assert.IsFalse(my.IsDirty("Int32Arr"));
						if (my.Int32Arr == null || (gen.GetInt32() % 10 == 0))
						{
							my.Int32Arr = gen.GetArray<int>(rand.Next(8, 40));
						}
						var arrIndex = rand.Next(my.Int32Arr.Length - 1);
						Int32 arrItem;
						do
						{
							arrItem = gen.GetInt32();
						} while (arrItem == my.Int32Arr[arrIndex]);
						var arrCopy = new int[my.Int32Arr.Length];
						Array.Copy(my.Int32Arr, arrCopy, arrCopy.Length);
						arrCopy[arrIndex] = arrItem;
						my.Int32Arr = arrCopy;
						Assert.IsTrue(my.IsDirty("Int32Arr"));
						break;
					case 12:
						Assert.AreEqual(my.Int64, previous.Int64);
						Assert.IsFalse(my.IsDirty("Int64"));
						Int64 int64;
						do
						{
							int64 = gen.GetInt64();
						} while (int64 == my.Int64);
						my.Int64 = int64;
						Assert.IsTrue(my.IsDirty("Int64"));
						break;
					case 13:
						Assert.AreEqual(my.MyByteEnum, previous.MyByteEnum);
						Assert.IsFalse(my.IsDirty("MyByteEnum"));
						MyByteEnum myByteEnum;
						do
						{
							myByteEnum = gen.GetEnum<MyByteEnum>();
						} while (myByteEnum == my.MyByteEnum);
						my.MyByteEnum = myByteEnum;
						Assert.IsTrue(my.IsDirty("MyByteEnum"));
						break;
					case 14:
						Assert.AreEqual(my.MyInt16Enum, previous.MyInt16Enum);
						Assert.IsFalse(my.IsDirty("MyInt16Enum"));
						MyInt16Enum myInt16Enum;
						do
						{
							myInt16Enum = gen.GetEnum<MyInt16Enum>();
						} while (myInt16Enum == my.MyInt16Enum);
						my.MyInt16Enum = myInt16Enum;
						Assert.IsTrue(my.IsDirty("MyInt16Enum"));
						break;
					case 15:
						Assert.AreEqual(my.MyInt32Enum, previous.MyInt32Enum);
						Assert.IsFalse(my.IsDirty("MyInt32Enum"));
						MyInt32Enum myInt32Enum;
						do
						{
							myInt32Enum = gen.GetEnum<MyInt32Enum>();
						} while (myInt32Enum == my.MyInt32Enum);
						my.MyInt32Enum = myInt32Enum;
						Assert.IsTrue(my.IsDirty("MyInt32Enum"));
						break;
					case 16:
						Assert.IsTrue(Nullable.Equals(my.NullableBoolean, previous.NullableBoolean));
						Assert.IsFalse(my.IsDirty("NullableBoolean"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableBoolean.HasValue)
						{
							my.NullableBoolean = new bool?();
							break;
						}
						var nullbool = new bool?(my.NullableBoolean.HasValue && !my.NullableBoolean.Value);
						my.NullableBoolean = nullbool;
						Assert.IsTrue(my.IsDirty("NullableBoolean"));
						break;
					case 17:
						Assert.IsTrue(Nullable.Equals(my.NullableByte, previous.NullableByte));
						Assert.IsFalse(my.IsDirty("NullableByte"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableByte.HasValue)
						{
							my.NullableByte = new Byte?();
							break;
						}
						Byte? nullByte;
						do
						{
							nullByte = gen.GetByte();
						} while (Nullable.Equals(my.NullableByte, nullByte));
						my.NullableByte = nullByte;
						Assert.IsTrue(my.IsDirty("NullableByte"));
						break;
					case 18:
						Assert.IsTrue(Nullable.Equals(my.NullableChar, previous.NullableChar));
						Assert.IsFalse(my.IsDirty("NullableChar"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableChar.HasValue)
						{
							my.NullableChar = new Char?();
							break;
						}
						Char? nullChar;
						do
						{
							nullChar = gen.GetChar();
						} while (Nullable.Equals(my.NullableChar, nullChar));
						my.NullableChar = nullChar;
						Assert.IsTrue(my.IsDirty("NullableChar"));
						break;
					case 19:
						Assert.IsTrue(Nullable.Equals(my.NullableDateTime, previous.NullableDateTime));
						Assert.IsFalse(my.IsDirty("NullableDateTime"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableDateTime.HasValue)
						{
							my.NullableDateTime = new DateTime?();
							break;
						}
						DateTime? nullDateTime;
						do
						{
							nullDateTime = gen.GetDateTime();
						} while (Nullable.Equals(my.NullableDateTime, nullDateTime));
						my.NullableDateTime = nullDateTime;
						Assert.IsTrue(my.IsDirty("NullableDateTime"));
						break;
					case 20:
						Assert.IsTrue(Nullable.Equals(my.NullableDateTimeOffset, previous.NullableDateTimeOffset));
						Assert.IsFalse(my.IsDirty("NullableDateTimeOffset"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableDateTimeOffset.HasValue)
						{
							my.NullableDateTimeOffset = new DateTimeOffset?();
							break;
						}
						DateTimeOffset? nullDateTimeOffset;
						do
						{
							nullDateTimeOffset = gen.GetDateTimeOffset();
						} while (Nullable.Equals(my.NullableDateTimeOffset, nullDateTimeOffset));
						my.NullableDateTimeOffset = nullDateTimeOffset;
						Assert.IsTrue(my.IsDirty("NullableDateTimeOffset"));
						break;
					case 21:
						Assert.IsTrue(Nullable.Equals(my.NullableDecimal, previous.NullableDecimal));
						Assert.IsFalse(my.IsDirty("NullableDecimal"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableDecimal.HasValue)
						{
							my.NullableDecimal = new Decimal?();
							break;
						}
						Decimal? nullDecimal;
						do
						{
							nullDecimal = gen.GetDecimal();
						} while (Nullable.Equals(my.NullableDecimal, nullDecimal));
						my.NullableDecimal = nullDecimal;
						Assert.IsTrue(my.IsDirty("NullableDecimal"));
						break;
					case 22:
						Assert.IsTrue(Nullable.Equals(my.NullableDouble, previous.NullableDouble));
						Assert.IsFalse(my.IsDirty("NullableDouble"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableDouble.HasValue)
						{
							my.NullableDouble = new Double?();
							break;
						}
						Double? nullDouble;
						do
						{
							nullDouble = gen.GetDouble();
						} while (Nullable.Equals(my.NullableDouble, nullDouble));
						my.NullableDouble = nullDouble;
						Assert.IsTrue(my.IsDirty("NullableDouble"));
						break;
					case 23:
						Assert.IsTrue(Nullable.Equals(my.NullableFloat, previous.NullableFloat));
						Assert.IsFalse(my.IsDirty("NullableFloat"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableFloat.HasValue)
						{
							my.NullableFloat = new Single?();
							break;
						}
						Single? nullFloat;
						do
						{
							nullFloat = gen.GetSingle();
						} while (Nullable.Equals(my.NullableFloat, nullFloat));
						my.NullableFloat = nullFloat;
						Assert.IsTrue(my.IsDirty("NullableFloat"));
						break;
					case 24:
						Assert.IsTrue(Nullable.Equals(my.NullableGuid, previous.NullableGuid));
						Assert.IsFalse(my.IsDirty("NullableGuid"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableGuid.HasValue)
						{
							my.NullableGuid = new Guid?();
							break;
						}
						Guid? nullGuid;
						do
						{
							nullGuid = gen.GetGuid();
						} while (Nullable.Equals(my.NullableGuid, nullGuid));
						my.NullableGuid = nullGuid;
						Assert.IsTrue(my.IsDirty("NullableGuid"));
						break;
					case 25:
						Assert.IsTrue(Nullable.Equals(my.NullableInt16, previous.NullableInt16));
						Assert.IsFalse(my.IsDirty("NullableInt16"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableInt16.HasValue)
						{
							my.NullableInt16 = new Int16?();
							break;
						}
						Int16? nullInt16;
						do
						{
							nullInt16 = gen.GetInt16();
						} while (Nullable.Equals(my.NullableInt16, nullInt16));
						my.NullableInt16 = nullInt16;
						Assert.IsTrue(my.IsDirty("NullableInt16"));
						break;
					case 26:
						Assert.IsTrue(Nullable.Equals(my.NullableInt32, previous.NullableInt32));
						Assert.IsFalse(my.IsDirty("NullableInt32"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableInt32.HasValue)
						{
							my.NullableInt32 = new Int32?();
							break;
						}
						Int32? nullInt32;
						do
						{
							nullInt32 = gen.GetInt32();
						} while (Nullable.Equals(my.NullableInt32, nullInt32));
						my.NullableInt32 = nullInt32;
						Assert.IsTrue(my.IsDirty("NullableInt32"));
						break;
					case 27:
						Assert.IsTrue(Nullable.Equals(my.NullableInt64, previous.NullableInt64));
						Assert.IsFalse(my.IsDirty("NullableInt64"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableInt64.HasValue)
						{
							my.NullableInt64 = new Int64?();
							break;
						}
						var nullInt64 = new Int64?(gen.GetInt64());
						if (!Nullable.Equals(nullInt64, my.NullableInt64))
						{
							my.NullableInt64 = nullInt64;
						}
						Assert.IsTrue(my.IsDirty("NullableInt64"));
						break;
					case 28:
						Assert.IsTrue(Nullable.Equals(my.NullableMyByteEnum, previous.NullableMyByteEnum));
						Assert.IsFalse(my.IsDirty("NullableMyByteEnum"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableMyByteEnum.HasValue)
						{
							my.NullableMyByteEnum = new MyByteEnum?();
							break;
						}
						MyByteEnum? nullMyByteEnum;
						do
						{
							nullMyByteEnum = gen.GetEnum<MyByteEnum>();
						} while (Nullable.Equals(my.NullableMyByteEnum, nullMyByteEnum));
						my.NullableMyByteEnum = nullMyByteEnum;
						Assert.IsTrue(my.IsDirty("NullableMyByteEnum"));
						break;
					case 29:
						Assert.IsTrue(Nullable.Equals(my.NullableMyInt16Enum, previous.NullableMyInt16Enum));
						Assert.IsFalse(my.IsDirty("NullableMyInt16Enum"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableMyInt16Enum.HasValue)
						{
							my.NullableMyInt16Enum = new MyInt16Enum?();
							break;
						}
						MyInt16Enum? nullMyInt16Enum;
						do
						{
							nullMyInt16Enum = gen.GetEnum<MyInt16Enum>();
						} while (Nullable.Equals(my.NullableMyInt16Enum, nullMyInt16Enum));
						my.NullableMyInt16Enum = nullMyInt16Enum;
						Assert.IsTrue(my.IsDirty("NullableMyInt16Enum"));
						break;
					case 30:
						Assert.IsTrue(Nullable.Equals(my.NullableMyInt32Enum, previous.NullableMyInt32Enum));
						Assert.IsFalse(my.IsDirty("NullableMyInt32Enum"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableMyInt32Enum.HasValue)
						{
							my.NullableMyInt32Enum = new MyInt32Enum?();
							break;
						}
						MyInt32Enum? nullMyInt32Enum;
						do
						{
							nullMyInt32Enum = gen.GetEnum<MyInt32Enum>();
						} while (Nullable.Equals(my.NullableMyInt32Enum, nullMyInt32Enum));
						my.NullableMyInt32Enum = nullMyInt32Enum;
						Assert.IsTrue(my.IsDirty("NullableMyInt32Enum"));
						break;
					case 31:
						Assert.IsTrue(Nullable.Equals(my.NullableSByte, previous.NullableSByte));
						Assert.IsFalse(my.IsDirty("NullableSByte"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableSByte.HasValue)
						{
							my.NullableSByte = new SByte?();
							break;
						}
						SByte? nullSByte;
						do
						{
							nullSByte = gen.GetSByte();
						} while (Nullable.Equals(my.NullableSByte, nullSByte));
						my.NullableSByte = nullSByte;
						Assert.IsTrue(my.IsDirty("NullableSByte"));
						break;
					case 32:
						Assert.IsTrue(Nullable.Equals(my.NullableUInt16, previous.NullableUInt16));
						Assert.IsFalse(my.IsDirty("NullableUInt16"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableUInt16.HasValue)
						{
							my.NullableUInt16 = new UInt16?();
							break;
						}
						UInt16? nullUInt16;
						do
						{
							nullUInt16 = gen.GetUInt16();
						} while (Nullable.Equals(my.NullableUInt16, nullUInt16));
						my.NullableUInt16 = nullUInt16;
						Assert.IsTrue(my.IsDirty("NullableUInt16"));
						break;
					case 33:
						Assert.IsTrue(Nullable.Equals(my.NullableUInt32, previous.NullableUInt32));
						Assert.IsFalse(my.IsDirty("NullableUInt32"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableUInt32.HasValue)
						{
							my.NullableUInt32 = new UInt32?();
							break;
						}
						UInt32? nullUInt32;
						do
						{
							nullUInt32 = gen.GetUInt32();
						} while (Nullable.Equals(my.NullableUInt32, nullUInt32));
						my.NullableUInt32 = nullUInt32;
						Assert.IsTrue(my.IsDirty("NullableUInt32"));
						break;
					case 34:
						Assert.IsTrue(Nullable.Equals(my.NullableUInt64, previous.NullableUInt64));
						Assert.IsFalse(my.IsDirty("NullableUInt64"));
						leaveNull = gen.GetBoolean();
						if (leaveNull && my.NullableUInt64.HasValue)
						{
							my.NullableUInt64 = new UInt64?();
							break;
						}
						UInt64? nullUInt64;
						do
						{
							nullUInt64 = gen.GetUInt64();
						} while (Nullable.Equals(my.NullableUInt64, nullUInt64));
						my.NullableUInt64 = nullUInt64;
						Assert.IsTrue(my.IsDirty("NullableUInt64"));
						break;
					case 35:
						Assert.AreEqual(my.SByte, previous.SByte);
						Assert.IsFalse(my.IsDirty("SByte"));
						SByte sByte;
						do
						{
							sByte = gen.GetSByte();
						} while (sByte == my.SByte);
						my.SByte = sByte;
						Assert.IsTrue(my.IsDirty("SByte"));
						break;
					case 36:
						Assert.AreEqual(my.String, previous.String);
						Assert.IsFalse(my.IsDirty("String"));
						String @string;
						do
						{
							@string = gen.GetString(rand.Next(80, 400));
						} while (@string == my.String);
						my.String = @string;
						Assert.IsTrue(my.IsDirty("String"));
						break;
					case 37:
						Assert.AreEqual(my.UInt16, previous.UInt16);
						Assert.IsFalse(my.IsDirty("UInt16"));
						UInt16 uInt16;
						do
						{
							uInt16 = gen.GetUInt16();
						} while (uInt16 == my.UInt16);
						my.UInt16 = uInt16;
						Assert.IsTrue(my.IsDirty("UInt16"));
						break;
					case 38:
						Assert.AreEqual(my.UInt32, previous.UInt32);
						Assert.IsFalse(my.IsDirty("UInt32"));
						UInt32 uInt32;
						do
						{
							uInt32 = gen.GetUInt32();
						} while (uInt32 == my.UInt32);
						my.UInt32 = uInt32;
						Assert.IsTrue(my.IsDirty("UInt32"));
						break;
					case 39:
						Assert.AreEqual(my.UInt64, previous.UInt64);
						Assert.IsFalse(my.IsDirty("UInt64"));
						UInt64 uInt64;
						do
						{
							uInt64 = gen.GetUInt64();
						} while (uInt64 == my.UInt64);
						my.UInt64 = uInt64;
						Assert.IsTrue(my.IsDirty("UInt64"));
						break;
					case 40:
						Assert.IsTrue(my.StringCollection.SequenceEqual(previous.StringCollection));
						Assert.IsFalse(my.IsDirty("StringCollection"));
						if (gen.GetInt32() % 5 == 0)
						{
							my.StringCollection.Clear();
						}
						else
						{
							my.StringCollection.Add(gen.GetWords(2));
						}
						Assert.IsTrue(my.IsDirty("StringCollection"));
						break;
					case 41:
						Assert.IsTrue(my.StringList.SequenceEqual(previous.StringList));
						Assert.IsFalse(my.IsDirty("StringList"));
						if (gen.GetInt32() % 5 == 0)
						{
							my.StringList.Clear();
						}
						else
						{
							my.StringList.Add(gen.GetString(5));
						}
						Assert.IsTrue(my.IsDirty("StringList"));
						break;
				}

				var mydirty = my.GetDirtyFlags();
				var pdirty = previous.GetDirtyFlags();
				Assert.AreNotEqual(mydirty, pdirty);

				ISamplingOfTypes deserialized;
				using (var serialized = SerializeToStream(my))
				{
					deserialized = DeserializeFromStream<ISamplingOfTypes>(serialized);
				}
				Assert.AreNotSame(my, deserialized);
				Assert.AreEqual(my, deserialized);
			}
			Console.WriteLine(String.Concat("Mutated ", test.ItemsToSerialize, " instances ", mutation.Total, " times in ",
																			time.Elapsed));
		}

		public static T DeserializeFromStream<T>(MemoryStream stream)
		{
			IFormatter formatter = new BinaryFormatter();
			stream.Seek(0, SeekOrigin.Begin);
			var o = (T) formatter.Deserialize(stream);
			return o;
		}

		public static MemoryStream SerializeToStream<T>(T o)
		{
			var stream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, o);
			return stream;
		}

		class Mutation
		{
			public int Total { get; private set; }
			public int Value { get; set; }

			public void Increment()
			{
				Total++;
			}
		}
	}
}