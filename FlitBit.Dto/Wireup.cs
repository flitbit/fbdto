using System;
using FlitBit.Dto;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;

[assembly: Wireup(typeof(WireupThisAssembly))]

namespace FlitBit.Dto
{
	/// <summary>
	///   Wires up this assembly.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class WireupThisAssembly : IWireupCommand
	{
		#region IWireupCommand Members

		/// <summary>
		///   Wires up this assembly.
		/// </summary>
		/// <param name="coordinator"></param>
		public void Execute(IWireupCoordinator coordinator)
		{
			// no dependencies... we should remove this if there truly is nothing to do.
		}

		#endregion
	}
}