using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public float order; // crime / half standing -> peeps and buildings?
	public float integrity; //standing building / buildings amount
	public float peeps; // peeps alive / total peeps amount


	public int totalBuildingAmount;
	public int damagedBuildingAmount;

	public int totalPeepsAmount;
	public int damagedPeepsAmount;

	private void Awake()
	{
		Instance = this;
		totalBuildingAmount = 0;
		damagedBuildingAmount = 0;

		totalPeepsAmount = 0;
		damagedPeepsAmount = 0;

		order = 1f;
		integrity = 1f;
		peeps = 1f;
	}

	public void Register(BuildingScript building)
	{
		building.StateComponent.OnStateChange.AddListener(OnBuildingStateChanged);
		totalBuildingAmount++;
	}

	private void OnBuildingStateChanged(State state)
	{
		if (state != State.Destroyed)
		{
			return;
		}

		damagedBuildingAmount++;

		integrity = 1f - (float) damagedBuildingAmount / (float) totalBuildingAmount;
		if (damagedBuildingAmount == totalBuildingAmount)
		{
			Lose();
		}
	}

	public void Register(PeepScript peep)
	{
		peep.StateComponent.OnStateChange.AddListener(OnPeepStateChanged);
		totalPeepsAmount++;
	}

	private void OnPeepStateChanged(State state)
	{
		if (state != State.Destroyed)
		{
			return;
		}

		damagedPeepsAmount++;

		peeps = 1f - (float) damagedPeepsAmount / (float) totalPeepsAmount;
		if (damagedPeepsAmount == totalPeepsAmount)
		{
			Lose();
		}
	}

	private void Lose()
	{
		Debug.Log("Lose");
	}
}