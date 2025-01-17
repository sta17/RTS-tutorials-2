using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class HeroController : UnitController {

    #region variables

    [Header("Hero")]
    [SerializeField] private AttributeAmount[] CurrentAttributes;
    [SerializeField] private int currentHeroExperience = 0;
    [SerializeField] private int currentHeroLevel = 1;

    #endregion

    #region Setup and Constructors

    public void Start()
    {
        base.Start();
        if (getUnitStatsSO().getUsePersonalEquipment)
        {
            RegisterHero();
        }
    }

    private void RegisterHero()
    {
        var tempplayer = (PlayerManager)getFaction();
        tempplayer.AddToHeroQuickDisplay(this);
    }

    //public void OnValidate() { base.OnValidate(); }

    #endregion

}
