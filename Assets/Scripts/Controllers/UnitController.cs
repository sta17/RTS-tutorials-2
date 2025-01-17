using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour, I_UI_SlotInterfacer, I_Selectable, IDropHandler
{
    #region variables

    [Header("Player/Faction")]
    [SerializeField] private FactionManager player;
    [SerializeField] private bool usePlayerColour = true;
    [SerializeField] private Color unitColor;

    [Header("Unit Properties")]
    [SerializeField] private UI_PersonalInventorySystem personalInventory;
    [SerializeField] private UI_PersonalInventorySystem personalInventoryEquiptment;
    [SerializeField] private int inventorySize = 1;
    [SerializeField] private int EquiptmentInventorySize = 7;
    [SerializeField] private bool StartWithMaxHealth = true;
    [SerializeField] private float currentHealth;
    [SerializeField] private int unitID;
    [SerializeField] private SO_Unit_Data unitStats;

    [Header("Misc")]
    private NavMeshAgent navAgent;
    private Transform currentTarget;
    private float attackTimer;
    [SerializeField] public UnitAnimationHandler animations;
    [SerializeField] private int StoppingDistanceDefault = 2;

    [Header("Selection")]
    [SerializeField] private SelectableTypes selectionType = SelectableTypes.Unit;
    [SerializeField] private bool selected = false;
    [SerializeField] private int selectionID;

    [Header("Unit UI")]
    [SerializeField] private Canvas SelectionCircle;
    [SerializeField] private UI_Healthbar healthbar;

    #endregion

    #region Setup and Constructors

    public void Start()
    {
        //unitState.SetMapPresence(this);
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = StoppingDistanceDefault;
        navAgent.autoBraking = true;
        attackTimer = unitStats.attackSpeed;

        if (StartWithMaxHealth)
        {
            healthbar.HideBar();
        }

        if (unitStats.getDisplayCharacterInUI)
        {
            RegisterHero();
        }

        if (StartWithMaxHealth)
        {
            currentHealth = unitStats.unitMaxHealth;
        }

        if (usePlayerColour)
        {
            unitColor = player.playerColor;
        }

        personalInventory = new UI_PersonalInventorySystem(inventorySize);

        if (unitStats.getUsePersonalEquipment) {
            personalInventoryEquiptment = new UI_PersonalInventorySystem(EquiptmentInventorySize);
        } else {
            personalInventoryEquiptment = new UI_PersonalInventorySystem(0);
        }
        

        GetComponent<Renderer>().material.color = getUnitColor();
    }

    private void RegisterHero()
    {
        var tempplayer = (PlayerManager)player;
        tempplayer.AddToHeroQuickDisplay(this);
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;
        if ((currentTarget != null))
        {
            if (currentTarget.CompareTag("Unit"))
            {
                navAgent.destination = currentTarget.position;

                var distance = (transform.position - currentTarget.position).magnitude;
                navAgent.stoppingDistance = StoppingDistanceDefault + unitStats.attackRange;

                if (distance <= unitStats.attackRange)
                {
                    navAgent.stoppingDistance = StoppingDistanceDefault;
                    navAgent.ResetPath();
                    Attack();
                }
            }
            else if (currentTarget.CompareTag("Item"))
            {
                navAgent.destination = currentTarget.position;
                navAgent.stoppingDistance = StoppingDistanceDefault + unitStats.attackRange;

                var itemModelHandler = currentTarget.GetComponent<MAP_Interactable_Item>();

                var distance = (itemModelHandler.transform.position - this.gameObject.transform.position).magnitude;

                if (distance <= itemModelHandler.getInteractRange())
                {
                    navAgent.stoppingDistance = StoppingDistanceDefault;
                    navAgent.ResetPath();
                    InteractWithItem(itemModelHandler);
                }
            }
        }

        if (animations != null)
        {
            if (IsMoving())
            {
                animations.IsMoving(true);
            }
            else
            {
                animations.IsMoving(false);
            }
        }
    }

    public void OnValidate()
    {
        if (StartWithMaxHealth)
        {
            currentHealth = unitStats.unitMaxHealth;
        }

        if (usePlayerColour)
        {
            if(player != null)
            {
                unitColor = player.playerColor;
            }
        }
    }

    #endregion

    #region Interactions and Movements

    public void MoveUnit(Vector3 dest)
    {
        currentTarget = null;
        navAgent.destination = dest;
    }

    public bool IsMoving()
    {
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void Attack()
    {
        if (attackTimer >= unitStats.attackSpeed)
        {
            RTSGameManager.UnitTakeDamage(this, currentTarget.GetComponent<UnitController>());
            attackTimer = 0;

            if (animations != null)
            {
                animations.Attack();
            }
        }
    }

    public void InteractWithItem(MAP_Interactable_Item itemHandler)
    {
        var result = false;
        if (itemHandler.getType() == ItemPickupTypes.Pickup)
        {
            result = itemHandler.HandlePickupType(this);
        }
        else
        {
            result = AddItem(itemHandler.getItem());
        }
        if (result)
        {
            itemHandler.ModelCleanUp();
        }
    }

    public bool AddItem(INGAME_Item_Data item)
    {
        var result = personalInventory.Add((item));

        if (result)
        {
            if (player.playerType == PlayerTypes.Human)
            {
                var HumanPlayer = (PlayerManager)player;
                HumanPlayer.RaiseItemChangeNotification(this);
            }
        }

        return result;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            GameObject eventGameObject = eventData.pointerDrag;

            var itemSlot = eventGameObject.GetComponent<UI_Slot_Item>();
            var itemStateDropped = (INGAME_Item_Data)itemSlot.GetObjInSlot();
            var listener = (I_UI_IconItemListener) itemSlot.GetListener();
            var displayedUnit = (UnitController)listener.GetDisplayedObject();

            //Debug.Log("this: " + this.name);
            //Debug.Log("displayedUnit: " + displayedUnit.name);
            if (this != displayedUnit)
            {
                var result = personalInventory.Add(itemStateDropped);
                if (result)
                {
                    // remove item from slot now
                    itemSlot.NotifyListenerDragDrop();
                }
            }
        }
    }

    #endregion

    #region Take Damage

    public void TakeDamage(UnitController enemy, float damage)
    {

        if (!healthbar.gameObject.activeInHierarchy)
        {
            healthbar.ShowBar();
        }
        if (currentHealth > 0)
        {
            StartCoroutine(Flasher());

            currentHealth -= damage;
            healthbar.SetHealth(unitStats.unitMaxHealth, currentHealth);
        }
        else
        {
            ZeroHealth();
            //Debug.Log("Dead");
        }


    }

    IEnumerator Flasher()
    {
        var renderer = GetComponent<Renderer>();
        for (int i = 0; i < 2; i++)
        {
            renderer.material.color = Color.gray;
            yield return new WaitForSeconds(.05f);
            renderer.material.color = getUnitColor();
            yield return new WaitForSeconds(.05f);
        }
    }

    private void ZeroHealth()
    {
        player.RaiseDeathChangeNotification(this);
        Destroy(this.gameObject);
    }

    #endregion

    #region Getters and Setters

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public int getSelectionID()
    {
        return selectionID;
    }

    public Sprite getIcon()
    {
        return unitStats.icon;
    }

    public FactionManager getFaction()
    {
        return player;
    }

    public SelectableTypes GetSelectableType()
    {
        return selectionType;
    }

    public void SetSelected(bool isSelected)
    {
        selected = isSelected;
        SelectionCircle.gameObject.SetActive(isSelected);
    }

    public bool GetSelectedStatus()
    {
        return selected;
    }

    public void SetNewTarget(Transform enemy)
    {
        currentTarget = enemy;
    }

    public Color getUnitColor()
    {
        if (usePlayerColour)
        {
            return player.playerColor;
        }
        else
        {
            return unitColor;
        }
    }

    public UI_PersonalInventorySystem GetPersonalInventorySystem()
    {
        return personalInventory;
    }
    public UI_PersonalInventorySystem GetpersonalInventoryEquiptmentSystem()
    {
        return personalInventoryEquiptment;
    }

    public SO_Unit_Data getUnitStatsSO()
    {
        return unitStats;
    }

    #endregion

    #region Misc

    public void iconInteract()
    {
        Debug.Log("Clicked On");
    }

    public int getID()
    {
        return unitID;
    }

    public string GetColouredName()
    {
        return unitStats.ColouredName;
    }

    public string GetTooltipInfoText()
    {
        return unitStats.TooltipInfoText;
    }

    #endregion

}
