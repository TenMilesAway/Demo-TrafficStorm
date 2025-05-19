using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human") && tag == "Human")
            return;

        // �������ȥ����Ч����Ч
        PoolMgr.GetInstance().PushObj(gameObject.name.ToString(), gameObject);

        EventCenter.GetInstance().EventTrigger("UpdateHealth", 0.5f);

        MusicMgr.GetInstance().PlaySound("collision", false);
    }
}
