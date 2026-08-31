"""
Discovery component.

Responsible for discovering new entities from CueScore.
"""

from bs4 import BeautifulSoup

class Discover:

    def __init__(self, client):

        self.client = client

    def discover_matches(self, player_id):

        html = self.client.get_player_matches(
            player_id
        )

        soup = BeautifulSoup(
            html,
            "html.parser"
        )

        links = soup.find_all(
            "a",
            href=True
        )

        match_ids = set()

        for link in links:

            href = link["href"]

            if "matchId=" not in href:
                continue

            match_id = int(
                href.split("matchId=")[1]
            )

            match_ids.add(
                match_id
            )

        return match_ids
