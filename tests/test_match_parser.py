from crawler.parsers.match import MatchParser


def test_parser_extracts_match_and_tournament_ids():
    with open(
        "tests/fixtures/sample_match.html",
        encoding="utf-8",
    ) as file:
        html = file.read()

    parser = MatchParser()

    data = parser.parse(html)

    assert data["match_id"] == "83791531"
    assert data["tournament_id"] == "83713144"