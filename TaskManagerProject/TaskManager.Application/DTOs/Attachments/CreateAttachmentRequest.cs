using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26

namespace TaskManager.Application.DTOs.Attachments;


public class CreateAttachmentRequest
{
    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Guid TaskId { get; set; }
    public Guid? UserId { get; set; }
}

