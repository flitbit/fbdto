﻿using System;
using System.Diagnostics;
using System.Threading;
using FlitBit.Dto;
using FlitBit.Dto.Tests.Model;
using FlitBit.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlitBit.Core.Tests.Dto
{
	[TestClass]
	public class DtoTests
	{
		[TestMethod]
		public void ContainerCanConstructInterfaceMarkedAsDTO()
		{
			var test = new { ItemsToCreate = 1000000 };
			int id = -1;
			using (var create = Create.NewContainer())
			{
				// Subscribe to the container's creation events for the DTO type,
				// we'll use that to set the new item's ID.
				create.Subscribe<IJustAnID>((t, item, name, kind) =>
				{
					if (kind == CreationEventKind.Created)
					{
						item.ID = Interlocked.Increment(ref id);
					}
				});

				var my = create.New<IJustAnID>(LifespanTracking.External);

				Assert.IsNotNull(my);
				Assert.AreEqual(id, my.ID);

				Stopwatch time = new Stopwatch();
				time.Start();
				for (int i = 0; i < test.ItemsToCreate; i++)
				{
					var previous = my;
					my = create.DtoCopy<IJustAnID>(my);

					Assert.IsNotNull(my);
					Assert.AreNotSame(previous, my);
					Assert.AreEqual(id, my.ID);
				}
				Console.WriteLine(String.Concat("Created ", test.ItemsToCreate, " Dto instances in ", time.Elapsed));
			}
		}

		[TestMethod]
		public void CanConstructAndMutateDTOs()
		{
			var test = new { ItemsToMutate = 1000000 };
			int id = -1;
			using (var create = Create.NewContainer())
			{
				// our mutator method... it will set the ID's value...
				Action<IJustAnID> setter = (IJustAnID item) => { item.ID = Interlocked.Increment(ref id); };
				var my = create.Mutation(create.New<IJustAnID>(), setter);
				Assert.IsNotNull(my);
				Assert.AreEqual(id, my.ID);

				Stopwatch time = new Stopwatch();
				time.Start();
				for (int i = 0; i < test.ItemsToMutate; i++)
				{
					var previous = my;
					my = create.Mutation(my, setter);

					Assert.IsNotNull(my);
					Assert.AreNotSame(previous, my);
					Assert.AreEqual(id, my.ID);
				}
				Console.WriteLine(String.Concat("Mutated ", test.ItemsToMutate, " Dto instances in ", time.Elapsed));
			}
		}
		
		[TestMethod]
		public void DtoSupportsAllNativeTypeProperties()
		{
			var test = new { ItemsToCreate = 1000000 };
			using (var create = Create.NewContainer())
			{
				var gen = Create.New<DataGenerator>();
				var rand = new Random(Environment.TickCount);
				var mutation = 0;

				var my = create.New<IAllNativeTypes>();
				Assert.IsNotNull(my);

				// Mutate one property per mutation...
				var mutator = new Action<IAllNativeTypes>((IAllNativeTypes item) =>
				{
					Assert.AreEqual(my, item, "mutated copy should always be equal to source upon mutator call");
					switch (mutation)
					{
						case 0:
							while (true)
							{
								var @byte = gen.GetByte();
								if (@byte != item.Byte)
								{
									item.Byte = @byte;
									break;
								}
							}
							break;
						case 1:
							item.Boolean = !item.Boolean;
							break;
						case 2:
							while (true)
							{
								var @char = gen.GetChar();
								if (@char != item.Char)
								{
									item.Char = @char;
									break;
								}
							}
							break;
						case 3:
							while (true)
							{
								var @double = gen.GetDouble();
								if (!Double.IsNaN(@double) && @double != item.Double)
								{
									item.Double = @double;
									break;
								}
							} break;
						case 4:
							while (true)
							{
								var @decimal = gen.GetDecimal();
								if (@decimal != item.Decimal)
								{
									item.Decimal = @decimal;
									break;
								}
							} break;
						case 5:
							while (true)
							{
								var @float = gen.GetSingle();
								if (!Single.IsNaN(@float) && @float != item.Float)
								{
									item.Float = @float;
									break;
								}
							} break;
						case 6:
							while (true)
							{
								var @enum = gen.GetEnum<MyEnum>();
								if (@enum != item.MyEnum)
								{
									item.MyEnum = @enum;
									break;
								}

							} break;
						case 7:
							while (true)
							{
								var @guid = gen.GetGuid();
								if (@guid != item.Guid)
								{
									item.Guid = @guid;
									break;
								}
							} break;
						case 8:
							while (true)
							{
								var @string = gen.GetString(200);
								if (@string != item.String)
								{
									item.String = @string;
									break;
								}
							} break;
						case 9:
							while (true)
							{

								var @sbyte = gen.GetSByte();
								if (@sbyte != item.SByte)
								{
									item.SByte = @sbyte;
									break;
								}
							} break;
						case 10:
							while (true)
							{
								var @int16 = gen.GetInt16();
								if (@int16 != item.Int16)
								{
									item.Int16 = @int16;
									break;
								}
							} break;
						case 11:
							while (true)
							{
								var @int32 = gen.GetInt32();
								if (@int32 != item.Int32)
								{
									item.Int32 = @int32;
									break;
								}
							} break;
						case 12:
							while (true)
							{
								var @int64 = gen.GetInt64();
								if (@int64 != item.Int64)
								{
									item.Int64 = @int64;
									break;
								}
							} break;
						case 13:
							while (true)
							{
								var @uint16 = gen.GetUInt16();
								if (@uint16 != item.UInt16)
								{
									item.UInt16 = @uint16;
									break;
								}
							} break;
						case 14:
							while (true)
							{
								var @uint32 = gen.GetUInt32();
								if (@uint32 != item.UInt32)
								{
									item.UInt32 = @uint32;
									break;
								}
							} break;
						case 15:
							while (true)
							{
								var @uint64 = gen.GetUInt64();
								if (@uint64 != item.UInt64)
								{
									item.UInt64 = @uint64;
									break;
								}
							} break;
						case 16:
							while (true)
							{
								var leaveNull = gen.GetBoolean();
								if (leaveNull && item.NullableInt32.HasValue)
								{
									item.NullableInt32 = new Nullable<int>();
									break;
								}
								else
								{
									var @nullint32 = new Nullable<int>(gen.GetInt32());
									if (!Nullable.Equals(@nullint32, item.Int32))
									{
										item.NullableInt32 = @nullint32;
										break;
									}
								}
							} break;
						case 17:
							while (true)
							{
								var modElm = gen.GetBoolean();
								if (modElm)
								{
									var currArr = item.Int32Arr;
									var elm = rand.Next(currArr.Length);
									var newElm = rand.Next();
									if (currArr[elm] != newElm)
									{
										currArr[elm] = newElm;
										break;
									}
								}
								else
								{
									item.Int32Arr = gen.GetArray<int>(rand.Next(100));
									break;
								}
							} break;
					}
				});

				Stopwatch time = new Stopwatch();
				time.Start();
				for (int i = 0; i < test.ItemsToCreate; i++)
				{
					var previous = my;
					mutation = i % 16; // cycle through the mutations

					my = create.Mutation(my, mutator);

					Assert.IsNotNull(my);
					Assert.AreNotSame(previous, my);
					Assert.AreNotEqual(previous, my);
					// hash collision is indeed possible, encountered about every 500k items during testing.
					//Assert.AreNotEqual(previous.GetHashCode(), my.GetHashCode());

					switch (mutation)
					{
						case 0:
							Assert.AreNotEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 1:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreNotEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 2:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreNotEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 3:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreNotEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 4:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreNotEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 5:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreNotEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 6:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreNotEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 7:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreNotEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 8:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreNotEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 9:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreNotEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 10:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreNotEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 11:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreNotEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 12:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreNotEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 13:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreNotEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 14:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreNotEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 15:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreNotEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 16:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreNotEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsTrue(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
						case 17:
							Assert.AreEqual(previous.Byte, my.Byte);
							Assert.AreEqual(previous.Boolean, my.Boolean);
							Assert.AreEqual(previous.Char, my.Char);
							Assert.AreEqual(previous.Double, my.Double);
							Assert.AreEqual(previous.Decimal, my.Decimal);
							Assert.AreEqual(previous.Float, my.Float);
							Assert.AreEqual(previous.MyEnum, my.MyEnum);
							Assert.AreEqual(previous.Guid, my.Guid);
							Assert.AreEqual(previous.String, my.String);
							Assert.AreEqual(previous.SByte, my.SByte);
							Assert.AreEqual(previous.Int16, my.Int16);
							Assert.AreEqual(previous.Int32, my.Int32);
							Assert.AreEqual(previous.Int64, my.Int64);
							Assert.AreEqual(previous.UInt16, my.UInt16);
							Assert.AreEqual(previous.UInt32, my.UInt32);
							Assert.AreEqual(previous.UInt64, my.UInt64);
							Assert.AreEqual(previous.NullableInt32, my.NullableInt32);
							Assert.IsFalse(previous.Int32Arr.EqualsOrItemsEqual(my.Int32Arr));
							break;
					}
				}
				Console.WriteLine(String.Concat("Mutated ", test.ItemsToCreate, " Dto instances in ", time.Elapsed));
			}
		}

		public class MyService
		{
			public IThing NameTheThing(IThing thing)
			{
				Assert.IsNotNull(thing);

				using (var create = Create.SharedOrNewContainer())
				{
					return create.Mutation<IThing>(thing, th => { th.Name = String.Concat("My Name is :", th.Identity); });
				}
			}
		}

		[TestMethod]
		public void MyServiceCanNameTheThing()
		{
			var seed = 0;

			using (var create = Create.SharedOrNewContainer())
			{
				create.Subscribe<IThing>((t, item, name, kind) => { item.Identity = Interlocked.Increment(ref seed); });

				var svc = create.New<MyService>();

				var orig = create.New<IThing>();
				Assert.IsNotNull(orig);
				Assert.IsTrue(orig.Identity > 0);

				var other = svc.NameTheThing(orig);
				Assert.AreNotSame(orig, other);
				Assert.AreEqual(orig.Identity, other.Identity);
				Assert.AreNotEqual(orig.Name, other.Name);
			}
		}		
	}
}
