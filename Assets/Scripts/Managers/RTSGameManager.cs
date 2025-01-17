using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSGameManager : MonoBehaviour
{
    public static void UnitTakeDamage(UnitController Attacking, UnitController Attacked)
    {
        var damage = Attacked.getUnitStatsSO().attackDamage;

        Attacked.TakeDamage(Attacking, damage);
    }
}
