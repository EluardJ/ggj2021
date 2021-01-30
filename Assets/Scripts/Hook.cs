using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    #region Variables
    public bool isHoldingSomething = false;

    [SerializeField] float throwForce = 0;
    [SerializeField] Transform hookParent = default;
    [SerializeField] PhysicMaterial draggedObjectMaterial = default;
    [SerializeField] Rigidbody hookRb = default;

    [HideInInspector] public Grapple grapple = default;

    float grabbedMass = 0;
    GameObject grabbedObject = default;
    bool isActive = false;
    #endregion

    #region Unity Callbacks
    private void OnCollisionEnter(Collision collision)
    {
        if (isActive && !isHoldingSomething && collision.gameObject.CompareTag("Movable"))
            Grab(collision.transform, collision.GetContact(0).normal);
    }
    #endregion

    #region Functions
    public void ToggleActivate(bool activate)
    {
        isActive = activate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (grabbedObject == null)
        {
            return;
        }
        ComptoirChunk comptoirChunk = other.GetComponent<ComptoirChunk>();
        Item grabbedItem = grabbedObject.GetComponent<Item>();
        if (comptoirChunk != null && grabbedItem != null)
        {
            comptoirChunk.OnItemEnter(grabbedItem);
            DropItem();
            if (grapple != null) {
                grapple.Drop();
            }
        }
    }

    private void Grab(Transform objectToGrab, Vector3 contactNormal)
    {
        isHoldingSomething = true;
        isActive = false;
        grabbedObject = objectToGrab.gameObject;

        transform.rotation.SetLookRotation(contactNormal);

        objectToGrab.parent = transform;
        Rigidbody otherRB = objectToGrab.GetComponent<Rigidbody>();
        hookRb.mass += otherRB.mass;
        grabbedMass = otherRB.mass;
        Destroy(objectToGrab.GetComponent<Obi.ObiRigidbody>());
        Destroy(otherRB);
        objectToGrab.GetComponent<Collider>().material = draggedObjectMaterial;

        grapple.Grab();
    }

    public void Throw(Vector3 throwDirection)
    {
        hookRb.mass -= grabbedMass;
        grabbedObject.transform.parent = null;

        Rigidbody otherRB = grabbedObject.AddComponent<Rigidbody>();
        otherRB.mass = grabbedMass;
        otherRB.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        isHoldingSomething = false;
    }
    public void DropItem () {
        if (isHoldingSomething) {
            hookRb.mass -= grabbedMass;
        }
        isHoldingSomething = false;
        grabbedObject = null;
    }
    #endregion
}
