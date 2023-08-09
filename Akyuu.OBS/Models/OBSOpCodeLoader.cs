using System.Reflection;
using Akyuu.OBS.Models.OpCodes;

namespace Akyuu.OBS.Models;

internal static class OBSOpCodeLoader
{
    private static readonly Dictionary<int, Type> _opCodes = new();

    static OBSOpCodeLoader()
    {
        var asm = typeof(IOpCode).Assembly;
        foreach (var type in asm.GetTypes())
        {
            var attribute = type.GetCustomAttribute<OpCodeAttribute>();
            if (attribute != null)
            {
                _opCodes[attribute.OpCode] = type;
            }
        }
    }

    public static Type GetDataType(int opCode)
    {
        if (!_opCodes.TryGetValue(opCode, out var test))
        {
            throw new InvalidOperationException($"Opcode {opCode} unimplemented");
        }

        return test;
    }
}