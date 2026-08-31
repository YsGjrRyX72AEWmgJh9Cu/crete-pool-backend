import sys
import subprocess

if len(sys.argv) != 2:
    print("Usage: python3 import_cuescore.py <tournament_id>")
    sys.exit(1)

tournament_id = sys.argv[1]

print("\n=== TOURNAMENT ===")
subprocess.run(
    ["python3", "import_cuescore_tournament.py", tournament_id]
)

print("\n=== PLAYERS ===")
subprocess.run(
    ["python3", "import_cuescore_players.py", tournament_id]
)

print("\n=== MATCHES ===")
subprocess.run(
    ["python3", "import_cuescore_matches.py", tournament_id]
)

print("\nImport completed")
