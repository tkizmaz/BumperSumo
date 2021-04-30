using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvents : MonoBehaviour
{

    [SerializeField] private int carCount = 4;
    void DestroyCollisionListener(string carName)
    {
        carCount--;
        if (carCount == 1)
        {
            //Game over scene.
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ZoneController.DestroyCollision += DestroyCollisionListener;
    }
}
