namespace Snippets4.Persistence.Custom
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
        	#region PersistTimeoutsInterfaces [4.4,5.0]

        	public interface IPersistTimeouts
		{
			/// <summary>
			/// Retrieves the next range of timeouts that are due.
			/// </summary>
			/// <param name="startSlice">The time where to start retrieving the next slice, the slice should exclude this date.</param>
			/// <param name="nextTimeToRunQuery">Returns the next time we should query again.</param>
			/// <returns>Returns the next range of timeouts that are due.</returns>
			IEnumerable<Tuple<string, DateTime>> GetNextChunk(DateTime startSlice, out DateTime nextTimeToRunQuery);
			
			/// <summary>
			/// Adds a new timeout.
			/// </summary>
			/// <param name="timeout">Timeout data.</param>
			void Add(TimeoutData timeout);
			
			/// <summary>
			/// Removes the timeout if it hasn't been previously removed.
			/// </summary>
			/// <param name="timeoutId">The timeout id to remove.</param>
			/// <param name="timeoutData">The timeout data of the removed timeout.</param>
			/// <returns><c>true</c> it the timeout was successfully removed.</returns>
			bool TryRemove(string timeoutId, out TimeoutData timeoutData);
			
			/// <summary>
			/// Removes the time by saga id.
			/// </summary>
			/// <param name="sagaId">The saga id of the timeouts to remove.</param>
			void RemoveTimeoutBy(Guid sagaId);
		}

		public interface IPersistTimeoutsV2
		{
			/// <summary>
			/// Reads timeout data.
			/// </summary>
			/// <param name="timeoutId">The timeout id to read.</param>
			/// <returns><see cref="TimeoutData"/> of the timeout if it was found. <c>null</c> otherwise.</returns>
			TimeoutData Peek(string timeoutId);
				
			/// <summary>
			/// Removes the timeout if it hasn't been previously removed.
			/// </summary>
			/// <param name="timeoutId">The timeout id to remove.</param>
			/// <returns><c>true</c> if the timeout has been successfully removed or <c>false</c> if there was no timeout to remove.</returns>
			bool TryRemove(string timeoutId);
		}
		
        	#endregion
        }
    }
}
