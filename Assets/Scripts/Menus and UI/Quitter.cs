using UnityEngine;
//using UnityEditor; //Remove this before building

public class Quitter : MonoBehaviour
{
    public void Quit() 
    {
        Debug.Log("Quitting");
        //Quit the game application
        Application.Quit();

        //End the game in-editor - remove this code before building
        //EditorApplication.isPlaying = false;
    }
}
