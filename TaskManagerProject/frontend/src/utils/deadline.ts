interface RelativeDeadlineResult {
  text: string
  className: string
  isOverdue: boolean
  isToday: boolean
}

export function getRelativeDeadline(deadline: string, status: string): RelativeDeadlineResult | null {
  if (!deadline || deadline.startsWith('0001-01-01')) return null

  const due = new Date(deadline)
  const now = new Date()
  const isOverdue = status !== 'Done' && status !== 'Cancelled' && due < now

  const dueDay = new Date(due.getFullYear(), due.getMonth(), due.getDate())
  const todayDay = new Date(now.getFullYear(), now.getMonth(), now.getDate())
  const diffDays = Math.round((dueDay.getTime() - todayDay.getTime()) / (1000 * 60 * 60 * 24))

  if (status === 'Done' || status === 'Cancelled') {
    return {
      text: new Date(deadline).toLocaleDateString(),
      className: 'text-muted-foreground',
      isOverdue: false,
      isToday: false,
    }
  }

  if (diffDays === 0) {
    return {
      text: 'Due today',
      className: 'text-orange-500 font-medium',
      isOverdue: false,
      isToday: true,
    }
  }

  if (diffDays === 1) {
    return {
      text: 'Due tomorrow',
      className: 'text-yellow-600 dark:text-yellow-400 font-medium',
      isOverdue: false,
      isToday: false,
    }
  }

  if (diffDays > 0 && diffDays <= 7) {
    return {
      text: `${diffDays} days left`,
      className: 'text-muted-foreground',
      isOverdue: false,
      isToday: false,
    }
  }

  if (diffDays > 7) {
    return {
      text: new Date(deadline).toLocaleDateString(),
      className: 'text-muted-foreground',
      isOverdue: false,
      isToday: false,
    }
  }

  const overdueDays = Math.abs(diffDays)
  return {
    text: overdueDays === 1 ? 'Overdue by 1 day' : `Overdue by ${overdueDays} days`,
    className: 'text-red-500 font-medium',
    isOverdue: true,
    isToday: false,
  }
}

export function isOverdue(deadline: string, status: string): boolean {
  if (!deadline || deadline.startsWith('0001-01-01') || status === 'Done' || status === 'Cancelled') return false
  return new Date(deadline) < new Date()
}

export function isDueToday(deadline: string, status: string): boolean {
  if (!deadline || deadline.startsWith('0001-01-01') || status === 'Done' || status === 'Cancelled') return false
  const due = new Date(deadline)
  const now = new Date()
  return due.getFullYear() === now.getFullYear() &&
    due.getMonth() === now.getMonth() &&
    due.getDate() === now.getDate()
}

const PRIORITY_SCORE: Record<string, number> = { Critical: 4, High: 3, Medium: 2, Low: 1 }

export function getPriorityScore(priority: string): number {
  return PRIORITY_SCORE[priority] ?? 1
}

export function getUrgencyScore(deadline: string, status: string): number {
  if (!deadline || deadline.startsWith('0001-01-01') || status === 'Done' || status === 'Cancelled') return 0
  const now = new Date()
  const due = new Date(deadline)
  const diffHours = (due.getTime() - now.getTime()) / (1000 * 60 * 60)

  if (diffHours < 0) return 100 + Math.min(Math.abs(diffHours) / 24, 50)
  if (diffHours < 24) return 90
  if (diffHours < 72) return 70
  if (diffHours < 168) return 50
  if (diffHours < 336) return 30
  return 10
}

export interface SmartScore {
  total: number
  urgency: number
  priority: number
}

export function getSmartScore(task: { deadline: string; priority: string; status: string }): SmartScore {
  const urgency = getUrgencyScore(task.deadline, task.status)
  const priority = getPriorityScore(task.priority)
  const total = urgency * priority
  return { total, urgency, priority }
}

export function sortBySmartScore<T extends { deadline: string; priority: string; status: string }>(tasks: T[]): T[] {
  return [...tasks].sort((a, b) => {
    const sa = getSmartScore(a)
    const sb = getSmartScore(b)
    if (sb.total !== sa.total) return sb.total - sa.total
    return new Date(a.deadline).getTime() - new Date(b.deadline).getTime()
  })
}
