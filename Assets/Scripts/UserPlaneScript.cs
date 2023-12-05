using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlaneScript : PlaneMoveScript
{
    //Selection function that is called when a user controllable plane is clicked.
    private void OnMouseUpAsButton()
    {
        //Checks if the game state is playing and the previous selection is not the current selection.
        if ((GameManager.instance.State == GameState.Playing) && (GameManager.instance.PreviousSelection != gameObject))
        {
            if (GameManager.instance.PreviousSelection != null)
            {
                //Calls the PlaneDeselection function in the Game Manager.
                GameManager.instance.PlaneDeselection();
            }
            //Sets the current selection to the previous selection.
            GameManager.instance.PreviousSelection = gameObject;
            //Calls the PlaneSelection function in the Game Manager.
            GameManager.instance.PlaneSelection();
        }
    }
}
