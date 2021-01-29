using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    MovementController movementController;
    DashController dashController;
    HeightController heightController;
    List<IController> orderedControllers = new List<IController>();
    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<MovementController>();
        dashController = GetComponent<DashController>();
        heightController = GetComponent<HeightController>();
        
        orderedControllers.Add(dashController);
        orderedControllers.Add(heightController);
        orderedControllers.Add(movementController);

    }

    // Update is called once per frame
    void Update()
    {  
        foreach (IController controller in orderedControllers) {
            controller.UpdateController();
        }
    }
    void FixedUpdate () 
    {
        foreach (IController controller in orderedControllers) {
            controller.FixedUpdateController();
        }
    }
}
