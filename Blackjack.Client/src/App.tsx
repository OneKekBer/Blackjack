import { Route, Routes } from 'react-router-dom'
import LobbyPage from './pages/lobby/LobbyPage'
import GamePage from './pages/game/GamePage'
import { useEffect } from 'react'
import ErrorPage from './pages/error/ErrorPage'
import { v4 as uuidv4 } from 'uuid'
import { useCookies } from 'react-cookie'
import { ToastContainer } from 'react-toastify'

function App() {
	const [cookies, setCookie] = useCookies()

	useEffect(() => {
		if (!cookies['user-id']) {
			const newId = uuidv4()

			setCookie('user-id', newId, {
				path: '/',
				maxAge: 60 * 60 * 24 * 2, // two days
				sameSite: 'lax',
			})
			console.log('New player id set:', newId)
		} else {
			console.log('Existing player id:', cookies['user-id'])
		}
	}, [cookies, setCookie])

	return (
		<>
			<div>
				<ToastContainer />
				<Routes>
					<Route element={<LobbyPage />} path='/' />
					<Route element={<GamePage />} path='/game/:gameId' />
					<Route element={<ErrorPage />} path='*' />
				</Routes>
			</div>
		</>
	)
}

export default App
