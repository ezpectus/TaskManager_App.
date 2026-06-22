import { useNavigate } from 'react-router-dom'
import { Home, ArrowLeft } from 'lucide-react'

export default function NotFoundPage() {
  const navigate = useNavigate()

  return (
    <div className="flex min-h-screen flex-col items-center justify-center bg-secondary p-4">
      <h1 className="mb-2 text-6xl font-bold text-muted-foreground">404</h1>
      <p className="mb-6 text-lg text-muted-foreground">Page not found</p>
      <div className="flex gap-2">
        <button className="btn-outline" onClick={() => navigate(-1)}>
          <ArrowLeft size={18} /> Go back
        </button>
        <button className="btn-primary" onClick={() => navigate('/')}>
          <Home size={18} /> Home
        </button>
      </div>
    </div>
  )
}
