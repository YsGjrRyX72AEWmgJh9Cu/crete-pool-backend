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

inserted = 0
skipped = 0

with engine.connect() as conn:
    tournament_row = conn.execute(
        text("""
            SELECT id
            FROM tournaments
            WHERE cuescore_id = :cuescore_id
        """),
        {
            "cuescore_id": TOURNAMENT_ID
        }
    ).fetchone()

    if not tournament_row:
        print("Tournament not found")
        exit()

    database_tournament_id = tournament_row.id

    for match in data["matches"]:
        existing_match = conn.execute(
            text("""
                SELECT id
                FROM tournament_matches
                WHERE cuescore_match_id = :cuescore_match_id
            """),
            {
                "cuescore_match_id": match["matchId"]
            }
        ).fetchone()

        if existing_match:

            skipped += 1
            continue
        player_a = match.get("playerA")
        player_b = match.get("playerB")

        if not player_a or not player_b:
            continue

        player_a_row = conn.execute(
            text("""
                SELECT id
                FROM players
                WHERE cuescore_id = :cuescore_id
            """),
            {
                "cuescore_id": player_a["playerId"]
            }
        ).fetchone()

        player_b_row = conn.execute(
            text("""
                SELECT id
                FROM players
                WHERE cuescore_id = :cuescore_id
            """),
            {
                "cuescore_id": player_b["playerId"]
            }
        ).fetchone()

        if not player_a_row or not player_b_row:
            continue

        score_a = match.get("scoreA", 0)
        score_b = match.get("scoreB", 0)

        winner_id = (
            player_a_row.id
            if score_a > score_b
            else player_b_row.id
        )

        conn.execute(
            text("""
                INSERT INTO tournament_matches (
                    tournament_id,
                    player_a_id,
                    player_b_id,
                    score_a,
                    score_b,
                    winner_id,
                    round_name,
                    status,
                    cuescore_match_id
                )
                VALUES (
                    :tournament_id,
                    :player_a_id,
                    :player_b_id,
                    :score_a,
                    :score_b,
                    :winner_id,
                    :round_name,
                    'completed',
                    :cuescore_match_id
                )
            """),
            {
                "tournament_id": database_tournament_id,
                "player_a_id": player_a_row.id,
                "player_b_id": player_b_row.id,
                "score_a": score_a,
                "score_b": score_b,
                "winner_id": winner_id,
                "round_name": match["roundName"],
                "cuescore_match_id": match["matchId"]
            }
        )

        inserted += 1

    conn.commit()

print(f"Matches imported: {inserted}")
print(f"Matches skipped: {skipped}")
