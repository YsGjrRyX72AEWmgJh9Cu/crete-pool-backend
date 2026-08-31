"""
CueScore Match Parser.

Transforms CueScore JSON into the internal match model.
"""


class MatchParser:
    """
    Maps CueScore match JSON into the internal domain model.
    """

    def parse(self, match: dict) -> dict:
        """
        Transform CueScore JSON into the internal match model.
        """

        return {
            "match_id": match["matchId"],
            "tournament_id": match["tournamentId"],

            "player_a": {
                "id": match["playerA"]["playerId"],
                "name": match["playerA"]["name"],
            },

            "player_b": {
                "id": match["playerB"]["playerId"],
                "name": match["playerB"]["name"],
            },

            "score": {
                "player_a": match["scoreA"],
                "player_b": match["scoreB"],
            },

            "round": {
                "name": match["roundName"],
                "code": match["roundCode"],
            },

            "discipline": match["discipline"],

            "status": match["matchstatusCode"],
        }