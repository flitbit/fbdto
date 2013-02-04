using FlitBit.Core;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using System;

[assembly: WireupDependency(typeof(FlitBit.Emit.WireupThisAssembly))]
[assembly: Wireup(typeof(FlitBit.Dto.WireupThisAssembly))]

namespace FlitBit.Dto
{
	/// <summary>
	/// Wires up this assembly.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class WireupThisAssembly : IWireupCommand
	{
		/// <summary>
		/// Wires up this assembly.
		/// </summary>
		/// <param name="coordinator"></param>
		public void Execute(IWireupCoordinator coordinator)
		{
		}
	}
}
