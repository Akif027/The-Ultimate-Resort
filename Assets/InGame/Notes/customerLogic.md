[SerializeField]
private float probExitingRoom = 0.1f;
[SerializeField]
private float probGoingToilet = 0.15f;
[SerializeField]
private float probResting = 0.05f;
[SerializeField]
private float probEating = 0.1f; // Probability for the new state
[SerializeField]
private float probShopping = 0.1f; // Probability for the new state
[SerializeField]
private float totalProbability = 0.52f; // Updated to reflect the sum of the five probabilities

private CustomerState DetermineNextState()
{
float randomValue = UnityEngine.Random.value \* totalProbability;
float cumulativeProbability = 0;

    if (randomValue <= probExitingRoom) return CustomerState.ExitingRoom;
    else if (randomValue <= probExitingRoom + probGoingToilet) return CustomerState.GoingToilet;
    else if (randomValue <= probExitingRoom + probGoingToilet + probResting) return CustomerState.Resting;
    else if (randomValue <= probExitingRoom + probGoingToilet + probResting + probEating) return CustomerState.Eating;
    else return CustomerState.Shopping; // Default to the last state if none of the previous conditions are met

}
