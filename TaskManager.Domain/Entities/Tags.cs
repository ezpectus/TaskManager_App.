using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskManager.Domain.Entities;
//updated 05.01.26

namespace TaskManager.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string TagName { get; set; } = null!;
        public string TagColor { get; set; } = "#FFFFFF";

        public List<TaskTag> TaskTags { get; set; } = new();
    }

}





/*CREATE TABLE IF NOT EXISTS public."Tags"
(
    "TagsID" uuid NOT NULL,
    "TagsName" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "TagsColor" character varying(30) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Tags_pkey" PRIMARY KEY ("TagsID")
        USING INDEX TABLESPACE "TaskManagerDB"
)

TABLESPACE "TaskManagerDB";

ALTER TABLE IF EXISTS public."Tags"
    OWNER to postgres;*/