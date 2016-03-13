using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour {

    /// <summary>
    /// Ray length.
    /// </summary>
    public float rayLength = 1;

    /// <summary>
    /// Layer to collide.
    /// </summary>
    public string layer;

    [System.NonSerialized]
    public bool actived = false;

    /// <summary>
    /// Transform that determines the origin of the ray.
    /// </summary>
    public Transform handleSwordTransform;

    /// <summary>
    /// FX when it hits.
    /// </summary>
    public GameObject hitFXPrefab;

	void FixedUpdate()
    {
        if (actived)
        {
            RaycastHit hit;
            Ray ray = new Ray(handleSwordTransform.position, -transform.forward);
            if (Physics.Raycast(ray, out hit, rayLength, 1 << LayerMask.NameToLayer(layer)))
            {
                GameObject.Instantiate(hitFXPrefab, hit.point, Quaternion.identity);
                actived = false;
            }
        }
	}

    /// <summary>
    /// Active raycast of the sword.
    /// </summary>
    public void ActiveSword()
    {
        actived = true;
    }

    /// <summary>
    /// Deactive raycast of the sword.
    /// </summary>
    public void DeactiveSword()
    {
        actived = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Draw raycast.
        Gizmos.DrawLine(handleSwordTransform.position, handleSwordTransform.position - transform.forward * rayLength);
        Gizmos.DrawSphere(handleSwordTransform.position - transform.forward * rayLength, 0.01f);
    }
}
