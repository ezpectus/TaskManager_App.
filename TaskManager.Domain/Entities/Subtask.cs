using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
//updated 05.01.26


namespace TaskManager.Domain.Entities
{
    public class Subtask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }

        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;
    }

}





/*
CREATE TABLE IF NOT EXISTS public."Subtasks"
(
    "SubtasksID" uuid NOT NULL,
    "Subtasks_tittle" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "IsCompleted" boolean NOT NULL DEFAULT false,
    "TaskId" uuid NOT NULL,
    CONSTRAINT "Subtasks_pkey" PRIMARY KEY("SubtasksID")
        USING INDEX TABLESPACE "TaskManagerDB",
    CONSTRAINT "TaskId_fkey" FOREIGN KEY("TaskId")
        REFERENCES public."Task"("TaskID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."Subtasks"
    OWNER to postgres;*/