# In Touch database

The app expects a simple sqlite database with a few tables. 
  * **Collection** - A group of contacts. See work and personal contacts separately.
  * **Contact** - This is a list of businesses or people.
  * **ContactMethod** - For each contact, this is a list of methods to communicate with them. (Ex: phone, email, etc.)
  * **Communication** - For each contact, this is a list of all the communication you have documented.

## Connection String
The API needs a connection string to your database. **Update the path to your database** in the of `ConnectionStrings` section of `intouch/api/appsettings.json`!

## SQL

```sql
CREATE TABLE IF NOT EXISTS "Collection" (
  "Id" integer PRIMARY KEY NOT NULL,
  "Name" varchar(128) NOT NULL,
  "Created" timestamp(128) NOT NULL,
  "Modified" timestamp(128) NOT NULL,
  "Description" text,
  "Deleted" integer NOT NULL DEFAULT(0)
);
```

```sql
INSERT INTO Collection (Name, Created, Modified, Description, Deleted) VALUES ('Default', '2022-05-07 01:25:48.902596', '2022-05-07 01:25:48.902596', 'The default collection.', 0);
```

```sql
CREATE TABLE IF NOT EXISTS "Contact" (
  "Id" integer PRIMARY KEY AUTOINCREMENT NOT NULL,
  "Name" varchar(128) NOT NULL,
  "Created" timestamp(128) NOT NULL,
  "Modified" timestamp(128) NOT NULL,
  "Description" text,
  "Deleted" integer NOT NULL DEFAULT(0)
);
```

```sql
CREATE TABLE IF NOT EXISTS "ContactMethod" (
  "Id" integer PRIMARY KEY AUTOINCREMENT NOT NULL,
  "Type" varchar NOT NULL,
  "Created" timestamp(128) NOT NULL,
  "Modified" timestamp(128) NOT NULL,
  "ContactId" integer NOT NULL,
  "Value" varchar NOT NULL,
  "Note" text,
  "Deleted" integer NOT NULL DEFAULT(0),
  FOREIGN KEY (ContactId) REFERENCES "Contact" (Id)
);
```

```sql
CREATE TABLE IF NOT EXISTS "Communication" (
  "Id" integer PRIMARY KEY AUTOINCREMENT NOT NULL,
  "Created" timestamp(128) NOT NULL,
  "Modified" timestamp(128) NOT NULL,
  "Description" text NOT NULL,
  "ExpectMeToFollowUp" integer NOT NULL,
  "ExpectContactToFollowUp" integer NOT NULL,
  "ContactId" integer NOT NULL,
  "When" timestamp(128) NOT NULL,
  "Deleted" integer NOT NULL DEFAULT(0),
  FOREIGN KEY (ContactId) REFERENCES "Contact" (Id)
);
```