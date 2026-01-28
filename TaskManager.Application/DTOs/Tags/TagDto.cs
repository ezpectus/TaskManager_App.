using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26


namespace TaskManager.Application.DTOs.Tags;

public class TagDto
{
    public Guid Id { get; set; }
    //  public Guid TagId { get; set; }
    public string TagName { get; set; } = null!;
    public string TagColor { get; set; } = "#FFFFFF";
}

