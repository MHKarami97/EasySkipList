using EasySkipList;

namespace EasySkipListUnitTest;

public class ConcurrentSkipListTests
{
	private readonly ConcurrentSkipList<int, string> _concurrentSkipList;

	public ConcurrentSkipListTests()
	{
		_concurrentSkipList = new ConcurrentSkipList<int, string>();
	}

	[Fact]
	public void Add_ShouldAddItem()
	{
		// Act
		_concurrentSkipList.Add(1, "One");

		// Assert
		Assert.True(_concurrentSkipList.TryGetValue(1, out var value));
		Assert.Equal("One", value);
	}

	[Fact]
	public void TryGetValue_ShouldReturnTrue_WhenKeyExists()
	{
		// Arrange
		_concurrentSkipList.Add(2, "Two");

		// Act
		var result = _concurrentSkipList.TryGetValue(2, out var value);

		// Assert
		Assert.True(result);
		Assert.Equal("Two", value);
	}

	[Fact]
	public void TryGetValue_ShouldReturnFalse_WhenKeyDoesNotExist()
	{
		// Act
		var result = _concurrentSkipList.TryGetValue(3, out var value);

		// Assert
		Assert.False(result);
		Assert.Null(value);
	}

	[Fact]
	public void Remove_ShouldReturnTrue_WhenKeyExists()
	{
		// Arrange
		_concurrentSkipList.Add(4, "Four");

		// Act
		var result = _concurrentSkipList.Remove(4);

		// Assert
		Assert.True(result);
		Assert.False(_concurrentSkipList.TryGetValue(4, out _));
	}

	[Fact]
	public void Remove_ShouldReturnFalse_WhenKeyDoesNotExist()
	{
		// Act
		var result = _concurrentSkipList.Remove(5);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task ConcurrentAccess_ShouldHandleMultipleThreadsCorrectly()
	{
		// Arrange
		var tasks = new Task[100];

		for (var i = 0; i < 50; i++)
		{
			var index = i; // Capture loop variable
			tasks[i] = Task.Run(() => _concurrentSkipList.Add(index, $"Value {index}"));
		}

		// Add tasks to retrieve values
		for (var i = 50; i < 75; i++)
		{
			var index = i - 50; // Capture loop variable
			tasks[i] = Task.Run(() =>
			{
				_concurrentSkipList.TryGetValue(index, out _);
			});
		}

		// Add tasks to remove values
		for (var i = 75; i < 100; i++)
		{
			var index = i - 75; // Capture loop variable
			tasks[i] = Task.Run(() => _concurrentSkipList.Remove(index));
		}

		// Act
		await Task.WhenAll(tasks).ConfigureAwait(true);

		// Assert
		for (var i = 0; i < 50; i++)
		{
			var exists = _concurrentSkipList.TryGetValue(i, out _);
			if (exists)
			{
				// If the key still exists, attempt to remove it
				Assert.True(_concurrentSkipList.Remove(i));
			}
			else
			{
				// If the key doesn't exist, removal should return false
				Assert.False(_concurrentSkipList.Remove(i));
			}
		}
	}

	[Fact]
	public void Add_ShouldOverwriteExistingValue()
	{
		// Arrange
		_concurrentSkipList.Add(6, "Six");

		// Act
		_concurrentSkipList.Add(6, "Six Updated");

		// Assert
		Assert.True(_concurrentSkipList.TryGetValue(6, out var value));
		Assert.Equal("Six Updated", value);
	}
}
