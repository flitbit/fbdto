#region COPYRIGHT© 2009-2013 Phillip Clark. All rights reserved.

// For licensing information see License.txt (MIT style licensing).

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using FlitBit.Core;
using FlitBit.Core.Collections;
using FlitBit.Dto.SPI;
using FlitBit.Emit;
using Extensions = FlitBit.Core.Extensions;

namespace FlitBit.Dto
{
	/// <summary>
	///   Utility class for fulfilling the data-transfer-object stereotype by emitting DTOs.
	/// </summary>
	internal static class DataTransferObjects
	{
		internal static readonly string CDirtyFlagsBackingFieldName = "<DirtyFlags>";

		static readonly Lazy<EmittedModule> LazyModule =
			new Lazy<EmittedModule>(() => RuntimeAssemblies.DynamicAssembly.DefineModule("DataTransferObjects", null),
															LazyThreadSafetyMode.ExecutionAndPublication
				);

		static EmittedModule Module { get { return LazyModule.Value; } }

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		internal static Type ConcreteType<T>()
		{
			Contract.Ensures(Contract.Result<Type>() != null);

			var targetType = typeof(T);
			var typeName = RuntimeAssemblies.PrepareTypeName(targetType, "DTO");

			var module = Module;
			lock (module)
			{
				var type = module.Builder.GetType(typeName, false, false) ?? EmitDTO<T>.BuildDTO(module, typeName);
				return type;
			}
		}

		static class EmitDTO<T>
		{
			internal static Type BuildDTO(EmittedModule module, string typeName)
			{
				Contract.Requires(module != null);
				Contract.Requires(typeName != null);
				Contract.Requires(typeName.Length > 0);
				Contract.Ensures(Contract.Result<Type>() != null);

				var builder = module.DefineClass(typeName, EmittedClass.DefaultTypeAttributes, typeof(object), null);
				builder.Attributes = TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.BeforeFieldInit;

				builder.SetCustomAttribute<SerializableAttribute>();

				var chashCodeSeed = builder.DefineField<int>("CHashCodeSeed");
				chashCodeSeed.IncludeAttributes(FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.InitOnly);
				var cctor = builder.DefineCCtor();
				cctor.ContributeInstructions((m, il) =>
				{
					il.LoadType(builder.Builder);
					il.CallVirtual(typeof(Type).GetProperty("AssemblyQualifiedName")
																		.GetGetMethod());
					il.CallVirtual<object>("GetHashCode");
					il.StoreField(chashCodeSeed);
				});

				var dirtyFlags = builder.DefineField<BitVector>(CDirtyFlagsBackingFieldName);
				dirtyFlags.ClearAttributes();

				var ctor = builder.DefineDefaultCtor();

				var propChanged = ImplementINotifyPropertyChanged(builder);
				var props = new List<PropertyInfo>();
				foreach (var intf in from type in typeof(T).GetTypeHierarchyInDeclarationOrder()
														where type.IsInterface
															&& type != typeof(IEquatable<T>)
															&& type != typeof(IDataTransferObject)
															&& type != typeof(ICloneable)
															&& type != typeof(INotifyPropertyChanged)
														select type)
				{
					builder.AddInterfaceImplementation(intf);
					ImplementPropertiesForInterface(intf, builder, props, dirtyFlags, propChanged);
					builder.StubMethodsForInterface(intf, true, true);
				}
				ImplementSpecializedEquals(builder);
				ImplementSpecializedGetHashCode(builder, chashCodeSeed);
				ImplementIDataTransferObject(builder, cctor, props, dirtyFlags);

				ctor.ContributeInstructions(
																	 (m, il) =>
																	 {
																		il.Nop();
																		il.LoadArg_0();
																		il.LoadValue(props.Count);
																		il.New<BitVector>(typeof(int));
																		il.StoreField(dirtyFlags);
																		il.Nop();
																		foreach (var prop in props)
																		{
																			var fieldType = prop.PropertyType;
																			if (fieldType.IsInterface && fieldType.IsGenericType)
																			{
																				var genericDef = fieldType.GetGenericTypeDefinition();
																				if (genericDef == typeof(IList<>) || genericDef == typeof(ICollection<>))
																				{
																					EmittedProperty ep;
																					if (builder.TryGetProperty(prop.Name, out ep))
																					{
																						il.Nop();
																						il.LoadArg_0();
																						il.LoadNull();
																						il.Call(ep.Setter.Builder);
																						il.Nop();
																					}
																				}
																			}
																		}
																	 });

				builder.Compile();
				return builder.Ref.Target;
			}

