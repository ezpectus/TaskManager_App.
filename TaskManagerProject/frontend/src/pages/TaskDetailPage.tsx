import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { taskService } from '../services/taskService'
import type { TaskDto } from '../types'
import { ArrowLeft, Trash2, Save } from 'lucide-react'

export default function TaskDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const [task, setTask] = useState<TaskDto | null>(null)
  const [loading, setLoading] = useState(true)
  const [editing, setEditing] = useState(false)
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')

  useEffect(() => {
    if (!id) return
    const fetch = async () => {
      try {
        const data = await taskService.getById(id)
        setTask(data)
        setTitle(data.title)
        setDescription(data.description)
      } catch {
        // handle error
      } finally {
        setLoading(false)
      }
    }
    fetch()
  }, [id])

  const handleSave = async () => {
    if (!id) return
    await taskService.update(id, { title, description, priority: task!.priority })
    setEditing(false)
    const data = await taskService.getById(id)
    setTask(data)
  }

  const handleDelete = async () => {
    if (!id || !confirm('Delete this task?')) return
    await taskService.delete(id)
    navigate('/')
  }

  if (loading) return <div className="p-6 text-muted-foreground">Loading...</div>
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
          <span className="badge bg-blue-100 text-blue-800">{task.status}</span>
          <span className="badge bg-orange-100 text-orange-800">{task.priority}</span>
        </div>

        <div className="grid grid-cols-2 gap-6">
          <div>
            <h2 className="mb-3 text-lg font-semibold">Subtasks</h2>
            {task.subtasks.length === 0 ? (
              <p className="text-sm text-muted-foreground">No subtasks</p>
            ) : (
              <ul className="space-y-2">
                {task.subtasks.map((st) => (
                  <li key={st.id} className="flex items-center gap-2">
                    <input type="checkbox" checked={st.isCompleted} readOnly className="h-4 w-4" />
                    <span className={st.isCompleted ? 'line-through text-muted-foreground' : ''}>
                      {st.title}
                    </span>
                  </li>
                ))}
              </ul>
            )}
          </div>

          <div>
            <h2 className="mb-3 text-lg font-semibold">Comments</h2>
            {task.comments.length === 0 ? (
              <p className="text-sm text-muted-foreground">No comments</p>
            ) : (
              <ul className="space-y-3">
                {task.comments.map((c) => (
                  <li key={c.id} className="card p-3">
                    <p className="text-sm">{c.content}</p>
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
