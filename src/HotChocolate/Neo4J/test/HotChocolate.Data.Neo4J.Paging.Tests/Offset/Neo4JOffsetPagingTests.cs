using CookieCrumble;
using HotChocolate.Execution;

namespace HotChocolate.Data.Neo4J.Paging;

public class Neo4JOffsetPagingTests : IClassFixture<Neo4JFixture>
{
    private readonly Neo4JFixture _fixture;

    private const string FooEntitiesCypher = @"
            CREATE
                (:Foo {Bar: 'a'}),
                (:Foo {Bar: 'b'}),
                (:Foo {Bar: 'd'}),
                (:Foo {Bar: 'e'}),
                (:Foo {Bar: 'f'})";

    private sealed class Foo
    {
        public string Bar { get; set; } = default!;
    }

    public Neo4JOffsetPagingTests(Neo4JFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task OffsetPaging_SchemaSnapshot()
    {
        // arrange
        var tester = await _fixture.GetOrCreateSchema<Foo>(FooEntitiesCypher);
        tester.Schema.MatchSnapshot();
    }

    [Fact]
    public async Task Simple_StringList_Default_Items()
    {
        // arrange
        var tester = await _fixture.GetOrCreateSchema<Foo>(FooEntitiesCypher);

        // act
        // assert
        var res1 = await tester.ExecuteAsync(
            @"{
                root {
                    items {
                        bar
                    }
                    pageInfo {
                        hasNextPage
                        hasPreviousPage
                    }
                }
            }");

        await SnapshotExtensions.Add(
                Snapshot
                    .Create(), res1)
            .MatchAsync();
    }

    [Fact]
    public async Task Simple_StringList_Take_2()
    {
        // arrange
        var tester = await _fixture.GetOrCreateSchema<Foo>(FooEntitiesCypher);

        //act
        var result = await tester.ExecuteAsync(
            @"{
                root(take: 2) {
                    items {
                        bar
                    }
                    pageInfo {
                        hasNextPage
                        hasPreviousPage
                    }
                }
            }");

        // assert
        await SnapshotExtensions.Add(
                Snapshot
                    .Create(), result)
            .MatchAsync();
    }

    [Fact]
    public async Task Simple_StringList_Take_2_After()
    {
        // arrange
        var tester = await _fixture.GetOrCreateSchema<Foo>(FooEntitiesCypher);

        // act
        var result = await tester.ExecuteAsync(
            @"{
                root(take: 2 skip: 2) {
                    items {
                        bar
                    }
                    pageInfo {
                        hasNextPage
                        hasPreviousPage
                    }
                }
            }");

        // assert
        await SnapshotExtensions.Add(
                Snapshot
                    .Create(), result)
            .MatchAsync();
    }
}
