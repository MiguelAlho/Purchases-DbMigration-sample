# Purchases-DbMigration-sample
A sample web service that describes a db automation setup

This repo demos a possible setup for app-to-database integration testing, in a .Net core app (actually for fw 451 due to unported dependencies). It was used fora talk I did with Eduardo Piairo, about Database migrations @PortoData: http://www.portodata.net/xxi-evento-porto-data-28-julho-2016-uptec/. 

The sample's "evolution" can be analysed though the "step branches". It is important to consider that there is a backing story to this set of steps - consider that, in an Agile manner, the team is building a new service and trying to prove some concepts as they go along, but want to move securely.

##step 1 : no database

The "step1-no_db" branch has an initial setup of a REST api, with static data. It has a simple .NetCore webapi project, that integrates Swagger. There is a very simple controller that is accessable through a GET request that returns a list of `Person`. Person is a simple class that represents a person in this domain and is composed of an Id and a (full) Name. The controller gets a list of Person through an InMemeroy implementation of the IPersonRepository that simply contains static data.

Interesting aspects of this "design":
 - Person is intended to be immutable, as a DTO. If we wanted a domain model (in the sense of an aggregate) it would probably not be that , nor that simple. Person in this case is more of a ViewModel. The example is simple enough to allow this.
 - We use the repository pattern to seperate data access from it's use. The team has the intention of storing Person objects somewhere. But, at the time that they attacked this story, they were not sure what database would be used or necessary. Really, they just wanted to prove that the service could work, get something that other teams (UI, for instance) could use, and get there build and delivery pipeline going right from the start
 - We have a container available from the start, so we can register the `InMemoryPersonRepository` implementation as the one that implements `IPersonRepository`
 - A unit test project is available to run tests for the classes, using xUnit and Moq

This implementation is really enough to prove concepts and get the build and delivery pipeline going

##step 2 - intial sql scripts and repo implementation

So the team was able to prove that the person model was ok and the frontend team is already using the deployed service for implementing the frontend of the app, even though the data is static. The team is going to evolve the service and has settled on using SQL Server as a database for this service.

The next story here is really implementing the SqlServerPErsonRepository - an implementation of the `IPersonRepository` interface that is designed to connect to and query SqlServer databases for Person objects.

Looking at the code in the "step2-create_sql_person" branch, the only changes in the main app are the inclusion of the `DbConfiguration` class (eses .NetCore's Options Model to pass configuration data around) and `SqlServerPersonRepository` that implements the data layer using ADO.Net SQLCLient data provider. Dapper would also fit really nicely here. The `GetListOfPersons`method basiccly opens a connection, executes a query, and creates Person objects based on the data in the reader.

Two important things to consider here: 
- both the controller and the `Person` have not changed to implement the new repository, so the unit tests for these classes should stay the same.
- because we are using SQL, it is very useful to validate that the SQL we are using is actually correct, and that the reader-to-object mapping code actually works. The second item is possible with unit tests, buy hard and verbose. Integration testing works very well here for this. We can still have pretty fast tests IF we limit the test surface to just the repository class/interface's surface. Testing from the controller down to the database and back adds too many elements and makes setup harder. Limiting the test surface to the repository is a nice tradeoff between speed and complexity, while warenting good coverage and confidence.

Because the app will use a database, the team has considered the following:
- they want to be able to deliver continuously. As such, they found that a db migration strategy is important. They have opted to implement the migrations as simple, ordered, sql scripts, and will use both the scripts as test db schema generators and as deployment artifacts in the CI/CD pipeline.

Since the production database will be SqlServer, they consider LocalDb to be a fine element to include in the test strategy. LocalDb has everything they need for now, and the same SQL scripts and clients work correctly. In order to manage the migrations, the team has opted for two compatible strategies. For CI/CD deployments, they will use flyway to manage the migration, since the ops team has plenty of experience from other projects in using it. For the tests, and since Flyway cannot connect to LocalDb to perform the migration, they will use DBUp in the test project to perfrom the migration.

The `RestApi.SqlServer.IntegrationTests`project includes the relevant code for testing and manages the SQLScripts for the migrations. Scripts are placed in the project's sql folder and can be picked up and packaged in the build process. For this step in the story, a single script is necessary and includes the `Create Table Person(...)` command. For the demo, we can show tests working for us by creating invalid commands (like using `uuid` instead of `uniqueidentifier`, to simulate dev errors). A key element in this project is the `DbTestFixture` class that has the setup code for the tests. All test classes in this project will/should implement `IClassFixture<DbTestFixture>` allowing this class to be constructed a single time in the test run. The `CreateTestDb()` method gets called in this process and does the following:
- Makes sure the test db folder is available
- clearing any previous database, including detatching it
- creating a new database file for this test run (connecting to the server)
- Rebuilding the schema (connectiong to the database)

Rebuilding the schema is done with DBup, by creating an `UpgradeEngine` instance (using the `DeployChanges` builder with the scripts in the sql folder) and calling `PerformUpgrade()`. `DbTestFixture` also exposes Connection and ConnectionString getters, and the `ClearRecords` method that can be used to clear out any data that previous tests may have included. It should be called before each test is run.

`SqlServerPersonRepositoryTests` implements a pair of tests that actually use the database. Notice that at the beginning of each test there isa call to `_fixture.ClearRecords()`to clear out data. Also, any data that is necessary in the database for the test can be included next, as the `CanReadPersonListFromDatabase` test does in adding persons to the database. This involves creating mechanisms to add data in a controlled manner, in order for assertiosn to be correct. `AddPersonsToDatabase()`adds two records that the team controls completely, and can Assert on.

With all of this, the build pipline can add script packaging steps and the deployment process can start with a database migration step, using Flyway to run the unapplied scripts.

##step 3 - changes in business

So, The service is a success, and as usual, one client has a request. They would like that the first and last name be seperated since it would be easier and useful for other types of operations and stories they would like to include as they move foward. That team has agreed to include these changes in the next iteration. The Ops team has run some requested analysis on the table and verified that all the records are simply first and last name, seperated by a space, and can easily be changed without data loss.

The `step3-aleter_sql_person`branch is the result of this change. `Person` now includes `FirstName` and `LastName` properties, but maintains the Name property for API compatibility. `SqlServerPersonRepository`has also slightly changed - the query garantees the new columns can be obtained, and the reader maps the colomns to the object correctly. Demowise, we can show how tests help us catch errors in the part of the code while performing the changes (bad column names in the query and reader, missing column at the test db , etc.).

At the integration test project, we've included a new script to run the chnages as a migration. The migration here involves add the new columns (preserving the existing ones), moving data from the original Name column to the new columns, and then optionally droping the name column (depending on guaranteeing that nothing else is using it, as in our case here). Test ise, slight changes were made to reflect the changes in schema and assert on the new expectations. This is where we may feel most of the burden of managing and maintaining the integration test suite, though the benifits of having it far outweight its absence.

With everything implemented, tests passing and the build job building, the new version can be deployed at any time.


###Note:

The code may not be completely correct (such as using `SELECT * ` but during the demo those changes anc corrections can be performed to show off the test suite helping use change the code confidently.

This repo actually has absolutely nothing to do with purchases, but my initial idea for the sample domain was commerce. The only idea that stood through was really the Person entity, and was enough to show the idea and how-to


