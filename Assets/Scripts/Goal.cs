using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        GameManager.singleton.NextLevel();
    }
}
