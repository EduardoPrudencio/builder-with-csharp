OrgChartMember builder = OrgChartMember.Create("1", "Mário")
    .AddSubordinate("2", "Pedro", b => b
        .AddSubordinate("3", "Jorge")
    )
    .AddSubordinate("4", "João");

    Console.WriteLine(builder.Team.Count);

public sealed class OrgChartMember
{
    public string Id { get; private set; }
    public string Name { get; private set; }

    private OrgChartMember(string id, string name)
    {
       (Id, Name) = (id, name);
    }
    
    Lazy<List<OrgChartMember>> _team = new();
    public IReadOnlyList<OrgChartMember> Team => _team.Value;

    public static OrgChartBuilder Create(string id, string name)
        => new OrgChartBuilder( new OrgChartMember(id, name));
    
    public class OrgChartBuilder 
    {
        readonly OrgChartMember _root;

        public OrgChartBuilder(OrgChartMember root)
        {
            _root = root;
        }

        public OrgChartBuilder AddSubordinate(string id, string name)
        {
            _root._team.Value.Add(new OrgChartMember(id, name));
            return this;
        }

        public OrgChartBuilder AddSubordinate(string id, string name, Action<OrgChartBuilder> builderAction)
        {
            var b = new OrgChartBuilder( new OrgChartMember(id, name));
            builderAction(b);
            _root._team.Value.Add(b.Build());
            return this;
        }

        public OrgChartMember Build() => _root;

        public static implicit operator OrgChartMember(OrgChartBuilder input) => input._root;
    }        
}