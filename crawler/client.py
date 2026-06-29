"""
CueScore HTTP client.

All communication with CueScore goes through this class.
"""

import time
import requests

from .settings import (
    CUESCORE_BASE_URL,
    CUESCORE_API_URL,
    REQUEST_DELAY,
    REQUEST_TIMEOUT,
)

class CueScoreClient:

    def __init__(self):

        self.session = requests.Session()

        self.session.headers.update(
            {
                "User-Agent": (
                    "HellenicAmericanPoolHistory/1.0"
                )
            }
        )

    def _get(self, url):

        response = self.session.get(
            url,
            timeout=REQUEST_TIMEOUT,
        )

        time.sleep(REQUEST_DELAY)

        response.raise_for_status()

        return response

    def get_player(self, player_id):

        url = (
            f"{CUESCORE_BASE_URL}/player/player/{player_id}"
        )

        response = self._get(url)

        return response.text

    def get_player_matches(self, player_id):

        url = (
            f"{CUESCORE_BASE_URL}/player/player/{player_id}"
        )

        response = self._get(url)
        
        return response.text

    def get_match(self, match_id):

        url = (
            f"{CUESCORE_BASE_URL}/match/?matchId={match_id}"
        )

        response = self._get(url)
        
        return response.text

    def get_tournament(self, tournament_id):

        url = (
            f"{CUESCORE_API_URL}/tournament/?lang=en&id={tournament_id}"
        )

        response = self._get(url)

        return response.json()
