using System.IO;
using JetBrains.ReSharper.Feature.Services.StackTraces.StackTrace;
using JetBrains.ReSharper.Feature.Services.StackTraces.StackTrace.Nodes;
using JetBrains.Util;
using RiderPlugin.UnrealLink.Parsing.Visitor;

namespace RiderPlugin.UnrealLink.Parsing.Node.ScriptStack
{
    public class ScriptStackHeaderNode : StackTraceNode
    {
        public ScriptStackHeaderNode(TextRange range) : base(range)
        {
        }

        public override void Accept(StackTraceVisitor visitor)
        {
            ((UnrealLogVisitor) visitor).VisitScriptStackHeaderNode(this);
        }

        public override void Dump(TextWriter writer)
        {
            writer.Write("{ScriptStackHeader: ");
            writer.Write("}");
        }
    }
}