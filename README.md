# Database Integration Tests Sample

Project purpose is to demonstrate how integration tests *may* be done.

Solution consists of two projects *DAL* - demo database abstraction layer library and *Tests* - actual integration tests.

*DAL* has two sample entites and repositories *Users* ([Dapper Fast CRUD](https://github.com/MoonStorm/Dapper.FastCRUD)) and *Posts* ([Entity Framework](https://github.com/aspnet/EntityFramework))

## How it works

Main key point is to run each test in [transaction](https://msdn.microsoft.com/en-US/library/ms174377.aspx), after test run no matter what test results are we are [rolling back](https://msdn.microsoft.com/en-us/library/ms181299.aspx) transaction - that allow us to run tests again and again.

Also before running tests we are initializing database. This step may be optional if test database is always available or for example you may recreate database each time. In this demo database is being created only once.

## Sample

```csharp
[TestClass]
public class SampleRepositoryTests
{
    private TransactionScope _trans;

    /// <summary>
    /// Runs once
    ///
</summary>
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
    {
        // Optional: create/update database if needed
    }

    /// <summary>
    /// Runs before each test
    /// Starts new transaction
    ///
</summary>
    [TestInitialize]
    public void InitializeTest()
    {
        _trans = new TransactionScope();
    }

    [TestMethod]
    public void TestMethod1()
    {
        // Arrange
        var total = Repository.Total();
        // Act
        Repository.Save(new Sample());
        // Assert
        Assert.AreEqual(total + 1, Repository.Total());
    }

    /// <summary>
    /// Runs after each test
    /// Rollback transaction
    ///
</summary>
    [TestCleanup]
    public void Cleanup()
    {
        _trans.Dispose();
    }
}
```