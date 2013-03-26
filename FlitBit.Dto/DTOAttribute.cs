#region COPYRIGHT© 2009-2013 Phillip Clark. All rights reserved.

// For licensing information see License.txt (MIT style licensing).

#endregion

using System;
using System.Reflection;
using FlitBit.Core;
using FlitBit.Core.Factory;
using FlitBit.Emit;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;

namespace FlitBit.Dto
{
	/// <summary>
	///   Marks an interface or class as a stereotypical DTO and implements the stereotypical DTO behavior for interfaces.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface)]
	public class DTOAttribute : WireupTaskAttribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DTOAttribute()
			: base(WireupPhase.Tasks)
		{
		}

		/// <summary>
		/// Placeholder for wireup logic.
		/// </summary>
		protected override void PerformTask(Wireup.IWireupCoordinator coordinator)
		{
		}
	}

	internal static class DTOWireupObserver
	{
		static readonly MethodInfo ConcreteTypeMethod = typeof(DataTransferObjects).MatchGenericMethod("ConcreteType", BindingFlags.Static | BindingFlags.NonPublic, 1, typeof(Type));
		
		static readonly MethodInfo RegisterMethod = typeof(IFactory).MatchGenericMethod("RegisterImplementationType", 2,
																																										typeof(void));
		public static readonly Guid WireupObserverKey = new Guid("427C4C0A-7F66-47B4-85F4-7C1A4132769D");

		class DtoObserver : IWireupObserver
		{
			public void NotifyWireupTask(IWireupCoordinator coordinator, WireupTaskAttribute task, Type target)
			{
				var cra = task as DTOAttribute;
				if (cra != null && target != null)
				{
					var concreteMethod = ConcreteTypeMethod.MakeGenericMethod(target);
					var concrete = (Type) concreteMethod.Invoke(null, null);
					var reg = RegisterMethod.MakeGenericMethod(target, concrete);
					reg.Invoke(FactoryProvider.Factory, null);
				}
			}

			/// <summary>
			/// Gets the observer's key.
			/// </summary>
			public Guid ObserverKey { get { return WireupObserverKey; } }
		}

		readonly static IWireupObserver __observer = new DtoObserver();

		public static IWireupObserver Observer { get { return __observer; } }
	}
}