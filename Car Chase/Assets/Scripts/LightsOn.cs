using UnityEngine;
using System.Collections;

public class LightsOn : MonoBehaviour {
	private PoliceCarController target;
	public float flickeringFreq=0.5f;
    
	IEnumerator Start ()
	{
        target = gameObject.GetComponentInParent<PoliceCarController>();
        
        while (true)
		{
			if( !target.patrolMode)
			{
			    GetComponent<Light>().enabled = !(GetComponent<Light>().enabled); //toggle on/off the enabled property
				gameObject.GetComponent<Light>().color=Color.blue;
			}
            else
            {
                gameObject.GetComponent<Light>().color = Color.white;
                GetComponent<Light>().enabled = true; //toggle on/off the enabled property
            }
            yield return new WaitForSeconds(flickeringFreq);
		}
	}
}
