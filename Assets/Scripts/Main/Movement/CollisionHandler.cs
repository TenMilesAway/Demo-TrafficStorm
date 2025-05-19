using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human") && tag == "Human")
            return;

        // 这里可以去加特效和音效
        PoolMgr.GetInstance().PushObj(gameObject.name.ToString(), gameObject);

        EventCenter.GetInstance().EventTrigger("UpdateHealth", 0.5f);

        MusicMgr.GetInstance().PlaySound("collision", false);
    }
}
