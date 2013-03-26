using FlitBit.Dto;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;

[assembly: HookWirupCoordinatorTask]
[assembly: Wireup(typeof(AssemblyWireup))]

namespace FlitBit.Dto
{
	/// <summary>
	///   Wires up this assembly.
	/// </summary>
	public sealed class AssemblyWireup : IWireupCommand
	{
		#region IWireupCommand Members

		/// <summary>
		///   Wires up this assembly.
		/// </summary>
		/// <param name="coordinator"></param>
		public void Execute(IWireupCoordinator coordinator)
		{
		}

		#endregion
	}

	/// <summary>
	///   Wires this module.
	/// </summary>
	public class HookWirupCoordinatorTask : WireupTaskAttribute
	{
		/// <summary>
		///   Creates a new instance.
		/// </summary>
		public HookWirupCoordinatorTask()
			: base(WireupPhase.BeforeDependencies) { }

		/// <summary>
		///   Performs wireup.
		/// </summary>
		/// <param name="coordinator"></param>
		protected override void PerformTask(IWireupCoordinator coordinator)
		{
			// Attach the root container as a wireup observer...
			coordinator.RegisterObserver(DTOWireupObserver.Observer);
		}
	}
}