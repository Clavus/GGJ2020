using System;
using UnityEngine;

public static class ColliderExtensions
{
	/// <summary>
	/// Gets the component attached to the collider. If not found, also checks for the component at the attached rigidbody.
	/// </summary>
	public static T GetComponentOrAtBody<T>(this Collider collider)
	{
		T component = collider.GetComponent<T>();
		if (component == null && collider.attachedRigidbody != null)
		{
			component = collider.attachedRigidbody.GetComponent<T>();
		}
		return component;
	}

	/// <summary>
	/// Gets the component attached to the collider. If not found, also checks for the component at the attached rigidbody. Returns true if a component is found.
	/// </summary>
	public static bool TryGetComponentOrAtBody<T>(this Collider collider, out T component)
	{
		if (collider.TryGetComponent<T>(out component))
			return true;
		else if (collider.attachedRigidbody != null)
			return collider.attachedRigidbody.TryGetComponent<T>(out component);

		return false;
	}

	/// <summary>
	/// Gets the components attached to the collider and at its attached rigidbody.
	/// </summary>
	public static T[] GetComponentsAndAtBody<T>(this Collider collider)
	{
		T[] bodyComponents = new T[0];
		T[] colliderComponents = collider.GetComponents<T>();
		if (collider.attachedRigidbody != null)
		{
			bodyComponents = collider.attachedRigidbody.GetComponents<T>();
		}

		T[] result = new T[bodyComponents.Length + colliderComponents.Length];
		if (bodyComponents.Length > 0)
			Array.Copy(bodyComponents, result, bodyComponents.Length);
		if (colliderComponents.Length > 0)
			Array.Copy(colliderComponents, 0, result, bodyComponents.Length, colliderComponents.Length);

		return result;
	}
}