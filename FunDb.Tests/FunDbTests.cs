using static FunDb.__FunDbInternals;

namespace FunDb.Tests;

public class CRUDTest
{
    public class MyRecord
    {
        public int Id { get; set; }
        public string Message { get; set; } = "";
    }

    public class MyOtherRecord
    {
        public int Id { get; set; }
    }

    [Fact]
    public async Task Insert_sets_Id()
    {
        var db = new Db();

        // Arrange 1
        var firstInsert = new MyRecord();
        var secondInsert = new MyRecord();

        // Act 1
        var id1 = await db.InsertAsync(firstInsert);
        var id2 = await db.InsertAsync(secondInsert);

        // Assert 1
        Assert.Equal(1, firstInsert.Id);
        Assert.Equal(id1, firstInsert.Id);
        Assert.Equal(2, secondInsert.Id);
        Assert.Equal(id2, secondInsert.Id);


        /* The incremented Id starts at 1 for each table. */

        // Arrange 2
        var otherTableInsert = new MyOtherRecord();

        // Act 2
        var otherId = await db.InsertAsync(otherTableInsert);

        // Assert 2
        Assert.Equal(1, otherTableInsert.Id);
        Assert.Equal(otherId, otherTableInsert.Id);
    }

    [Fact]
    public async Task QuerySingle()
    {
        var db = new Db();

        var id1 = await db.InsertAsync(new MyRecord { Message = "Hello world!" });
        var id2 = await db.InsertAsync(new MyRecord { Message = "Hello a second time!" });

        Assert.Equal("Hello world!", (await db.QuerySingleAsync<MyRecord>(x => x.Id == id1)).Message);
        Assert.Equal("Hello a second time!", (await db.QuerySingleAsync<MyRecord>(x => x.Id == id2)).Message);
    }


    [Fact]
    public async Task QuerySingle_throws()
    {
        var db = new Db();

        var id1 = await db.InsertAsync(new MyRecord { Message = "Hello world!" });
        var id2 = await db.InsertAsync(new MyRecord { Message = "Hello a second time!" });

        var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await db.QuerySingleAsync<MyRecord>(x => x.Message.Contains("e")));
        Assert.Contains("more than one", ex.Message);
    }

    [Fact]
    public async Task Query()
    {
        // Arrange
        var db = new Db();
        var id1 = await db.InsertAsync(new MyRecord { Message = "Hello world!" });
        var id2 = await db.InsertAsync(new MyRecord { Message = "Hello a second time!" });

        // Act & assert 1
        var record1 = Assert.Single(await db.QueryAsync<MyRecord>(x => x.Message.Contains("world")));
        Assert.Equal("Hello world!", record1.Message);

        // Act & assert 2
        var record2 = Assert.Single(await db.QueryAsync<MyRecord>(x => x.Message.Contains("second")));
        Assert.Equal("Hello a second time!", record2.Message);

        // Act & assert 3
        var records = await db.QueryAsync<MyRecord>(x => x.Message.Contains("e"));
        Assert.Equal(2, records.Count());
    }

    [Fact]
    public async Task Updates_do_not_need_a_method()
    {
        // Arrange
        var db = new Db();
        var originalRecord = new MyRecord { Message = "Hello world!" };
        await db.InsertAsync(originalRecord);

        // Act
        originalRecord.Message = "Bye!";

        // Assert
        var retrieved = await db.QuerySingleAsync<MyRecord>(x => x.Id == originalRecord.Id);
        Assert.Equal("Bye!", retrieved.Message);
    }

    [Fact]
    public async Task QuerySingleOrDefault()
    {
        var db = new Db();
        Assert.Null(await db.QueryFSingleOrDefaultAsync<MyRecord>(x => x.Message == "not fuond"));
    }
}