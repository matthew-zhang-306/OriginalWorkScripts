using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable {

	List<T> queue;
	int size;

	public PriorityQueue() {
		queue = new List<T>();
		size = 0;
	}

	public void Enqueue(T item) {
		queue.Add(item);
		int index = size;
		size++;

		while (index != 0 && queue[parentOf(index)].CompareTo(queue[index]) > 0) {
			swap(parentOf(index), index);
			index = parentOf(index);
		}
	}

	public T Dequeue() {
		if (IsEmpty)
			throw new IndexOutOfRangeException("Nothing to dequeue!");

		int index = 0;
		size--;

		swap(index, size);
		T item = queue[size];
		queue.RemoveAt(size);

		while (childOf(index, true) < size) {
			int desired = childOf(index, true);
			if (desired + 1 < size && queue[desired+1].CompareTo(queue[desired]) < 0)
				desired++;
			
			if (queue[index].CompareTo(queue[desired]) > 0) {
				swap(index, desired);
				index = desired;
			}
			else
				break;
		}

		return item;
	}

	public T Peek() {
		if (IsEmpty)
			throw new IndexOutOfRangeException("Nothing to peek!");
		return queue[0];
	}

	public int Size { get { return size; }}
	public bool IsEmpty { get { return size == 0; }}

	void swap(int a, int b) {
		if (a != b) {
			T temp = queue[a];
			queue[a] = queue[b];
			queue[b] = temp;
		}
	}

	int childOf(int index, bool isLeft) { return 2 * index + (isLeft ? 1 : 2); }
	int parentOf(int index) { return (index - 1) / 2; }

	bool CheckForHeapStatus() {
		for (int i = 0; i < queue.Count; i++) {
			if (childOf(i, true) < queue.Count && queue[i].CompareTo(queue[childOf(i, true)]) > 0)
				return false;
			if (childOf(i, false) < queue.Count && queue[i].CompareTo(queue[childOf(i, false)]) > 0)
				return false;
		}
		return true;
	}

	public override string ToString() {
		return "[" + String.Join(", ", queue.Select(o => o.ToString()).ToArray()) + "]";
	}
}
