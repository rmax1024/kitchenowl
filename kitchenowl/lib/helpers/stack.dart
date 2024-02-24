class Stack<T> {
  List<T> _stack = [];
  final int? limit;

  Stack({this.limit = null});

  // Check if the stack is empty
  bool isEmpty() {
    return _stack.isEmpty;
  }

  // Push an element onto the stack
  void push(T element) {
    _stack.add(element);
    if (limit != null && _stack.length > limit!) {
      _stack.removeAt(0);
    }
  }

  // Pop an element from the stack
  T pop() {
    if (isEmpty()) {
      throw Exception('Stack is empty');
    }
    return _stack.removeLast();
  }

  // Peek at the top element of the stack without removing it
  T peek() {
    if (isEmpty()) {
      throw Exception('Stack is empty');
    }
    return _stack.last;
  }

  // Get the size of the stack
  int size() {
    return _stack.length;
  }

  // Clear the stack
  void clear() {
    _stack.clear();
  }
}