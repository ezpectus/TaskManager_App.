using AutoMapper;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Users;
using TaskManager.Application.DTOs.Roles;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.DTOs.Subtasks;
using TaskManager.Application.DTOs.Comments;
using TaskManager.Application.DTOs.Attachments;
using TaskManager.Application.DTOs.Tags;
using TaskManager.Application.DTOs.Activity;

namespace TaskManager.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.Roles, o => o.MapFrom(s => s.UserRoles.Select(ur => ur.Role)));
        CreateMap<Role, RoleDto>();
        CreateMap<RoleDto, Role>();

        CreateMap<ActivityLog, ActivityLogReadDto>()
            .ForMember(d => d.TaskTitle, o => o.MapFrom(s => s.Task != null ? s.Task.Title : null))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.User != null ? s.User.Username : null));

        CreateMap<FileAttachment, FileAttachmentDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
        CreateMap<CreateAttachmentRequest, FileAttachment>();

        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
        CreateMap<CreateCommentRequest, Comment>();

        CreateMap<Subtask, SubtaskDto>();
        CreateMap<CreateSubtaskRequest, Subtask>();

        CreateMap<Tag, TagDto>();

        CreateMap<TaskItem, TaskDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
        CreateMap<CreateTaskRequest, TaskItem>();
        CreateMap<UpdateTaskRequest, TaskItem>()
            .ForAllMembers(o => o.Condition((src, dest, value) => value != null));
    }
}
