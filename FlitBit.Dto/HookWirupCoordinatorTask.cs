#region COPYRIGHT© 2009-2014 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using FlitBit.Core;
using FlitBit.Dto;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using FlitBit.Wireup.Recording;

[assembly: HookWirupCoordinatorTask]

namespace FlitBit.Dto
{
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