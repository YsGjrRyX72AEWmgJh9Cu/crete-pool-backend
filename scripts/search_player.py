import requests

PLAYER_ID = 1076143

url = f"https://api.cuescore.com/player/?id={PLAYER_ID}"

response = requests.get(url)

print("Status:", response.status_code)
print(response.text[:1000])
