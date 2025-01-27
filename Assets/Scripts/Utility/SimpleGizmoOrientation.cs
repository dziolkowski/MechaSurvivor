using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGizmoOrientation : MonoBehaviour
{
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(gameObject.transform.position, transform.forward);
    }
}
