using UnityEngine;

public class PauseManager : Menu
{
    public static bool isPaused;

    [SerializeField] private GameObject pauseMenu;

    private CursorManager cursorManager;

    private void Start()
    {
        cursorManager = GetComponent<CursorManager>();

        //Start un-paused
        SetPaused(false);
    }

    //override = replace the parent method with this new one
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            //Set the paused state depedning on if we're currently paused or not
            SetPaused(Time.timeScale > 0); //'Time.timeScale > 0' is either 'true' or 'false'
        }
    }

    public void SetPaused(bool isPaused) 
    {
        // Set the time scale to 0 or 1 depending on if we're paused
        Time.timeScale = isPaused ? 0.0f : 1.0f;

        //SetActive(true) if paused. (false) is unpaused
        pauseMenu.SetActive(isPaused);

        //'isFree' should be true if 'isPaused' so we pass it straight through
        cursorManager.SetCursor(isPaused);

        PauseManager.isPaused = isPaused;
    }

    /// <summary>
    /// OnDestroy runs when a game object is destroyed/unloaded
    /// </summary>
    private void OnDestroy()
    {
        SetPaused(false);
    }
}
