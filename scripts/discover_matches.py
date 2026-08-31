"""
Discover matches demo.

Demonstrates the Discover component.
"""

import sys

from crawler.client import CueScoreClient
from crawler.discover import Discover
from crawler.queue import Queue
from crawler.importer import Importer

client = CueScoreClient()

discover = Discover(client)

queue = Queue()

importer = Importer(
    client
)

if len(sys.argv) != 2:

    print(
        "Usage: python3 scripts/discover_matches.py <player_id>"
    )

    sys.exit(1)

player_id = int(
    sys.argv[1]
)

match_ids = discover.discover_matches(
    player_id
)

print(
    f"Discovered {len(match_ids)} matches"
)

print()

for match_id in sorted(match_ids):
    queue.add(
        {
            "type": "match",
            "id": match_id
        }
    )

    print(match_id)

print()

while not queue.empty():

    job = queue.get()

    response = importer.import_match(
    job["id"]
    )

    print(
        response[:100]
    )
