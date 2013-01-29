#region COPYRIGHT© 2009-2013 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using FlitBit.Dto.SPI;

namespace FlitBit.Dto
{
	public abstract partial class DataTransferObject<T> : IDataTransferObjectSPI<T>
	{
		/// <summary>
		/// Copies state from another instance.
		/// </summary>
		/// <param name="other">the other instance</param>
		public void CopyState(DataTransferObject<T> other)
		{
			PerformCopyState(other);	
		}

		/// <summary>
		/// Copies state from another instance.
		/// </summary>
		/// <param name="other">the other instance.</param>
		public void CopySource(T other)
		{
			PerformCopySource(other);
		}

		/// <summary>
		/// Overriden by subclasses to copy the state from another instance.
		/// </summary>
		/// <param name="other">the other instance</param>
		protected abstract void PerformCopySource(T other);

		/// <summary>
		/// Overriden by subclasses to copy the state from another instance.
		/// </summary>
		/// <param name="other">the other instance</param>
		protected abstract void PerformCopyState(DataTransferObject<T> other);
						
	}

	namespace SPI
	{
		/// <summary>
		/// DTO Service Provider Interface; used internally by the framework.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		public interface IDataTransferObjectSPI<T>
		{
			/// <summary>
			/// Copies state from another instance.
			/// </summary>
			/// <param name="other">the other instance</param>
			void CopyState(DataTransferObject<T> other);
			/// <summary>
			/// Copies state from another instance.
			/// </summary>
			/// <param name="other">the other instance.</param>
			void CopySource(T other);
		}
	}
}
