using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
//updated 26/01/26
namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;

        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public List<Subtask> Subtasks { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<FileAttachment> Attachments { get; set; } = new();
        public List<TaskTag> TaskTags { get; set; } = new();
        public List<ActivityLog> ActivityLogs { get; set; } = new();
    }

}




/*
CREATE TABLE IF NOT EXISTS public."Task"
(
    "TaskID" uuid NOT NULL,
    "Title" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Description" character varying(250) COLLATE pg_catalog."default" NOT NULL,
    status character varying(25) COLLATE pg_catalog."default" NOT NULL,
    "Priority" character varying(10) COLLATE pg_catalog."default" NOT NULL,
    "Deadline" timestamp without time zone NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "Task_pkey" PRIMARY KEY ("TaskID")
        USING INDEX TABLESPACE "TaskManagerDB",
    CONSTRAINT "UserID_fkey" FOREIGN KEY ("UserId")
        REFERENCES public."Users" ("UserID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL
        NOT VALID
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."Task"
    OWNER to postgres;*/