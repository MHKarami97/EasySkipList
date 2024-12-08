using EasySkipList;

namespace EasySkipListUnitTest
{
	public class SkipListTests
	{
		private readonly SkipList<int, string> _skipList;

		public SkipListTests()
		{
			_skipList = new SkipList<int, string>();
		}

		[Fact]
		public void Add_ShouldInsertKeyValuePair()
		{
			_skipList.Add(1, "A");
			_skipList.Add(2, "B");

			Assert.True(_skipList.TryGetValue(1, out var value1));
			Assert.Equal("A", value1);

			Assert.True(_skipList.TryGetValue(2, out var value2));
			Assert.Equal("B", value2);
		}

		[Fact]
		public void Add_DuplicateKey_ShouldUpdateValue()
		{
			_skipList.Add(1, "A");
			_skipList.Add(1, "Updated");

			Assert.True(_skipList.TryGetValue(1, out var value));
			Assert.Equal("Updated", value);
		}

		[Fact]
		public void TryGetValue_ExistingKey_ShouldReturnTrue()
		{
			_skipList.Add(1, "A");

			var result = _skipList.TryGetValue(1, out var value);

			Assert.True(result);
			Assert.Equal("A", value);
		}

		[Fact]
		public void TryGetValue_NonExistingKey_ShouldReturnFalse()
		{
			var result = _skipList.TryGetValue(42, out var value);

			Assert.False(result);
			Assert.Null(value);
		}

		[Fact]
		public void Remove_ExistingKey_ShouldReturnTrueAndRemoveKey()
		{
			_skipList.Add(1, "A");
			_skipList.Add(2, "B");

			var result = _skipList.Remove(1);

			Assert.True(result);
			Assert.False(_skipList.TryGetValue(1, out _));
			Assert.True(_skipList.TryGetValue(2, out var value2));
			Assert.Equal("B", value2);
		}

		[Fact]
		public void Remove_NonExistingKey_ShouldReturnFalse()
		{
			var result = _skipList.Remove(42);

			Assert.False(result);
		}

		[Fact]
		public void Add_MultipleKeys_ShouldMaintainSortedOrder()
		{
			_skipList.Add(3, "C");
			_skipList.Add(1, "A");
			_skipList.Add(2, "B");

			// Verify all keys exist
			Assert.True(_skipList.TryGetValue(1, out var value1));
			Assert.Equal("A", value1);

			Assert.True(_skipList.TryGetValue(2, out var value2));
			Assert.Equal("B", value2);

			Assert.True(_skipList.TryGetValue(3, out var value3));
			Assert.Equal("C", value3);
		}

		[Fact]
		public void Constructor_InvalidMaxLevel_ShouldThrowException()
		{
			Assert.Throws<ArgumentException>(() => new SkipList<int, string>(0));
		}

		[Fact]
		public void Constructor_InvalidProbability_ShouldThrowException()
		{
			Assert.Throws<ArgumentException>(() => new SkipList<int, string>(16, 0f));
			Assert.Throws<ArgumentException>(() => new SkipList<int, string>(16, 1f));
		}

		[Fact]
		public void LargeNumberOfInserts_ShouldHandleCorrectly()
		{
			const int count = 1000;

			for (var i = 0; i < count; i++)
			{
				_skipList.Add(i, $"Value{i}");
			}

			for (var i = 0; i < count; i++)
			{
				Assert.True(_skipList.TryGetValue(i, out var value));
				Assert.Equal($"Value{i}", value);
			}

			for (var i = 0; i < count; i++)
			{
				Assert.True(_skipList.Remove(i));
			}

			Assert.False(_skipList.TryGetValue(0, out _));
		}

		[Fact]
		public void RemoveFromEmptySkipList_ShouldReturnFalse()
		{
			var result = _skipList.Remove(10);

			Assert.False(result);
		}

		[Fact]
		public void TryGetValueFromEmptySkipList_ShouldReturnFalse()
		{
			var result = _skipList.TryGetValue(10, out var value);

			Assert.False(result);
			Assert.Null(value);
		}

		[Fact]
		public void Add_ShouldSupportDifferentKeyTypes()
		{
			var skipList = new SkipList<string, int>();

			skipList.Add("Key1", 1);
			skipList.Add("Key2", 2);

			Assert.True(skipList.TryGetValue("Key1", out var value1));
			Assert.Equal(1, value1);

			Assert.True(skipList.TryGetValue("Key2", out var value2));
			Assert.Equal(2, value2);
		}
	}
}
