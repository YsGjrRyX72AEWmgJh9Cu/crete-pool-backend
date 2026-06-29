import os
import re
import requests

from dotenv import load_dotenv
from sqlalchemy import create_engine, text

load_dotenv()

engine = create_engine(
    os.getenv("DATABASE_URL")
)

all_tournaments = set()

with engine.connect() as conn:

    players = conn.execute(
        text("""
            SELECT cuescore_id
            FROM players
            WHERE cuescore_id IS NOT NULL
            LIMIT 100
        """)
    ).fetchall()

total_players = len(players)

for index, player in enumerate(players, start=1):

    print(
        f"Processing {index}/{total_players} "
        f"(player {player.cuescore_id})"
    )

    player_id = player.cuescore_id

    try:

        player_url = (
            f"https://cuescore.com/player/player/{player_id}"
        )

        response = requests.get(player_url)

        html = response.text

        match_ids = set(
            re.findall(
                r'matchId=(\d+)',
                html
            )
        )

        for match_id in match_ids:

            match_url = (
                f"https://cuescore.com/match/?matchId={match_id}"
            )

            match_html = requests.get(
                match_url
            ).text

            match = re.search(
                r'tournament-id="(\d+)"',
                match_html
            )

            if match:

                all_tournaments.add(
                    match.group(1)
                )

    except Exception:

        pass

print()
print(
    f"Found {len(all_tournaments)} tournaments"
)
print()

missing_count = 0
imported_count = 0

with engine.connect() as conn:

    for tournament_id in sorted(all_tournaments):

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

        try:

            tournament = requests.get(
                f"https://api.cuescore.com/tournament/?lang=en&id={tournament_id}"
            ).json()

            name = tournament.get(
                "name",
                "Unknown"
            )

        except Exception:

            name = "Unknown"

        if existing:

            imported_count += 1

        else:

            missing_count += 1

            print(
                f"✗ {tournament_id} - {name}"
            )

print()
print(f"Imported: {imported_count}")
print(f"Missing: {missing_count}")
               
