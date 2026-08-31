import json

from crawler.client import CueScoreClient


client = CueScoreClient()

match = client.get_match(83791531)

with open(
    "tests/fixtures/json/sample_match.json",
    "w",
    encoding="utf-8",
) as file:

    json.dump(
        match,
        file,
        indent=4,
        ensure_ascii=False,
    )

print("Fixture created successfully.")