using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26

namespace TaskManager.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }

}



/*CREATE TABLE IF NOT EXISTS public."Comments"
(
    "CommentsID" uuid NOT NULL,
    "Content" text COLLATE pg_catalog."default" NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "TaskID" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "Comments_pkey" PRIMARY KEY ("CommentsID")
        USING INDEX TABLESPACE "TaskManagerDB",
    CONSTRAINT "TaskId_fkey" FOREIGN KEY ("TaskID")
        REFERENCES public."Task" ("TaskID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT "UserId_fkey" FOREIGN KEY ("UserId")
        REFERENCES public."Users" ("UserID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL
        NOT VALID
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."Comments"
    OWNER to postgres;*/



/*
 * namespace TaskManager.Domain.Entities
{
    public class Comments
    {
        public Guid CommentsID { get; set; }
        public string Text { get; set; } = null!; 
        public DateTime CreatedAt { get; set; }

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public Guid? UserId { get; set; } 
        public User? User { get; set; }
    }

    // This class represents a comment associated with a task in the Task Manager domain.
}

*/