using Framework;
using UnityEngine;

public class PoolKitExample : MonoBehaviour
{
    public GameObject Cube;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PoolManager.Instance.Get<Cube>(Cube);
        }
    }
}