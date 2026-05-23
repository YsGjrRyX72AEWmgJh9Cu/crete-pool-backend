from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from sqlalchemy import text

from database import engine
from models import Base, Player

Base.metadata.create_all(bind=engine)

from rating import (
    expected_score,
    margin_multiplier,
    calculate_new_rating,
    get_category
)

app = FastAPI()
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

class MatchSubmission(BaseModel):

    player_a_id: int
    player_b_id: int

    score_a: int
    score_b: int

    race_to: int

    game_type: str


class PlayerCreate(BaseModel):

    full_name: str
    city: str
    category: str = "D"

class AdminLogin(BaseModel):
    password: str

class TournamentCreate(BaseModel):

    name: str
    game_type: str
    race_to: int

@app.post("/admin-login")
def admin_login(data: AdminLogin):

    ADMIN_PASSWORD = "cretepooladmin"

    if data.password != ADMIN_PASSWORD:

        raise HTTPException(
            status_code=401,
            detail="Invalid password"
        )

    return {
        "success": True
    }

@app.post("/create-tournament")
def create_tournament(data: TournamentCreate):

    with engine.connect() as connection:

        connection.execute(
            text(
                """
                INSERT INTO tournaments (
                    name,
                    game_type,
                    race_to,
                    status
                )
                VALUES (
                    :name,
                    :game_type,
                    :race_to,
                    :status
                )
                """
            ),
            {
                "name": data.name,
                "game_type": data.game_type,
                "race_to": data.race_to,
                "status": "upcoming"
            }
        )

        connection.commit()

    return {
        "success": True
    }

@app.get("/tournaments")
def get_tournaments():

    with engine.connect() as connection:

        result = connection.execute(
            text(
                """
                SELECT *
                FROM tournaments
                ORDER BY id DESC
                """
            )
        )

        tournaments = []

        for row in result:
            tournaments.append(
                dict(row._mapping)
            )

        return tournaments
        
@app.post("/admin-login")
def admin_login(data: AdminLogin):

    ADMIN_PASSWORD = "cretepooladmin"

    if data.password != ADMIN_PASSWORD:

        raise HTTPException(
            status_code=401,
            detail="Invalid password"
        )

    return {
        "success": True
    }

@app.get("/")
def home():
    return {
        "message": "Crete Pool Rating API is running"
    }


@app.get("/players")
def get_players():

    with engine.connect() as connection:

        result = connection.execute(
            text(
                """
                SELECT *
                FROM players
                ORDER BY current_rating DESC
                """
            )
        )

        players = []

        for row in result:

            player = dict(row._mapping)

            matches_result = connection.execute(
                text(
                    """
                    SELECT *
                    FROM matches
                    WHERE
                        player_a_id = :id
                        OR player_b_id = :id
                    ORDER BY played_at DESC
                 """
                ),
                {"id": player["id"]}
            )

            matches = matches_result.fetchall()

            current_streak = 0
            streak_type = None

            for match in matches:

                won = match.winner_id == player["id"]

                if streak_type is None:

                    streak_type = (
                        "win" if won else "loss"
                    )

                    current_streak = 1

                elif streak_type == "win" and won:

                    current_streak += 1

                elif streak_type == "loss" and not won:

                    current_streak += 1

                else:
                    break

            player["current_streak"] = current_streak
            player["streak_type"] = streak_type

            rating_history_result = connection.execute(
                text(
                    """
                    SELECT new_rating
                    FROM rating_history
                    WHERE player_id = :id
                    ORDER BY created_at DESC
                    LIMIT 2
                    """
                ),
                {"id": player["id"]}
        )

            rating_history = rating_history_result.fetchall()

            movement = "same"

            if len(rating_history) >= 2:

                latest = rating_history[0].new_rating
                previous = rating_history[1].new_rating

                if latest > previous:
                    movement = "up"

                elif latest < previous:
                    movement = "down"

            player["movement"] = movement

            players.append(player)

        return players

@app.get("/simulate-match")
def simulate_match():

    rating_a = 550
    rating_b = 500

    score_a = 9
    score_b = 5

    race_to = 9

    expected_a = expected_score(
        rating_a,
        rating_b
    )

    expected_b = expected_score(
        rating_b,
        rating_a
    )

    margin = margin_multiplier(
        score_a,
        score_b,
        race_to
    )

    new_rating_a = calculate_new_rating(
        rating_a,
        expected_a,
        1,
        margin
    )

    new_rating_b = calculate_new_rating(
        rating_b,
        expected_b,
        0,
        margin
    )

    return {
        "player_a_old": rating_a,
        "player_a_new": new_rating_a,

        "player_b_old": rating_b,
        "player_b_new": new_rating_b,

        "margin_multiplier": margin
    }


