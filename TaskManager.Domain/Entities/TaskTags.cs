using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//updated 26.01.26
namespace TaskManager.Domain.Entities;

public class TaskTag
{
    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}





/*CREATE TABLE IF NOT EXISTS public."TaskTags"
(
    "TaskID" uuid NOT NULL,
    "TagID" uuid NOT NULL,
    CONSTRAINT "TagID_fkey" FOREIGN KEY ("TagID")
        REFERENCES public."Tags" ("TagsID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT "TaskID_fkey" FOREIGN KEY ("TaskID")
        REFERENCES public."Task" ("TaskID") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."TaskTags"
    OWNER to postgres;*/