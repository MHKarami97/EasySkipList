using System.Security.Cryptography;

namespace EasySkipList;

/// <summary>
/// A SkipList is a probabilistic data structure that allows fast search, insertion, and deletion operations.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must be comparable.</typeparam>
/// <typeparam name="TValue">The type of the value associated with the key.</typeparam>
public class SkipList<TKey, TValue> where TKey : IComparable<TKey>
{
	/// <summary>
	/// Represents a single node in the SkipList.
	/// </summary>
	private sealed class Node
	{
		/// <summary>
		/// Gets the key of the node.
		/// </summary>
		public TKey Key { get; }

		/// <summary>
		/// Gets or sets the value of the node.
		/// </summary>
		public TValue Value { get; set; }

		/// <summary>
		/// Gets the array of forward references to other nodes.
		/// </summary>
		public Node[] Forward { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Node"/> class.
		/// </summary>
		/// <param name="key">The key of the node.</param>
		/// <param name="value">The value of the node.</param>
		/// <param name="level">The level of the node in the SkipList.</param>
		public Node(TKey key, TValue value, int level)
		{
			Key = key;
			Value = value;
			Forward = new Node[level];
		}
	}

	private readonly int _maxLevel;
	private readonly float _probability;
	private readonly Node _head;
	private int _currentLevel;

	/// <summary>
	/// Initializes a new instance of the <see cref="SkipList{TKey, TValue}"/> class.
	/// </summary>
	/// <param name="maxLevel">The maximum level of the SkipList. Default is 16.</param>
	/// <param name="probability">The probability for level promotion. Default is 0.5.</param>
	public SkipList(int maxLevel = 16, float probability = 0.5f)
	{
		if (maxLevel < 1)
		{
			throw new ArgumentException("Max level must be greater than 0.", nameof(maxLevel));
		}

		if (probability is <= 0 or >= 1)
		{
			throw new ArgumentException("Probability must be between 0 and 1.", nameof(probability));
		}

		_maxLevel = maxLevel;
		_probability = probability;
		_head = new Node(default!, default!, maxLevel);
		_currentLevel = 1;
	}

	/// <summary>
	/// Tries to get the value associated with the specified key.
	/// </summary>
	/// <param name="key">The key to locate in the SkipList.</param>
	/// <param name="value">The value associated with the key, if found.</param>
	/// <returns><c>true</c> if the key was found; otherwise, <c>false</c>.</returns>
	public bool TryGetValue(TKey key, out TValue value)
	{
		var current = _head;

		for (var i = _currentLevel - 1; i >= 0; i--)
		{
			while (current.Forward[i] != null && current.Forward[i].Key.CompareTo(key) < 0)
			{
				current = current.Forward[i];
			}
		}

		current = current.Forward[0];
		if (current != null && current.Key.CompareTo(key) == 0)
		{
			value = current.Value;
			return true;
		}

		value = default!;
		return false;
	}

	/// <summary>
	/// Adds a new key-value pair to the SkipList.
	/// </summary>
	/// <param name="key">The key to add.</param>
	/// <param name="value">The value to associate with the key.</param>
	public void Add(TKey key, TValue value)
	{
		var update = new Node[_maxLevel];
		var current = _head;

		for (var i = _currentLevel - 1; i >= 0; i--)
		{
			while (current.Forward[i] != null && current.Forward[i].Key.CompareTo(key) < 0)
			{
				current = current.Forward[i];
			}

			update[i] = current;
		}

		current = current.Forward[0];

		if (current != null && current.Key.CompareTo(key) == 0)
		{
			current.Value = value;
			return;
		}

		var newLevel = RandomLevel();
		if (newLevel > _currentLevel)
		{
			for (var i = _currentLevel; i < newLevel; i++)
			{
				update[i] = _head;
			}

			_currentLevel = newLevel;
		}

		var newNode = new Node(key, value, newLevel);
		for (var i = 0; i < newLevel; i++)
		{
			newNode.Forward[i] = update[i].Forward[i];
			update[i].Forward[i] = newNode;
		}
	}

	/// <summary>
	/// Removes a key-value pair from the SkipList.
	/// </summary>
	/// <param name="key">The key to remove.</param>
	/// <returns><c>true</c> if the key was removed; otherwise, <c>false</c>.</returns>
	public bool Remove(TKey key)
	{
		var update = new Node[_maxLevel];
		var current = _head;

		for (var i = _currentLevel - 1; i >= 0; i--)
		{
			while (current.Forward[i] != null && current.Forward[i].Key.CompareTo(key) < 0)
			{
				current = current.Forward[i];
			}

			update[i] = current;
		}

		current = current.Forward[0];

		if (current == null || current.Key.CompareTo(key) != 0)
		{
			return false;
		}

		for (var i = 0; i < _currentLevel; i++)
		{
			if (update[i].Forward[i] != current)
			{
				break;
			}

			update[i].Forward[i] = current.Forward[i];
		}

		while (_currentLevel > 1 && _head.Forward[_currentLevel - 1] == null)
		{
			_currentLevel--;
		}

		return true;
	}

	/// <summary>
	/// Generates a random level for a new node.
	/// </summary>
	/// <returns>An integer representing the level.</returns>
	private int RandomLevel()
	{
		var level = 1;

		while (level < _maxLevel && IsNextLevelPromoted())
		{
			level++;
		}

		return level;
	}

	/// <summary>
	/// Determines whether to promote the current node to the next level.
	/// </summary>
	/// <returns><c>true</c> if the node should be promoted; otherwise, <c>false</c>.</returns>
	private bool IsNextLevelPromoted()
	{
#if NET8_0_OR_GREATER
		// Generate a random byte and normalize it between 0 and 1
		var randomByte = RandomNumberGenerator.GetInt32(0, 256);
		var probability = randomByte / 255.0;

		return probability < _probability;
#else
		using var rng = RandomNumberGenerator.Create();

		var randomBytes = new byte[1];
		rng.GetBytes(randomBytes);

		// Normalize byte value to a probability between 0 and 1
		var probability = randomBytes[0] / 255.0;

		return probability < _probability;
#endif
	}
}
