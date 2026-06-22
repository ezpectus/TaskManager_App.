import type { TaskDto } from '../types'

export function exportTasksToCSV(tasks: TaskDto[]): void {
  const headers = ['Title', 'Description', 'Status', 'Priority', 'Deadline', 'Created At', 'Updated At']
  const rows = tasks.map((t) => [
    escapeCSV(t.title),
    escapeCSV(t.description),
    t.status,
    t.priority,
    t.deadline,
    new Date(t.createdAt).toISOString(),
    new Date(t.updatedAt).toISOString(),
  ])

  const csv = [headers.join(','), ...rows.map((r) => r.join(','))].join('\n')
  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = `tasks-export-${new Date().toISOString().split('T')[0]}.csv`
  link.click()
  URL.revokeObjectURL(url)
}

function escapeCSV(value: string): string {
  if (value.includes(',') || value.includes('"') || value.includes('\n')) {
    return `"${value.replace(/"/g, '""')}"`
  }
  return value
}
