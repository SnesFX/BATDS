using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntLock
{
	[Serializable]
	public class LockContainer
	{
		public float m_LockTimer;

		public bool m_StaticLock;

		public bool m_IsLocked;

		public void DoCountdown()
		{
			m_LockTimer -= Time.deltaTime;
			if (m_LockTimer <= 0f)
			{
				m_IsLocked = false;
			}
		}
	}

	private string m_LockName = "NO LOCK NAME SET";

	private List<LockContainer> ActiveLocks = new List<LockContainer>();

	public IntLock(string LockName)
	{
		m_LockName = LockName;
	}

	public void DoUpdate()
	{
		LockContainer[] array = ActiveLocks.ToArray();
		foreach (LockContainer lockContainer in array)
		{
			if (!lockContainer.m_IsLocked)
			{
				ActiveLocks.Remove(lockContainer);
			}
			else if (!lockContainer.m_StaticLock)
			{
				lockContainer.DoCountdown();
			}
		}
	}

	public bool IsLocked()
	{
		return ActiveLocks.Count != 0;
	}

	public void Lock(bool isStatic, float Timer = 0f)
	{
		LockContainer lockContainer = new LockContainer();
		lockContainer.m_LockTimer = Timer;
		lockContainer.m_StaticLock = isStatic;
		lockContainer.m_IsLocked = true;
		ActiveLocks.Add(lockContainer);
	}

	public void UnlockStatic()
	{
		List<LockContainer>.Enumerator enumerator = ActiveLocks.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.m_StaticLock)
			{
				ActiveLocks.Remove(enumerator.Current);
				break;
			}
		}
	}

	public void ForceUnlockAll()
	{
		ActiveLocks.Clear();
	}
}
