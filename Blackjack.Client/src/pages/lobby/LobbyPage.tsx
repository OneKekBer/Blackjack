import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { CreateGame, GetAllGames } from './api'
import type { Game } from '../../types/Game'

const LobbyPageComponent: React.FC = () => {
	const [games, setGames] = useState<Game[]>([])
	const navigate = useNavigate()

	const fetchGames = async () => {
		const games = await GetAllGames()
		console.log(games)
		if (!games) throw new Error('There no games to connect')
		setGames(games)
	}

	const handleConnectToGame = async (gameId: string) => {
		navigate(`/game/${gameId}`)
	}

	const handleCreateGame = async () => {
		await CreateGame()
	}

	useEffect(() => {
		fetchGames()
	}, [])

	return (
		<div className='min-h-screen bg-gray-100 p-6'>
			<div className='max-w-3xl mx-auto bg-white shadow-xl rounded-2xl p-6 space-y-6'>
				<h1 className='text-3xl font-bold text-center text-gray-800'>
					Lobby
				</h1>

				<div className='flex justify-center gap-4'>
					<button
						onClick={fetchGames}
						className='px-4 py-2 bg-blue-500 hover:bg-blue-600 text-white rounded-xl shadow'
					>
						Refresh
					</button>
					<button
						onClick={handleCreateGame}
						className='px-4 py-2 bg-green-500 hover:bg-green-600 text-white rounded-xl shadow'
					>
						Create New Game
					</button>
				</div>

				<div className='space-y-2'>
					{games.length ? (
						games.map((item, k) => (
							<div
								key={k}
								className='p-4 bg-gray-50 border border-gray-200 rounded-xl shadow-sm'
							>
								<p className='text-gray-700 font-medium'>
									Game ID: {item.id}
								</p>
								<button
									onClick={() => handleConnectToGame(item.id)}
									className='mt-2 px-3 py-1 bg-indigo-500 text-white rounded hover:bg-indigo-600'
								>
									Connect
								</button>
							</div>
						))
					) : (
						<p className='text-center text-gray-500'>No games found.</p>
					)}
				</div>
			</div>
		</div>
	)
}

export default LobbyPageComponent