@app.post("/submit-match")
def submit_match(match: MatchSubmission):

    if match.player_a_id == match.player_b_id:
        raise HTTPException(
            status_code=400,
            detail="Players must be different"
        )

    if match.score_a < 0 or match.score_b < 0:
        raise HTTPException(
            status_code=400,
            detail="Scores cannot be negative"
        )

    if (
        match.score_a > match.race_to
        or match.score_b > match.race_to
    ):
        raise HTTPException(
            status_code=400,
            detail="Score cannot exceed race_to"
        )

    if match.score_a == match.score_b:
        raise HTTPException(
            status_code=400,
            detail="Matches cannot end in a draw"
        )

    if (
        match.score_a != match.race_to
        and match.score_b != match.race_to
    ):
        raise HTTPException(
            status_code=400,
            detail="One player must reach race_to"
        )

    with engine.connect() as connection:

        player_a_result = connection.execute(
            text(
                "SELECT * FROM players WHERE id = :id"
            ),
            {"id": match.player_a_id}
        )

        player_b_result = connection.execute(
            text(
                "SELECT * FROM players WHERE id = :id"
            ),
            {"id": match.player_b_id}
        )

        player_a = player_a_result.fetchone()
        player_b = player_b_result.fetchone()

        if not player_a or not player_b:
            raise HTTPException(
                status_code=404,
                detail="Player not found"
            )

        rating_a = player_a.current_rating
        rating_b = player_b.current_rating

        expected_a = expected_score(
            rating_a,
            rating_b
        )

        expected_b = expected_score(
            rating_b,
            rating_a
        )

        margin = margin_multiplier(
            match.score_a,
            match.score_b,
            match.race_to
        )

        actual_a = 1 if match.score_a > match.score_b else 0
        actual_b = 1 if match.score_b > match.score_a else 0

        new_rating_a = calculate_new_rating(
            rating_a,
            expected_a,
            actual_a,
            margin
        )

        new_rating_b = calculate_new_rating(
            rating_b,
            expected_b,
            actual_b,
            margin
        )
        new_category_a = get_category(
            new_rating_a
        )

        new_category_b = get_category(
            new_rating_b
        )

        connection.execute(
            text(
                """
                UPDATE players
                SET
                    current_rating = :rating,
                    category = :category
                WHERE id = :id
                """
            ),
            {
                "rating": new_rating_a,
                "category": new_category_a,
                "id": match.player_a_id
            }
        )

        connection.execute(
            text(
                """
                UPDATE players
                SET
                    current_rating = :rating,
                    category = :category
                WHERE id = :id
                """
            ),
            {
                "rating": new_rating_b,
                "category": new_category_b,
                "id": match.player_b_id
            }
        )

        connection.execute(
            text(
                """
                INSERT INTO matches (
                    player_a_id,
                    player_b_id,
                    score_a,
                    score_b,
                    race_to,
                    game_type,
                    winner_id
                )
                VALUES (
                    :player_a_id,
                    :player_b_id,
                    :score_a,
                    :score_b,
                    :race_to,
                    :game_type,
                    :winner_id
                )
                """
            ),
            {
                "player_a_id": match.player_a_id,
                "player_b_id": match.player_b_id,

                "score_a": match.score_a,
                "score_b": match.score_b,

                "race_to": match.race_to,

                "game_type": match.game_type,

                "winner_id":
                    match.player_a_id
                    if match.score_a > match.score_b
                    else match.player_b_id
            }
        )
        winner_id = (
            match.player_a_id
            if match.score_a > match.score_b
            else match.player_b_id
        )

        loser_id = (
            match.player_b_id
            if match.score_a > match.score_b
            else match.player_a_id
        )

        connection.execute(
            text(
                """
                UPDATE players
                SET
                    wins = wins + 1,
                    matches_played = matches_played + 1
                WHERE id = :id
                """
            ),
            {"id": winner_id}
        )

        connection.execute(
            text(
                """
                UPDATE players
                SET
                    losses = losses + 1,
                    matches_played = matches_played + 1
                WHERE id = :id
                """
            ),
            {"id": loser_id}
        )
        player_a_change = (
            new_rating_a - rating_a
        )

        player_b_change = (
            new_rating_b - rating_b
        )

        latest_match = connection.execute(
            text(
                """
                SELECT id
                FROM matches
                ORDER BY id DESC
                LIMIT 1
                """
            )
        ).fetchone()

        match_id = latest_match.id

        connection.execute(
            text(
                """
                INSERT INTO rating_history (
                    player_id,
                    match_id,
                    old_rating,
                    new_rating,
                    rating_change,
                    category_before,
                    category_after
                )
                VALUES (
                    :player_id,
                    :match_id,
                    :old_rating,
                    :new_rating,
                    :rating_change,
                    :category_before,
                    :category_after
                )
                """
            ),
            {
                "player_id": match.player_a_id,
                "match_id": match_id,

                "old_rating": rating_a,
                "new_rating": new_rating_a,

                "rating_change": player_a_change,

                "category_before": player_a.category,
                "category_after": new_category_a
            }
        )

        connection.execute(
            text(
                """
                INSERT INTO rating_history (
                    player_id,
                    match_id,
                    old_rating,
                    new_rating,
                    rating_change,
                    category_before,
                    category_after
                )
                VALUES (
                    :player_id,
                    :match_id,
                    :old_rating,
                    :new_rating,
                    :rating_change,
                    :category_before,
                    :category_after
                )
                """
            ),
            {
                "player_id": match.player_b_id,
                "match_id": match_id,

                "old_rating": rating_b,
                "new_rating": new_rating_b,

                "rating_change": player_b_change,

                "category_before": player_b.category,
                "category_after": new_category_b
            }
        )
        connection.commit()
        return {
            "player_a_old": rating_a,
            "player_a_new": new_rating_a,

            "player_b_old": rating_b,
            "player_b_new": new_rating_b,

            "margin_multiplier": margin
        }

