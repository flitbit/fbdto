using FlitBit.Core;
using FlitBit.Dto.Tests.Model;
using FlitBit.Wireup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlitBit.Dto.Tests
{
	/// <summary>
	///   Summary description for DtoFactoryTests
	/// </summary>
	[TestClass]
	public class DtoFactoryTests
	{
		
		[TestMethod]
		public void DtoFactory_CanCreateInstances()
		{
			var factory = FactoryProvider.Factory;
			var it = factory.CreateInstance<IJustAnID>();
			Assert.IsNotNull(it);
		}

		public TestContext TestContext { get; set; }

	}
}