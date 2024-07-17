using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class GraphForTypesHierarchy
{
    public readonly Node head;

    private GraphForTypesHierarchy(Node head)
    {
        this.head = head;
    }

    public struct Meta
    {
        public Node node;
        public MemberInfo member;
    }
    public class Node
    {
        public readonly Type type;
        public Node @in;
        public int layer;
        public List<Node> Outs { get; } = new List<Node>();
        public List<Meta> Reverse { get; } = new List<Meta>();
        public List<Meta> Navigations { get; } = new List<Meta>();

        private Node inTemp;
        private int layerCurrent;

        public Node(Type type)
        {
            this.type = type;
        }

        public Node Discover(Dictionary<Type, Node> nodesByTypes)
        {
            var fields = ReflectionsHelperForFields.GetAllInstanceFields(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var type = field.FieldType;
                if (nodesByTypes.TryGetValue(type, out var node))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
        }
    }

    public static GraphForTypesHierarchy CreateGraph(Type startType)
    {
        var start = new Node(startType);
        var nodesByTypes = new Dictionary<Type, Node>() { [startType] = start };
        var head = start.Discover(nodesByTypes);
        return new GraphForTypesHierarchy(head);
    }
}