@app.get("/leaderboard")
def leaderboard():

    with engine.connect() as connection:

        result = connection.execute(
            text(
                """
                SELECT
                    id,
                    full_name,
                    city,
                    current_rating,
                    category,
                    wins,
                    losses,
                    matches_played
                FROM players
                ORDER BY current_rating DESC
                """
            )
        )

        players = []

        rank = 1

    for row in result:

        player = dict(row._mapping)

        total_matches = player["matches_played"]

        win_rate = 0

        if total_matches > 0:
            win_rate = round(
                (player["wins"] / total_matches) * 100,
                2
            )

        player["win_rate"] = win_rate
        player["rank"] = rank

        recent_matches = connection.execute(
            text(
                """
                SELECT winner_id
                FROM matches
                WHERE player_a_id = :id
                   OR player_b_id = :id
                ORDER BY played_at DESC
                LIMIT 5
                """
            ),
            {"id": player["id"]}
        ).fetchall()

        recent_form = []

        for match in recent_matches:

            if match.winner_id == player["id"]:
                recent_form.append("W")
            else:
                recent_form.append("L")

        player["recent_form"] = recent_form

        players.append(player)

        rank += 1

    return players

@app.get("/player/{player_id}")
def player_profile(player_id: int):

    with engine.connect() as connection:

        result = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE id = :id
                """
            ),
            {"id": player_id}
        )

        player = result.fetchone()

        if not player:
            raise HTTPException(
                status_code=404,
                detail="Player not found"
            )

        matches_result = connection.execute(
            text(
                """
                SELECT
                    matches.id,

                    player_a.full_name AS player_a_name,
                    player_b.full_name AS player_b_name,

                    matches.score_a,
                    matches.score_b,

                    matches.race_to,
                    matches.game_type,
                    matches.played_at

                FROM matches

                JOIN players AS player_a
                ON matches.player_a_id = player_a.id

                JOIN players AS player_b
                ON matches.player_b_id = player_b.id

                WHERE
                    matches.player_a_id = :id
                    OR matches.player_b_id = :id

                ORDER BY matches.played_at DESC

                LIMIT 5
                """
            ),
            {"id": player_id}
        )

        recent_matches = []

        for row in matches_result:
            recent_matches.append(dict(row._mapping))

        recent_form = []

        for match in recent_matches:

            winner = None

            if match["score_a"] > match["score_b"]:
                winner = "A"
            else:
                winner = "B"

            is_win = False

            if (
                winner == "A"
                and match["player_a_name"] == player.full_name
            ):
                is_win = True

            if (
                winner == "B"
                and match["player_b_name"] == player.full_name
            ):
                is_win = True

            recent_form.append(
                "W" if is_win else "L"
            )

        return {
            "player": dict(player._mapping),
            "recent_matches": recent_matches,
            "recent_form": recent_form
        }

@app.get("/player/{player_id}/matches")
def player_matches(player_id: int):

    with engine.connect() as connection:

        player_result = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE id = :id
                """
            ),
            {"id": player_id}
        )

        player = player_result.fetchone()

        if not player:
            raise HTTPException(
                status_code=404,
                detail="Player not found"
            )

        result = connection.execute(
            text(
                """
                SELECT
                    matches.id,

                    player_a.full_name AS player_a_name,
                    player_b.full_name AS player_b_name,

                    matches.score_a,
                    matches.score_b,

                   matches.race_to,
                   matches.game_type,
                   matches.played_at,

                   matches.winner_id

                FROM matches

                JOIN players AS player_a
                ON matches.player_a_id = player_a.id

                JOIN players AS player_b
                ON matches.player_b_id = player_b.id

                WHERE
                   matches.player_a_id = :id
                   OR matches.player_b_id = :id

                ORDER BY matches.played_at DESC
                """
            ),
            {"id": player_id}
        )

        matches = []

        for row in result:
            matches.append(dict(row._mapping))

        return matches


