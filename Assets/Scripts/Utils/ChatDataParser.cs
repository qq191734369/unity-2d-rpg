using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// �����������ݽṹ�Ĺ�����
public class ChatDataParser
{
    static public Dictionary<string, ChatSectionByGroup> BuildChatData(TextAsset textAsset)
    {
        string data = textAsset.text;
        string[] textRows = data.Split("\n").Where(d => d != null && d != "").ToArray();
        // ��ֺõĵ�Ԫ������
        List<string[]> rows = new List<string[]>();
        foreach (string row in textRows)
        {
            // ���ÿһ������
            rows.Add(row.Split(","));
        }
        rows.RemoveAt(0);

        Dictionary<string, List<string[]>> groupedByName = GroupByName(rows);

        Dictionary<string, ChatSectionByGroup> res = new Dictionary<string, ChatSectionByGroup>();

        foreach (var name in groupedByName.Keys)
        {
            res.Add(name, GroupByChatGroup(groupedByName[name]));
        }

        return res;
    }

    // ����ɫ����Ի�
    static private Dictionary<string, List<string[]>> GroupByName(List<string[]> rows)
    {
        Dictionary<string, List<string[]>> res = new Dictionary<string, List<string[]>>();
        foreach (string[] row in rows)
        {
            string name = row[2];

            if (res.ContainsKey(name))
            {
                res[name].Add(row);
            }
            else
            {
                res.Add(name, new List<string[]>());
                res[name].Add(row);
            }
        }

        return res;
    }

    // ��group�ֶη����ɫ�ĶԻ�
    static private ChatSectionByGroup GroupByChatGroup(List<string[]> rows)
    {
        Dictionary<string, List<string[]>> groupedRows = new Dictionary<string, List<string[]>>();
        foreach (string[] row in rows)
        {
            string groupName = row[3];
            if (groupedRows.ContainsKey(groupName))
            {
                groupedRows[groupName].Add(row);
            }
            else
            {
                groupedRows.Add(groupName, new List<string[]>());
                groupedRows[groupName].Add(row);
            }
        }

        Dictionary<string, ChatSection> res = new Dictionary<string, ChatSection>();
        foreach (var key in groupedRows.Keys)
        {
            res.Add(key, BuildChatSeciton(groupedRows[key]));
        }

        return new ChatSectionByGroup
        {
            groups = res
        };
    }

    static private ChatSection BuildChatSeciton(List<string[]> groupedRows)
    {
        if (groupedRows.Count == 0)
        {
            return null;
        }
        // ǳ����
        List<string[]> trimRows = new List<string[]>();
        // ȥ�ո�
        foreach (string[] row in groupedRows)
        {
            trimRows.Add(
               row.Select<string, string>((d) => {
                   if (d == null)
                   {
                       return null;
                   }

                   return d.Trim();
               }).ToArray());
        }

        // �ҵ���һ������Ϊnormal����
        string[] headRow = trimRows.Find((r) =>
        {
            string type = r[1];
            return type == "normal";
        });

        if (headRow == null)
        {
            return null;
        }

        string type = headRow[1];

        ChatSection chatSection = new ChatSection
        {
            Id = int.Parse(headRow[0]),
            Name = headRow[4],
            Contents = new List<ChatContent>(new[] { new ChatContent
                {
                    Text = headRow[7]
                } }),
        };
        // nextָ��
        string nextString = headRow[5];
        string optionsString = headRow[6];
        if (optionsString != null && optionsString != "#")
        {
            // ѡ��ڵ�
            string[] optionIds = optionsString.Split('-');
            List<ChatOption> options = new List<ChatOption>();
            foreach (string optionId in optionIds)
            {
                int optionIndex = trimRows.FindIndex((d) => d[0] == optionId);
                if (optionIndex <= 0)
                {
                    continue;
                }
                options.Add(BuildChatOption(trimRows.GetRange(optionIndex, trimRows.Count - optionIndex)));
            }

            chatSection.ChatOptions = options;
        }// ��ͨ�ڵ�
        else if (nextString != null && nextString != "#")
        {
            int nextRowIndex = trimRows.FindIndex((d) => d[0] == nextString);
            if (nextRowIndex <= 0)
            {
                return null;
            }
            chatSection.Next = BuildChatSeciton(trimRows.GetRange(nextRowIndex, groupedRows.Count - nextRowIndex));
        }

        return chatSection;
    }

    static private ChatOption BuildChatOption(List<string[]> groupedRows)
    {
        if (groupedRows == null || groupedRows.Count == 0)
        {
            return null;
        }

        string[] headRow = groupedRows[0];
        ChatOption chatOption = new ChatOption
        {
            Id = int.Parse(headRow[0]),
            Text = headRow[7],
            ActionWhenChatOver = ParseAction(headRow[8])
        };

        // nextָ��
        string nextString = headRow[5];
        if (nextString != null && nextString != "#")
        {
            int nextRowIndex = groupedRows.FindIndex((d) => d[0] == nextString);
            if (nextRowIndex <= 0)
            {
                return null;
            }
            chatOption.Next = BuildChatSeciton(groupedRows.GetRange(nextRowIndex, groupedRows.Count - nextRowIndex));
        }

        return chatOption;
    }

    static ChatOption.Action ParseAction(string data)
    {
        switch (data) {
            case "battle":
                return ChatOption.Action.Battle;
            case "love":
                return ChatOption.Action.Love;
            case "join":
                return ChatOption.Action.Join;
            default:
                return ChatOption.Action.None;
        }
    }
}
