using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemDuplicateRemover : MonoBehaviour
{
    void OnEnable() {
        if (MainMenu.Instance != null)
            Destroy(gameObject);
    }
}
