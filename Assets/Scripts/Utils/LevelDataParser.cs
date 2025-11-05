using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDataParser
{
    public static Dictionary<int, long> BuildLevelExpMap(TextAsset textAsset) {
        Dictionary<int, long> keyValuePairs = new Dictionary<int, long>();

        string[] rows = textAsset.text.Split("\n").Where(d => d.Trim() != "").ToArray();

        for (int i = 0; i < rows.Length; i++)
        {
            if (i != 0)
            {
                string[] cols = rows[i].Trim().Split(',');
                int level = int.Parse(cols[0].Trim());
                long exp = long.Parse(cols[1].Trim());
                keyValuePairs.Add(level, exp);
            }
        }

        return keyValuePairs;
    }
}
