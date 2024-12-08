# EasySkipList

`EasySkipList` is a high-performance, thread-safe data structure designed to efficiently handle ordered collections in both single-threaded and multi-threaded applications. It offers fast search, insertion, and deletion operations with logarithmic time complexity. This package includes two main classes: `SkipList` and `ConcurrentSkipList`.

---

## Table of Contents

1. [Overview](#1-overview)
2. [SkipList](#2-skiplist)
3. [ConcurrentSkipList](#3-concurrentskiplist)
4. [Key Features](#4-key-features)
5. [Installation](#5-installation)
6. [Performance Considerations](#6-performance-considerations)
7. [Known Limitations](#7-known-limitations)
8. [Contributing](#8-contributing)
9. [Frequently Asked Questions (FAQ)](#9-frequently-asked-questions-faq)
10. [License](#10-license)

---

## 1. Overview

A `SkipList` is a probabilistic data structure that allows for fast search, insertion, and deletion of elements in an ordered collection. It improves upon traditional linked lists by maintaining multiple levels of linked lists that provide shortcuts to make the search process faster, with an average time complexity of O(log N) for basic operations.

The `ConcurrentSkipList` class extends `SkipList` with thread-safety features, making it suitable for multi-threaded environments where multiple threads may be accessing and modifying the list concurrently.

---

## 2. SkipList

### Overview

`SkipList` is an ordered collection that allows fast searching, inserting, and deleting of elements. It provides an alternative to balanced trees with simpler code and offers similar average performance.

### Key Operations

- **Insert**: Add a new key-value pair to the list.
- **Search**: Find a value associated with a given key.
- **Delete**: Remove a key-value pair from the list.

### Code Example

```csharp
var skipList = new SkipList<int, string>();
skipList.Add(1, "One");
skipList.Add(2, "Two");

if (skipList.TryGetValue(1, out var value))
{
    Console.WriteLine(value);  // Output: "One"
}
skipList.Remove(2);
```

3\. ConcurrentSkipList
----------------------

### Overview

`ConcurrentSkipList` is an extension of `SkipList` with added thread-safety for use in multi-threaded applications. It uses a `ReaderWriterLockSlim` to allow multiple threads to read concurrently while ensuring that write operations are exclusive.

### Key Operations

*   **Add**: Adds a new key-value pair in a thread-safe manner.
*   **TryGetValue**: Safely retrieves a value associated with the key.
*   **Remove**: Safely removes a key-value pair from the list.

### Code Example

```csharp
var concurrentSkipList = new ConcurrentSkipList<int, string>(); concurrentSkipList.Add(1, "One"); concurrentSkipList.Add(2, "Two");  if (concurrentSkipList.TryGetValue(1, out var value)) {     Console.WriteLine(value);  // Output: "One" } concurrentSkipList.Remove(2);`
```
* * *

4\. Key Features
----------------

*   **Fast Performance**: Both `SkipList` and `ConcurrentSkipList` offer O(log N) time complexity for search, insert, and delete operations, making them highly efficient even for large collections.
*   **Thread-Safety**: `ConcurrentSkipList` provides thread-safety, allowing multiple threads to perform operations concurrently without data corruption.
*   **Easy to Use**: The APIs for `SkipList` and `ConcurrentSkipList` are simple and intuitive, making them easy to integrate into your application.
*   **Memory Efficient**: Although `SkipList` maintains multiple levels of linked lists, it is more memory-efficient than other balanced tree data structures like AVL or Red-Black trees.

* * *

5\. Installation
----------------

You can install the `EasySkipList` package from NuGet Package Manager:

### NuGet Package Manager

bash

Copy code

`Install-Package SkipList`

### .NET CLI

You can also use the .NET CLI to install the package:

bash

Copy code

`dotnet add package SkipList`

Once installed, you can start using the `SkipList` and `ConcurrentSkipList` classes in your application.

* * *

6\. Performance Considerations
------------------------------

The `SkipList` and `ConcurrentSkipList` classes are designed for high performance in both single-threaded and multi-threaded environments. The primary trade-off for their efficiency lies in the balancing act between memory usage and lookup speed.

*   **Single-threaded Performance**:  
	The `SkipList` operates with a time complexity of O(log N) for search, insert, and delete operations. This allows for fast access to elements even in large collections.

*   **Thread-safety for ConcurrentSkipList**:  
	The `ConcurrentSkipList` uses a `ReaderWriterLockSlim`, which allows multiple threads to perform read operations concurrently while ensuring that write operations are exclusive. This provides good concurrency performance, especially in scenarios with many read-heavy operations.


### Scalability

Both the `SkipList` and `ConcurrentSkipList` scale well with the number of elements in the list. As the number of elements increases, the complexity remains O(log N) for all main operations, which ensures the list can handle large collections effectively.

* * *

7\. Known Limitations
---------------------

*   **Memory Usage**:  
	While `SkipList` provides fast access times, it requires additional memory compared to a basic array or linked list. This is due to the multiple levels of linked lists that maintain order and allow efficient searching.

*   **ConcurrentSkipList Performance**:  
	For workloads that involve frequent writes, the use of `ReaderWriterLockSlim` can create contention when multiple threads are trying to write at the same time. For read-heavy workloads, `ConcurrentSkipList` performs well, but for write-heavy workloads, you may want to consider other concurrency mechanisms depending on the application’s needs.


* * *

8\. Contributing
----------------

We welcome contributions to the `EasySkipList` project! If you'd like to contribute, please fork the repository and submit a pull request with your proposed changes.

### Steps to Contribute

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Implement your changes, ensuring that all unit tests pass.
4.  Submit a pull request for review.

* * *

9\. Frequently Asked Questions (FAQ)
------------------------------------

**Q1: What is the primary difference between `SkipList` and `ConcurrentSkipList`?**

*   `SkipList` is not thread-safe, and it is used for single-threaded scenarios where thread-safety is not required.
*   `ConcurrentSkipList` adds thread-safety, making it suitable for multi-threaded applications where multiple threads may be adding, removing, or reading from the list concurrently.

**Q2: Can I use `SkipList` in a highly concurrent environment?**

*   While `SkipList` offers great performance for single-threaded use, it is not thread-safe. For concurrent scenarios, use `ConcurrentSkipList`, which is designed to handle multiple threads accessing the list at once.

**Q3: What is the best use case for a `SkipList`?**

*   `SkipList` is ideal for situations where you need to maintain an ordered collection and perform frequent insertions, deletions, and lookups. It offers efficient average-case performance with O(log N) complexity for these operations.

**Q4: Can `ConcurrentSkipList` handle thousands of concurrent operations?**

*   Yes, `ConcurrentSkipList` can handle a large number of concurrent read operations very well. However, for write-heavy workloads, consider the potential performance impact due to lock contention when multiple threads are trying to write simultaneously.

* * *

10\. License
------------

This project is licensed under the MIT License. See the LICENSE file for more details.  
Feel free to modify or use it as part of your project
