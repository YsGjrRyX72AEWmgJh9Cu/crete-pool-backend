"""
Global configuration for the CueScore crawler.

Only this file should contain project-wide settings.
The rest of the crawler imports values from here.
"""

# ---------------------------------------------------------------------
# Supported American Pool disciplines
# ---------------------------------------------------------------------

SUPPORTED_DISCIPLINES = {
    "8-ball",
    "9-ball",
    "10-ball",
}

# ---------------------------------------------------------------------
# CueScore
# ---------------------------------------------------------------------

CUESCORE_BASE_URL = "https://cuescore.com"
CUESCORE_API_URL = "https://api.cuescore.com"

# ---------------------------------------------------------------------
# Networking
# ---------------------------------------------------------------------

REQUEST_TIMEOUT = 15

REQUEST_DELAY = 0.30

MAX_RETRIES = 3

# ---------------------------------------------------------------------
# Discovery
# ---------------------------------------------------------------------

DISCOVER_PLAYERS = True

DISCOVER_TOURNAMENTS = True

# ---------------------------------------------------------------------
# Import
# ---------------------------------------------------------------------

IMPORT_PLAYERS = True

IMPORT_MATCHES = True

IMPORT_TOURNAMENTS = True
