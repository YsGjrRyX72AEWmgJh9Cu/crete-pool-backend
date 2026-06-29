"""
CueScore Match Parser.

Responsible for transforming CueScore match HTML
into structured Python data.

The parser does NOT communicate with the database.
"""

from bs4 import BeautifulSoup


class MatchParser:
    """
    Parses a CueScore match page.
    """

    def parse(self, html: str) -> dict:
        """
        Parse a CueScore match page.

        Args:
            html: Raw HTML returned by CueScore.

        Returns:
            Structured match data.
        """

        soup = BeautifulSoup(
            html,
            "html.parser",
        )

        match_page = soup.find("cs-match-page")

        match_id = None
        tournament_id = None

        if match_page:
            match_id = match_page.get("match-id")
            tournament_id = match_page.get("tournament-id")

        return {
            "match_id": match_id,
            "tournament_id": tournament_id,
            "players": [],
            "frames": [],
            "metadata": {},
        }