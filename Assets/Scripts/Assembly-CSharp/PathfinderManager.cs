using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PathfinderManager : MonoBehaviour
{
	public List<PathfinderNode> BendySpawnableNodeList = new List<PathfinderNode>();

	public List<PathfinderNode> GlobalPathNodeList { get; private set; }

	public void BuildPaths()
	{
		ResetNodeList();
		GlobalPathNodeList = UnityEngine.Object.FindObjectsOfType<PathfinderNode>().ToList();
		GameManager.Instance.OnBuildPathsComplete();
	}

	public void RegisterBendySpawnPoints()
	{
		for (int i = 0; i < GlobalPathNodeList.Count; i++)
		{
			if (!(GlobalPathNodeList[i] == null) && Vector3.Distance(GlobalPathNodeList[i].transform.position, GameManager.Instance.Player.transform.position) > 60f)
			{
				GameManager.Instance.PATH_MANAGER.BendySpawnableNodeList.Add(GlobalPathNodeList[i]);
				GlobalPathNodeList[i].IsBendyNode = true;
			}
		}
	}

	private void ResetNodeList()
	{
		if (GlobalPathNodeList != null)
		{
			GlobalPathNodeList.Clear();
		}
		else
		{
			GlobalPathNodeList = new List<PathfinderNode>();
		}
	}
}
