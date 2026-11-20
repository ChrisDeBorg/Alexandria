
INSERT INTO "RelationTypes"
    ("Id", "Name", "DisplayName", "Description",
     "IsSymmetric", "IsTransitive", "IsHierarchical",
     "InverseRelationTypeId", "CreatedAt", "UpdatedAt")
VALUES
(
    'a1000000-0000-0000-0000-000000000001',
    'worksFor', 'arbeitet für',
    'Eine Person arbeitet für eine Organisation.',
    FALSE, FALSE, FALSE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000002',
    'employs', 'beschäftigt',
    'Eine Organisation beschäftigt eine Person.',
    FALSE, FALSE, FALSE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000003',
    'owns', 'besitzt',
    'Eine Person oder Organisation besitzt eine Entity.',
    FALSE, FALSE, FALSE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000004',
    'ownedBy', 'ist im Besitz von',
    'Eine Entity befindet sich im Besitz einer Person oder Organisation.',
    FALSE, FALSE, FALSE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000005',
    'locatedIn', 'befindet sich in',
    'Eine Entity befindet sich an einem bestimmten Ort.',
    FALSE, TRUE, TRUE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000006',
    'contains', 'umfasst',
    'Ein Ort oder eine Organisation umfasst eine andere Entity.',
    FALSE, FALSE, TRUE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000007',
    'parentOf', 'ist Elternteil von',
    'Eine Person ist Elternteil einer anderen Person.',
    FALSE, FALSE, TRUE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000008',
    'childOf', 'ist Kind von',
    'Eine Person ist Kind einer anderen Person.',
    FALSE, FALSE, FALSE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000009',
    'knows', 'kennt',
    'Eine Person kennt eine andere Person.',
    TRUE, FALSE, FALSE, NULL, NOW(), NULL
),
(
    'a1000000-0000-0000-0000-000000000010',
    'spouseOf', 'ist verheiratet mit',
    'Zwei Personen sind miteinander verheiratet.',
    TRUE, FALSE, FALSE, NULL, NOW(), NULL
)
ON CONFLICT ("Name") DO NOTHING;


-- Inverse Beziehungen verknüpfen.
UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000002'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000001';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000001'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000002';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000004'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000003';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000003'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000004';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000006'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000005';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000005'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000006';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000008'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000007';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000007'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000008';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000010'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000009';

UPDATE "RelationTypes" SET "InverseRelationTypeId" =
    'a1000000-0000-0000-0000-000000000009'
WHERE "Id" = 'a1000000-0000-0000-0000-000000000010';


ALTER TABLE "RelationTypes"
ADD COLUMN IF NOT EXISTS "ParentRelationTypeId" uuid NULL;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'FK_RelationTypes_ParentRelationTypes'
    ) THEN
        ALTER TABLE "RelationTypes"
        ADD CONSTRAINT "FK_RelationTypes_ParentRelationTypes"
        FOREIGN KEY ("ParentRelationTypeId")
        REFERENCES "RelationTypes" ("Id")
        ON DELETE RESTRICT;
    END IF;
END $$;

