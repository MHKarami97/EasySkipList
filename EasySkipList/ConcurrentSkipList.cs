namespace EasySkipList;

/// <summary>
/// A thread-safe implementation of the SkipList data structure.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must be comparable.</typeparam>
/// <typeparam name="TValue">The type of the value associated with the key.</typeparam>
public class ConcurrentSkipList<TKey, TValue> where TKey : IComparable<TKey>
{
	private readonly SkipList<TKey, TValue> _skipList = new();
	private readonly ReaderWriterLockSlim _lock = new();

	/// <summary>
	/// Tries to get the value associated with the specified key in a thread-safe manner.
	/// </summary>
	/// <param name="key">The key to locate in the SkipList.</param>
	/// <param name="value">The value associated with the key, if found.</param>
	/// <returns><c>true</c> if the key was found; otherwise, <c>false</c>.</returns>
	public bool TryGetValue(TKey key, out TValue value)
	{
		_lock.EnterReadLock();
		try
		{
			return _skipList.TryGetValue(key, out value);
		}
		finally
		{
			_lock.ExitReadLock();
		}
	}

	/// <summary>
	/// Adds a new key-value pair to the SkipList in a thread-safe manner.
	/// </summary>
	/// <param name="key">The key to add.</param>
	/// <param name="value">The value to associate with the key.</param>
	public void Add(TKey key, TValue value)
	{
		_lock.EnterWriteLock();
		try
		{
			_skipList.Add(key, value);
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	/// <summary>
	/// Removes a key-value pair from the SkipList in a thread-safe manner.
	/// </summary>
	/// <param name="key">The key to remove.</param>
	/// <returns><c>true</c> if the key was removed; otherwise, <c>false</c>.</returns>
	public bool Remove(TKey key)
	{
		_lock.EnterWriteLock();
		try
		{
			return _skipList.Remove(key);
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}
}
