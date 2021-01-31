using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    #region Variables
    public bool isHoldingSomething = false;

    [SerializeField] Transform hookParent = default;
    [SerializeField] PhysicMaterial draggedObjectMaterial = default;
    [SerializeField] Rigidbody hookRb = default;

    [SerializeField] ParticleSystem grabParticles = default;
    [SerializeField] ParticleSystem throwParticles = default;

    [HideInInspector] public Grapple grapple = default;
    [HideInInspector] public GameObject grabbedObject = default;

    float grabbedMass = 0;
    bool isActive = false;
    #endregion

    #region Unity Callbacks
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (isActive && !isHoldingSomething && collision.gameObject.CompareTag("Movable"))
    //        Grab(collision.transform);
    //}

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
            bool hasBeenDropped = comptoirChunk.OnItemEnter(grabbedItem);
            if (hasBeenDropped)
            {
                DropItem();
                if (grapple != null)
                {
                    grapple.Drop();
                }
            }
        }
    }
    #endregion

    #region Functions
    public void ToggleActivate(bool activate)
    {
        isActive = activate;
    }

    public void Grab(Transform objectToGrab)
    {
        isHoldingSomething = true;
        isActive = false;
        grabbedObject = objectToGrab.gameObject;

        Vector3 contactNormal = (objectToGrab.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(contactNormal);

        objectToGrab.parent = transform;
        Rigidbody otherRB = objectToGrab.GetComponent<Rigidbody>();
        hookRb.mass += otherRB.mass;
        grabbedMass = otherRB.mass;
        Destroy(objectToGrab.GetComponent<Obi.ObiRigidbody>());
        Destroy(otherRB);
        objectToGrab.GetComponent<Collider>().material = draggedObjectMaterial;

        grabParticles.Play();

        grapple.Grab();
    }

    public void Throw(Vector3 force)
    {
        Movable movable = grabbedObject.GetComponent<Movable>();
        if (movable != null)
            movable.TemporarilyDisableCollisionsWithPlayer();

        hookRb.mass -= grabbedMass;
        grabbedObject.transform.parent = null;

        Rigidbody otherRB = grabbedObject.AddComponent<Rigidbody>();
        otherRB.mass = grabbedMass;
        otherRB.AddForce(force, ForceMode.Impulse);

        throwParticles.Play();

        isHoldingSomething = false;
    }
    public void DropItem()
    {
        if (isHoldingSomething)
        {
            hookRb.mass -= grabbedMass;
        }
        isHoldingSomething = false;
        grabbedObject = null;
    }
    #endregion
}
