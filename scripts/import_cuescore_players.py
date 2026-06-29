import os
import sys
import requests

from dotenv import load_dotenv
from sqlalchemy import create_engine, text

load_dotenv()

engine = create_engine(os.getenv("DATABASE_URL"))

TOURNAMENT_ID = int(sys.argv[1])

data = requests.get(
    f"https://api.cuescore.com/tournament/?lang=en&id={TOURNAMENT_ID}"
).json()

players = {}

for match in data["matches"]:

    if match.get("playerA"):
        players[
            match["playerA"]["playerId"]
        ] = match["playerA"]["name"]

    if match.get("playerB"):
        players[
            match["playerB"]["playerId"]
        ] = match["playerB"]["name"]

inserted = 0
updated = 0

with engine.connect() as conn:

    for cuescore_id, player_name in players.items():

        existing = conn.execute(
            text("""
                SELECT id
                FROM players
                WHERE full_name = :full_name
            """),
            {
                "full_name": player_name
            }
        ).fetchone()

        if existing:

            conn.execute(
                text("""
                    UPDATE players
                    SET cuescore_id = :cuescore_id
                    WHERE id = :player_id
                """),
                {
                    "cuescore_id": cuescore_id,
                    "player_id": existing.id
                }
            )

            updated += 1

        else:

            conn.execute(
                text("""
                    INSERT INTO players (
                        full_name,
                        city,
                        category,
                        current_rating,
                        wins,
                        losses,
                        matches_played,
                        cuescore_id
                    )
                    VALUES (
                        :full_name,
                        '',
                        'C',
                        500,
                        0,
                        0,
                        0,
                        :cuescore_id
                    )
                """),
                {
                    "full_name": player_name,
                    "cuescore_id": cuescore_id
                }
            )

            inserted += 1

    conn.commit()

print(f"Updated: {updated}")
print(f"Inserted: {inserted}")
