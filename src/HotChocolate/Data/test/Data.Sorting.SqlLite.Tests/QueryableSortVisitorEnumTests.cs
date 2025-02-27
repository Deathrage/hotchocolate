using System.Threading.Tasks;
using CookieCrumble;
using HotChocolate.Execution;

namespace HotChocolate.Data.Sorting.Expressions;

public class QueryableSortVisitorEnumTests
    : IClassFixture<SchemaCache>
{
    private static readonly Foo[] _fooEntities =
    {
        new() { BarEnum = FooEnum.BAR },
        new() { BarEnum = FooEnum.BAZ },
        new() { BarEnum = FooEnum.FOO },
        new() { BarEnum = FooEnum.QUX }
    };

    private static readonly FooNullable[] _fooNullableEntities =
    {
        new() { BarEnum = FooEnum.BAR },
        new() { BarEnum = FooEnum.BAZ },
        new() { BarEnum = FooEnum.FOO },
        new() { BarEnum = null },
        new() { BarEnum = FooEnum.QUX }
    };

    private readonly SchemaCache _cache;

    public QueryableSortVisitorEnumTests(SchemaCache cache)
    {
        _cache = cache;
    }

    [Fact]
    public async Task Create_Enum_OrderBy()
    {
        // arrange
        var tester = _cache.CreateSchema<Foo, FooSortType>(_fooEntities);

        // act
        var res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery("{ root(order: { barEnum: ASC}){ barEnum}}")
                .Create());

        var res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery("{ root(order: { barEnum: DESC}){ barEnum}}")
                .Create());

        // assert
        await SnapshotExtensions.Add(
                SnapshotExtensions.Add(
                    Snapshot
                        .Create(), res1, "ASC"), res2, "DESC")
            .MatchAsync();
    }

    [Fact]
    public async Task Create_Enum_OrderBy_Nullable()
    {
        // arrange
        var tester = _cache.CreateSchema<FooNullable, FooNullableSortType>(
            _fooNullableEntities);

        // act
        var res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery("{ root(order: { barEnum: ASC}){ barEnum}}")
                .Create());

        var res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery("{ root(order: { barEnum: DESC}){ barEnum}}")
                .Create());

        // assert
        await SnapshotExtensions.Add(
                SnapshotExtensions.Add(
                    Snapshot
                        .Create(), res1, "ASC"), res2, "DESC")
            .MatchAsync();
    }

    public class Foo
    {
        public int Id { get; set; }

        public FooEnum BarEnum { get; set; }
    }

    public class FooNullable
    {
        public int Id { get; set; }

        public FooEnum? BarEnum { get; set; }
    }

    public enum FooEnum
    {
        FOO,
        BAR,
        BAZ,
        QUX
    }

    public class FooSortType : SortInputType<Foo>
    {
    }

    public class FooNullableSortType : SortInputType<FooNullable>
    {
    }
}
