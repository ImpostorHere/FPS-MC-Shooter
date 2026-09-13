using UnityEngine;

public class HitboxChecker : MonoBehaviour
{
    public float maxHp = 100;
    public float currentHp;


    void Start()
    {
        currentHp = maxHp;
    }

    public void RemoveHp(float value)
    {
        currentHp -= value;
    }
}
