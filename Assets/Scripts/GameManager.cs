using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public float order; // crime / half standing -> peeps and buildings?
	public float integrity; //standing building / buildings amount
	public float peeps; // peeps alive / total peeps amount

	public float ratioIntegrity = 0.5f;
	public float ratioPeeps = 0.5f;
	public int totalOrderAmount => AliveBuildingAmount * orderBuildingWeight + AlivePeepsAmount + orderPeepWeight;
	public int notOrderAmount;

	public float ratioOrder = 0.5f;

	public int orderBuildingWeight = 10;
	public int orderPeepWeight = 1;

	public int totalBuildingAmount;
	public int damagedBuildingAmount;
	
	public int AliveBuildingAmount => totalBuildingAmount - damagedBuildingAmount;

	public int totalPeepsAmount;
	public int damagedPeepsAmount;
	
	public int AlivePeepsAmount => totalPeepsAmount - damagedPeepsAmount;

	[SerializeField] private RectTransform populationBar;
	[SerializeField] private RectTransform orderBar;
	[SerializeField] private RectTransform integrityBar;
	[Header("Game Over Stuff")]
	[SerializeField] private GameObject gameOver;
	[SerializeField] private TextMeshProUGUI gameOverReason;
	[SerializeField] private Sound gameOverSound;
	[SerializeField] private Color peepsColor, orderColor, integrityColor;

	private bool lostGame = false;

	float populationWidth, orderWidth, integrityWidth, populationHeight, orderHeight, integrityHeight;

	private void Awake()
	{
		Instance = this;
		
		notOrderAmount = 0;
		
		totalBuildingAmount = 0;
		damagedBuildingAmount = 0;

		totalPeepsAmount = 0;
		damagedPeepsAmount = 0;

		order = 1f;
		integrity = 1f;
		peeps = 1f;
		
		populationWidth = populationBar.rect.width;
		orderWidth = populationBar.rect.width;
		integrityWidth= populationBar.rect.width;
		populationHeight = populationBar.rect.height;
		orderHeight = populationBar.rect.height;
		integrityHeight = populationBar.rect.height;

		gameOver.SetActive(false);
	}

    private void Update()
    {
		if (lostGame)
		{
			if (Input.GetKeyDown(KeyCode.Space))
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			else if (Input.GetKeyDown(KeyCode.Escape))
				SceneManager.LoadScene(0);
		}
	}

    public void Register(BuildingScript building)
	{
		building.StateComponent.OnStateChange.AddListener(OnBuildingStateChanged);
		building.StateComponent.OnStateEnter.AddListener(OnBuildingStateEnterOrder);
		building.StateComponent.OnStateExit.AddListener(OnBuildingStateExitOrder);
		totalBuildingAmount++;
	}
	
	public void Register(PeepScript peep)
	{
		peep.StateComponent.OnStateChange.AddListener(OnPeepStateChanged);
		peep.StateComponent.OnStateEnter.AddListener(OnPeepStateEnterOrder);
		peep.StateComponent.OnStateExit.AddListener(OnPeepStateExitOrder);
		totalPeepsAmount++;

		orderBar.sizeDelta = new Vector2(order * integrityWidth, integrityHeight);
		populationBar.sizeDelta = new Vector2((peeps * populationWidth), populationHeight);
		integrityBar.sizeDelta = new Vector2((integrity * integrityWidth), integrityHeight);
	}

	#region Order

	private void OnBuildingStateEnterOrder(State state)
	{
		if (state == State.Destroyed)
		{
			UpdateOrder();
		}
		if (state != State.Criminal)
		{
			return;
		}

		UpdateOrder(orderBuildingWeight);

		if (notOrderAmount >= ratioOrder * totalOrderAmount)
		{
			Lose(2);
		}
	}
	
	private void OnBuildingStateExitOrder(State state)
	{
		if (state == State.Destroyed)
		{
			UpdateOrder();
		}
		if (state != State.Criminal)
		{
			return;
		}

		UpdateOrder(-orderBuildingWeight);
	}
	
	private void OnPeepStateEnterOrder(State state)
	{
		if (state != State.Criminal)
		{
			return;
		}

		UpdateOrder(orderPeepWeight);

		if (notOrderAmount >= ratioOrder * totalOrderAmount)
		{
			Lose(0);
		}
	}
	
	private void OnPeepStateExitOrder(State state)
	{
		if (state != State.Criminal)
		{
			return;
		}

		UpdateOrder(-orderPeepWeight);
	}

	private void UpdateOrder(int amount = 0)
	{
		notOrderAmount += amount;
		
		order = 1f - (float) notOrderAmount / (ratioOrder * (float) totalOrderAmount);
		orderBar.sizeDelta = new Vector2(order * orderWidth, orderHeight);
	}

	#endregion

	#region Integrity

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

		integrity = 1f - (float) damagedBuildingAmount / (ratioIntegrity * (float) totalBuildingAmount);
		if (damagedBuildingAmount >= (ratioIntegrity * (float) totalBuildingAmount))
		{
			Lose(1);
		}
	}
	#endregion



	#region Peeps
	private void OnPeepStateChanged(State state)
	{
		if (state != State.Destroyed)
		{
			return;
		}

		damagedPeepsAmount++;

		peeps = 1f - (float) damagedPeepsAmount / (ratioPeeps * (float) totalPeepsAmount);

		integrityBar.sizeDelta = new Vector2(order * integrityWidth, integrityHeight);
		populationBar.sizeDelta = new Vector2((peeps * populationWidth), populationHeight);
		integrityBar.sizeDelta = new Vector2((integrity * integrityWidth), integrityHeight);

		if (damagedPeepsAmount >= (ratioPeeps * (float) totalPeepsAmount))
		{
			Lose(0);
		}
	}
	
	#endregion

	private void Lose(int loseCondition)
	{
		gameOver.SetActive(true);
		// 0 Peeps Lose
		// 1 Integrity Lose
		// 2 Order Lose
		switch (loseCondition)
		{
			case 0:
				gameOverReason.text = "ashford is now a ghost town";
				gameOverReason.color = peepsColor;
				break;
			case 1:
				gameOverReason.text = "ashford was reduced to rubble";
				gameOverReason.color = integrityColor;
				break;
			case 2:
				gameOverReason.text = "ashford was lost to crime";
				gameOverReason.color = orderColor;
				break;
		}

		if (gameOverSound != null)
			AudioManager.instance.Play(gameOverSound, true);

		lostGame = true;
	}
}