			static void ImplementICloneableClone(EmittedClass builder, List<PropertyInfo> props, EmittedField dirtyFlags)
			{
				var method = builder.DefineMethod("Clone");
				method.ClearAttributes();
				method.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				method.ReturnType = TypeRef.FromType<object>();
				method.ContributeInstructions(
																		 (m, il) =>
																		 {
																			var res = il.DeclareLocal(builder.Builder);
																			il.DeclareLocal(typeof(object));
																			var flag = il.DeclareLocal(typeof(bool));
																			il.Nop();
																			il.LoadArg_0();
																			il.Call<Object>("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
																			il.CastClass(builder.Builder);
																			il.StoreLocal_0();
																			il.LoadLocal_0();
																			il.LoadArg_0();
																			il.LoadFieldAddress(dirtyFlags);
																			il.Call<BitVector>("Copy");
																			il.StoreField(dirtyFlags);

																			il.LoadLocal_0();
																			il.LoadNull();
																			il.StoreField(builder.Fields.Single(f => f.Name == "<PropertyChanged>")
																													.Builder);

																			foreach (var prop in props)
																			{
																				var fieldType = prop.PropertyType;
																				if (fieldType.IsInterface && fieldType.IsGenericType)
																				{
																					var genericDef = fieldType.GetGenericTypeDefinition();
																					if (genericDef == typeof(IList<>) || genericDef == typeof(ICollection<>))
																					{
																						EmittedProperty ep;
																						if (builder.TryGetProperty(prop.Name, out ep))
																						{
																							il.Nop();
																							il.LoadLocal_0();
																							il.LoadArg_0();
																							il.LoadField(ep.BoundField);
																							il.Call(ep.Setter.Builder);
																							il.Nop();
																						}
																					}
																				}
																				else if (fieldType.IsArray)
																				{
																					// copy the first rank
																					EmittedProperty ep;
																					if (builder.TryGetProperty(prop.Name, out ep))
																					{
																						var after = il.DefineLabel();
																						var len = il.DeclareLocal(typeof(int));
																						il.LoadArg_0();
																						il.LoadField(ep.BoundField);
																						il.LoadNull();
																						il.CompareEqual();
																						il.StoreLocal(flag);
																						il.LoadLocal(flag);
																						il.BranchIfTrue(after);
																						il.Nop();
																						il.LoadArg_0();
																						il.LoadField(ep.BoundField);
																						il.Emit(OpCodes.Ldlen);
																						il.ConvertToInt32();
																						il.StoreLocal(len);
																						il.LoadLocal(res);
																						il.LoadLocal(len);
																						il.Emit(OpCodes.Newarr, fieldType.GetElementType());
																						il.StoreField(ep.BoundField);
																						il.LoadArg_0();
																						il.LoadField(ep.BoundField);
																						il.LoadLocal(res);
																						il.LoadField(ep.BoundField);
																						il.LoadLocal(len);
																						il.Call<Array>("Copy", BindingFlags.Static | BindingFlags.Public, typeof(Array), typeof(Array),
																													typeof(int));
																						il.Nop();
																						il.Nop();
																						il.MarkLabel(after);
																					}
																				}
																			}

																			il.LoadLocal_0();
																			il.StoreLocal_1();
																			var exit = il.DefineLabel();
																			il.Branch_ShortForm(exit);
																			il.MarkLabel(exit);
																			il.LoadLocal_1();
																		 });
			}

			static void ImplementIDataTransferObject(EmittedClass builder, EmittedConstructor cctor, List<PropertyInfo> props,
				EmittedField dirtyFlags)
			{
				builder.AddInterfaceImplementation(typeof(IDataTransferObject));

				ImplementIDataTransferObjectGetDirtyFlags(builder, dirtyFlags);
				ImplementIDataTransferObjectResetDirtyFlags(builder, props, dirtyFlags);
				ImplementIDataTransferObjectIsDirty(builder, cctor, props, dirtyFlags);
				ImplementICloneableClone(builder, props, dirtyFlags);
			}

			static void ImplementIDataTransferObjectGetDirtyFlags(EmittedClass builder, EmittedField dirtyFlags)
			{
				var method = builder.DefineMethod("GetDirtyFlags");
				method.ClearAttributes();
				method.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				method.ReturnType = TypeRef.FromType<BitVector>();
				method.ContributeInstructions(
																		 (m, il) =>
																		 {
																			il.DeclareLocal(typeof(BitVector));
																			il.Nop();
																			il.LoadArg_0();
																			il.LoadFieldAddress(dirtyFlags);
																			il.Call<BitVector>("Clone");
																			il.UnboxAny(typeof(BitVector));
																			il.StoreLocal_0();
																			var exit = il.DefineLabel();
																			il.Branch_ShortForm(exit);
																			il.MarkLabel(exit);
																			il.LoadLocal_0();
																		 });
			}

			static void ImplementIDataTransferObjectIsDirty(EmittedClass builder, EmittedConstructor cctor,
				List<PropertyInfo> props,
				EmittedField dirtyFlags)
			{
				var fieldMap = builder.DefineField<string[]>("__fieldMap");
				fieldMap.ClearAttributes();
				fieldMap.IncludeAttributes(FieldAttributes.Static | FieldAttributes.InitOnly | FieldAttributes.Private);
				cctor.ContributeInstructions((m, il) =>
				{
					var arr = il.DeclareLocal(typeof(String[]));
					il.Nop();
					il.NewArr(typeof(string), props.Count);
					il.StoreLocal(arr);
					for (var i = 0; i < props.Count; i++)
					{
						il.LoadLocal(arr);
						il.LoadValue(i);
						il.LoadValue(props[i].Name);
						il.Emit(OpCodes.Stelem, typeof(string));
					}
					il.LoadLocal(arr);
					il.StoreField(fieldMap);
					il.Nop();
					il.Nop();
				});

				var method = builder.DefineMethod("IsDirty");
				method.ClearAttributes();
				method.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				method.ReturnType = TypeRef.FromType<bool>();
				method.DefineParameter("member", typeof(String));
				method.ContributeInstructions(
																		 (m, il) =>
																		 {
																			il.DeclareLocal(typeof(Int32));
																			il.DeclareLocal(typeof(bool));
																			il.DeclareLocal(typeof(bool));
																			var proceed = il.DefineLabel();
																			var yep = il.DefineLabel();
																			il.Nop();
																			il.LoadArg_1();
																			il.LoadNull();
																			il.CompareEqual();
																			il.LoadValue(false);
																			il.CompareEqual();
																			il.StoreLocal_2();
																			il.LoadLocal_2();
																			il.BranchIfTrue_ShortForm(proceed);
																			il.LoadValue("member");
																			il.New<ArgumentNullException>(typeof(string));
																			il.Throw();
																			il.MarkLabel(proceed);
																			il.LoadField(fieldMap);
																			il.LoadArg_1();
																			var indexOf = typeof(Array).MatchGenericMethod("IndexOf", BindingFlags.Public | BindingFlags.Static,
																																										1, typeof(int), typeof(string[]), typeof(string));
																			il.Call(indexOf.MakeGenericMethod(typeof(string)));
																			il.StoreLocal_0();
																			il.LoadLocal_0();
																			il.LoadValue(0);
																			il.CompareLessThan();
																			il.LoadValue(0);
																			il.CompareEqual();
																			il.StoreLocal_2();
																			il.LoadLocal_2();
																			il.BranchIfTrue(yep);
																			il.Nop();
																			il.LoadValue("member");
																			il.LoadValue(String.Concat(typeof(T).GetReadableSimpleName(), " does not define property: `"));
																			il.LoadArg_1();
																			il.LoadValue("`.");
																			il.Call<string>("Concat", BindingFlags.Static | BindingFlags.Public, typeof(string), typeof(string),
																											typeof(string));
																			il.NewObj(
																							 typeof(ArgumentOutOfRangeException).GetConstructor(new[] {typeof(string), typeof(string)}));
																			il.Throw();
																			il.MarkLabel(yep);
																			il.LoadArg_0();
																			il.LoadFieldAddress(dirtyFlags);
																			il.LoadLocal_0();
																			il.Call<BitVector>("get_Item", typeof(int));
																			il.StoreLocal_1();
																			il.DefineAndMarkLabel();
																			il.LoadLocal_1();
																		 });
			}

			static void ImplementIDataTransferObjectResetDirtyFlags(EmittedClass builder, List<PropertyInfo> props,
				EmittedField dirtyFlags)
			{
				var method = builder.DefineMethod("ResetDirtyFlags");
				method.ClearAttributes();
				method.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				method.ContributeInstructions(
																		 (m, il) =>
																		 {
																			il.Nop();
																			il.LoadArg_0();
																			il.LoadValue(props.Count);
																			il.New<BitVector>(typeof(int));
																			il.StoreField(dirtyFlags);
																			il.Nop();
																		 });
			}

			static EmittedMethod ImplementINotifyPropertyChanged(EmittedClass builder)
			{
				var evtType = typeof(PropertyChangedEventHandler);
				builder.AddInterfaceImplementation(typeof(INotifyPropertyChanged));

				var propertyChanged = builder.Builder.DefineEvent("PropertyChanged", EventAttributes.None, evtType);

				var propertyChangedBackingField = builder.DefineField<PropertyChangedEventHandler>("<PropertyChanged>");
				propertyChangedBackingField.ClearAttributes();
				propertyChangedBackingField.IncludeAttributes(FieldAttributes.Private);
				var ctor = typeof(NonSerializedAttribute).GetConstructor(Type.EmptyTypes);
				Debug.Assert(ctor != null, "ctor != null");
				propertyChangedBackingField.Builder.SetCustomAttribute(new CustomAttributeBuilder(ctor, new object[0]));

				// Emit a standard add <event handler> method (similar to what the C# compiler does)...
				/* 				 
				public void add_PropertyChanged(PropertyChangedEventHandler value)
				{
						PropertyChangedEventHandler orig;
						PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
						do
						{
								orig = propertyChanged;
								PropertyChangedEventHandler updated = (PropertyChangedEventHandler) Delegate.Combine(orig, value);
								check = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChanged, updated, orig);
						}
						while (check != orig);
				}
				*/

				var add = builder.DefineMethod("add_PropertyChanged");
				add.ClearAttributes();
				add.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
					MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				add.DefineParameter("value", evtType);
				add.ContributeInstructions(
																	 (m, il) =>
																	 {
																		var retry = il.DefineLabel();
																		il.DeclareLocal(evtType);
																		il.DeclareLocal(evtType);
																		il.DeclareLocal(evtType);
																		il.DeclareLocal(typeof(bool));
																		il.LoadArg_0();
																		il.LoadField(propertyChangedBackingField);
																		il.StoreLocal_0();
																		il.MarkLabel(retry);
																		il.LoadLocal_0();
																		il.StoreLocal_1();
																		il.LoadLocal_1();
																		il.LoadArg_1();
																		il.Call<Delegate>("Combine", BindingFlags.Static | BindingFlags.Public, typeof(Delegate),
																											typeof(Delegate));
																		il.CastClass<PropertyChangedEventHandler>();
																		il.StoreLocal_2();
																		il.LoadArg_0();
																		il.LoadFieldAddress(propertyChangedBackingField);
																		il.LoadLocal_2();
																		il.LoadLocal_1();
																		var compex = (from c in typeof(Interlocked).GetMethods(BindingFlags.Static | BindingFlags.Public)
																									where c.IsGenericMethodDefinition && c.Name == "CompareExchange"
																									select c).Single();
																		il.Call(compex.MakeGenericMethod(evtType));
																		il.StoreLocal_0();
																		il.LoadLocal_0();
																		il.LoadLocal_1();
																		il.CompareEqual();
																		il.StoreLocal_3();
																		il.LoadLocal_3();
																		il.BranchIfTrue_ShortForm(retry);
																	 });
				var remove = builder.DefineMethod("remove_PropertyChanged");
				remove.ClearAttributes();
				remove.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
					MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				remove.DefineParameter("value", evtType);
				remove.ContributeInstructions(
																		 (m, il) =>
																		 {
																			var retry = il.DefineLabel();
																			il.DeclareLocal(evtType);
																			il.DeclareLocal(evtType);
																			il.DeclareLocal(evtType);
																			il.DeclareLocal(typeof(bool));
																			il.LoadArg_0();
																			il.LoadField(propertyChangedBackingField);
																			il.StoreLocal_0();
																			il.MarkLabel(retry);
																			il.LoadLocal_0();
																			il.StoreLocal_1();
																			il.LoadLocal_1();
																			il.LoadArg_1();
																			il.Call<Delegate>("Remove", BindingFlags.Static | BindingFlags.Public, typeof(Delegate),
																												typeof(Delegate));
																			il.CastClass<PropertyChangedEventHandler>();
																			il.StoreLocal_2();
																			il.LoadArg_0();
																			il.LoadFieldAddress(propertyChangedBackingField);
																			il.LoadLocal_2();
																			il.LoadLocal_1();
																			var compex = (from c in typeof(Interlocked).GetMethods(BindingFlags.Static | BindingFlags.Public)
																										where c.IsGenericMethodDefinition && c.Name == "CompareExchange"
																										select c).Single();
																			il.Call(compex.MakeGenericMethod(evtType));
																			il.StoreLocal_0();
																			il.LoadLocal_0();
																			il.LoadLocal_1();
																			il.CompareEqual();
																			il.StoreLocal_3();
																			il.LoadLocal_3();
																			il.BranchIfTrue_ShortForm(retry);
																		 });
				propertyChanged.SetAddOnMethod(add.Builder);
				propertyChanged.SetRemoveOnMethod(remove.Builder);

				var onPropertyChanged = builder.DefineMethod("HandlePropertyChanged");
				onPropertyChanged.ClearAttributes();
				onPropertyChanged.IncludeAttributes(MethodAttributes.HideBySig);
				onPropertyChanged.DefineParameter("propName", typeof(String));
				onPropertyChanged.ContributeInstructions(
																								 (m, il) =>
																								 {
																									var exit = il.DefineLabel();
																									il.DeclareLocal(typeof(bool));
																									il.Nop();
																									il.LoadArg_0();
																									il.LoadField(propertyChangedBackingField);
																									il.LoadNull();
																									il.CompareEqual();
																									il.StoreLocal_0();
																									il.LoadLocal_0();
																									il.BranchIfTrue_ShortForm(exit);
																									il.Nop();
																									il.LoadArg_0();
																									il.LoadField(propertyChangedBackingField);
																									il.LoadArg_0();
																									il.LoadArg_1();
																									il.New<PropertyChangedEventArgs>(typeof(string));
																									il.CallVirtual<PropertyChangedEventHandler>("Invoke", typeof(object),
																																															typeof(PropertyChangedEventArgs));
																									il.Nop();
																									il.Nop();
																									il.MarkLabel(exit);
																								 });

				return onPropertyChanged;
			}

			static void ImplementPropertiesForInterface(Type intf, EmittedClass builder, List<PropertyInfo> props,
				EmittedField dirtyFlags, EmittedMethod propChanged)
			{
				var properties = intf.GetProperties();
				foreach (var p in properties)
				{
					ImplementPropertyFor(intf, builder, props, p, dirtyFlags, propChanged);
					props.Add(p);
				}
			}

			static void ImplementPropertyFor(Type intf, EmittedClass builder, List<PropertyInfo> props, PropertyInfo property,
				EmittedField dirtyFlags, EmittedMethod propChanged)
			{
				var fieldName = String.Concat("<", intf.Name, "_", property.Name, ">_field");
				var prop = builder.DefinePropertyFromPropertyInfo(property);
				EmittedField field = null;
				var fieldType = property.PropertyType;
				Type observableCollection = null;
				Type genericArgType = null;
				if (fieldType.IsInterface && fieldType.IsGenericType)
				{
					var genericDef = fieldType.GetGenericTypeDefinition();
					if (genericDef == typeof(IList<>) || genericDef == typeof(ICollection<>))
					{
						genericArgType = fieldType.GetGenericArguments()[0];
						observableCollection = typeof(ObservableCollection<>).MakeGenericType(genericArgType);
						field = builder.DefineField(fieldName, observableCollection);
					}
				}
				if (field == null)
				{
					field = builder.DefineField(fieldName, property.PropertyType);
				}
				prop.BindField(field);
				var captureDirtyFlagsIndex = props.Count;

				prop.AddGetter()
						.ContributeInstructions((m, il) =>
						{
							il.LoadArg_0();
							il.LoadField(field);
						});
				if (property.CanWrite)
				{
					prop.AddSetter()
							.ContributeInstructions((m, il) =>
							{
								var exit = il.DefineLabel();

								il.DeclareLocal(typeof(bool));
								il.Nop();

								if (fieldType.IsArray)
								{
									var elmType = fieldType.GetElementType();
									LoadFieldFromThisAndValue(il, field);
									il.Call(typeof(Extensions).GetMethod("EqualsOrItemsEqual", BindingFlags.Static | BindingFlags.Public)
																						.MakeGenericMethod(elmType));
								}
								else if (fieldType.IsClass)
								{
									var opEquality = fieldType.GetMethod("op_Equality", BindingFlags.Public | BindingFlags.Static);
									if (opEquality != null)
									{
										LoadFieldFromThisAndValue(il, field);
										il.Call(opEquality);
									}
									else
									{
										il.Call(typeof(EqualityComparer<>).MakeGenericType(fieldType)
																											.GetMethod("get_Default", BindingFlags.Static | BindingFlags.Public));
										LoadFieldFromThisAndValue(il, field);
										il.CallVirtual(typeof(IEqualityComparer<>).MakeGenericType(fieldType)
																															.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance,
																																				null,
																																				new[] {fieldType, fieldType},
																																				null
																		));
									}
								}
								else
								{
									LoadFieldFromThisAndValue(il, field);
									il.CompareEquality(fieldType);
								}
								il.StoreLocal_0();
								il.LoadLocal_0();
								il.BranchIfTrue_ShortForm(exit);

								il.Nop();
								il.LoadArg_0();
								il.LoadArg_1();
								il.StoreField(field);

								il.LoadArg_0();
								il.LoadFieldAddress(dirtyFlags);
								il.LoadValue(captureDirtyFlagsIndex);
								il.LoadValue(true);
								il.Call<BitVector>("set_Item");

								il.Nop();
								il.LoadArg_0();
								il.LoadValue(prop.Name);
								il.Call(propChanged);

								il.Nop();
								il.Nop();
								il.MarkLabel(exit);
							});
				}
				else if (observableCollection != null)
				{
					var observer =
						builder.DefineMethod(String.Concat("<", intf.Name, "_", property.Name, ">_field_CollectionChanged"));
					observer.ClearAttributes();
					observer.IncludeAttributes(MethodAttributes.Private | MethodAttributes.HideBySig);
					observer.DefineParameter("sender", typeof(object));
					observer.DefineParameter("e", typeof(NotifyCollectionChangedEventArgs));
					observer.ContributeInstructions(
																				 (m, il) =>
																				 {
																					il.Nop();
																					il.LoadArg_0();
																					il.LoadFieldAddress(dirtyFlags);
																					il.LoadValue(captureDirtyFlagsIndex);
																					il.LoadValue(true);
																					il.Call<BitVector>("set_Item");
																					il.Nop();
																					il.LoadArg_0();
																					il.LoadValue(property.Name);
																					il.Call(propChanged);
																					il.Nop();
																				 });

					var setter = prop.AddSetter();
					setter.ExcludeAttributes(MethodAttributes.Public);
					setter.IncludeAttributes(MethodAttributes.Private);
					setter.ContributeInstructions((m, il) =>
					{
						var isnull = il.DefineLabel();
						var after = il.DefineLabel();
						il.DeclareLocal<bool>();
						il.Nop();
						il.LoadArg_1();
						il.LoadNull();
						il.CompareEqual();
						il.StoreLocal_0();
						il.LoadLocal_0();
						il.BranchIfTrue_ShortForm(isnull);
						il.Nop();
						il.LoadArg_0();
						il.LoadArg_1();
						il.NewObj(observableCollection.GetConstructor(new[] {typeof(IEnumerable<>).MakeGenericType(genericArgType)}));
						il.StoreField(field);
						il.Nop();
						il.Branch_ShortForm(after);
						il.MarkLabel(isnull);
						il.Nop();
						il.LoadArg_0();
						il.NewObj(observableCollection.GetConstructor(Type.EmptyTypes));
						il.StoreField(field);
						il.Nop();
						il.MarkLabel(after);
						il.LoadArg_0();
						il.LoadField(field);
						il.LoadArg_0();
						il.Emit(OpCodes.Ldftn, observer.Builder);
						// This seems really brittle...
						il.NewObj(typeof(NotifyCollectionChangedEventHandler).GetConstructor(new[] {typeof(object), typeof(IntPtr)}));
						il.CallVirtual(observableCollection.GetEvent("CollectionChanged")
																							.GetAddMethod());
						il.Nop();
					});
				}
			}

