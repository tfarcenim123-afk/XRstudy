using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class TimeLineCtrl : MonoBehaviour
{
  public PlayableDirector PD;

  void Start()
  {
    PD.Pause();
  }

  void Update()
  {
    if (Keyboard.current.anyKey.wasPressedThisFrame)
    {
      PD.gameObject.SetActive(true);
      PD.Play();
    }
  }
}
