using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPathRequestManager
{
	private NavMeshPath m_path = new NavMeshPath();

	private static NavMeshPathRequestManager m_Instance;

	public static NavMeshPathRequestManager Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new NavMeshPathRequestManager();
			}
			return m_Instance;
		}
	}

	public bool RequestPath(Vector3 path_start, Vector3 path_end, List<Vector3> path)
	{
		Vector3[] array = new Vector3[200];
		NavMesh.CalculatePath(path_start, path_end, -1, m_path);
		if (m_path.status == NavMeshPathStatus.PathComplete || m_path.status == NavMeshPathStatus.PathPartial)
		{
			int cornersNonAlloc = m_path.GetCornersNonAlloc(array);
			path.Clear();
			for (int i = 0; i < cornersNonAlloc; i++)
			{
				path.Add(array[i]);
			}
			return true;
		}
		return false;
	}

	public bool RequestClosestPath(Vector3 path_start, Vector3 path_end, List<Vector3> path, float sampleDistance)
	{
		Vector3[] array = new Vector3[200];
		NavMesh.CalculatePath(path_start, path_end, -1, m_path);
		if (m_path.status == NavMeshPathStatus.PathComplete || m_path.status == NavMeshPathStatus.PathPartial)
		{
			int cornersNonAlloc = m_path.GetCornersNonAlloc(array);
			path.Clear();
			for (int i = 0; i < cornersNonAlloc; i++)
			{
				path.Add(array[i]);
			}
			return true;
		}
		NavMeshHit hit;
        if (NavMesh.SamplePosition(path_end, out hit, sampleDistance, -1) && NavMesh.CalculatePath(path_start, hit.position, -1, m_path) && (m_path.status == NavMeshPathStatus.PathComplete || m_path.status == NavMeshPathStatus.PathPartial))
		{
			int cornersNonAlloc = m_path.GetCornersNonAlloc(array);
			path.Clear();
			for (int j = 0; j < cornersNonAlloc; j++)
			{
				path.Add(array[j]);
			}
			return true;
		}
		return false;
	}

	public NavMeshPathStatus RequestPathWithStatus(Vector3 path_start, Vector3 path_end, List<Vector3> path)
	{
		Vector3[] array = new Vector3[200];
		NavMesh.CalculatePath(path_start, path_end, -1, m_path);
		if (m_path.status == NavMeshPathStatus.PathComplete || m_path.status == NavMeshPathStatus.PathPartial)
		{
			int cornersNonAlloc = m_path.GetCornersNonAlloc(array);
			path.Clear();
			for (int i = 0; i < cornersNonAlloc; i++)
			{
				path.Add(array[i]);
			}
		}
		return m_path.status;
	}
}
