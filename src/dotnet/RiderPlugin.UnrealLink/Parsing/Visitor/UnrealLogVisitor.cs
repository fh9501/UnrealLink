using JetBrains.ReSharper.Feature.Services.StackTraces.StackTrace;
using JetBrains.ReSharper.Feature.Services.StackTraces.StackTrace.Nodes;
using RiderPlugin.UnrealLink.Parsing.Node;
using RiderPlugin.UnrealLink.Parsing.Node.ScriptStack;
using IdentifierNode = RiderPlugin.UnrealLink.Parsing.Node.IdentifierNode;

namespace RiderPlugin.UnrealLink.Parsing.Visitor
{
    public abstract class UnrealLogVisitor : StackTraceVisitor
    {
        protected UnrealLogVisitor(StackTraceOptions options) : base(options)
        {
        }

        public override void VisitCompositeNode(CompositeNode node)
        {
            if (node == null)
                return;

            foreach (var childNode in node.Nodes)
                childNode.Accept(this);
        }

        public abstract void VisitIdentifierNode(IdentifierNode identifierNode);
        public abstract void VisitScriptStackHeaderNode(ScriptStackHeaderNode scriptStackHeaderNode);
        public abstract void VisitBlueprintPathNode(BlueprintPathNode blueprintPathNode);
        public abstract void VisitScriptStackFrameOuterNode(ScriptStackFrameOuterNode scriptStackFrameOuterNode);
        public abstract void VisitScriptStackFrameInnerNode(ScriptStackFrameInnerNode scriptStackFrameInnerNode);
        public abstract void VisitScriptMsgHeaderNode(ScriptMsgHeaderNode scriptMsgHeaderNode);
    }
}