using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//updated 05.01.26
namespace TaskManager.Domain.Entities;

public class UserRole
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}





/*
CREATE TABLE IF NOT EXISTS public."User_Roles"
(
    "UserID" uuid NOT NULL,
    "RoleID" uuid NOT NULL,
    CONSTRAINT "RoleID_fkey" FOREIGN KEY ("RoleID")
        REFERENCES public."Roles" ("RolesID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
        NOT VALID,
    CONSTRAINT "UserID_fkey" FOREIGN KEY ("UserID")
        REFERENCES public."Users" ("UserID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
        NOT VALID
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."User_Roles"
    OWNER to postgres;*/





