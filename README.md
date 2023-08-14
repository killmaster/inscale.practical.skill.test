# Practical skill test for .NET position

## Sumary

This is an implementation in .Net 6 of the technical challenge proposed.

First started with a console application (`dummy.proj1`) since it was what was requested, but after clearing some doubts I pivoted to a WebAPI (`dummy.api`).

## Some details

The project was implemented in .Net 6, with EF Core, connected to a Postgres database. The tests are using xUnit and Moq.

The `dummy.proj1` deals with populating the database consuming data from the `https://dummyjson.com/` API. For this I wrote a simple and crude service just to get the data I needed to solve the challenge. It also has queries in Linq for the points (2.a.i) and (3.a)

The `dummy.contracts` project is just a class library where I defined the expected requests, responses, and base data models for the WebAPI. This was so it could be released as a deliverable for someone who might want to use it. Kind of like a simplified version of a `wsdl` file in a SOAP API.

The `dummy.api` is the WebAPI. It has swagger injected so it displays some API documentation. It has AWS Lambda support, although the released version doesn't work on Lambda for reasons I'll explain further down. It injects the Database Context properly and adds the `DummyDBService` implmentation as Scoped but since state isn't a problem we could change it to Transient if we wanted it to scale better.

I've only implemented methods that solved the specific requests in the challenge document:

```
GET /api/Dummy/Todos
    With parameter:
        int NumberOfPosts

GET /api/Dummy/Users
    With parameters:
        int Reactions
        string Tag

GET /api/Dummy/Posts
    With parameter:
        string Cardtype
```

The `dummy.tests` project has a simple test for the controller.

## Queries in SQL

Although I've wrote the queries in EF and Linq in my service. I've thought best to do them also in SQL directly.

Query to get users with number of posts and todos, for posts with at least 1 reaction and "history" tag:

```sql
SELECT u."Id" AS "UserId", u."Username", u."FirstName", u."LastName",
       COUNT(DISTINCT p."Id") AS "TotalPosts",
       COUNT(DISTINCT t."Id") AS "TotalTodos"
FROM "Users" u
LEFT JOIN "Posts" p ON u."Id" = p."UserId"
LEFT JOIN "TodoModels" t ON u."Id" = t."UserId"
WHERE EXISTS (
    SELECT 1
    FROM "Posts" p2
    WHERE p2."UserId" = u."Id"
    AND p2."Reactions" > 0
    AND p2."Tags" @> ARRAY['history']
)
GROUP BY u."Id", u."Username", u."FirstName", u."LastName";
```

Query to find the Users with more than 2 posts and show their todo's:

```sql
SELECT DISTINCT t."Id" AS TodoId, t."Todo", t."Completed"
FROM "Users" u
JOIN "Posts" p ON u."Id" = p."UserId"
JOIN "TodoModels" t ON u."Id" = t."UserId"
WHERE (
    SELECT COUNT(*) FROM "Posts" WHERE "UserId" = u."Id"
) > 2;
```

Query to find Users that use `mastercard` as their cardtype and retrieve their posts:

```sql
SELECT DISTINCT p."Id" AS PostId, p."Reactions", p."Tags"
FROM "Users" u
JOIN "Bank" b ON u."Id" = b."UserId"
JOIN "Posts" p ON u."Id" = p."UserId"
WHERE b."CardType" = 'mastercard';

```

## AWS

So this was the part where I am least versed and spent most of my time. Having a monolithic and self-hosted/on-premise background wouldn't stop me from at least trying.

After analysing the solution I've decided that the best course of action was to deploy the database to an RDS instance and deploy the WebAPI as an AWS Lambda function.

So I did just that. There's a Postgres instance running on RDS. You can see the `appsettings.json` for the API, but it's missing the password since it's stored as a dotnet user-secret.
To replicate you just need to change the connection string to your own and run `dotnet ef database update` and it should just work.

I then deployed the WebAPI as a function using `dotnet lambda deploy-function`. But not before I changed some things in my code so it would run properly.

On the `DummyContext.cs` I had to remove references to the User Secrets, since they don't deploy with the package, and hardcode the connection string with password for the RDS instance. The correct way to do it was to use AWS Secrets Manager, but since it's paid I didn't get to try it yet.

I also created an API Gateway just to try it.

You can access it on [https://rdhodu5njf7fik5hdgevolrptm0ywbpd.lambda-url.eu-west-3.on.aws/](https://rdhodu5njf7fik5hdgevolrptm0ywbpd.lambda-url.eu-west-3.on.aws/) and the API Gateway version is on [https://nv79xin2z0.execute-api.eu-west-3.amazonaws.com/](https://nv79xin2z0.execute-api.eu-west-3.amazonaws.com/)

## Tools used

Used mostly Visual Studio Code with dotnet-cli, since I wanted to try developing without the full fledged IDE, but ended up using Visual Studio 2022 to test AWS.
Used Windows and Linux environments for developing.
Used docker on a home server to deploy an initial version of the database before migrating to AWS RDS.
