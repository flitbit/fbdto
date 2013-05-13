using FlitBit.Core;
using FlitBit.Dto;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using FlitBit.Wireup.Recording;
using AssemblyWireup = FlitBit.Wireup.AssemblyWireup;

[assembly: HookWirupCoordinatorTask]
[assembly: WireupDependency(typeof(AssemblyWireup))]

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
		{}

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
			: base(WireupPhase.BeforeDependencies)
		{}

		/// <summary>
		///   Performs wireup.
		/// </summary>
		/// <param name="coordinator"></param>
		/// <param name="context"></param>
		protected override void PerformTask(IWireupCoordinator coordinator, WireupContext context)
		{
			context.Sequence.Push(string.Concat("Registering wireup observer: ",
																					typeof(HookWirupCoordinatorTask).GetReadableFullName()));
			// Attach the root container as a wireup observer...
			coordinator.RegisterObserver(DTOWireupObserver.Observer);
		}
	}
}