using System;
using System.Diagnostics.Contracts;
using FlitBit.Core;
using FlitBit.Core.Factory;

namespace FlitBit.Dto
{
	/// <summary>
	/// Utility class for accessing data-transfer-objects.
	/// </summary>
	public static class DtoFactory
	{
		/// <summary>
		/// Creates an instance of DTO type T.
		/// </summary>
		/// <typeparam name="T">DTO type T.</typeparam>
		/// <returns>a new instance</returns>
		public static T CreateInstance<T>()
		{
			Contract.Ensures(Contract.Result<T>() != null);
			var f = FactoryFactory.GetFactoryOrFallback(() => new DefaultDtoAccessFactory());
			return f.CreateInstance<T>();			
		}

		private sealed class DefaultDtoAccessFactory : IFactory
		{
			public T CreateInstance<T>()
			{
				var rt = DataTransfer.ConcreteType<T>();
				return (T)Activator.CreateInstance(rt);
			}
		}
	}
}
