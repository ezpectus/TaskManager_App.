using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26

namespace TaskManager.Domain.Entities;

    public class FileAttachment
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public DateTime UploadedAt { get; set; }

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }







/*CREATE TABLE IF NOT EXISTS public."Attachments"
(
    "AttachmentsID" uuid NOT NULL,
    "FileName" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Url" text COLLATE pg_catalog."default" NOT NULL,
    "UploadedAt" timestamp with time zone NOT NULL,
    "TaskId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "Attachments_pkey" PRIMARY KEY ("AttachmentsID")
        USING INDEX TABLESPACE "TaskManagerDB",
    CONSTRAINT "TaskId_fkey" FOREIGN KEY ("TaskId")
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

ALTER TABLE IF EXISTS public."Attachments"
    OWNER to postgres;*/




/*namespace TaskManager.Domain.Entities
{
    public class Attachments
    {
        public Guid AttachmentsID { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!; 
        public DateTime UploadedAt { get; set; }

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }

    // This class represents an attachment associated with a task in the Task Manager domain.
}

*/