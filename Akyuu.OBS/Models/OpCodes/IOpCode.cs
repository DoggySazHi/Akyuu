using System.Reflection;
using JetBrains.Annotations;

namespace Akyuu.OBS.Models.OpCodes;

public interface IOpCode
{
    public OBSMessage<IOpCode> BuildMessage()
    {
        var attribute = GetType().GetCustomAttribute<OpCodeAttribute>();
        if (attribute == null)
            throw new InvalidOperationException("IOpCode with unassigned opcode");
        // redesigning this a third time for a required getter in the interface but i'm lazy now
        
        return new OBSMessage<IOpCode>(this, attribute.OpCode);
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, Inherited = false)]
[BaseTypeRequired(typeof(IOpCode))]
[MeansImplicitUse]
public class OpCodeAttribute : Attribute
{
    public int OpCode { get; }

    public OpCodeAttribute(int opCode)
    {
        OpCode = opCode;
    }
}