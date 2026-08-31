import os
import requests

from dotenv import load_dotenv
from sqlalchemy import create_engine, text

load_dotenv()

engine = create_engine(os.getenv("DATABASE_URL"))

TOURNAMENT_ID = 83713144
DATABASE_TOURNAMENT_ID = 6

data = requests.get(
    f"https://api.cuescore.com/tournament/?lang=en&id={TOURNAMENT_ID}"
).json()

inserted = 0

with engine.connect() as conn:

    for match in data["matches"]:

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
                    status
                )
                VALUES (
                    :tournament_id,
                    :player_a_id,
                    :player_b_id,
                    :score_a,
                    :score_b,
                    :winner_id,
                    :round_name,
                    'completed'
                )
            """),
            {
                "tournament_id": DATABASE_TOURNAMENT_ID,
                "player_a_id": player_a_row.id,
                "player_b_id": player_b_row.id,
                "score_a": score_a,
                "score_b": score_b,
                "winner_id": winner_id,
                "round_name": match["roundName"]
            }
        )

        inserted += 1

    conn.commit()

print(f"Matches imported: {inserted}")
