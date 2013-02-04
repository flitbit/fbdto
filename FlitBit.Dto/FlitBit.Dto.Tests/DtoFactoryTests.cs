using FlitBit.Core;
using FlitBit.Dto.Tests.Model;
using FlitBit.Wireup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlitBit.Dto.Tests
{
	/// <summary>
	/// Summary description for DtoFactoryTests
	/// </summary>
	[TestClass]
	public class DtoFactoryTests
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
		}

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void DtoFactory_CanCreateInstances()
		{
			var factory = FactoryFactory.Instance;
			var it = factory.CreateInstance<IJustAnID>();
			Assert.IsNotNull(it);
		}
	}
}
