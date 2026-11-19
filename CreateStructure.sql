
BEGIN;

-- 1. Gemeinsame Entity-Tabelle (TPH)
CREATE TABLE IF NOT EXISTS "Entities"
(
    "Id" uuid NOT NULL,
    "Name" varchar(500) NOT NULL,
    "Description" varchar(4000),
    "EntityTypeId" uuid NOT NULL,
    "MetadataJson" jsonb,

    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,

    -- EF-Core TPH Shadow Property
    "Discriminator" text NOT NULL,

    -- Entity
    "ShortName" varchar(200),
    "IsVerified" boolean NOT NULL DEFAULT false,

    -- Person
    "BirthDate" date,
    "DeathDate" date,
    "GivenName" varchar(200),
    "FamilyName" varchar(200),
    "CallingName" varchar(200),
    "AdditionalName" varchar(200),
    "AlternateNames" text[] NOT NULL DEFAULT ARRAY[]::text[],
    "Gender" varchar(50),
    "Ethnicity" varchar(200),

    -- Organization
    "FoundingDate" date,
    "DissolutionDate" date,
    "Website" varchar(1000),

    -- Company
    "StockSymbol" varchar(20),
    "Exchange" varchar(100),
    "EmployeeCount" integer,
    "AnnualRevenue" numeric(20,2),

    CONSTRAINT "PK_Entities" PRIMARY KEY ("Id"),

    CONSTRAINT "CK_Entities_Discriminator"
        CHECK ("Discriminator" IN
            ('BaseEntity', 'Entity', 'Person', 'Organization', 'Company'))
);

CREATE INDEX IF NOT EXISTS "IX_Entities_Name"
    ON "Entities" ("Name");


-- 2. RelationTypes
CREATE TABLE IF NOT EXISTS "RelationTypes"
(
    "Id" uuid NOT NULL,
    "Name" varchar(200) NOT NULL,
    "DisplayName" varchar(200) NOT NULL,
    "Description" varchar(4000),

    "IsSymmetric" boolean NOT NULL,
    "IsTransitive" boolean NOT NULL,
    "IsHierarchical" boolean NOT NULL,

    "InverseRelationTypeId" uuid,

    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,

    CONSTRAINT "PK_RelationTypes" PRIMARY KEY ("Id"),

    CONSTRAINT "FK_RelationTypes_InverseRelationTypes"
        FOREIGN KEY ("InverseRelationTypeId")
        REFERENCES "RelationTypes" ("Id")
        ON DELETE RESTRICT
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_RelationTypes_Name"
    ON "RelationTypes" ("Name");


-- 3. Relations
CREATE TABLE IF NOT EXISTS "Relations"
(
    "Id" uuid NOT NULL,

    "SourceEntityId" uuid NOT NULL,
    "TargetEntityId" uuid NOT NULL,
    "RelationTypeId" uuid NOT NULL,

    "ValidFrom" date,
    "ValidTo" date,
    "Context" varchar(4000),
    "IsInferred" boolean NOT NULL,

    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,

    CONSTRAINT "PK_Relations" PRIMARY KEY ("Id"),

    CONSTRAINT "FK_Relations_SourceEntity"
        FOREIGN KEY ("SourceEntityId")
        REFERENCES "Entities" ("Id")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_Relations_TargetEntity"
        FOREIGN KEY ("TargetEntityId")
        REFERENCES "Entities" ("Id")
        ON DELETE RESTRICT,

    CONSTRAINT "FK_Relations_RelationTypes"
        FOREIGN KEY ("RelationTypeId")
        REFERENCES "RelationTypes" ("Id")
        ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_Relations_SourceEntityId"
    ON "Relations" ("SourceEntityId");

CREATE INDEX IF NOT EXISTS "IX_Relations_TargetEntityId"
    ON "Relations" ("TargetEntityId");

CREATE INDEX IF NOT EXISTS "IX_Relations_RelationTypeId"
    ON "Relations" ("RelationTypeId");

COMMIT;



CREATE TABLE "EntityTypes"
(
    "Id" uuid PRIMARY KEY,
    "Name" varchar(200) NOT NULL,
    "DisplayName" varchar(200) NULL,
    "Description" varchar(4000) NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "ParentTypeId" uuid NULL
);

CREATE UNIQUE INDEX "IX_EntityTypes_Name"
    ON "EntityTypes" ("Name");

ALTER TABLE "EntityTypes"
    ADD CONSTRAINT "FK_EntityTypes_EntityTypes_ParentTypeId"
    FOREIGN KEY ("ParentTypeId")
    REFERENCES "EntityTypes" ("Id")
    ON DELETE RESTRICT;

-- Bestehende EntityTypeIds übernehmen.
INSERT INTO "EntityTypes"
    ("Id", "Name", "DisplayName", "Description", "CreatedAt")
SELECT DISTINCT
    e."EntityTypeId",
    'legacy_' || e."EntityTypeId"::text,
    NULL,
    'Automatisch übernommener EntityType. Bitte fachlich zuordnen.',
    CURRENT_TIMESTAMP
FROM "Entities" e
WHERE NOT EXISTS
(
    SELECT 1
    FROM "EntityTypes" t
    WHERE t."Id" = e."EntityTypeId"
);

ALTER TABLE "Entities"
    ADD CONSTRAINT "FK_Entities_EntityTypes_EntityTypeId"
    FOREIGN KEY ("EntityTypeId")
    REFERENCES "EntityTypes" ("Id")
    ON DELETE RESTRICT;

GRANT SELECT, INSERT, UPDATE, DELETE
ON TABLE "EntityTypes"
TO kb_user;

GRANT SELECT, INSERT, UPDATE, DELETE
ON ALL TABLES IN SCHEMA public
TO kb_user;