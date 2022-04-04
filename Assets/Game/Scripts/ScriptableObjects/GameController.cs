using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Castoor/GameController")]
public class GameController : ScriptableObject
{
    public UnityEvent onResumeEvent;
    public UnityEvent onPauseEvent;


    public void ResumeGame()
    {
        onResumeEvent?.Invoke();
    }

    public void PauseGame()
    {
        onPauseEvent?.Invoke();
    }



}
