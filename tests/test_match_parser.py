import json

from crawler.parsers.match import MatchParser


def test_parser_extracts_match_data():

    with open(
        "tests/fixtures/json/sample_match.json",
        encoding="utf-8",
    ) as file:

        match = json.load(file)

    parser = MatchParser()

    data = parser.parse(match)

    assert data["match_id"] == 83791531
    assert data["tournament_id"] == 83713144

    assert data["player_a"]["id"] == 1076143
    assert data["player_a"]["name"] == "Manos Menioudakis"

    assert data["player_b"]["name"] == "Ντράγκαν Πετρουλάκης"

    assert data["discipline"] == "9-Ball"