@app.get("/player/{player_id}/stats")
def player_stats(player_id: int):

    with engine.connect() as connection:

        player_result = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE id = :id
                """
            ),
            {"id": player_id}
        )

        player = player_result.fetchone()

        if not player:
            raise HTTPException(
                status_code=404,
                detail="Player not found"
            )

        total_matches = player.matches_played
        wins = player.wins
        losses = player.losses

        win_rate = 0

        if total_matches > 0:
            win_rate = round(
                (wins / total_matches) * 100,
                2
            )

        matches_result = connection.execute(
            text(
                """
                SELECT *
                FROM matches
                WHERE
                    player_a_id = :id
                    OR player_b_id = :id
                ORDER BY played_at DESC
                """
            ),
            {"id": player_id}
        )

        matches = matches_result.fetchall()

        recent_form = []

        for match in matches[:5]:

            if match.winner_id == player_id:
                recent_form.append("W")

            else:
                recent_form.append("L")

        current_streak = 0
        streak_type = None

        for match in matches:

            won = match.winner_id == player_id

            if streak_type is None:

                streak_type = "win" if won else "loss"
                current_streak = 1

            elif streak_type == "win" and won:

                current_streak += 1

            elif streak_type == "loss" and not won:

                current_streak += 1

            else:
                break

        return {
            "player_id": player.id,
            "full_name": player.full_name,

            "rating": player.current_rating,
            "category": player.category,

            "matches_played": total_matches,

            "wins": wins,
            "losses": losses,

            "win_rate": win_rate,

            "current_streak": current_streak,
            "streak_type": streak_type,
            "recent_form": recent_form
        }

@app.get("/player/{player_id}/rating-history")
def player_rating_history(player_id: int):

    with engine.connect() as connection:

        result = connection.execute(
            text(
                """
                SELECT *
                FROM rating_history
                WHERE player_id = :id
                ORDER BY created_at DESC
                """
            ),
            {"id": player_id}
        )

        history = []

        for row in result:
            history.append(dict(row._mapping))

        return history

@app.get("/player/{player_a_id}/vs/{player_b_id}")
def head_to_head(
    player_a_id: int,
    player_b_id: int
):

    with engine.connect() as connection:

        player_a_result = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE id = :id
                """
            ),
            {"id": player_a_id}
        )

        player_b_result = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE id = :id
                """
            ),
            {"id": player_b_id}
        )

        player_a = player_a_result.fetchone()
        player_b = player_b_result.fetchone()

        if not player_a or not player_b:
            raise HTTPException(
                status_code=404,
                detail="Player not found"
            )

        matches_result = connection.execute(
            text(
                """
                SELECT *
                FROM matches
                WHERE
                    (
                        player_a_id = :player_a_id
                        AND player_b_id = :player_b_id
                    )
                    OR
                    (
                        player_a_id = :player_b_id
                        AND player_b_id = :player_a_id
                    )
                """
            ),
            {
                "player_a_id": player_a_id,
                "player_b_id": player_b_id
            }
        )

        matches = matches_result.fetchall()

        player_a_wins = 0
        player_b_wins = 0

        for match in matches:

            if match.winner_id == player_a_id:
                player_a_wins += 1

            elif match.winner_id == player_b_id:
                player_b_wins += 1

        return {
            "player_a": player_a.full_name,
            "player_b": player_b.full_name,

            "matches_played": len(matches),

            "player_a_wins": player_a_wins,
            "player_b_wins": player_b_wins
        }

@app.post("/create-player")
def create_player(player: PlayerCreate):

    with engine.connect() as connection:

        existing_player = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE full_name = :full_name
                """
            ),
            {
                "full_name": player.full_name
            }
        ).fetchone()

        if existing_player:
            raise HTTPException(
                status_code=400,
                detail="Player already exists"
            )

        connection.execute(
            text(
                """
                INSERT INTO players (
                    full_name,
                    city,
                    current_rating,
                    category,
                    wins,
                    losses,
                    matches_played
                )
                VALUES (
                    :full_name,
                    :city,
                    500,
                    :category,
                    0,
                    0,
                    0
                )
                """
            ),
            {
                "full_name": player.full_name,
                "city": player.city,
                "category": player.category
            }
        )

        connection.commit()

        return {
            "message": "Player created successfully"
        }

