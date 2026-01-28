using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

//updated 05.01.26
namespace TaskManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<UserRole> UserRoles { get; set; } = new();
        public List<TaskItem> Tasks { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }

}



/*CREATE TABLE IF NOT EXISTS public."Users"
(
    "UserID" uuid NOT NULL,
    "Username" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Email" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "PasswordHash" text COLLATE pg_catalog."default" NOT NULL,
    "CreatedAt" timestamp without time zone NOT NULL,
    CONSTRAINT "Users_pkey" PRIMARY KEY ("UserID")
        USING INDEX TABLESPACE "TaskManagerDB"
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."Users"
    OWNER to postgres;*/





