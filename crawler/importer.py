"""
Importer component.

Responsible for importing validated data into the historical archive.
"""

from crawler.parsers.match import MatchParser

class Importer:

    def __init__(self, client):

        self.client = client

        self.parser = MatchParser()

    def import_match(self, match_id):

        html = self.client.get_match(
            match_id
        )

        return self.parser.parse(
            html
        )