			static EmittedMethod ImplementSpecializedEquals(EmittedClass builder)
			{
				var equatable = typeof(IEquatable<>).MakeGenericType(builder.Builder);
				builder.AddInterfaceImplementation(equatable);

				var specializedEquals = builder.DefineMethod("Equals");
				specializedEquals.ClearAttributes();
				specializedEquals.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				specializedEquals.ReturnType = TypeRef.FromType<bool>();
				var other = specializedEquals.DefineParameter("other", builder.Ref);

				specializedEquals.ContributeInstructions((m, il) =>
				{
					il.DeclareLocal(typeof(bool));
					var exitFalse = il.DefineLabel();
					il.Nop();

					var fields =
						new List<EmittedField>(
							builder.Fields.Where(f => f.IsStatic == false && f.FieldType.Target != typeof(PropertyChangedEventHandler)));
					for (var i = 0; i < fields.Count; i++)
					{
						var field = fields[i];
						var fieldType = field.FieldType.Target;
						if (fieldType.IsArray)
						{
							var elmType = fieldType.GetElementType();
							LoadFieldsFromThisAndParam(il, field, other);
							il.Call(typeof(Extensions).GetMethod("EqualsOrItemsEqual", BindingFlags.Static | BindingFlags.Public)
																				.MakeGenericMethod(elmType));
						}
						else if (fieldType.IsClass)
						{
							if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
							{
								// compare observable collections for member equality...
								var genericArg = fieldType.GetGenericArguments()[0];
								var etype = typeof(IEnumerable<>).MakeGenericType(genericArg);
								var sequenceEquals = typeof(Enumerable).MatchGenericMethod("SequenceEqual",
																																					BindingFlags.Static | BindingFlags.Public, 1, typeof(bool), etype, etype);
								LoadFieldsFromThisAndParam(il, field, other);
								il.Call(sequenceEquals.MakeGenericMethod(genericArg));
							}
							else
							{
								var opEquality = fieldType.GetMethod("op_Equality", BindingFlags.Public | BindingFlags.Static);
								if (opEquality != null)
								{
									LoadFieldsFromThisAndParam(il, field, other);
									il.Call(opEquality);
								}
								else
								{
									il.Call(typeof(EqualityComparer<>).MakeGenericType(fieldType)
																										.GetMethod("get_Default", BindingFlags.Static | BindingFlags.Public));
									LoadFieldsFromThisAndParam(il, field, other);
									il.CallVirtual(typeof(IEqualityComparer<>).MakeGenericType(fieldType)
																														.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance,
																																			null,
																																			new[] {fieldType, fieldType},
																																			null
																	));
								}
							}
						}
						else
						{
							LoadFieldsFromThisAndParam(il, field, other);
							il.CompareEquality(fieldType);
						}
						if (i < fields.Count - 1)
						{
							il.BranchIfFalse(exitFalse);
						}
					}
					var exit = il.DefineLabel();
					il.Branch(exit);
					il.MarkLabel(exitFalse);
					il.Load_I4_0();
					il.MarkLabel(exit);
					il.StoreLocal_0();
					var fin = il.DefineLabel();
					il.Branch(fin);
					il.MarkLabel(fin);
					il.LoadLocal_0();
				});

