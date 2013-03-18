using System;
using FlitBit.Core;
using FlitBit.Emit;
using FlitBit.Wireup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlitBit.Dto.Tests
{
	[DTO]
	public interface IIntAndString
	{
		int IntProp { get; set; }
		String StringProp { get; set; }
	}

	[TestClass]
	public class DtoHashCodeAndEqualityTests
	{
		[TestMethod]
		public void Dto_DtosWithEqualPropertyValuesEvaluateEqual()
		{
			var rand = new Random();
			int i;
			string s;

			var factory = FactoryProvider.Factory;
			var orig = factory.CreateInstance<IIntAndString>();
			Assert.IsNotNull(orig);

			i = rand.Next();
			s = String.Concat("Yo, this is ", i.ToString("X"));
			orig.IntProp = i;
			orig.StringProp = s;

			Assert.AreEqual(i, orig.IntProp);
			Assert.AreEqual(s, orig.StringProp);

			var other = factory.CreateInstance<IIntAndString>();
			other.IntProp = i;
			other.StringProp = s;

			Assert.AreEqual(i, other.IntProp);
			Assert.AreEqual(s, other.StringProp);

			Assert.AreEqual(orig, other);
			Assert.AreEqual(orig.GetHashCode(), other.GetHashCode());
		}

		[TestInitialize]
		public void Init()
		{
			RuntimeAssemblies.WriteDynamicAssemblyOnExit = true;
			WireupCoordinator.SelfConfigure();
		}
	}
}