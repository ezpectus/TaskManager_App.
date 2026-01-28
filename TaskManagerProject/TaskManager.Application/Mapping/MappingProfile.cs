using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs;
//updated 27.01.26


// DTOs groups
using TaskManager.Application.DTOs.Users;
using TaskManager.Application.DTOs.Roles;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.DTOs.Subtasks;
using TaskManager.Application.DTOs.Comments;
using TaskManager.Application.DTOs.Attachments;
using TaskManager.Application.DTOs.Tags;
using TaskManager.Application.DTOs.Activity;

namespace TaskManager.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ======================
            // USER
            // ======================
            CreateMap<User, UserDto>();

            // ======================
            // ROLE
            // ======================
            CreateMap<Role, RoleDto>();

            // ======================
            // ACTIVITY LOG
            // ======================
            CreateMap<ActivityLog, ActivityLogReadDto>()
                .ForMember(
                    d => d.TaskTitle,
                    o => o.MapFrom(s => s.Task != null ? s.Task.Title : null)
                )
                .ForMember(
                    d => d.Username,
                    o => o.MapFrom(s => s.User != null ? s.User.Username : null)
                );

            // ======================
            // FILE ATTACHMENT
            // ======================
            CreateMap<FileAttachment, FileAttachmentDto>()
                .ForMember(
                    d => d.Username,
                    o => o.MapFrom(s => s.User != null ? s.User.Username : null)
                );

            CreateMap<CreateAttachmentRequest, FileAttachment>();

            // ======================
            // COMMENT
            // ======================
            CreateMap<Comment, CommentDto>()
                .ForMember(
                    d => d.Username,
                    o => o.MapFrom(s => s.User != null ? s.User.Username : null)
                );

            CreateMap<CreateCommentRequest, Comment>();

            // ======================
            // SUBTASK
            // ======================
            CreateMap<Subtask, SubtaskDto>();
            CreateMap<CreateSubtaskRequest, Subtask>();

            // ======================
            // TAG
            // ======================
            CreateMap<Tag, TagDto>();

            // ======================
            // TASK
            // ======================
            CreateMap<TaskItem, TaskDto>()
                .ForMember(
                    d => d.Username,
                    o => o.MapFrom(s => s.User != null ? s.User.Username : null)
                );

            CreateMap<CreateTaskRequest, TaskItem>();

            CreateMap<UpdateTaskRequest, TaskItem>()
                .ForAllMembers(o =>
                    o.Condition((src, dest, value) => value != null)
                );
        }
    }
}
