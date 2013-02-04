using FlitBit.Dto.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlitBit.Dto.Tests
{
	/// <summary>
	/// Summary description for DtoFactoryTests
	/// </summary>
	[TestClass]
	public class DtoFactoryTests
	{
		
		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void DtoFactory_CanCreateInstances()
		{			
			var it = DtoFactory.CreateInstance<IJustAnID>();
			Assert.IsNotNull(it);
		}
	}
}
