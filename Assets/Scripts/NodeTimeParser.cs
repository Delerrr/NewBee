using System.Collections.Generic;
using UnityEngine;

public class NodeTimeParser : MonoBehaviour {
    enum NodeTag { Move = 1, Other = 2 }

    public GameObject eventNodePrefeb;

    public TextAsset nodeResource;

    private List<float> times = new();
    private void InitEventNodesTime() {
        if (nodeResource == null) {
            Debug.LogError("缺少node数据源");
            return;
        }

        string text = nodeResource.text;
        string[] lines = text.Split("\n");
        for (int i = 0; i < lines.Length; i++) {
            string[] dataLine = lines[i].Split("\t");
            if (dataLine.Length == 3 && int.Parse(dataLine[2]) == (int)NodeTag.Move) {
                times.Add(float.Parse(dataLine[0]));
            }
        }

    }

    public float GetTime(int idx) {
        if (times.Count == 0) {
            InitEventNodesTime();
        }
        if (idx >= times.Count) {
            return -1;
        }
        return times[idx];
    }
}
