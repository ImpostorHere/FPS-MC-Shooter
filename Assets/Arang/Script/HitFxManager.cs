using UnityEngine;
using UnityEngine.SceneManagement;

public class HitFxManager : MonoBehaviour
{
    public static HitFxManager Instance { get; set; }

    public int enemy_layer;

    public GameObject hitFx_Enemy;
    public GameObject hitFx_Environment;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnHitFx(RaycastHit hitInfo)
    {
    }
}