				var contributedEquals = new Action<EmittedMethodBase, ILGenerator>((m, il) =>
				{
					var exitFalse2 = il.DefineLabel();
					var exit = il.DefineLabel();
					il.DeclareLocal(typeof(bool));
					il.Nop();
					il.LoadArg_1();
					il.IsInstance(builder.Builder);
					il.BranchIfFalse(exitFalse2);
					il.LoadArg_0();
					il.LoadArg_1();
					il.CastClass(builder.Builder);
					il.Call(specializedEquals);
					il.Branch(exit);
					il.MarkLabel(exitFalse2);
					il.LoadValue(false);
					il.MarkLabel(exit);
					il.StoreLocal_0();
					var fin = il.DefineLabel();
					il.Branch(fin);
					il.MarkLabel(fin);
					il.LoadLocal_0();
				});

				var equatableT = typeof(IEquatable<>).MakeGenericType(typeof(T));
				builder.AddInterfaceImplementation(equatableT);
				var equalsT = builder.DefineMethod("Equals");
				equalsT.ClearAttributes();
				equalsT.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
					MethodAttributes.Virtual | MethodAttributes.Final);
				equalsT.ReturnType = TypeRef.FromType<bool>();
				equalsT.DefineParameter("other", typeof(T));
				equalsT.ContributeInstructions(contributedEquals);

