import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { taskService } from '../services/taskService'
import { subtaskService } from '../services/subtaskService'
import { commentService } from '../services/commentService'
import type { TaskDto, SubtaskDto } from '../types'
import { useToast } from '../context/ToastContext'
import { ArrowLeft, Trash2, Save, Plus, X, MessageSquare, ListChecks } from 'lucide-react'

const STATUS_COLORS: Record<string, string> = {
  Todo: 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200',
  InProgress: 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200',
  Done: 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200',
}

const PRIORITY_COLORS: Record<string, string> = {
  Low: 'bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-300',
  Medium: 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-200',
  High: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200',
}

export default function TaskDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const { showToast } = useToast()
  const [task, setTask] = useState<TaskDto | null>(null)
  const [loading, setLoading] = useState(true)
  const [editing, setEditing] = useState(false)
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [newSubtaskTitle, setNewSubtaskTitle] = useState('')
  const [newComment, setNewComment] = useState('')

  const fetchTask = async () => {
    if (!id) return
    try {
      const data = await taskService.getById(id)
      setTask(data)
      setTitle(data.title)
      setDescription(data.description)
    } catch {
      showToast('Failed to load task', 'error')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchTask()
  }, [id])

  const handleSave = async () => {
    if (!id || !task) return
    try {
      await taskService.update(id, { title, description, priority: task.priority })
      showToast('Task updated', 'success')
      setEditing(false)
      fetchTask()
    } catch {
      showToast('Failed to update task', 'error')
    }
  }

  const handleDelete = async () => {
    if (!id || !confirm('Delete this task?')) return
    try {
      await taskService.delete(id)
      showToast('Task deleted', 'success')
      navigate('/')
    } catch {
      showToast('Failed to delete task', 'error')
    }
  }

  const handleAddSubtask = async () => {
    if (!id || !newSubtaskTitle.trim()) return
    try {
      await subtaskService.create({ title: newSubtaskTitle, taskId: id })
      setNewSubtaskTitle('')
      showToast('Subtask added', 'success')
      fetchTask()
    } catch {
      showToast('Failed to add subtask', 'error')
    }
  }

  const handleToggleSubtask = async (subtask: SubtaskDto) => {
    try {
      await subtaskService.update(subtask.id, {
        ...subtask,
        isCompleted: !subtask.isCompleted,
      })
      fetchTask()
    } catch {
      showToast('Failed to update subtask', 'error')
    }
  }

  const handleDeleteSubtask = async (subtaskId: string) => {
    try {
      await subtaskService.delete(subtaskId)
      showToast('Subtask deleted', 'success')
      fetchTask()
    } catch {
      showToast('Failed to delete subtask', 'error')
    }
  }

  const handleAddComment = async () => {
    if (!id || !newComment.trim()) return
    try {
      await commentService.create({ content: newComment, taskId: id })
      setNewComment('')
      showToast('Comment added', 'success')
      fetchTask()
    } catch {
      showToast('Failed to add comment', 'error')
    }
  }

  const handleDeleteComment = async (commentId: string) => {
    try {
      await commentService.delete(commentId)
      showToast('Comment deleted', 'success')
      fetchTask()
    } catch {
      showToast('Failed to delete comment', 'error')
    }
  }

  if (loading) {
    return (
      <div className="mx-auto max-w-4xl p-6">
        <div className="skeleton mb-4 h-10 w-24" />
        <div className="card p-6">
          <div className="skeleton mb-4 h-8 w-3/4" />
          <div className="skeleton mb-6 h-24 w-full" />
          <div className="grid grid-cols-2 gap-6">
            <div className="skeleton h-40 w-full" />
            <div className="skeleton h-40 w-full" />
          </div>
        </div>
      </div>
    )
  }

  if (!task) return <div className="p-6 text-muted-foreground">Task not found</div>

  return (
    <div className="mx-auto max-w-4xl p-6">
      <button className="btn-outline mb-4" onClick={() => navigate('/')}>
        <ArrowLeft size={18} /> Back
      </button>

      <div className="card p-6">
        <div className="mb-4 flex items-start justify-between">
          {editing ? (
            <input className="input flex-1" value={title} onChange={(e) => setTitle(e.target.value)} />
          ) : (
            <h1 className="text-2xl font-bold">{task.title}</h1>
          )}
          <div className="flex gap-2">
            {editing ? (
              <button className="btn-primary" onClick={handleSave}>
                <Save size={18} /> Save
              </button>
            ) : (
              <button className="btn-outline" onClick={() => setEditing(true)}>Edit</button>
            )}
            <button className="btn-destructive" onClick={handleDelete}>
              <Trash2 size={18} /> Delete
            </button>
          </div>
        </div>

        {editing ? (
          <textarea className="input min-h-[120px]" value={description} onChange={(e) => setDescription(e.target.value)} />
        ) : (
          <p className="mb-6 whitespace-pre-wrap text-muted-foreground">{task.description}</p>
        )}

        <div className="mb-6 flex gap-2">
          <span className={`badge ${STATUS_COLORS[task.status]}`}>{task.status}</span>
          <span className={`badge ${PRIORITY_COLORS[task.priority]}`}>{task.priority}</span>
        </div>

        <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
          {/* Subtasks */}
          <div>
            <div className="mb-3 flex items-center gap-2">
              <ListChecks className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Subtasks</h2>
            </div>

            <div className="mb-3 flex gap-2">
              <input
                className="input"
                placeholder="Add subtask..."
                value={newSubtaskTitle}
                onChange={(e) => setNewSubtaskTitle(e.target.value)}
                onKeyDown={(e) => e.key === 'Enter' && handleAddSubtask()}
              />
              <button className="btn-primary shrink-0" onClick={handleAddSubtask}>
                <Plus size={18} />
              </button>
            </div>

            {task.subtasks.length === 0 ? (
              <p className="text-sm text-muted-foreground">No subtasks yet</p>
            ) : (
              <ul className="space-y-2">
                {task.subtasks.map((st) => (
                  <li key={st.id} className="flex items-center gap-2 rounded-md border p-2">
                    <input
                      type="checkbox"
                      checked={st.isCompleted}
                      onChange={() => handleToggleSubtask(st)}
                      className="h-4 w-4"
                    />
                    <span className={`flex-1 text-sm ${st.isCompleted ? 'line-through text-muted-foreground' : ''}`}>
                      {st.title}
                    </span>
                    <button
                      onClick={() => handleDeleteSubtask(st.id)}
                      className="rounded p-1 text-muted-foreground hover:text-destructive"
                    >
                      <X size={14} />
                    </button>
                  </li>
                ))}
              </ul>
            )}
          </div>

          {/* Comments */}
          <div>
            <div className="mb-3 flex items-center gap-2">
              <MessageSquare className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Comments</h2>
            </div>

            <div className="mb-3 flex gap-2">
              <input
                className="input"
                placeholder="Write a comment..."
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                onKeyDown={(e) => e.key === 'Enter' && handleAddComment()}
              />
              <button className="btn-primary shrink-0" onClick={handleAddComment}>
                <Plus size={18} />
              </button>
            </div>

            {task.comments.length === 0 ? (
              <p className="text-sm text-muted-foreground">No comments yet</p>
            ) : (
              <ul className="space-y-3">
                {task.comments.map((c) => (
                  <li key={c.id} className="card p-3">
                    <div className="flex items-start justify-between">
                      <p className="text-sm">{c.content}</p>
                      <button
                        onClick={() => handleDeleteComment(c.id)}
                        className="ml-2 rounded p-1 text-muted-foreground hover:text-destructive"
                      >
                        <X size={14} />
                      </button>
                    </div>
                    <p className="mt-1 text-xs text-muted-foreground">
                      {new Date(c.createdAt).toLocaleDateString()}
                    </p>
                  </li>
                ))}
              </ul>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}
