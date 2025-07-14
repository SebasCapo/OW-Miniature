using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace OWMiniature.Visuals
{
	public class CustomShip : MonoBehaviour
	{
		protected void Awake()
		{
			var rg = gameObject.AddComponent<Rigidbody>();
			var owr = gameObject.AddComponent<OWRigidbody>();
			var krg = gameObject.AddComponent<KinematicRigidbody>();


			GameObject rfValueObj = new GameObject();
		}
	}
}