@app.delete("/player/{player_id}")
def delete_player(player_id: int):

    with engine.connect() as connection:

        player_result = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE id = :id
                """
            ),
            {"id": player_id}
        )

        player = player_result.fetchone()

        if not player:
            raise HTTPException(
                status_code=404,
                detail="Player not found"
            )

        connection.execute(
            text(
                """
                DELETE FROM rating_history
                WHERE player_id = :id
                """
            ),
            {"id": player_id}
        )

        connection.execute(
            text(
                """
                DELETE FROM matches
                WHERE
                    player_a_id = :id
                    OR player_b_id = :id
                """
            ),
            {"id": player_id}
        )

        connection.execute(
            text(
                """
                DELETE FROM players
                WHERE id = :id
                """
            ),
            {"id": player_id}
        )

        connection.commit()

        return {
            "message": "Player deleted"
        }

@app.put("/player/{player_id}")
def update_player(
    player_id: int,
    player: PlayerCreate
):

    with engine.connect() as connection:

        existing_player = connection.execute(
            text(
                """
                SELECT *
                FROM players
                WHERE id = :id
                """
            ),
            {"id": player_id}
        ).fetchone()

        if not existing_player:

            raise HTTPException(
                status_code=404,
                detail="Player not found"
            )

        connection.execute(
            text(
                """
                UPDATE players
                SET
                    full_name = :full_name,
                    city = :city,
                    category = :category
                WHERE id = :id
                """
            ),
            {
                "id": player_id,
                "full_name": player.full_name,
                "city": player.city,
                "category": player.category
            }
        )

        connection.commit()

        return {
            "message": "Player updated"
        }


@app.get("/matches")
def get_matches():

    with engine.connect() as conn:

        result = conn.execute(text("""
            SELECT *
            FROM matches
            ORDER BY played_at DESC
        """))

        matches = []

        for row in result:

            player_a = conn.execute(
                text("SELECT full_name FROM players WHERE id = :id"),
                {"id": row.player_a_id}
            ).fetchone()

            player_b = conn.execute(
                text("SELECT full_name FROM players WHERE id = :id"),
                {"id": row.player_b_id}
            ).fetchone()

            winner = conn.execute(
                text("SELECT full_name FROM players WHERE id = :id"),
                {"id": row.winner_id}
            ).fetchone()

            matches.append({
                "id": row.id,
                "player_a_name": player_a.full_name,
                "player_b_name": player_b.full_name,
                "score_a": row.score_a,
                "score_b": row.score_b,
                "winner_name": winner.full_name,
                "game_type": row.game_type,
                "played_at": str(row.played_at)
            })

        return matches

@app.delete("/match/{match_id}")
def delete_match(match_id: int):

    with engine.connect() as connection:

        match_result = connection.execute(
            text(
                """
                SELECT *
                FROM matches
                WHERE id = :id
                """
            ),
            {"id": match_id}
        )

        match = match_result.fetchone()

        if not match:
            raise HTTPException(
                status_code=404,
                detail="Match not found"
            )

        connection.execute(
            text(
                """
                DELETE FROM rating_history
                WHERE match_id = :id
                """
            ),
            {"id": match_id}
        )

        connection.execute(
            text(
                """
                DELETE FROM matches
                WHERE id = :id
                """
            ),
            {"id": match_id}
        )

        connection.commit()

        return {
            "message": "Match deleted"
        }