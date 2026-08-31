import sys
import re
import os
import requests
import subprocess

from dotenv import load_dotenv
from sqlalchemy import create_engine, text

load_dotenv()

engine = create_engine(
    os.getenv("DATABASE_URL")
)

if len(sys.argv) < 2:
    print(
        "Usage: python3 discover_tournaments.py <player_id> [--import]"
    )
    sys.exit(1)

player_id = sys.argv[1]

auto_import = (
    len(sys.argv) > 2
    and sys.argv[2] == "--import"
)

player_url = f"https://cuescore.com/player/player/{player_id}"

response = requests.get(player_url)

if response.status_code != 200:
    print("Could not load player page")
    sys.exit(1)

html = response.text

match_ids = set(
    re.findall(r'matchId=(\d+)', html)
)

print(f"Matches found: {len(match_ids)}")

tournament_ids = set()

for match_id in sorted(match_ids):

    match_url = (
        f"https://cuescore.com/match/?matchId={match_id}"
    )

    match_response = requests.get(match_url)

    match_html = match_response.text

    match = re.search(
        r'tournament-id="(\d+)"',
        match_html
    )

    if match:
        tournament_ids.add(
            match.group(1)
        )

print()
print("Tournaments found:")
print()

for tournament_id in sorted(tournament_ids):

    try:

        tournament = requests.get(
            f"https://api.cuescore.com/tournament/?lang=en&id={tournament_id}"
        ).json()

        with engine.connect() as conn:

            existing = conn.execute(
                text("""
                    SELECT id
                    FROM tournaments
                    WHERE cuescore_id = :cuescore_id
                """),
                {
                    "cuescore_id": int(tournament_id)
                }
            ).fetchone()

        status = (
            "✓ Imported"
            if existing
            else "✗ Missing"
        )

        print(
            f"{tournament_id} - "
            f"{tournament['name']} - "
            f"{status}"
        )

        if auto_import and not existing:

            print(
                f"Importing {tournament_id}..."
            )

            subprocess.run(
                [
                    "python3",
                    "import_cuescore.py",
                    tournament_id
                ]
            )

    except Exception:

        print(tournament_id)
