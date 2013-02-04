#region COPYRIGHT© 2009-2013 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using System;
using FlitBit.Core.Meta;
using FlitBit.Emit.Meta;

namespace FlitBit.Dto
{
	/// <summary>
	/// Marks an interface or class as a stereotypical DTO and implements the stereotypical DTO behavior for interfaces.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
	[CLSCompliant(false)]
	public class DTOAttribute : AutoImplementedAttribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DTOAttribute()
		{
			this.RecommemdedScope = InstanceScopeKind.OnDemand;
		}


		/// <summary>
		/// Implements the stereotypical DTO behavior for interfaces of type T.
		/// </summary>
		/// <typeparam name="T">target type T</typeparam>
		/// <param name="complete">callback invoked with the implementation type or the type's factory function.</param>
		/// <returns><em>true</em> if the DTO was generated; otherwise <em>false</em>.</returns>
		public override bool GetImplementation<T>(Action<Type, Func<T>> complete)
		{
			complete(DataTransfer.ConcreteType<T>(), null);
			return true;
		}
	}
}
