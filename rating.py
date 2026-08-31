def expected_score(rating_a, rating_b):

    return 1 / (1 + 10 ** ((rating_b - rating_a) / 400))


def margin_multiplier(score_a, score_b, race_to):

    rack_diff = abs(score_a - score_b)

    return 1 + (rack_diff / race_to)


def calculate_new_rating(
    rating,
    expected,
    actual,
    margin,
    k=20
):

    return round(
        rating + (k * (actual - expected) * margin)
    )
def get_category(rating):

    if rating < 450:
        return "D"

    elif rating < 550:
        return "C"

    elif rating < 650:
        return "B"

    elif rating < 750:
        return "A"

    return "Pro"