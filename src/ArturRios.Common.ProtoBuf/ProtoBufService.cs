using System.Reflection;
using System.Text;
using ProtoBuf;
using ProtoBuf.Meta;

namespace ArturRios.Common.ProtoBuf;

public static class ProtoBufService
{
    public static string GenerateDefinition(IEnumerable<Type> protoTypes, string packageName, string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("syntax = \"proto2\";");
        sb.AppendLine("");
        sb.AppendLine($"package {packageName};");
        sb.AppendLine("option optimize_for = LITE_RUNTIME;");
        sb.AppendLine("option cc_enable_arenas = true;");
        sb.AppendLine("");

        var getProto = typeof(RuntimeTypeModel).GetMethod("GetSchema", [typeof(Type)])!;
        var allProto = new Dictionary<string, string>();
        var removeNames = new List<string> { "\r", "package" };

        foreach (var type in protoTypes)
        {
            var result = (string)getProto.Invoke(RuntimeTypeModel.Default, [type])!;
            var splitMsg = result.Split(["message "], StringSplitOptions.None).ToList();

            foreach (var s in splitMsg)
            {
                var splitEnum = s.Split(["enum "], StringSplitOptions.None).ToList();
                var sMsg = splitEnum[0];
                var sMsgName = sMsg.Split(["{"], StringSplitOptions.None)[0];

                if (!sMsg.TrimStart().StartsWith("syntax =") &&
                    !sMsg.TrimStart().StartsWith("package") &&
                    removeNames.All(n => !sMsg.StartsWith(n)))
                {
                    var messageBodyStart = sMsg.IndexOf('{');
                    var messageBodyEnd = sMsg.LastIndexOf('}');
                    if (messageBodyStart != -1 && messageBodyEnd != -1)
                    {
                        var messageHeader = sMsg[..(messageBodyStart + 1)];
                        var messageBody = sMsg.Substring(messageBodyStart + 1, messageBodyEnd - messageBodyStart - 1);

                        var fields = messageBody.Split([';'], StringSplitOptions.RemoveEmptyEntries);
                        var labeledFields = new List<string>();

                        foreach (var prop in type.GetProperties())
                        {
                            var attr = prop.GetCustomAttribute<ProtoMemberAttribute>();

                            if (attr == null)
                            {
                                continue;
                            }

                            var label = attr.IsRequired ? "required" : "optional";
                            var field = fields.FirstOrDefault(f => f.Contains(prop.Name));
                            if (!string.IsNullOrEmpty(field))
                            {
                                labeledFields.Add($"{label} {field.Trim()};");
                            }
                        }

                        var newMessageBody = string.Join("\n  ", labeledFields);
                        var newMessage = messageHeader + "\n  " + newMessageBody + "\n}";
                        allProto[sMsgName] = "message " + newMessage;
                    }
                    else
                    {
                        allProto[sMsgName] = "message " + sMsg;
                    }
                }

                removeNames.Add(sMsgName);

                if (splitEnum.Count <= 1)
                {
                    continue;
                }

                for (var i = 1; i < splitEnum.Count; i++)
                {
                    var sEnum = splitEnum[i];
                    var sEnumName = sEnum.Split(["{"], StringSplitOptions.None)[0];

                    if (removeNames.All(n => !sEnum.StartsWith(n)))
                    {
                        allProto[sEnumName] = "enum " + sEnum;
                    }

                    removeNames.Add(sEnumName);
                }
            }
        }

        var proto = string.Join(string.Empty, allProto.OrderBy(a => a.Key).Select(a => a.Value));
        sb.Append(proto);
        var protoContent = sb.ToString();

        SaveToFile(packageName, protoContent, path);

        return protoContent;
    }

    private static void SaveToFile(string protoFileName, string protoContent, string path)
    {
        if (string.IsNullOrWhiteSpace(protoFileName))
        {
            return;
        }

        if (!protoFileName.EndsWith(".proto"))
        {
            protoFileName += ".proto";
        }

        var filePath = Path.Combine(path, protoFileName);

        File.WriteAllText(filePath, protoContent);
    }
}
