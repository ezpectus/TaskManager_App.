using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//updated 05.01.26
namespace TaskManager.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;

    public List<UserRole> UserRoles { get; set; } = new();
}



// This class represents a role in the Task Manager domain, which can be assigned to users.

/*
CREATE TABLE IF NOT EXISTS public."Roles"
(
    "RolesID" uuid NOT NULL,
    "RoleName" character varying(30) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "RolesID_pkey" PRIMARY KEY ("RolesID")
        USING INDEX TABLESPACE "TaskManagerDB"
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."Roles"
    OWNER to postgres;*/