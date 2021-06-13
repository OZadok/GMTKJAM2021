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

	[SerializeField] private RectTransform populationBar;
	[SerializeField] private RectTransform orderBar;
	[SerializeField] private RectTransform integrityBar;

	float populationWidth, orderWidth, integrityWidth, populationHeight, orderHeight, integrityHeight;

	private void Awake()
	{
		populationWidth = populationBar.rect.width;
		orderWidth = populationBar.rect.width;
		integrityWidth= populationBar.rect.width;
		populationHeight = populationBar.rect.height;
		orderHeight = populationBar.rect.height;
		integrityHeight = populationBar.rect.height;

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

		integrityBar.sizeDelta = new Vector2(order * integrityWidth, integrityHeight);
		populationBar.sizeDelta = new Vector2((peeps * populationWidth), populationHeight);
		integrityBar.sizeDelta = new Vector2((integrity * integrityWidth), integrityHeight);

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

		integrityBar.sizeDelta = new Vector2(order * integrityWidth, integrityHeight);
		populationBar.sizeDelta = new Vector2((peeps * populationWidth), populationHeight);
		integrityBar.sizeDelta = new Vector2((integrity * integrityWidth), integrityHeight);
	}

	private void OnPeepStateChanged(State state)
	{
		if (state != State.Destroyed)
		{
			return;
		}

		damagedPeepsAmount++;

		peeps = 1f - (float) damagedPeepsAmount / (float) totalPeepsAmount;

		integrityBar.sizeDelta = new Vector2(order * integrityWidth, integrityHeight);
		populationBar.sizeDelta = new Vector2((peeps * populationWidth), populationHeight);
		integrityBar.sizeDelta = new Vector2((integrity * integrityWidth), integrityHeight);

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