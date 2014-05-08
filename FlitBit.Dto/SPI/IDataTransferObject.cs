#region COPYRIGHT© 2009-2014 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using System;
using System.ComponentModel;
using FlitBit.Core.Collections;

namespace FlitBit.Dto.SPI
{
	/// <summary>
	///   DTO service provider interface.
	/// </summary>
	public interface IDataTransferObject : ICloneable, INotifyPropertyChanged
	{
		/// <summary>
		///   Gets the object's dirty flags.
		/// </summary>
		/// <returns></returns>
		BitVector GetDirtyFlags();

		/// <summary>
		///   Indicates whether the identified member has been updated.
		/// </summary>
		/// <param name="member">the member's name</param>
		/// <returns>
		///   <em>true</em> if the member has been updated; otherwise <em>false</em>.
		/// </returns>
		bool IsDirty(string member);

		/// <summary>
		///   Resets all dirty flags, effectively marking the instance as clean.
		/// </summary>
		void ResetDirtyFlags();
	}
}