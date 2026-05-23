from sqlalchemy import Column, Integer, String, DateTime
from sqlalchemy.orm import declarative_base
from datetime import datetime

Base = declarative_base()


class Player(Base):
    __tablename__ = "players"

    id = Column(Integer, primary_key=True, index=True)
    full_name = Column(String)
    city = Column(String)
    category = Column(String)

    current_rating = Column(Integer, default=500)

    wins = Column(Integer, default=0)
    losses = Column(Integer, default=0)

    matches_played = Column(Integer, default=0)


class Match(Base):
    __tablename__ = "matches"

    id = Column(Integer, primary_key=True, index=True)

    player_a_id = Column(Integer)
    player_b_id = Column(Integer)

    score_a = Column(Integer)
    score_b = Column(Integer)

    race_to = Column(Integer)

    game_type = Column(String)

    winner_id = Column(Integer)

    played_at = Column(DateTime, default=datetime.utcnow)


class RatingHistory(Base):
    __tablename__ = "rating_history"

    id = Column(Integer, primary_key=True, index=True)

    player_id = Column(Integer)
    match_id = Column(Integer)

    old_rating = Column(Integer)
    new_rating = Column(Integer)

    rating_change = Column(Integer)

    category_before = Column(String)
    category_after = Column(String)
    
    created_at = Column(DateTime, default=datetime.utcnow)

class Tournament(Base):
    __tablename__ = "tournaments"

    id = Column(Integer, primary_key=True, index=True)

    name = Column(String)

    game_type = Column(String)

    race_to = Column(Integer)

    status = Column(String, default="upcoming")

class TournamentPlayer(Base):
    __tablename__ = "tournament_players"

    id = Column(Integer, primary_key=True, index=True)

    tournament_id = Column(Integer)

    player_id = Column(Integer)