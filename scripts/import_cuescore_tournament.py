import os
import requests

from dotenv import load_dotenv
from sqlalchemy import create_engine, text

load_dotenv()

engine = create_engine(os.getenv("DATABASE_URL"))

import sys

TOURNAMENT_ID = int(sys.argv[1])

data = requests.get(
    f"https://api.cuescore.com/tournament/?lang=en&id={TOURNAMENT_ID}"
).json()

with engine.connect() as conn:

    existing = conn.execute(
        text("""
            SELECT id
            FROM tournaments
            WHERE cuescore_id = :cuescore_id
        """),
        {
            "cuescore_id": data["tournamentId"]
        }
    ).fetchone()

    if existing:

        print("Tournament already exists")

    else:

        conn.execute(
            text("""
                INSERT INTO tournaments (
                    name,
                    game_type,
                    race_to,
                    status,
                    cuescore_id
                )
                VALUES (
                    :name,
                    :game_type,
                    :race_to,
                    :status,
                    :cuescore_id
                )
            """),
            {
                "name": data["name"],
                "game_type": data["discipline"],
                "race_to": data["defaultRaceTo"],
                "status": data["status"],
                "cuescore_id": data["tournamentId"]
            }
        )

        conn.commit()

        print("Tournament imported")
