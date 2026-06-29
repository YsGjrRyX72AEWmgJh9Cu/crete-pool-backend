"""
Queue component.

Responsible for managing work to be processed.
"""

from collections import deque

class Queue:

    def __init__(self):

        self._queue = deque()

    def add(self, job):

        self._queue.append(
            job
        )

    def get(self):

        return self._queue.popleft()

    def empty(self):

        return len(self._queue) == 0
