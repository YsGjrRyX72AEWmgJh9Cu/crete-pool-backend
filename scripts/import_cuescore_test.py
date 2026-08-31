import requests

TOURNAMENT_ID = 83713144

url = f"https://api.cuescore.com/tournament/?lang=en&id={TOURNAMENT_ID}"

response = requests.get(url)

print("Status:", response.status_code)

data = response.json()

print("Tournament ID:", data["tournamentId"])
print("Tournament Name:", data["name"])
print("Status:", data["status"])
print("Discipline:", data["discipline"])

print("Matches:", len(data["matches"]))
