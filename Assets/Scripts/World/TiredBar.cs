using UnityEngine;
using UnityEngine.UI;

public class TiredBar : MonoBehaviour
{
    public double fatigue;
    public Image fatigueBar;
    public void Update(){
        fatigue = ST07.Player.PlayerStats.instance.currentFatigueSeconds;
        fatigueBar.fillAmount = (float)(fatigue / ST07.Player.PlayerStats.instance.maxFatigueSeconds);
    }
}
