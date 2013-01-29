#region COPYRIGHT© 2009-2013 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using System;
using FlitBit.Core.Representation;
using FlitBit.IoC;
using FlitBit.IoC.Stereotype;

namespace FlitBit.Dto
{
	/// <summary>
	/// Marks an interface or class as a stereotypical DTO and implements the stereotypical DTO behavior for interfaces.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
	[CLSCompliant(false)]
	public class DTOAttribute : StereotypeAttribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DTOAttribute()
			: base(StereotypeBehaviors.AutoImplementedBehavior)
		{
		}

		/// <summary>
		/// Implements the stereotypical DTO behavior for interfaces of type T.
		/// </summary>
		/// <typeparam name="T">interface type T</typeparam>
		/// <returns>concrete implementation implementing the 
		/// stereotypical DTO behavior</returns>
		public override bool RegisterStereotypeImplementation<T>(IoC.IContainer container)
		{
			RequireTypeIsInterface<T>();

			Type concreteType = DataTransfer.ConcreteType<T>();
			Type jsonRepresentation = typeof(DelegatedJsonRepresentationLoose<,>).MakeGenericType(typeof(T), concreteType);

			Container.Root.ForType<T>()
				.Register(concreteType)
				.ResolveAnInstancePerRequest()
				.End()
			.ForType<IJsonRepresentation<T>>()
				.Register(jsonRepresentation)
				.ResolveAnInstancePerScope()
				.End();

			return true;
		}
	}
}
