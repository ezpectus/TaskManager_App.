using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
//updated 05.01.26

namespace TaskManager.Domain.Entities;

    public class ActivityLog
    {
        public Guid Id { get; set; }

        public string ActionType { get; set; } = null!;
        public DateTime Timestamp { get; set; }

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }







// This class represents an activity log entry in the Task Manager domain, which records actions performed on tasks.

/*
CREATE TABLE IF NOT EXISTS public."ActivityLog"
(
    "ActivityID" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "TaskId " uuid NOT NULL,
    "ActionType " character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "ActivityLog_pkey" PRIMARY KEY ("ActivityID")
        USING INDEX TABLESPACE "TaskManagerDB",
    CONSTRAINT "TaskId " FOREIGN KEY ("TaskId ")
        REFERENCES public."Task" ("TaskID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
        NOT VALID,
    CONSTRAINT "UserId_fkey" FOREIGN KEY ("UserId")
        REFERENCES public."Users" ("UserID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."ActivityLog"
    OWNER to postgres;*/





