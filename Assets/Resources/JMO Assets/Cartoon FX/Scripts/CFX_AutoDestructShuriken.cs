using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;
	public new string name;
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			if (!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if (OnlyDeactivate)
				{
					Effect.Instance.ReturnEffectToPool(name, gameObject);
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
}
