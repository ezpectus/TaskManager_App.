import { describe, it, expect } from 'vitest'
import {
  getRelativeDeadline,
  isOverdue,
  isDueToday,
  getPriorityScore,
  getUrgencyScore,
  getSmartScore,
  sortBySmartScore,
} from '../utils/deadline'

describe('getRelativeDeadline', () => {
  it('returns null for empty deadline', () => {
    expect(getRelativeDeadline('', 'Todo')).toBeNull()
  })

  it('returns null for default date', () => {
    expect(getRelativeDeadline('0001-01-01T00:00:00', 'Todo')).toBeNull()
  })

  it('returns "Due today" for today deadline', () => {
    const today = new Date().toISOString()
    const result = getRelativeDeadline(today, 'Todo')
    expect(result?.text).toBe('Due today')
    expect(result?.isToday).toBe(true)
  })

  it('returns "Due tomorrow" for tomorrow deadline', () => {
    const tomorrow = new Date(Date.now() + 86400000).toISOString()
    const result = getRelativeDeadline(tomorrow, 'Todo')
    expect(result?.text).toBe('Due tomorrow')
  })

  it('returns "X days left" for 3 days away', () => {
    const future = new Date(Date.now() + 3 * 86400000).toISOString()
    const result = getRelativeDeadline(future, 'Todo')
    expect(result?.text).toBe('3 days left')
  })

  it('returns "Overdue by X days" for past deadline', () => {
    const past = new Date(Date.now() - 3 * 86400000).toISOString()
    const result = getRelativeDeadline(past, 'Todo')
    expect(result?.isOverdue).toBe(true)
    expect(result?.text).toBe('Overdue by 3 days')
  })

  it('returns formatted date for completed tasks', () => {
    const future = new Date(Date.now() + 3 * 86400000).toISOString()
    const result = getRelativeDeadline(future, 'Done')
    expect(result?.isOverdue).toBe(false)
    expect(result?.text).toBe(new Date(future).toLocaleDateString())
  })

  it('returns formatted date for cancelled tasks', () => {
    const future = new Date(Date.now() + 3 * 86400000).toISOString()
    const result = getRelativeDeadline(future, 'Cancelled')
    expect(result?.isOverdue).toBe(false)
    expect(result?.text).toBe(new Date(future).toLocaleDateString())
  })
})

describe('isOverdue', () => {
  it('returns false for empty deadline', () => {
    expect(isOverdue('', 'Todo')).toBe(false)
  })

  it('returns false for completed tasks', () => {
    const past = new Date(Date.now() - 86400000).toISOString()
    expect(isOverdue(past, 'Done')).toBe(false)
  })

  it('returns false for cancelled tasks', () => {
    const past = new Date(Date.now() - 86400000).toISOString()
    expect(isOverdue(past, 'Cancelled')).toBe(false)
  })

  it('returns true for past deadline on non-completed task', () => {
    const past = new Date(Date.now() - 86400000).toISOString()
    expect(isOverdue(past, 'Todo')).toBe(true)
  })

  it('returns false for future deadline', () => {
    const future = new Date(Date.now() + 86400000).toISOString()
    expect(isOverdue(future, 'Todo')).toBe(false)
  })
})

describe('isDueToday', () => {
  it('returns true for today', () => {
    const today = new Date().toISOString()
    expect(isDueToday(today, 'Todo')).toBe(true)
  })

  it('returns false for tomorrow', () => {
    const tomorrow = new Date(Date.now() + 86400000).toISOString()
    expect(isDueToday(tomorrow, 'Todo')).toBe(false)
  })

  it('returns false for cancelled tasks', () => {
    const today = new Date().toISOString()
    expect(isDueToday(today, 'Cancelled')).toBe(false)
  })
})

describe('getPriorityScore', () => {
  it('returns 4 for Critical', () => {
    expect(getPriorityScore('Critical')).toBe(4)
  })

  it('returns 3 for High', () => {
    expect(getPriorityScore('High')).toBe(3)
  })

  it('returns 2 for Medium', () => {
    expect(getPriorityScore('Medium')).toBe(2)
  })

  it('returns 1 for Low', () => {
    expect(getPriorityScore('Low')).toBe(1)
  })

  it('returns 1 for unknown priority', () => {
    expect(getPriorityScore('Unknown')).toBe(1)
  })
})

describe('getUrgencyScore', () => {
  it('returns 0 for empty deadline', () => {
    expect(getUrgencyScore('', 'Todo')).toBe(0)
  })

  it('returns 0 for completed tasks', () => {
    const today = new Date().toISOString()
    expect(getUrgencyScore(today, 'Done')).toBe(0)
  })

  it('returns 0 for cancelled tasks', () => {
    const today = new Date().toISOString()
    expect(getUrgencyScore(today, 'Cancelled')).toBe(0)
  })

  it('returns 100+ for overdue tasks', () => {
    const past = new Date(Date.now() - 48 * 3600000).toISOString()
    const score = getUrgencyScore(past, 'Todo')
    expect(score).toBeGreaterThanOrEqual(100)
  })

  it('returns 90 for tasks due within 24 hours', () => {
    const soon = new Date(Date.now() + 12 * 3600000).toISOString()
    expect(getUrgencyScore(soon, 'Todo')).toBe(90)
  })

  it('returns 70 for tasks due within 72 hours', () => {
    const soon = new Date(Date.now() + 48 * 3600000).toISOString()
    expect(getUrgencyScore(soon, 'Todo')).toBe(70)
  })
})

describe('getSmartScore', () => {
  it('calculates total as urgency * priority', () => {
    const overdue = new Date(Date.now() - 48 * 3600000).toISOString()
    const score = getSmartScore({ deadline: overdue, priority: 'High', status: 'Todo' })
    expect(score.priority).toBe(3)
    expect(score.urgency).toBeGreaterThanOrEqual(100)
    expect(score.total).toBe(score.urgency * score.priority)
  })

  it('returns 0 total for cancelled tasks', () => {
    const today = new Date().toISOString()
    const score = getSmartScore({ deadline: today, priority: 'High', status: 'Cancelled' })
    expect(score.total).toBe(0)
  })
})

describe('sortBySmartScore', () => {
  it('sorts by total score descending', () => {
    const tasks = [
      { deadline: new Date(Date.now() + 7 * 86400000).toISOString(), priority: 'Low', status: 'Todo' },
      { deadline: new Date(Date.now() - 86400000).toISOString(), priority: 'High', status: 'Todo' },
      { deadline: new Date(Date.now() + 86400000).toISOString(), priority: 'Medium', status: 'Todo' },
    ]
    const sorted = sortBySmartScore(tasks)
    expect(sorted[0].priority).toBe('High')
    expect(sorted[0].deadline).toBe(tasks[1].deadline)
  })
})
