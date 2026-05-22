from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import declarative_base

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