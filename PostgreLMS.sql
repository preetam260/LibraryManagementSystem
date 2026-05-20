INSERT INTO "MembershipTypes" ("Id", "Name", "MaxBorrowLimit", "MaxBorrowDays")
VALUES (1, 'Basic', 2, 7), (2, 'Student', 3, 10), (3, 'Premium', 5, 15);

INSERT INTO "BookCategories" ("Name") VALUES ('Programming'), ('Databases'), ('Networks');

-- stored procedure that calculates fine per member
CREATE OR REPLACE FUNCTION calculate_member_fine(member_id_param INT)
RETURNS NUMERIC AS
$$
BEGIN
    RETURN (
        SELECT COALESCE(SUM(f."Amount"),0)
        FROM "Fines" f
        JOIN "Borrowings" b
        ON f."BorrowingId" = b."Id"
        WHERE b."MemberId" = member_id_param
        AND f."IsPaid" = false
    );
END;
$$ LANGUAGE plpgsql;

-- to test fine generation
update "Borrowings" 
set "DueDate" = NOW() - INTERVAL '5 DAYS'
WHERE "Id" = 4;

select * from "Fines";

-- calling the stored procedure
select calculate_member_fine(6);


select * from "BookCopies";

select * from "Borrowings";

select * from "Fines";


-- generating fine for simulation of fine limit block
INSERT INTO "Fines" ("Amount", "IsPaid", "BorrowingId") VALUES (600, false, 1);

-- generating fines via fake duedate 
UPDATE "Borrowings" SET "DueDate" = NOW() - INTERVAL '4 days' WHERE "Id" = 17;

SELECT "DueDate" FROM "Borrowings" WHERE "Id" = 17;
SELECT "Id", "Status" FROM "BookCopies" WHERE "Id" = 18;