				builder.DefineOverrideMethod(typeof(Object).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public, null,
																															new[] {typeof(object)}, null))
							.ContributeInstructions(contributedEquals);

				return specializedEquals;
			}

			static void ImplementSpecializedGetHashCode(EmittedClass builder, EmittedField chashCodeSeed)
			{
				Contract.Requires<ArgumentNullException>(builder != null);

				var method = builder.DefineMethod("GetHashCode");
				method.ClearAttributes();
				method.IncludeAttributes(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual);
				method.ReturnType = TypeRef.FromType<int>();
				method.ContributeInstructions((m, il) =>
				{
					var prime = il.DeclareLocal(typeof(Int32));
					var result = il.DeclareLocal(typeof(Int32));
					il.DeclareLocal(typeof(Int32));
					il.DeclareLocal(typeof(bool));
					il.Nop();
					il.LoadValue(Constants.NotSoRandomPrime);
					il.StoreLocal(prime);
					il.LoadValue(chashCodeSeed);
					il.LoadLocal(prime);
					il.Multiply();
					il.StoreLocal(result);
					var exit = il.DefineLabel();
					var fields =
						new List<EmittedField>(
							builder.Fields.Where(f => f.IsStatic == false && f.FieldType.Target != typeof(PropertyChangedEventHandler)));
					foreach (var field in fields)
					{
						var fieldType = field.FieldType.Target;
						var tc = Type.GetTypeCode(fieldType);
						switch (tc)
						{
							case TypeCode.Boolean:
							case TypeCode.Byte:
							case TypeCode.Char:
							case TypeCode.Int16:
							case TypeCode.Int32:
							case TypeCode.SByte:
							case TypeCode.Single:
							case TypeCode.UInt16:
							case TypeCode.UInt32:
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadField(field);
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
								break;
							case TypeCode.DateTime:
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadFieldAddress(field);
								il.Constrained(typeof(DateTime));
								il.CallVirtual<object>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
								break;
							case TypeCode.Decimal:
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadFieldAddress(field);
								il.Call<Decimal>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
								break;
							case TypeCode.Double:
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadFieldAddress(field);
								il.Call<Double>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
								break;
							case TypeCode.Int64:
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadFieldAddress(field);
								il.Constrained(typeof(Int64));
								il.CallVirtual<object>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
								break;
							case TypeCode.Object:
								if (typeof(Guid).IsAssignableFrom(fieldType))
								{
									il.LoadLocal(result);
									il.LoadLocal(prime);
									il.LoadArg_0();
									il.LoadFieldAddress(field);
									il.Constrained(typeof(Guid));
									il.CallVirtual<object>("GetHashCode");
									il.Multiply();
									il.Xor();
									il.StoreLocal(result);
								}
								else if (fieldType.IsArray)
								{
									var elmType = fieldType.GetElementType();
									il.LoadLocal(result);
									il.LoadLocal(prime);
									il.LoadArg_0();
									il.LoadField(field);
									il.LoadLocal(result);
									il.Call(typeof(Extensions).GetMethod("CalculateCombinedHashcode", BindingFlags.Public | BindingFlags.Static)
																						.MakeGenericMethod(elmType));
									il.Multiply();
									il.Xor();
									il.StoreLocal(result);
								}
								else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
								{
									il.LoadLocal(result);
									il.LoadLocal(prime);
									il.LoadArg_0();
									il.LoadFieldAddress(field);
									il.Constrained(fieldType);
									il.CallVirtual<object>("GetHashCode");
									il.Multiply();
									il.Xor();
									il.StoreLocal(result);
								}
								else
								{
									il.LoadLocal(result);
									il.LoadLocal(prime);
									il.LoadArg_0();
									if (fieldType.IsValueType)
									{
										il.LoadFieldAddress(field);
										il.Constrained(fieldType);
									}
									else
									{
										il.LoadField(field);
									}
									il.CallVirtual<object>("GetHashCode");
									il.Multiply();
									il.Xor();
									il.StoreLocal(result);
								}
								break;
							case TypeCode.String:
								il.LoadArg_0();
								il.LoadField(field);
								il.LoadNull();
								il.CompareEqual();
								il.StoreLocal_2();
								il.LoadLocal_2();
								var lbl = il.DefineLabel();
								il.BranchIfTrue_ShortForm(lbl);
								il.Nop();
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadField(field);
								il.CallVirtual<object>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
								il.MarkLabel(lbl);
								break;
							case TypeCode.UInt64:
								il.LoadLocal(result);
								il.LoadLocal(prime);
								il.LoadArg_0();
								il.LoadFieldAddress(field);
								il.Constrained(typeof(UInt64));
								il.CallVirtual<object>("GetHashCode");
								il.Multiply();
								il.Xor();
								il.StoreLocal(result);
								break;
							default:
								throw new InvalidOperationException(String.Concat("Unable to produce hashcode for type: ",
																																	fieldType.GetReadableFullName()));
						}
					}
					il.LoadLocal(result);
					il.StoreLocal_2();
					il.Branch(exit);
					il.MarkLabel(exit);
					il.LoadLocal_2();
				});
			}

			static void LoadFieldFromThisAndValue(ILGenerator il, EmittedField field)
			{
				Contract.Requires<ArgumentNullException>(il != null);
				Contract.Requires<ArgumentNullException>(field != null);
				il.LoadArg_0();
				il.LoadField(field);
				il.LoadArg_1();
			}

			static void LoadFieldsFromThisAndParam(ILGenerator il, EmittedField field, EmittedParameter parm)
			{
				Contract.Requires<ArgumentNullException>(il != null);
				Contract.Requires<ArgumentNullException>(field != null);
				il.LoadArg_0();
				il.LoadField(field);
				il.LoadArg(parm);
				il.LoadField(field);
			}
		}
	}
}